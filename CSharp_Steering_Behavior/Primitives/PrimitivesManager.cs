using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System.Collections.Generic;

namespace CSharp_Steering_Behavior.Primitives
{
    public class PrimitivesManager
    {
        public GraphicsDevice graphics { get; set; }
        public BasicEffect BasicEffect { get; set; }
        public List<Triangle> Triangles { get; set; }

        public PrimitivesManager(GraphicsDevice graphics, float aspectRatio)
        {
            this.graphics = graphics;

            BasicEffect = new BasicEffect(graphics);
            BasicEffect.World = Matrix.Identity;
            BasicEffect.View = Matrix.CreateLookAt(new Vector3(0, 0, 5), new Vector3(0, 0, 0), Vector3.Up);
            BasicEffect.Projection = Matrix.CreatePerspectiveFieldOfView(MathHelper.ToRadians(45), aspectRatio, 0.01f, 100f);
            BasicEffect.VertexColorEnabled = true;

            var rasterizerState = new RasterizerState();
            rasterizerState.CullMode = CullMode.None;

            graphics.RasterizerState = rasterizerState;
            Triangles = new List<Triangle>();
        }

        public Triangle AddTriangle(Vector3 top, Vector3 right, Vector3 left)
        {
            var triangle = new Triangle(graphics, top, right, left);
            Triangles.Add(triangle);

            return triangle;
        }

        public void Draw()
        {
            foreach (var triangle in Triangles)
            {
                triangle.Draw(BasicEffect);
            }
        }
    }
}