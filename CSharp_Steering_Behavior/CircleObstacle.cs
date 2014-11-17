using System;

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace CSharp_Steering_Behavior
{
    public class CircleObstacle : IObstacle
    {
        public Vector3 Position { get; set; }

        private VertexPositionColor[] _vertices;
        private float _radius;

        public CircleObstacle(Vector3 position, float radius)
        {
            _radius = radius;
            Position = position;

            this.Init();
        }

        private void Init()
        {
            _vertices = new VertexPositionColor[100];

            for (int i = 0; i < 99; i++)
            {
                float angle = (float)(i / 100.0 * Math.PI * 2);
                _vertices[i] = new VertexPositionColor();
                _vertices[i].Position = new Vector3(200 + (float)Math.Cos(angle) * 100, 200 + (float)Math.Sin(angle) * 100, 0);
                _vertices[i].Color = Color.Black;
            }
            _vertices[99] = _vertices[0];
        }

        public void Draw(PrimitiveBatch primitiveBatch)
        {
            primitiveBatch.AddVertices(_vertices);
        }

        public float GetRadius()
        {
            return 4.5f;
        }
    }
}
