using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace CSharp_Steering_Behavior.Primitives
{
    public class Triangle
    {
        public Vector3 Position { get; set; }

        private SteeringBehavior _steering;
        private Vector3[] _lines;
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

            this._lines = new Vector3[3];
            this._lines[0] = top;
            this._lines[1] = right;
            this._lines[2] = left;

            _vertices = new VertexPositionColor[3];
            _vertices[0] = new VertexPositionColor(top, Color.Red);
            _vertices[1] = new VertexPositionColor(right, Color.Red);
            _vertices[2] = new VertexPositionColor(left, Color.Red);

            _steering = new SteeringBehavior();
        }

        public void MoveTwoardsTarget(Vector3 target)
        {
            Position = _steering.Seek(Position, target);
        }       

        public void Draw(PrimitiveBatch primitiveBatch)
        {
            var world = this._centerMatrix * Matrix.CreateRotationZ(_steering.Angle) * Matrix.CreateTranslation(Position);

            var temp = new Vector3[3];
            Vector3.Transform(this._lines, ref world, temp);

            _vertices[0].Position = temp[0];
            _vertices[1].Position = temp[1];
            _vertices[2].Position = temp[2];

            primitiveBatch.AddVertices(_vertices);
        }
    }
}
