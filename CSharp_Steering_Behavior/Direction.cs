using Microsoft.Xna.Framework;

namespace CSharp_Steering_Behavior
{
    public class Direction
    {
        public Vector3 Vector { get; private set; }

        public static Direction North = new Direction(new Vector3(0, 0.01f, 0));
        public static Direction South = new Direction(new Vector3(0, -0.01f, 0));
        public static Direction East = new Direction(new Vector3(0.01f, 0, 0));
        public static Direction West = new Direction(new Vector3(-0.01f, 0, 0));

        public Direction(Vector3 direction)
        {
            this.Vector = direction;
        }
    }
}
