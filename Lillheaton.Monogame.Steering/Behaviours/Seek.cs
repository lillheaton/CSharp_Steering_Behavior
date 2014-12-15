
using Microsoft.Xna.Framework;

namespace Lillheaton.Monogame.Steering.Behaviours
{
    public partial class SteeringBehavior
    {
        /// <summary>
        /// Seeks the target
        /// </summary>
        /// <param name="target"></param>
        public void Seek(Vector3 target)
        {
            if (target != this.Host.Position)
            {
                this.Steering = Vector3.Add(this.Steering, this.DoSeek(target));
            }
        }

        private Vector3 DoSeek(Vector3 target)
        {
            this.DesiredVelocity = target - this.Host.Position;

            float distance = this.DesiredVelocity.Length();

            // Inside slowing radius
            if (distance <= SlowingRadius)
            {
                this.DesiredVelocity = Vector3.Normalize(this.DesiredVelocity) * this.Host.GetMaxVelocity() * (distance / SlowingRadius);
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
