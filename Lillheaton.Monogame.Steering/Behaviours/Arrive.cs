
using Microsoft.Xna.Framework;

namespace Lillheaton.Monogame.Steering.Behaviours
{
    public partial class SteeringBehavior
    {
        public void Arrive(Vector3 target)
        {
            if (target != this.Host.Position)
            {
                this.Steering = Vector3.Add(this.Steering, this.DoArrive(target));
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
