
using Microsoft.Xna.Framework;

namespace CSharp_Steering_Behavior.Extensions
{
    public static class VectorExtensions
    {
        public static void Truncate(this Vector3 vector, float max)
        {
            var i = max / vector.Length();
            i = i < 1.0f ? 1.0f : i;
            vector *= i;
        }

        public static void ScaleBy(this Vector3 vector, float scale)
        {
            vector *= scale;
        }
    }
}
