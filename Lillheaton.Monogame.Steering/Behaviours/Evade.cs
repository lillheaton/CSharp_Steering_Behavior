
namespace Lillheaton.Monogame.Steering.Behaviours
{
    public partial class SteeringBehavior
    {
        /// <summary>
        /// Evade target
        /// </summary>
        /// <param name="targetBoid"></param>
        public void Evade(IBoid targetBoid)
        {
            this.Flee(this.GetFuturePositionOfTarget(targetBoid));
        }
    }
}
