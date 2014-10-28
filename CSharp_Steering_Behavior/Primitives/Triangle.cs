
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace CSharp_Steering_Behavior.Primitives
{
    public class Triangle
    {
        private GraphicsDevice Graphics { get; set; }
        private BasicEffect BasicEffect { get;set; }
        private VertexBuffer VertexBuffer { get; set; }
        private RasterizerState RasterizerState { get; set; }

        private float radians = 0;

        public Triangle(GraphicsDevice graphics)
        {
            this.Graphics = graphics;

            var world = Matrix.CreateTranslation(0, 0, 0);
            var view = Matrix.CreateLookAt(new Vector3(0, 0, 3), new Vector3(0, 0, 0), Vector3.Up);
            var projection = Matrix.CreatePerspectiveFieldOfView(MathHelper.ToRadians(45), 800f / 480f, 0.01f, 100f);

            var vertices = new VertexPositionColor[3];
            vertices[0] = new VertexPositionColor(new Vector3(0, 1, 0), Color.Red);
            vertices[1] = new VertexPositionColor(new Vector3(+0.5f, 0, 0), Color.Green);
            vertices[2] = new VertexPositionColor(new Vector3(-0.5f, 0, 0), Color.Blue);

            VertexBuffer = new VertexBuffer(graphics, typeof(VertexPositionColor), 3, BufferUsage.WriteOnly);
            VertexBuffer.SetData<VertexPositionColor>(vertices);

            BasicEffect = new BasicEffect(graphics);
            BasicEffect.World = world;
            BasicEffect.View = view;
            BasicEffect.Projection = projection;
            BasicEffect.VertexColorEnabled = true;

            RasterizerState = new RasterizerState();
            RasterizerState.CullMode = CullMode.None;
        }

        public void Draw()
        {
            Graphics.SetVertexBuffer(VertexBuffer);
            Graphics.RasterizerState = RasterizerState;

            radians++;
            if (radians > 360)
            {
                radians = 0;
            }
            BasicEffect.World = Matrix.CreateTranslation(0, -0.4f, 0) * Matrix.CreateFromAxisAngle(new Vector3(0, 0, 1), MathHelper.ToRadians(radians));

            foreach (EffectPass pass in BasicEffect.CurrentTechnique.Passes)
            {
                pass.Apply();
                Graphics.DrawPrimitives(PrimitiveType.TriangleList, 0, 1);
            }
        }
    }
}
