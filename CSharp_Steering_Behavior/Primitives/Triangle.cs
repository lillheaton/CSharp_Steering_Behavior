
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace CSharp_Steering_Behavior.Primitives
{
    public class Triangle
    {
        private Vector3[] _localLines;
        private Vector3[] _transformedLines;
        private VertexPositionColor[] _vertices;
        private Matrix _centerMatrix;

        public Triangle(Vector3 top, Vector3 right, Vector3 left)
        {
            this.Init(top, right, left);
        }

        public Triangle()
        {
            this.Init(
                new Vector3(0, 0, 0), 
                new Vector3(12, 40, 0), 
                new Vector3(-12, 40, 0)
            );
        }

        private void Init(Vector3 top, Vector3 right, Vector3 left)
        {
            float centerX = -((top.X + right.X + left.X) / 3);
            float centerY = -((top.Y + right.Y + left.Y) / 3);
            var centerPosition = new Vector3(centerX, centerY, 0);
            this._centerMatrix = Matrix.CreateTranslation(centerPosition);

            // Set start position to centerPosition
            //this.Position = -centerPosition;

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

        public void Update(GameTime gameTime, float angle, Vector3 position)
        {
            // Calculate world and transform lines
            var world = this._centerMatrix * Matrix.CreateRotationZ(angle) * Matrix.CreateTranslation(position);
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
    }
}