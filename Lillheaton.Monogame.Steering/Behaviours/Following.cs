using System.Collections.Generic;

using Lillheaton.Monogame.Steering.Extensions;
using Microsoft.Xna.Framework;
using System.Linq;

namespace Lillheaton.Monogame.Steering.Behaviours
{
    /// <summary>
    /// Partial class for follow leader behaviour
    /// </summary>
    public partial class SteeringBehavior
    {
        public void FollowLeader(IBoid leader, List<IBoid> worldBoids = null)
        {
            Steering = Vector3.Add(Steering, this.DoFollowLeader(leader, worldBoids));
        }

        private Vector3 DoFollowLeader(IBoid leader, List<IBoid> worldBoids = null)
        {
            // Ahead vector
            var tv = Vector3.Normalize(leader.Velocity);
            tv = tv.ScaleBy(Settings.LeaderBehindDistance);
            var ahead = Vector3.Add(leader.Position, tv);

            // Behind vector
            tv = tv.ScaleBy(-1);
            var behind = Vector3.Add(leader.Position, tv);

            var force = new Vector3(0, 0, 0);

            // If the character is on the leader's sight, add a force
            // to evade the route immediately.
            if (this.IsOnleaderSight(leader, ahead))
            {
                force = Vector3.Add(force, this.DoFlee(this.GetFuturePositionOfTarget(leader)));
                force = force.ScaleBy(1.8f);
            }

            // Creates a force to arrive at the behind point
            force = Vector3.Add(force, this.DoSeek(behind));

            if (worldBoids != null)
            {
                // Add separation force
                force = Vector3.Add(force, this.Separation(worldBoids));    
            }

            return force;
        }

        private bool IsOnleaderSight(IBoid leader, Vector3 leaderAhead)
        {
            return Vector3.Distance(leaderAhead, this.Host.Position) <= Settings.LeaderSightRadius
                   || Vector3.Distance(leader.Position, this.Host.Position) <= Settings.LeaderSightRadius;
        }
    }
}