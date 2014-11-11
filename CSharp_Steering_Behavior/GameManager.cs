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
        private Triangle _playerTriangle;

        public GameManager(GraphicsDeviceManager graphicsDevice, GraphicsDevice graphics)
        {
            _graphicsDevice = graphicsDevice;
            this._graphics = graphics;

            this.Initialize();
        }

        public void Initialize()
        {
            _playerTriangle = new Triangle(new Vector3(300, 200, 0));
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
            _playerTriangle.MoveTwoardsTarget(new Vector3(mouse.X, mouse.Y, 0));
        }

        public void Update(GameTime gameTime)
        {
            UpdateKeyboardInput();
            UpdateMouseInput();

            _playerTriangle.Update(gameTime);
        }

        public void Draw(SpriteBatch spriteBatch, PrimitiveBatch primitiveBatch)
        {
            // Draw Triangles
            primitiveBatch.Begin(PrimitiveType.TriangleList);
            _playerTriangle.Draw(primitiveBatch);
            primitiveBatch.End();

            // Draw lines
            primitiveBatch.Begin(PrimitiveType.LineList);
            _playerTriangle.DrawForces(primitiveBatch);
            primitiveBatch.End();
        }
    }
}
