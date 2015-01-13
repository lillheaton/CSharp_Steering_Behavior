using Lillheaton.Monogame.Steering.Extensions;
using Microsoft.Xna.Framework;

namespace Lillheaton.Monogame.Steering.Behaviours
{
    public partial class SteeringBehavior
    {
        private float _wanderAngle = 0;

        /// <summary>
        /// Just wander around
        /// </summary>
        public void Wander()
        {
            this.Steering = Vector3.Add(this.Steering, this.DoWander());
        }

        private Vector3 DoWander()
        {
            var circleCenter = Vector3.Normalize(this.Host.Velocity).ScaleBy(Settings.CircleDistance);

            var displacement = new Vector3(0, -1, 0).ScaleBy(Settings.CircleRadius);

            this.SetAngle(ref displacement, this._wanderAngle);
            this._wanderAngle += (float)this.random.NextDouble() * Settings.AngleChange - Settings.AngleChange * 0.5f;

            return circleCenter + displacement;
        }
    }
}
