using System;

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace CSharp_Steering_Behavior.Primitives
{
    public class CircleObstacle : IObstacle
    {
        public Vector3 Position { get; set; }

        private VertexPositionColor[] _vertices;
        private float _radius;

        public CircleObstacle(Vector3 position, float radius)
        {
            this._radius = radius;
            this.Position = position;

            this.Init();
        }

        private void Init()
        {
            this._vertices = new VertexPositionColor[100];

            for (int i = 0; i < 99; i++)
            {
                float angle = (float)(i / 100.0 * Math.PI * 2);
                this._vertices[i] = new VertexPositionColor();
                this._vertices[i].Position = new Vector3(this.Position.X + (float)Math.Cos(angle) * this._radius, this.Position.Y + (float)Math.Sin(angle) * this._radius, 0);
                this._vertices[i].Color = Color.Black;
            }
            this._vertices[99] = this._vertices[0];
        }

        public void Draw(PrimitiveBatch primitiveBatch)
        {
            primitiveBatch.AddVertices(this._vertices);
        }

        public float GetRadius()
        {
            return this._radius;
        }
    }
}
