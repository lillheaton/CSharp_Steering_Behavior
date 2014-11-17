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

        private const int MaxSeeAhead = 4;
        private const float MaxForce = 5.4f;
        private const int SlowingRadius = 100;
        private const int CircleDistance = 6;
        private const int CircleRadius = 8;
        private const int AngleChange = 1;
        private const float MaxAvoidanceForce = 5;

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

        public void CollisionAvoidance(IObstacle[] objectsToAvoid)
        {
            Steering = Steering + this.DoCollisionAvoidance(objectsToAvoid);
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

        private Vector3 DoCollisionAvoidance(IObstacle[] objectsToAvoid)
        {
            // Ahead is the same as the velocity vector except it's longer
            var ahead = Host.Position + Vector3.Normalize(Host.Velocity) * MaxSeeAhead;
            var ahead2 = Host.Position + Vector3.Normalize(Host.Velocity) * MaxSeeAhead * 0.5f;

            var threat = this.MostThreatingObstacle(ahead, ahead2, objectsToAvoid);
            var avoidanceForce = new Vector3();

            if (threat != null)
            {
                avoidanceForce.X = ahead.X - threat.Position.X;
                avoidanceForce.Y = ahead.Y - threat.Position.Y;

                avoidanceForce = Vector3.Normalize(avoidanceForce);
                avoidanceForce = avoidanceForce.ScaleBy(MaxAvoidanceForce);
            }
            //else
            //{
            //    // nullify the avoidance force
            //    avoidance = avoidance.ScaleBy(0);
            //}

            return avoidanceForce;
        }

        private IObstacle MostThreatingObstacle(Vector3 ahead, Vector3 ahead2, IObstacle[] obstacles)
        {
            IObstacle mostThreating = null;

            foreach (var obstacle in obstacles)
            {
                if (this.LineIntersectsCircle(ahead, ahead2, obstacle))
                {
                    if (mostThreating == null || Vector3.Distance(Host.Position, obstacle.Position) < Vector3.Distance(Host.Position, mostThreating.Position))
                    {
                        mostThreating = obstacle;
                    }
                }
            }

            return mostThreating;
        }

        private bool LineIntersectsCircle(Vector3 ahead, Vector3 ahead2, IObstacle obstacle)
        {
            return Vector3.Distance(obstacle.Position, ahead) <= obstacle.GetRadius()
                   || Vector3.Distance(obstacle.Position, ahead2) <= obstacle.GetRadius();
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
