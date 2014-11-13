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
        private Triangle[] _playerTriangles;

        public GameManager(GraphicsDeviceManager graphicsDevice, GraphicsDevice graphics)
        {
            _graphicsDevice = graphicsDevice;
            this._graphics = graphics;

            this.Initialize();
        }

        public void Initialize()
        {
            const int NumberOfTriangles = 5;

            _playerTriangles = new Triangle[NumberOfTriangles];
            var random = new Random();

            for (int i = 0; i < NumberOfTriangles; i++)
            {
                this._playerTriangles[i] =
                    new Triangle(
                        new Vector3(
                            random.Next(0, _graphicsDevice.PreferredBackBufferWidth),
                            random.Next(0, _graphicsDevice.PreferredBackBufferHeight),
                            0));
            }
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
            var mosueVector = new Vector3(mouse.X, mouse.Y, 0);

            //foreach (var triangle in _playerTriangles)
            //{
            //    triangle.MoveTwoardsTarget(mosueVector, triangle.Steering.Wander);    
            //}

            _playerTriangles[0].MoveTwoardsTarget(mosueVector);

            for (int i = 1; i < 5; i++)
            {
                _playerTriangles[i].MoveTwoardsTarget(mosueVector);
                _playerTriangles[i].Evade(_playerTriangles[0]);
            }
            
        }

        public void Update(GameTime gameTime)
        {
            UpdateKeyboardInput();
            UpdateMouseInput();

            foreach (var triangle in _playerTriangles)
            {
                triangle.Update(gameTime);
            }
        }

        public void Draw(SpriteBatch spriteBatch, PrimitiveBatch primitiveBatch)
        {
            // Draw Triangles
            primitiveBatch.Begin(PrimitiveType.TriangleList);
            foreach (var triangle in _playerTriangles)
            {
                triangle.Draw(primitiveBatch);
            }
            primitiveBatch.End();

            // Draw lines
            primitiveBatch.Begin(PrimitiveType.LineList);
            foreach (var triangle in _playerTriangles)
            {
                triangle.DrawForces(primitiveBatch);
            }
            primitiveBatch.End();
        }
    }
}
