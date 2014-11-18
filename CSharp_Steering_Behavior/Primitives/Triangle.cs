using System;

using CSharp_Steering_Behavior.Extensions;

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace CSharp_Steering_Behavior.Primitives
{
    public class Triangle : IBoid
    {
        public SteeringBehavior SteeringBehavior { get; set; }
        public Vector3 Position { get; set; }
        public Vector3 Velocity { get; set; }
        public IObstacle[] Obstacles { get; set; }

        private Vector3[] _localLines;
        private Vector3[] _transformedLines;
        private VertexPositionColor[] _vertices;
        private Matrix _centerMatrix;

        private Vector3 _target;
        private IBoid _evadeBoid;

        public Triangle(Vector3 top, Vector3 right, Vector3 left)
        {
            this.Init(top, right, left);
        }

        public Triangle(Vector3 position)
        {
            this.Init(
                new Vector3(0, 0, 0), 
                new Vector3(12, 40, 0), 
                new Vector3(-12, 40, 0)
            );

            this.Position = position;
        }

        private void Init(Vector3 top, Vector3 right, Vector3 left)
        {
            float centerX = -((top.X + right.X + left.X) / 3);
            float centerY = -((top.Y + right.Y + left.Y) / 3);
            var centerPosition = new Vector3(centerX, centerY, 0);
            this._centerMatrix = Matrix.CreateTranslation(centerPosition);

            // Set start position to centerPosition
            this.Position = -centerPosition;

            this._transformedLines = new Vector3[3];
            this._localLines = new Vector3[3];
            this._localLines[0] = top;
            this._localLines[1] = right;
            this._localLines[2] = left;

            _vertices = new VertexPositionColor[3];
            _vertices[0] = new VertexPositionColor(top, Color.Red);
            _vertices[1] = new VertexPositionColor(right, Color.Red);
            _vertices[2] = new VertexPositionColor(left, Color.Red);

            Velocity = new Vector3(-1, -2, 0);
            Velocity = Velocity.Truncate(this.GetMaxVelocity());

            this.SteeringBehavior = new SteeringBehavior(this);
        }

        public void MoveTwoardsTarget(Vector3 target)
        {
            _target = target;
        }

        //public void PersuitTriangle(Triangle triangle)
        //{
        //    this._pursuitTriangle = triangle;
        //    this._pursuitMethod = SteeringBehavior.Pursuit;
        //}

        public void Evade(IBoid boid)
        {
            this._evadeBoid = boid;
        }

        public void Update(GameTime gameTime)
        {
            if (_target != Vector3.Zero)
            {
                SteeringBehavior.Seek(_target);
                SteeringBehavior.CollisionAvoidance(Obstacles);
            }

            if (_evadeBoid != null)
            {
                SteeringBehavior.Evade(_evadeBoid);
            }

            // If not clear target, just wander around...
            if(_target == Vector3.Zero && _evadeBoid == null)
            {
                SteeringBehavior.Wander();
                SteeringBehavior.CollisionAvoidance(Obstacles);
            }

            // Update steering
            SteeringBehavior.Update(gameTime);

            // Calculate world and transform lines
            var world = this._centerMatrix * Matrix.CreateRotationZ(this.SteeringBehavior.Angle) * Matrix.CreateTranslation(this.Position);
            Vector3.Transform(this._localLines, ref world, _transformedLines);
        }

        public float GetMaxVelocity()
        {
            return 3;
        }

        public float GetMass()
        {
            return 20;
        }

        public void Draw(PrimitiveBatch primitiveBatch)
        {
            _vertices[0].Position = _transformedLines[0];
            _vertices[1].Position = _transformedLines[1];
            _vertices[2].Position = _transformedLines[2];

            // Draw triangle
            primitiveBatch.AddVertices(_vertices);
        }

        public void DrawForces(PrimitiveBatch primitiveBatch)
        {
            const int Scale = 100;
            var velocityForce = Vector3.Normalize(Velocity);
            var steeringForce = Vector3.Normalize(this.SteeringBehavior.Steering);
            var desiredVelocityForce = Vector3.Normalize(this.SteeringBehavior.DesiredVelocity);

            var forces = new VertexPositionColor[6];
            forces[0] = new VertexPositionColor(this.Position, Color.Green);
            forces[1] = new VertexPositionColor(this.Position + velocityForce * Scale, Color.Green);

            forces[2] = new VertexPositionColor(this.Position, Color.Gray);
            forces[3] = new VertexPositionColor(this.Position + desiredVelocityForce * Scale, Color.Green);

            forces[4] = new VertexPositionColor(this.Position, Color.Red);
            forces[5] = new VertexPositionColor(this.Position + steeringForce * Scale, Color.Green);

            // Draw forces
            primitiveBatch.AddVertices(forces);
        }
    }
}
