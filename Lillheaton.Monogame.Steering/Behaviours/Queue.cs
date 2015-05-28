using System.Linq;

using Lillheaton.Monogame.Steering.Extensions;
using Microsoft.Xna.Framework;
using System.Collections.Generic;

namespace Lillheaton.Monogame.Steering.Behaviours
{
    public partial class SteeringBehavior
    {
        public void Queue(IBoid[] worldBoids)
        {
            Steering = Vector3.Add(Steering, this.DoQueue(worldBoids));
        }

        private Vector3 DoQueue(IBoid[] worldBoids)
        {
            var neighbor = this.GetCloseBoidAhead(worldBoids, Settings.MaxQueueAhead);
            var brake = new Vector3();

            if (neighbor != null)
            {
                brake.X = -Steering.X * 0.8f;
                brake.Y = -Steering.Y * 0.8f;

                brake = Vector3.Add(brake, this.Host.Velocity.ScaleBy(-1));
                brake = Vector3.Add(brake, this.Separation(worldBoids));

                if (Vector3.Distance(this.Host.Position, neighbor.Position) <= Settings.MaxQueueRadius)
                {
                    this.Host.Velocity = this.Host.Velocity.ScaleBy(0.3f);
                }
            }

            return brake;
        }

        private IBoid GetCloseBoidAhead(IBoid[] worldBoids, int aheadDistance)
        {
            IBoid neighbor = null;
            var tv = Vector3.Normalize(this.Host.Velocity).ScaleBy(aheadDistance);
            var ahead = Vector3.Add(this.Host.Position, tv);

            for (int i = 0; i < worldBoids.Length; i++)
            {
                if (worldBoids[i] != this.Host && Vector3.Distance(ahead, worldBoids[i].Position) <= aheadDistance)
                {
                    neighbor = worldBoids[i];
                }
            }
            
            return neighbor;
        }
    }
}
