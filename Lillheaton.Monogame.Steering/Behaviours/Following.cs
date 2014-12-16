using Microsoft.Xna.Framework;

namespace Lillheaton.Monogame.Steering.Behaviours
{
    /// <summary>
    /// Partial class for follow leader behaviour
    /// </summary>
    public partial class SteeringBehavior
    {
        private const int LeaderBehindDistance = 100;

        public void FollowLeader(IBoid leader)
        {
            Steering = Vector3.Add(Steering, this.DoSeek(this.DoFollowLeader(leader)));
        }

        private Vector3 DoFollowLeader(IBoid leader)
        {
            var tv = leader.Velocity * -1;
            tv = Vector3.Normalize(tv) * LeaderBehindDistance;
            var behind = Vector3.Add(leader.Position, tv);

            return behind;
        }
    }
}