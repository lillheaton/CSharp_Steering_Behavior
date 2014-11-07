using Microsoft.Xna.Framework;
using System;

namespace CSharp_Steering_Behavior
{
    public class SteeringBehavior
    {
        private static int MaxVelocity = 5;

        public static Vector3 Seek(Vector3 position, Vector3 target, out double angle)
        {
            var velocity = Vector3.Normalize(target - position) * MaxVelocity;
            angle = -Math.Atan2(velocity.X, velocity.Y) + Math.PI;
            return position + velocity;
        }
    }
}
