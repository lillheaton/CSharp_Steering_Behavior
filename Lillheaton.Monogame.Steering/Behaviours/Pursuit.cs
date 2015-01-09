
namespace Lillheaton.Monogame.Steering.Behaviours
{
    public partial class SteeringBehavior
    {
        /// <summary>
        /// Persuit target
        /// </summary>
        /// <param name="targetBoid"></param>
        public void Pursuit(IBoid targetBoid)
        {
            this.Arrive(this.GetFuturePositionOfTarget(targetBoid));
        }
    }
}
