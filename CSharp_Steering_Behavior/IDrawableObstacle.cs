using Lillheaton.Monogame.Steering;

namespace CSharp_Steering_Behavior
{
    public interface IDrawableObstacle : IObstacle
    {
        void Draw(PrimitiveBatch primitiveBatch);
    }
}
