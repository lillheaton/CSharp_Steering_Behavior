
using Microsoft.Xna.Framework;

namespace CSharp_Steering_Behavior
{
    public interface IObstacle
    {
        Vector3 Position { get; set; }

        void Draw(PrimitiveBatch primitiveBatch);

        float GetRadius();
    }
}
