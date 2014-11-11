using Microsoft.Xna.Framework;
using System;

namespace CSharp_Steering_Behavior
{
    public class SteeringBehavior
    {
        public float Angle { get; private set; }
        public Vector3 Velocity { get; private set; }

        private const int MaxVelocity = 5;

        public SteeringBehavior()
        {
            Angle = 0;
        }

        public Vector3 Seek(Vector3 position, Vector3 target)
        {
            Velocity = Vector3.Normalize(target - position) * MaxVelocity;
            Angle = (float)(-Math.Atan2(Velocity.X, Velocity.Y) + Math.PI);
            return position + Velocity;
        }
    }
}
