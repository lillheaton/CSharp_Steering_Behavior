using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace CSharp_Steering_Behavior.Primitives
{
    public class Triangle
    {
        private GraphicsDevice Graphics { get; set; }
        private VertexBuffer VertexBuffer { get; set; }
        private Matrix World { get; set; }
        public float CurrentRadians { get; set; }

        private float _centerX;
        private float _centerY;
        private Vector3 _position;

        public Triangle(GraphicsDevice graphics, Vector3 top, Vector3 right, Vector3 left)
        {
            this.Graphics = graphics;

            _centerX = -((top.X + right.X + left.X) / 3);
            _centerY = -((top.Y + right.Y + left.Y) / 3);
            _position = new Vector3(_centerX, _centerY, 0);

            World = Matrix.CreateTranslation(_position);
            var vertices = new VertexPositionColor[3];
            vertices[0] = new VertexPositionColor(top, Color.Red);
            vertices[1] = new VertexPositionColor(right, Color.Red);
            vertices[2] = new VertexPositionColor(left, Color.Red);

            VertexBuffer = new VertexBuffer(graphics, typeof(VertexPositionColor), 3, BufferUsage.WriteOnly);
            VertexBuffer.SetData<VertexPositionColor>(vertices);

            this.CurrentRadians = 0;
        }

        public void Rotate(float radians)
        {
            CurrentRadians = radians;
            World = Matrix.CreateTranslation(_position) * Matrix.CreateFromAxisAngle(Vector3.UnitZ, MathHelper.ToRadians(CurrentRadians));
        }

        public void MoveForward()
        {            
            _position = _position + new Vector3(0, 0.01f, 0);
            World = Matrix.CreateTranslation(_position) * Matrix.CreateFromAxisAngle(Vector3.UnitZ, MathHelper.ToRadians(CurrentRadians));
        }

        public void Draw(BasicEffect basicEffect)
        {
            Graphics.SetVertexBuffer(VertexBuffer);
            basicEffect.World = World;

            foreach (EffectPass pass in basicEffect.CurrentTechnique.Passes)
            {
                pass.Apply();
                Graphics.DrawPrimitives(PrimitiveType.TriangleList, 0, 1);
            }

            basicEffect.World = Matrix.Identity;
        }
    }
}
