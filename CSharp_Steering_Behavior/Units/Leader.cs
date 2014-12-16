using CSharp_Steering_Behavior.Primitives;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace CSharp_Steering_Behavior.Units
{
    public class Leader : BaseUnit
    {
        private Triangle _triangle;
        private Vector3 _target;

        public Leader(Vector3 position) : base(position)
        {
            _triangle = new Triangle();
        }

        public void MoveTwoardsTarget(Vector3 target)
        {
            _target = target;
        }

        public override void Update(GameTime gameTime)
        {
            // Seek target
            SteeringBehavior.Seek(_target);
            SteeringBehavior.CollisionAvoidance(Obstacles);

            // Calculate Steering
            base.Update(gameTime);

            // Update triangle rotation and postion
            _triangle.Update(gameTime, SteeringBehavior.Angle, this.Position);
        }

        public override float GetMaxVelocity()
        {
            return 1;
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