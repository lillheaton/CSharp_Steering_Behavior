using System.Linq;

using Lillheaton.Monogame.Steering.Extensions;
using Microsoft.Xna.Framework;
using System.Collections.Generic;

namespace Lillheaton.Monogame.Steering.Behaviours
{
    public partial class SteeringBehavior
    {
        public void Queue(List<IBoid> worldBoids)
        {
            Steering = Vector3.Add(Steering, this.DoQueue(worldBoids));
        }

        private Vector3 DoQueue(List<IBoid> worldBoids)
        {
            var neighbor = this.GetNeighborAhead(worldBoids);
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

        private IBoid GetNeighborAhead(List<IBoid> worldBoids)
        {
            IBoid neighbor = null;
            var tv = Vector3.Normalize(this.Host.Velocity).ScaleBy(Settings.MaxQueueAhead);
            var ahead = Vector3.Add(this.Host.Position, tv);

            foreach (var boid in worldBoids)
            {
                if (boid != this.Host && Vector3.Distance(ahead, boid.Position) <= Settings.MaxQueueAhead)
                {
                    neighbor = boid;
                }
            }

            return neighbor;
        }
    }
}
