using CSharp_Steering_Behavior.Primitives;
using CSharp_Steering_Behavior.Units;
using Lillheaton.Monogame.Steering;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using System;
using System.Linq;

namespace CSharp_Steering_Behavior
{
    public class GameManager
    {
        private GraphicsDevice _graphics;
        private GraphicsDeviceManager _graphicsDevice;
        private IObstacle[] _obstacles;
        private DrawableRectangle _drawableRectangle;

        private Leader _leader;
        private Follower[] _followers;

        public GameManager(GraphicsDeviceManager graphicsDevice, GraphicsDevice graphics)
        {
            _graphicsDevice = graphicsDevice;
            this._graphics = graphics;

            this.Initialize();
        }

        public void Initialize()
        {
            // Generate obsticals
            _drawableRectangle = new DrawableRectangle(_graphics, new Rectangle(300, 200, 50, 50));

            _obstacles = new IObstacle[3];
            _obstacles[0] = new CircleObstacle(new Vector3(100, 100, 0), 20);
            _obstacles[1] = new CircleObstacle(new Vector3(200, 200, 0), 50);
            _obstacles[2] = _drawableRectangle;

            // Create units
            var random = new Random(DateTime.Now.Millisecond);
            
            // Create the leader
            _leader =
                new Leader(
                    new Vector3(
                        random.Next(0, _graphicsDevice.PreferredBackBufferWidth),
                        random.Next(0, _graphicsDevice.PreferredBackBufferHeight),
                        0));

            _followers = new Follower[4];

            for (int i = 0; i < _followers.Length; i++)
            {
                this._followers[i] =
                    new Follower(
                        new Vector3(
                            random.Next(0, _graphicsDevice.PreferredBackBufferWidth),
                            random.Next(0, _graphicsDevice.PreferredBackBufferHeight),
                            0));

                this._followers[i].Leader = _leader;
            }
        }

        public void UpdateMouseInput()
        {
            var mouse = Mouse.GetState();
            var mosueVector = new Vector3(mouse.X, mouse.Y, 0);

            _leader.MoveTwoardsTarget(mosueVector);
        }

        public void Update(GameTime gameTime)
        {
            UpdateMouseInput();

            _leader.Update(gameTime);
            foreach (var unit in _followers)
            {
                unit.Update(gameTime);
            }
        }

        public void Draw(SpriteBatch spriteBatch, PrimitiveBatch primitiveBatch)
        {
            // Draw Rectangle
            spriteBatch.Begin();
            _drawableRectangle.Draw(spriteBatch);
            spriteBatch.End();

            // Draw Units
            _leader.Draw(primitiveBatch);
            foreach (var unit in _followers)
            {
                unit.Draw(primitiveBatch);
            }
            
            // Draw circles
            primitiveBatch.Begin(PrimitiveType.LineStrip);
            foreach (var obstacle in _obstacles.OfType<IDrawableObstacle>())
            {
                obstacle.Draw(primitiveBatch);
            }
            primitiveBatch.End();
        }
    }
}
