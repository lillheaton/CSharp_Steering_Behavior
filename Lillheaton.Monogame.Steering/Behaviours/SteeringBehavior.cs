using Lillheaton.Monogame.Steering.Extensions;
using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Lillheaton.Monogame.Steering.Behaviours
{
    public partial class SteeringBehavior
    {
        // Public
        public IBoid Host { get; private set; }
        public Vector3 Steering { get; private set; }
        public Vector3 DesiredVelocity { get; private set; }
        public float Angle { get; private set; }
        public Settings Settings { get; private set; }

        // Private
        private Random random;

        public SteeringBehavior(IBoid host, Settings settings = null)
        {
            this.Host = host;
            this.Settings = settings;
            this.Init();            
        }

        private void Init()
        {
            this.Angle = 0;
            this.random = new Random(DateTime.Now.Millisecond);
            this.Host.Velocity = this.Host.Velocity.Truncate(this.Host.GetMaxVelocity());
            if (this.Settings == null)
            {
                this.Settings = new Settings();
            }
        }

        public void Update(GameTime gameTime)
        {
            this.Steering = this.Steering.Truncate(Settings.MaxForce);
            this.Steering = this.Steering.ScaleBy((float)1 / this.Host.GetMass());

            this.Host.Velocity = Vector3.Add(this.Host.Velocity, this.Steering);
            this.Host.Velocity = this.Host.Velocity.Truncate(this.Host.GetMaxVelocity());

            this.Angle = this.GetAngle(this.Host.Velocity);

            this.Host.Position = Vector3.Add(this.Host.Position, this.Host.Velocity);
        }

        public void Stop()
        {
            this.ResetPath();

        }

        private Vector3 Separation(IBoid[] worldBoids)
        {
            var force = new Vector3();
            var neighbors = 0;

            for (int i = 0; i < worldBoids.Length; i++)
            {
                if(worldBoids[i] == this.Host)
                    continue;

                if (Vector3.Distance(worldBoids[i].Position, this.Host.Position) <= Settings.SeparationRadius)
                {
                    force.X += worldBoids[i].Position.X - this.Host.Position.X;
                    force.Y += worldBoids[i].Position.Y - this.Host.Position.Y;
                    neighbors++;
                }
            }

            if (neighbors > 0)
            {
                force.X /= neighbors;
                force.Y /= neighbors;

                force = force.ScaleBy(-1);
                force = Vector3.Normalize(force);
                force = force.ScaleBy(Settings.SeparationForce);
            }

            return force;
        }

        private Vector3 GetFuturePositionOfTarget(IBoid targetBoid)
        {
            var distance = targetBoid.Position - this.Host.Position;

            // T = updated ahead
            var T = distance.Length() / this.Host.GetMaxVelocity();

            return targetBoid.Position + targetBoid.Velocity * T;
        }

        private float GetAngle(Vector3 vector)
        {
            return (float)(-Math.Atan2(vector.X, vector.Y) + Math.PI);
        }

        private void SetAngle(ref Vector3 vector, float angle)
        {
            var len = vector.Length();
            vector.X = (float)Math.Cos(angle) * len;
            vector.Y = (float)Math.Sin(angle) * len;
        }
    }
}
