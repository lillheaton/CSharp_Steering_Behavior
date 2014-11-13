using System;

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace CSharp_Steering_Behavior.Primitives
{
    public class Triangle
    {
        public SteeringBehavior Steering { get; private set; }

        private Vector3 _target;
        private Vector3[] _localLines;
        private Vector3[] _transformedLines;
        private VertexPositionColor[] _vertices;
        private Matrix _centerMatrix;
        private Action<Vector3> _steeringMethod;

        private Action<Triangle> _pursuitMethod;
        private Triangle _pursuitTriangle;

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

            this.Steering.Position = position;
        }

        private void Init(Vector3 top, Vector3 right, Vector3 left)
        {
            float centerX = -((top.X + right.X + left.X) / 3);
            float centerY = -((top.Y + right.Y + left.Y) / 3);
            var centerPosition = new Vector3(centerX, centerY, 0);
            this._centerMatrix = Matrix.CreateTranslation(centerPosition);

            // Set start position to centerPosition
            this.Steering = new SteeringBehavior(-centerPosition);

            this._transformedLines = new Vector3[3];
            this._localLines = new Vector3[3];
            this._localLines[0] = top;
            this._localLines[1] = right;
            this._localLines[2] = left;

            _vertices = new VertexPositionColor[3];
            _vertices[0] = new VertexPositionColor(top, Color.Red);
            _vertices[1] = new VertexPositionColor(right, Color.Red);
            _vertices[2] = new VertexPositionColor(left, Color.Red);
        }

        public void MoveTwoardsTarget(Vector3 target)
        {
            _target = target;
            _steeringMethod = Steering.Seek;
        }

        public void PersuitTriangle(Triangle triangle)
        {
            this._pursuitTriangle = triangle;
            this._pursuitMethod = Steering.Pursuit;
        }

        public void EvadeTriangle(Triangle triangle)
        {
            this._pursuitTriangle = triangle;
            this._pursuitMethod = Steering.Evade;
        }

        public void Update(GameTime gameTime)
        {
            if (this._pursuitTriangle != null)
            {
                _pursuitMethod(_pursuitTriangle);
            }
            else if (_steeringMethod != null)
            {
                _steeringMethod(_target);    
            }
            else
            {
                Steering.Wander();
            }

            var world = this._centerMatrix * Matrix.CreateRotationZ(this.Steering.Angle) * Matrix.CreateTranslation(this.Steering.Position);
            Vector3.Transform(this._localLines, ref world, _transformedLines);
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
            var velocityForce = Vector3.Normalize(this.Steering.Velocity);
            var steeringForce = Vector3.Normalize(this.Steering.Steering);
            var desiredVelocityForce = Vector3.Normalize(this.Steering.DesiredVelocity);

            var forces = new VertexPositionColor[6];
            forces[0] = new VertexPositionColor(this.Steering.Position, Color.Green);
            forces[1] = new VertexPositionColor(this.Steering.Position + velocityForce * Scale, Color.Green);

            forces[2] = new VertexPositionColor(this.Steering.Position, Color.Gray);
            forces[3] = new VertexPositionColor(this.Steering.Position + desiredVelocityForce * Scale, Color.Green);

            forces[4] = new VertexPositionColor(this.Steering.Position, Color.Red);
            forces[5] = new VertexPositionColor(this.Steering.Position + steeringForce * Scale, Color.Green);

            // Draw forces
            primitiveBatch.AddVertices(forces);
        }
    }
}
