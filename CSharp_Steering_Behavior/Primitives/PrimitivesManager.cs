using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System.Collections.Generic;

namespace CSharp_Steering_Behavior.Primitives
{
    public class PrimitivesManager
    {
        public GraphicsDevice Graphics { get; set; }
        public BasicEffect BasicEffect { get; set; }
        public List<Triangle> Triangles { get; set; }

        public PrimitivesManager(GraphicsDevice graphics)
        {
            this.Graphics = graphics;

            Vector2 center;
            center.X = graphics.Viewport.Width * 0.5f;
            center.Y = graphics.Viewport.Height * 0.5f;

            BasicEffect = new BasicEffect(graphics);
            BasicEffect.World = Matrix.Identity;
            BasicEffect.View = Matrix.CreateLookAt(new Vector3(center, 0), new Vector3(center, 1), Vector3.Down);
            BasicEffect.Projection = Matrix.CreateOrthographic(center.X * 2, center.Y * 2, -0.5f, 1);
            //BasicEffect.Projection = Matrix.CreateOrthographicOffCenter(0, Graphics.Viewport.Width, Graphics.Viewport.Height, 0, 0, 1) * Matrix.CreateTranslation(-0.5f, -0.5f, 0);

            //BasicEffect.View = Matrix.Identity;
            //BasicEffect.Projection = Matrix.Identity;

            BasicEffect.VertexColorEnabled = true;

            var rasterizerState = new RasterizerState();
            rasterizerState.CullMode = CullMode.None;

            graphics.RasterizerState = rasterizerState;
            Triangles = new List<Triangle>();
        }

        public Triangle AddTriangle(Vector3 top, Vector3 right, Vector3 left)
        {
            var triangle = new Triangle(this.Graphics, top, right, left);
            Triangles.Add(triangle);

            return triangle;
        }

        public void Draw(SpriteBatch spriteBatch)
        {
            foreach (var triangle in Triangles)
            {
                triangle.Draw(BasicEffect);
            }
        }
    }
}