using Microsoft.Xna.Framework;

namespace Lillheaton.Monogame.Steering
{
    public interface IObstacle
    {
        Vector3 Position { get; set; }
        float GetRadius();
    }
}
