using CSharp_Steering_Behavior.Primitives;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

namespace CSharp_Steering_Behavior
{
    public class GameManager
    {
        private GraphicsDevice _graphics;
        private GraphicsDeviceManager _graphicsDevice;
        private PrimitivesManager _primitivesManager;

        private Triangle _playerTriangle;

        public GameManager(GraphicsDeviceManager graphicsDevice, GraphicsDevice graphics)
        {
            _graphicsDevice = graphicsDevice;
            this._graphics = graphics;
            this._primitivesManager = new PrimitivesManager(
                this._graphics,
                this._graphicsDevice.PreferredBackBufferWidth / this._graphicsDevice.PreferredBackBufferHeight);

            this.Initialize();
        }

        public void Initialize()
        {
            _playerTriangle = _primitivesManager.AddTriangle(
                new Vector3(0, 1, 0),
                new Vector3(+0.2f, 0, 0),
                new Vector3(-0.2f, 0, 0));
        }

        public void UpdateKeyboardInput()
        {
            KeyboardState keyState = Keyboard.GetState();
            GamePadState gamePad = GamePad.GetState(PlayerIndex.One);
            float epsilon = 0.1f;

            if (keyState.IsKeyDown(Keys.Left) || gamePad.ThumbSticks.Left.X < -epsilon)
            {
                _playerTriangle.Rotate(_playerTriangle.CurrentRadians + 1);
            }

            if (keyState.IsKeyDown(Keys.Right) || gamePad.ThumbSticks.Left.X > epsilon)
            {
                _playerTriangle.Rotate(_playerTriangle.CurrentRadians - 1);
            }

            if (keyState.IsKeyDown(Keys.Down) || gamePad.ThumbSticks.Left.Y < -epsilon)
            {
                // Move it backwards
            }

            if (keyState.IsKeyDown(Keys.Up) || gamePad.ThumbSticks.Left.Y > epsilon)
            {
                _playerTriangle.MoveForward();
            }
        }

        public void Update(GameTime gameTime)
        {
            UpdateKeyboardInput();
        }

        public void Draw(GameTime gameTime)
        {
            this._primitivesManager.Draw();
        }
    }
}
