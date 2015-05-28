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
            if (Vector3.Distance(target, this.Host.Position) > 2f)
            {
                this.Steering = Vector3.Add(this.Steering, this.DoSeek(target));
            }
        }
        public void Seek(Vector3 target, IBoid[] boidToSeparateFrom)
        {
            if (Vector3.Distance(target, this.Host.Position) > 2f)
            {
                this.Steering = this.Steering + (this.DoSeek(target));
                this.Steering = this.Steering + this.Separation(boidToSeparateFrom);
            }
        }

        private Vector3 DoSeek(Vector3 target)
        {
            this.DesiredVelocity = target - this.Host.Position;
            this.DesiredVelocity = Vector3.Normalize(this.DesiredVelocity) * this.Host.GetMaxVelocity();
            return this.DesiredVelocity - this.Host.Velocity;
        }
    }
}
