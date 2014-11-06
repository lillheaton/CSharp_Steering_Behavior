using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Microsoft.Xna.Framework;

namespace CSharp_Steering_Behavior
{
    public class SteeringBehavior
    {
        private static int MaxVelocity = 5;

        public static Vector3 Seek(Vector3 position, Vector3 target)
        {
            var velocity = Vector3.Normalize(target - position) * MaxVelocity;
            return position + velocity;
        }
    }
}
