using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace CSharp_Steering_Behavior.Primitives
{
    public class Triangle
    {
        private GraphicsDevice Graphics { get; set; }
        private VertexBuffer VertexBuffer { get; set; }
        private Matrix LocalWorld { get; set; }

        public Vector3 Position { get; set; }

        private double _radians;

        public Triangle(GraphicsDevice graphics, Vector3 top, Vector3 right, Vector3 left)
        {
            this.Graphics = graphics;

            float centerX = -((top.X + right.X + left.X) / 3);
            float centerY = -((top.Y + right.Y + left.Y) / 3);
            var centerPosition = new Vector3(centerX, centerY, 0);
            this.LocalWorld = Matrix.CreateTranslation(centerPosition);

            // Set start position to centerPosition
            Position = -centerPosition;

            var vertices = new VertexPositionColor[3];
            vertices[0] = new VertexPositionColor(top, Color.Red);
            vertices[1] = new VertexPositionColor(right, Color.Red);
            vertices[2] = new VertexPositionColor(left, Color.Red);

            VertexBuffer = new VertexBuffer(graphics, typeof(VertexPositionColor), 3, BufferUsage.WriteOnly);
            VertexBuffer.SetData<VertexPositionColor>(vertices);

            this._radians = 0;
        }

        public Triangle(GraphicsDevice graphics, Vector3 position)
        {
            
        }

        //public void Rotate(float degrees)
        //{
        //    this.CurrentAngle = degrees;
        //}

        //public void Move()
        //{
        //    var radians = MathHelper.ToRadians(CurrentAngle + 90);
        //    var target = new Vector3((float)Math.Cos(radians), (float)Math.Sin(radians), 0);
        //    //_position = _position + target * 0.03f;
        //}

        public void MoveTwoardsTarget(Vector3 target)
        {
            Position = SteeringBehavior.Seek(Position, target, out _radians);
        }

        public void Draw(BasicEffect basicEffect)
        {
            Graphics.SetVertexBuffer(VertexBuffer);

            basicEffect.World = LocalWorld * Matrix.CreateRotationZ((float)_radians) * Matrix.CreateTranslation(Position);

            foreach (EffectPass pass in basicEffect.CurrentTechnique.Passes)
            {
                pass.Apply();
                Graphics.DrawPrimitives(PrimitiveType.TriangleList, 0, 1);
            }

            //basicEffect.World = -basicEffect.World;
        }
    }
}
