using Lillheaton.Monogame.Steering;

namespace CSharp_Steering_Behavior
{
    public interface IDrawableObstacle : ICircleObstacle
    {
        void Draw(PrimitiveBatch primitiveBatch);
    }
}
