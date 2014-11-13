
using Microsoft.Xna.Framework;

namespace CSharp_Steering_Behavior
{
    public interface IBoid
    {
        SteeringBehavior SteeringBehavior { get; set; }
        Vector3 Position { get; set; }
        Vector3 Velocity { get; set; }

        void Update(GameTime gameTime);
        float GetMaxVelocity();
        float GetMass();
    }
}
