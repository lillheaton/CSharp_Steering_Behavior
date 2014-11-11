using CSharp_Steering_Behavior.Extensions;
using Microsoft.Xna.Framework;
using System;

namespace CSharp_Steering_Behavior
{
    public class SteeringBehavior
    {
        public float Angle { get; private set; }
        public Vector3 Velocity { get; private set; }
        public Vector3 DesiredVelocity { get; private set; }
        public Vector3 Steering { get; private set; }

        private const int MaxVelocity = 3;
        private const float MaxForce = 0.6f;
        private const int SlowingRadius = 100;

        // This number should be recived
        private const int Mass = 20;

        public SteeringBehavior()
        {
            Angle = 0;
            Velocity = new Vector3(-1, -2, 0);
            Velocity.Truncate(MaxVelocity);
        }

        public Vector3 Seek(Vector3 position, Vector3 target)
        {
            DesiredVelocity = target - position;

            float distance = DesiredVelocity.Length();

            // Inside slowing radius
            if (distance <= SlowingRadius)
            {
                DesiredVelocity = Vector3.Normalize(DesiredVelocity) * MaxVelocity * (distance / SlowingRadius);
            }
            else
            {
                // Far away
                DesiredVelocity = Vector3.Normalize(DesiredVelocity) * MaxVelocity;
            }

            Steering = DesiredVelocity - Velocity;
            return this.AddingForces(position);
        }

        public Vector3 Flee(Vector3 position, Vector3 target)
        {
            DesiredVelocity = Vector3.Normalize(position - target) * MaxVelocity;
            Steering = DesiredVelocity - Velocity;

            return this.AddingForces(position);
        }

        private Vector3 AddingForces(Vector3 position)
        {
            Steering.Truncate(MaxForce);
            Steering.ScaleBy((float)1 / Mass);

            Velocity = Velocity + Steering;
            Velocity.Truncate(MaxVelocity);

            Angle = (float)(-Math.Atan2(Velocity.X, Velocity.Y) + Math.PI);

            return position + Velocity;
        }
    }
}
