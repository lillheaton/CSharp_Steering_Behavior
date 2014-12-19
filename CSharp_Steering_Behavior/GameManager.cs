using CSharp_Steering_Behavior.Primitives;
using CSharp_Steering_Behavior.Units;
using Lillheaton.Monogame.Steering;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using System;
using System.Collections.Generic;
using System.Linq;

namespace CSharp_Steering_Behavior
{
    public class GameManager
    {
        private GraphicsDevice _graphics;
        private GraphicsDeviceManager _graphicsDevice;
        private IObstacle[] _obstacles;

        private DrawableRectangle _leftRectangle;
        private DrawableRectangle _rightRectangle;

        private List<IBoid> _boids;
        private Random _random;

        public GameManager(GraphicsDeviceManager graphicsDevice, GraphicsDevice graphics)
        {
            _graphicsDevice = graphicsDevice;
            this._graphics = graphics;

            this.Initialize();
        }

        public void Initialize()
        {
            _boids = new List<IBoid>();

            // Generate obsticals
            this._leftRectangle = new DrawableRectangle(_graphics, new Rectangle(0, 20, 350, 70));
            this._rightRectangle = new DrawableRectangle(_graphics, new Rectangle(_graphicsDevice.PreferredBackBufferWidth - 350, 20, 500, 70));

            _obstacles = new IObstacle[2];
            _obstacles[0] = this._leftRectangle;
            _obstacles[1] = this._rightRectangle;

            // Create units
            _random = new Random(DateTime.Now.Millisecond);

            for (int i = 0; i < 50; i++)
            {
                var seeker =
                    new Seeker(
                        new Vector3(
                            _random.Next(-40, _graphicsDevice.PreferredBackBufferWidth + 40),
                            (float)(this._graphicsDevice.PreferredBackBufferHeight + _random.NextDouble() * 200),
                            0));

                seeker.Target = new Vector3((float)_graphicsDevice.PreferredBackBufferWidth / 2, -10, 0);
                seeker.Obstacles = _obstacles;
                _boids.Add(seeker);
            }

            foreach (var boid in _boids.OfType<BaseUnit>())
            {
                boid.WorldBoids = _boids;
            }
        }

        public void UpdateMouseInput()
        {
            var mouse = Mouse.GetState();
            var mosueVector = new Vector3(mouse.X, mouse.Y, 0);

            //_leader.MoveTwoardsTarget(mosueVector);
        }

        public void Update(GameTime gameTime)
        {
            UpdateMouseInput();

            foreach (var unit in _boids)
            {
                unit.Update(gameTime);

                if (unit.Position.Y <= -5)
                {
                    unit.Position =
                        new Vector3(
                            (float)(this._graphicsDevice.PreferredBackBufferWidth * this._random.NextDouble()),
                            _graphicsDevice.PreferredBackBufferHeight * 1.2f,
                            0);
                }
            }
        }

        public void Draw(SpriteBatch spriteBatch, PrimitiveBatch primitiveBatch)
        {
            // Draw Rectangle
            spriteBatch.Begin();
            this._leftRectangle.Draw(spriteBatch);
            this._rightRectangle.Draw(spriteBatch);
            spriteBatch.End();

            // Draw Units
            foreach (var unit in _boids.OfType<Seeker>())
            {
                unit.Draw(primitiveBatch);
            }
            
            // Draw circles
            //primitiveBatch.Begin(PrimitiveType.LineStrip);
            //foreach (var obstacle in _obstacles.OfType<IDrawableObstacle>())
            //{
            //    obstacle.Draw(primitiveBatch);
            //}
            //primitiveBatch.End();
        }
    }
}
