using Microsoft.Xna.Framework;

namespace Lillheaton.Monogame.Steering.Extensions
{
    public static class VectorExtensions
    {
        public static Vector3 Truncate(this Vector3 vector, float max)
        {
            var i = max / vector.Length();
            i = i < 1.0f ? i : 1.0f;
            return vector * i;
        }

        public static Vector3 ScaleBy(this Vector3 vector, float scale)
        {
            return vector * scale;
        }

        public static Vector3 Clone(this Vector3 vector)
        {
            return new Vector3(vector.X, vector.Y, vector.Z);
        }
    }
}
