using CSharp_Steering_Behavior.Primitives;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace CSharp_Steering_Behavior.Units
{
    public class Follower : BaseUnit
    {
        private Triangle _triangle;
        public Leader Leader { get; set; }

        public Follower(Vector3 position) : base(position)
        {
            _triangle = new Triangle();
        }

        public override void Update(GameTime gameTime)
        {
            SteeringBehavior.FollowLeader(Leader, WorldBoids.ToArray());

            // Calculate Steering
            base.Update(gameTime);

            // Update triangle rotation and postion
            _triangle.Update(gameTime, SteeringBehavior.Angle, this.Position);
        }

        public override void Draw(PrimitiveBatch primitiveBatch)
        {
            primitiveBatch.Begin(PrimitiveType.TriangleList);
            _triangle.Draw(primitiveBatch);
            primitiveBatch.End();

            primitiveBatch.Begin(PrimitiveType.LineList);
            this.DrawForces(primitiveBatch);
            primitiveBatch.End();
        }
    }
}
