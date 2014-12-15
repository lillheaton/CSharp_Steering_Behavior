using Microsoft.Xna.Framework;
using System;

namespace Lillheaton.Monogame.Steering.Behaviours
{
    public partial class SteeringBehavior
    {
        private int _currentNodePath = 0;

        public void FollowPath(Path path)
        {
            this.Seek(this.DoFollowPath(path));
        }

        private Vector3 DoFollowPath(Path path)
        {
            if (path == null)
            {
                throw new Exception("Path should not be null");
            }

            var nodes = path.Get();
            var target = nodes[this._currentNodePath];

            if (Vector3.Distance(this.Host.Position, target) <= 10)
            {
                if (this._currentNodePath < nodes.Count - 1)
                {
                    this._currentNodePath++;
                }
            }

            return target;
        }
    }
}
