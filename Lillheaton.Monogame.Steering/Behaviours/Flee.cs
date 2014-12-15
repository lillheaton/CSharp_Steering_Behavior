
using Microsoft.Xna.Framework;

namespace Lillheaton.Monogame.Steering.Behaviours
{
    public partial class SteeringBehavior
    {
        /// <summary>
        /// Flee the target
        /// </summary>
        /// <param name="target"></param>
        public void Flee(Vector3 target)
        {
            this.Steering = Vector3.Add(this.Steering, this.DoFlee(target));
        }

        private Vector3 DoFlee(Vector3 target)
        {
            this.DesiredVelocity = Vector3.Normalize(this.Host.Position - target) * this.Host.GetMaxVelocity();
            return this.DesiredVelocity - this.Host.Velocity;
        }
    }
}
