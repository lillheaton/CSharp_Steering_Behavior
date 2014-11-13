using CSharp_Steering_Behavior.Extensions;
using Microsoft.Xna.Framework;
using System;

namespace CSharp_Steering_Behavior
{
    public class SteeringBehavior
    {
        public IBoid Host { get; private set; }
        public Vector3 Steering { get; private set; }
        public float Angle { get; private set; }

        public Vector3 DesiredVelocity { get; private set; }

        private Random random;
        private float _wanderAngle = 0;

        private const float MaxForce = 5.4f;
        private const int SlowingRadius = 100;
        private const int CircleDistance = 6;
        private const int CircleRadius = 8;
        private const int AngleChange = 1;

        public SteeringBehavior(IBoid host)
        {
            this.Host = host;
            this.Init();
        }

        private void Init()
        {
            Angle = 0;
            random = new Random(DateTime.Now.Millisecond);
            Host.Velocity = Host.Velocity.Truncate(Host.GetMaxVelocity());
        }

        public void Seek(Vector3 target)
        {
            Steering = Vector3.Add(Steering, this.DoSeek(target));
        }

        public void Flee(Vector3 target)
        {
            Steering = Vector3.Add(Steering, this.DoFlee(target));
        }

        public void Wander()
        {
            Steering = Vector3.Add(Steering, this.DoWander());
        }

        public void Pursuit(IBoid targetBoid)
        {
            this.Seek(this.GetFuturePositionOfTarget(targetBoid));
        }

        public void Evade(IBoid targetBoid)
        {
            this.Flee(this.GetFuturePositionOfTarget(targetBoid));
        }



        public void Update(GameTime gameTime)
        {
            Steering.Truncate(MaxForce);
            Steering = Steering.ScaleBy((float)1 / Host.GetMass());

            Host.Velocity = Host.Velocity + Steering;
            Host.Velocity = Host.Velocity.Truncate(Host.GetMaxVelocity());

            Angle = this.GetAngle(Host.Velocity);

            Host.Position = Host.Position + Host.Velocity;
        }




        private Vector3 DoSeek(Vector3 target)
        {
            DesiredVelocity = target - Host.Position;

            float distance = DesiredVelocity.Length();

            // Inside slowing radius
            if (distance <= SlowingRadius)
            {
                DesiredVelocity = Vector3.Normalize(DesiredVelocity) * Host.GetMaxVelocity() * (distance / SlowingRadius);
            }
            else
            {
                // Far away
                DesiredVelocity = Vector3.Normalize(DesiredVelocity) * Host.GetMaxVelocity();
            }

            return DesiredVelocity - Host.Velocity;
        }

        private Vector3 DoFlee(Vector3 target)
        {
            DesiredVelocity = Vector3.Normalize(Host.Position - target) * Host.GetMaxVelocity();
            return DesiredVelocity - Host.Velocity;
        }

        private Vector3 DoWander()
        {
            var circleCenter = Vector3.Normalize(Host.Velocity).ScaleBy(CircleDistance);

            var displacement = new Vector3(0, -1, 0).ScaleBy(CircleRadius);

            this.SetAngle(ref displacement, _wanderAngle);
            _wanderAngle += (float)random.NextDouble() * AngleChange - AngleChange * 0.5f;

            return circleCenter + displacement;
        }

        private Vector3 GetFuturePositionOfTarget(IBoid targetBoid)
        {
            var distance = targetBoid.Position - Host.Position;

            // T = updated ahead
            var T = distance.Length() / Host.GetMaxVelocity();

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
