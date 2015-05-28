using Microsoft.Xna.Framework;

namespace Lillheaton.Monogame.Steering.Behaviours
{
    public partial class SteeringBehavior
    {
        public void Arrive(Vector3 target)
        {
            if (Vector3.Distance(target, this.Host.Position) > 3f)
            {
                this.Steering = Vector3.Add(this.Steering, this.DoArrive(target));
            }
        }
        public void Arrive(Vector3 target, IBoid[] boidToSeparateFrom)
        {
            if (Vector3.Distance(target, this.Host.Position) > 3f)
            {
                this.Steering = this.Steering + (this.DoArrive(target));
                this.Steering = this.Steering + this.Separation(boidToSeparateFrom);
            }
        }

        public Vector3 DoArrive(Vector3 target)
        {
            this.DesiredVelocity = target - this.Host.Position;

            float distance = this.DesiredVelocity.Length();

            // Inside slowing radius
            if (distance <= Settings.SlowingRadius)
            {
                this.DesiredVelocity = Vector3.Normalize(this.DesiredVelocity) * this.Host.GetMaxVelocity() * (distance / Settings.SlowingRadius);
            }
            else
            {
                // Far away
                this.DesiredVelocity = Vector3.Normalize(this.DesiredVelocity) * this.Host.GetMaxVelocity();
            }

            return this.DesiredVelocity - this.Host.Velocity;
        }
    }
}
