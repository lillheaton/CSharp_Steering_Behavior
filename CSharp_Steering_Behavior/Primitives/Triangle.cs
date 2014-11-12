using System;

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace CSharp_Steering_Behavior.Primitives
{
    public class Triangle
    {
        public Vector3 Position { get; set; }

        private SteeringBehavior _steering;
        private Vector3 _target;
        private Vector3[] _localLines;
        private Vector3[] _transformedLines;
        private VertexPositionColor[] _vertices;
        private Matrix _centerMatrix;

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

            Position = position;
        }

        private void Init(Vector3 top, Vector3 right, Vector3 left)
        {
            float centerX = -((top.X + right.X + left.X) / 3);
            float centerY = -((top.Y + right.Y + left.Y) / 3);
            var centerPosition = new Vector3(centerX, centerY, 0);
            this._centerMatrix = Matrix.CreateTranslation(centerPosition);

            // Set start position to centerPosition
            Position = -centerPosition;

            this._transformedLines = new Vector3[3];
            this._localLines = new Vector3[3];
            this._localLines[0] = top;
            this._localLines[1] = right;
            this._localLines[2] = left;

            _vertices = new VertexPositionColor[3];
            _vertices[0] = new VertexPositionColor(top, Color.Red);
            _vertices[1] = new VertexPositionColor(right, Color.Red);
            _vertices[2] = new VertexPositionColor(left, Color.Red);

            _steering = new SteeringBehavior();
        }

        public void MoveTwoardsTarget(Vector3 target)
        {
            _target = target;
        }

        public void Update(GameTime gameTime)
        {
            //Position = _steering.Seek(Position, _target);

            Position = _steering.Wander(Position);
            
            Console.WriteLine(Position);

            var world = this._centerMatrix * Matrix.CreateRotationZ(_steering.Angle) * Matrix.CreateTranslation(Position);
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
            var velocityForce = Vector3.Normalize(_steering.Velocity);
            var steeringForce = Vector3.Normalize(_steering.Steering);
            var desiredVelocityForce = Vector3.Normalize(_steering.DesiredVelocity);

            var forces = new VertexPositionColor[6];
            forces[0] = new VertexPositionColor(Position, Color.Green);
            forces[1] = new VertexPositionColor(Position + velocityForce * Scale, Color.Green);

            forces[2] = new VertexPositionColor(Position, Color.Gray);
            forces[3] = new VertexPositionColor(Position + desiredVelocityForce * Scale, Color.Green);

            forces[4] = new VertexPositionColor(Position, Color.Red);
            forces[5] = new VertexPositionColor(Position + steeringForce * Scale, Color.Green);

            // Draw forces
            primitiveBatch.AddVertices(forces);
        }
    }
}
