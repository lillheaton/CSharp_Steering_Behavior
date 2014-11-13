using CSharp_Steering_Behavior.Extensions;
using CSharp_Steering_Behavior.Primitives;

using Microsoft.Xna.Framework;
using System;

namespace CSharp_Steering_Behavior
{
    public class SteeringBehavior
    {
        public float Angle { get; private set; }
        public Vector3 Position { get; set; }
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

        public SteeringBehavior(Vector3 position)
        {
            Position = position;
            Angle = 0;
            random = new Random(DateTime.Now.Millisecond);

            Velocity = new Vector3(-1, -2, 0);
            Velocity = Velocity.Truncate(MaxVelocity);
        }

        public void Seek(Vector3 target)
        {
            DesiredVelocity = target - Position;

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
            this.AddingForces();
        }

        public void Flee(Vector3 target)
        {
            DesiredVelocity = Vector3.Normalize(Position - target) * MaxVelocity;
            Steering = DesiredVelocity - Velocity;

            this.AddingForces();
        }

        public void Wander()
        {
            CircleCenter = Vector3.Normalize(Velocity).ScaleBy(CircleDistance);

            var displacement = new Vector3(0, -1, 0).ScaleBy(CircleRadius);

            this.SetAngle(ref displacement, _wanderAngle);
            _wanderAngle += (float)random.NextDouble() * AngleChange - AngleChange * 0.5f;

            Steering = CircleCenter + displacement;

            this.AddingForces();
        }

        public void Pursuit(Triangle triangle)
        {
            var distance = triangle.Steering.Position - this.Position;
            var T = distance.Length() / MaxVelocity;

            var futurePosition = triangle.Steering.Position + triangle.Steering.Velocity * T;
            this.Seek(futurePosition);
        }

        private void AddingForces()
        {
            Steering.Truncate(MaxForce);
            Steering = Steering.ScaleBy((float)1 / Mass);

            Velocity = Velocity + Steering;
            Velocity = Velocity.Truncate(MaxVelocity);

            Angle = this.GetAngle(Velocity);

            Position = Position + Velocity;            
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
