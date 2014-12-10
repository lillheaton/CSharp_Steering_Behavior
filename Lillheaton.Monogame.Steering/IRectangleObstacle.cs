
using Microsoft.Xna.Framework;

namespace Lillheaton.Monogame.Steering
{
    public interface IRectangleObstacle : IObstacle
    {
        Rectangle Rectangle { get; }
    }
}