using Lillheaton.Monogame.Steering.Extensions;
using Microsoft.Xna.Framework;

namespace Lillheaton.Monogame.Steering.Behaviours
{
    public partial class SteeringBehavior
    {
        private Vector3 _avoidanceForce = new Vector3(0, 0, 0);

        /// <summary>
        /// Avoid obsticals
        /// </summary>
        /// <param name="objectsToAvoid"></param>
        public void CollisionAvoidance(IObstacle[] objectsToAvoid)
        {
            this.Steering = Vector3.Add(this.Steering, this.DoCollisionAvoidance(objectsToAvoid));
        }

        private Vector3 DoCollisionAvoidance(IObstacle[] objectsToAvoid)
        {
            var tv = Vector3.Normalize(this.Host.Velocity);
            tv = tv.ScaleBy(Settings.MaxSeeAhead * this.Host.Velocity.Length() / this.Host.GetMaxVelocity());

            // Ahead is the same as the velocity vector except it's longer
            var ahead = Vector3.Add(this.Host.Position, tv);

            var threat = this.MostThreatingObstacle(ahead, objectsToAvoid);

            if (threat != null)
            {
                this._avoidanceForce.X = ahead.X - threat.Center.X;
                this._avoidanceForce.Y = ahead.Y - threat.Center.Y;

                this._avoidanceForce = Vector3.Normalize(this._avoidanceForce);
                this._avoidanceForce = this._avoidanceForce.ScaleBy(Settings.MaxAvoidanceForce);
            }
            else
            {
                // nullify the avoidance force
                this._avoidanceForce = this._avoidanceForce.ScaleBy(0);
            }

            return this._avoidanceForce;
        }

        private IObstacle MostThreatingObstacle(Vector3 ahead, IObstacle[] obstacles)
        {
            IObstacle mostThreating = null;

            for (int i = 0; i < obstacles.Length; i++)
            {
                bool collision = false;
                if (obstacles[i] is ICircleObstacle)
                {
                    collision = this.LineIntersectsCircle(ahead, obstacles[i] as ICircleObstacle);
                }

                if (obstacles[i] is IRectangleObstacle)
                {
                    collision = this.LineIntersectsRectangle(ahead, obstacles[i] as IRectangleObstacle);
                }

                if (collision && (mostThreating == null || Vector3.Distance(this.Host.Position, obstacles[i].Center) < Vector3.Distance(this.Host.Position, mostThreating.Center)))
                {
                    mostThreating = obstacles[i];
                }
            }

            return mostThreating;
        }

        private bool LineIntersectsCircle(Vector3 ahead, ICircleObstacle obstacle)
        {
            var tv = Vector3.Normalize(this.Host.Velocity);
            tv = tv.ScaleBy(Settings.MaxSeeAhead * 0.5f * this.Host.Velocity.Length() / this.Host.GetMaxVelocity());

            var ahead2 = Vector3.Add(this.Host.Position, tv);

            return Vector3.Distance(obstacle.Center, ahead) <= obstacle.GetRadius()
                   || Vector3.Distance(obstacle.Center, ahead2) <= obstacle.GetRadius()
                   || Vector3.Distance(obstacle.Center, this.Host.Position) <= obstacle.GetRadius();
        }

        private bool LineIntersectsRectangle(Vector3 ahead, IRectangleObstacle obstacle)
        {
            var tv = Vector3.Normalize(this.Host.Velocity);
            tv = tv.ScaleBy(Settings.MaxSeeAhead * 0.5f * this.Host.Velocity.Length() / this.Host.GetMaxVelocity());

            var ahead2 = Vector3.Add(this.Host.Position, tv);

            return this.IsInsideRectangle(ahead, obstacle) || this.IsInsideRectangle(ahead2, obstacle)
                   || this.IsInsideRectangle(this.Host.Position, obstacle);
        }

        private bool IsInsideRectangle(Vector3 position, IRectangleObstacle obstacle)
        {
            return obstacle.Rectangle.Contains(new Vector2(position.X, position.Y));
        }
    }
}