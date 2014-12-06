using System.Collections.Generic;
using Microsoft.Xna.Framework;

namespace Lillheaton.Monogame.Steering
{
    public class Path
    {
        private List<Vector3> _path;

        public Path(List<Vector3> path)
        {
            this._path = path;
        }

        public Path()
        {
            this._path = new List<Vector3>();
        }

        public void AddNode(Vector3 node)
        {
            this._path.Add(node);
        }

        public List<Vector3> Get()
        {
            return this._path;
        } 
    }
}