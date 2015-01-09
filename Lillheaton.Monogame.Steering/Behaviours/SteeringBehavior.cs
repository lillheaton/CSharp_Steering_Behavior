using System.Linq;

using Lillheaton.Monogame.Steering.Extensions;
using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;

namespace Lillheaton.Monogame.Steering.Behaviours
{
    public partial class SteeringBehavior
    {
        public IBoid Host { get; private set; }
        public Vector3 Steering { get; private set; }
        public Vector3 DesiredVelocity { get; private set; }
        public float Angle { get; private set; }       

        private Random random;
        private const int MaxSeeAhead = 50;
        private const float MaxForce = 5.4f;
        private const int SlowingRadius = 100;
        private const int CircleDistance = 6;
        private const int CircleRadius = 8;
        private const int AngleChange = 1;
        private const float MaxAvoidanceForce = 100;

        private const int SeparationRadius = 30;
        private const float SeparationForce = 2.0f;

        public SteeringBehavior(IBoid host)
        {
            this.Host = host;
            this.Init();
        }

        private void Init()
        {
            this.Angle = 0;
            this.random = new Random(DateTime.Now.Millisecond);
            this.Host.Velocity = this.Host.Velocity.Truncate(this.Host.GetMaxVelocity());
        }

        public void Update(GameTime gameTime)
        {
            this.Steering = this.Steering.Truncate(MaxForce);
            this.Steering = this.Steering.ScaleBy((float)1 / this.Host.GetMass());

            this.Host.Velocity = Vector3.Add(this.Host.Velocity, this.Steering);
            this.Host.Velocity = this.Host.Velocity.Truncate(this.Host.GetMaxVelocity());

            this.Angle = this.GetAngle(this.Host.Velocity);

            this.Host.Position = Vector3.Add(this.Host.Position, this.Host.Velocity);
        }

        private Vector3 Separation(List<IBoid> worldBoids)
        {
            var force = new Vector3();
            var neighbors = 0;

            foreach (var boid in worldBoids.Except(new[] { this.Host }))
            {
                if (Vector3.Distance(boid.Position, this.Host.Position) <= SeparationRadius)
                {
                    force.X += boid.Position.X - this.Host.Position.X;
                    force.Y += boid.Position.Y - this.Host.Position.Y;
                    neighbors++;
                }
            }

            if (neighbors > 0)
            {
                force.X /= neighbors;
                force.Y /= neighbors;

                force = force.ScaleBy(-1);
                force = Vector3.Normalize(force);
                force = force.ScaleBy(SeparationForce);
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
