using System;

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
            this._primitivesManager = new PrimitivesManager(this._graphics);

            this.Initialize();
        }

        public void Initialize()
        {
            _playerTriangle = _primitivesManager.AddTriangle(
                new Vector3(100, 80, 0),
                new Vector3(110, 120, 0),
                new Vector3(90, 120, 0));
        }

        public void UpdateKeyboardInput()
        {
            KeyboardState keyState = Keyboard.GetState();
            GamePadState gamePad = GamePad.GetState(PlayerIndex.One);
            float epsilon = 0.1f;

            if (keyState.IsKeyDown(Keys.Left) || gamePad.ThumbSticks.Left.X < -epsilon)
            {
                //_playerTriangle.Rotate(_playerTriangle.CurrentAngle + 2);
            }

            if (keyState.IsKeyDown(Keys.Right) || gamePad.ThumbSticks.Left.X > epsilon)
            {
                //_playerTriangle.Rotate(_playerTriangle.CurrentAngle - 2);
                //_playerTriangle.Move(Direction.East);
            }

            if (keyState.IsKeyDown(Keys.Down) || gamePad.ThumbSticks.Left.Y < -epsilon)
            {
                //_playerTriangle.Move(Direction.South);
            }

            if (keyState.IsKeyDown(Keys.Up) || gamePad.ThumbSticks.Left.Y > epsilon)
            {
                //_playerTriangle.Move();
            }
        }

        public void UpdateMouseInput()
        {
            var mouse = Mouse.GetState();

            //var ray = this.CalculateRay(
            //    new Vector2(mouse.X, mouse.Y),
            //    _primitivesManager.BasicEffect.View,
            //    _primitivesManager.BasicEffect.Projection,
            //    _graphics.Viewport);

            ////ray.
            _playerTriangle.MoveTwoardsTarget(new Vector3(mouse.X, mouse.Y, 0));
            //Console.WriteLine(new Vector2(mouse.X, mouse.Y));
        }

        public Ray CalculateRay(Vector2 point, Matrix view, Matrix projection, Viewport viewport)
        {
            Vector3 nearPoint = viewport.Unproject(
                new Vector3(point.X, point.Y, 0.0f),
                projection,
                view,
                Matrix.Identity);

            Vector3 farPoint = viewport.Unproject(
                new Vector3(point.X, point.Y, 1.0f),
                projection,
                view,
                Matrix.Identity);

            Vector3 direction = farPoint - nearPoint;
            direction.Normalize();

            return new Ray(nearPoint, direction);
        }

        public void Update(GameTime gameTime)
        {
            UpdateKeyboardInput();
            UpdateMouseInput();
        }

        public void Draw(SpriteBatch spriteBatch)
        {
            this._primitivesManager.Draw(spriteBatch);
        }
    }
}
