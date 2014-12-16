using Lillheaton.Monogame.Steering;
using Lillheaton.Monogame.Steering.Behaviours;
using Lillheaton.Monogame.Steering.Extensions;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace CSharp_Steering_Behavior.Units
{
    public abstract class BaseUnit : IBoid
    {
        public SteeringBehavior SteeringBehavior { get; private set; }
        public Vector3 Position { get; set; }
        public Vector3 Velocity { get; set; }
        public IObstacle[] Obstacles { get; set; }

        private VertexPositionColor[] _forces;

        protected BaseUnit(Vector3 position)
        {
            this.Position = position;
            this.Init();
        }

        private void Init()
        {
            SteeringBehavior = new SteeringBehavior(this);
            Obstacles = new IObstacle[0];

            Velocity = new Vector3(-1, -2, 0);
            Velocity = Velocity.Truncate(this.GetMaxVelocity());

            _forces = new VertexPositionColor[6];
            for (int i = 0; i < _forces.Length; i++)
            {
                _forces[i] = new VertexPositionColor(Position, Color.White);
            }
        }

        public virtual void Update(GameTime gameTime)
        {
            SteeringBehavior.Update(gameTime);
        }

        public virtual void Draw(PrimitiveBatch primitiveBatch)
        {
        }

        public virtual float GetMaxVelocity()
        {
            return 3f;
        }

        public virtual float GetMass()
        {
            return 20;
        }

        protected void DrawForces(PrimitiveBatch primitiveBatch)
        {
            const int Scale = 100;
            var velocityForce = Vector3.Normalize(Velocity);
            var steeringForce = Vector3.Normalize(this.SteeringBehavior.Steering);
            var desiredVelocityForce = Vector3.Normalize(this.SteeringBehavior.DesiredVelocity);

            _forces[0].Color = Color.Green;
            _forces[0].Position = Position;
            _forces[1].Color = Color.Green;
            _forces[1].Position = this.Position + velocityForce * Scale;

            _forces[2].Color = Color.Gray;
            _forces[2].Position = Position;
            _forces[3].Color = Color.Gray;
            _forces[3].Position = this.Position + desiredVelocityForce * Scale;

            _forces[4].Color = Color.Red;
            _forces[4].Position = Position;
            _forces[5].Color = Color.Red;
            _forces[5].Position = this.Position + steeringForce * Scale;

            // Draw forces
            primitiveBatch.AddVertices(_forces);
        }
    }
}