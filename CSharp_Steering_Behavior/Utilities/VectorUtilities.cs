using Microsoft.Xna.Framework;

namespace CSharp_Steering_Behavior.Utilities
{
    public class VectorUtilities
    {
        public static Vector3 Truncate(Vector3 vector, float max)
        {
            if (vector.Length() > max)
            {
                var normalizedCopy = Vector3.Normalize(vector);
                return normalizedCopy *= max;
            }

            return vector;
        }
    }
}
