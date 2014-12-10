using Lillheaton.Monogame.Steering;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace CSharp_Steering_Behavior.Primitives
{
    public class DrawableRectangle : IRectangleObstacle
    {
        public Rectangle Rectangle{ get; private set; }
        public Vector3 Center { get; set; }

        private Texture2D _texture;
        private Color[] _colorData;

        public DrawableRectangle(GraphicsDevice graphicsDevice, Rectangle rectangle)
        {
            this.Rectangle = rectangle;
            this.Init(graphicsDevice);
        }

        private void Init(GraphicsDevice graphicsDevice)
        {
            var centerX = (Rectangle.X + Rectangle.Width) / 2;
            var centerY = (Rectangle.Y + Rectangle.Height) / 2;
            this.Center = new Vector3(centerX, centerY, 0);

            _texture = new Texture2D(graphicsDevice, Rectangle.Width, Rectangle.Height);
            _colorData = new Color[Rectangle.Width * Rectangle.Height];
            for (int i = 0; i < _colorData.Length; i++)
            {
                _colorData[i] = Color.Black;
            }
            _texture.SetData(_colorData);
        }

        public void Draw(SpriteBatch spriteBatch)
        {
            spriteBatch.Draw(_texture, Rectangle, Color.Black);
        }
    }
}