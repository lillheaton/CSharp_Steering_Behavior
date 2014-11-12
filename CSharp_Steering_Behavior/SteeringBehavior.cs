using CSharp_Steering_Behavior.Extensions;
using CSharp_Steering_Behavior.Utilities;

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

        // Wander
        public Vector3 CircleCenter { get; private set; }

        private Random random;
        private float _wanderAngle = 0;

        private const int MaxVelocity = 3;
        private const float MaxForce = 5.4f;
        private const int SlowingRadius = 100;
        private const int CircleDistance = 6;
        private const int CircleRadius = 8;
        private const int AngleChange = 1;

        // This number should be recived
        private const int Mass = 20;

        public SteeringBehavior()
        {
            Angle = 0;
            Velocity = new Vector3(-1, -2, 0);

            Velocity = Velocity.Truncate(MaxVelocity);

            random = new Random();
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

        public Vector3 Wander(Vector3 position)
        {
            CircleCenter = Vector3.Normalize(Velocity).ScaleBy(CircleDistance);

            var displacement = new Vector3(0, -1, 0).ScaleBy(CircleRadius);

            this.SetAngle(ref displacement, _wanderAngle);
            _wanderAngle += (float)random.NextDouble() * AngleChange - AngleChange * 0.5f;

            Steering = CircleCenter + displacement;

            return this.AddingForces(position);
        }

        private Vector3 AddingForces(Vector3 position)
        {
            Steering.Truncate(MaxForce);
            Steering = Steering.ScaleBy((float)1 / Mass);

            Velocity = Velocity + Steering;
            Velocity = Velocity.Truncate(MaxVelocity);

            Angle = this.GetAngle(Velocity);

            return position + Velocity;
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
