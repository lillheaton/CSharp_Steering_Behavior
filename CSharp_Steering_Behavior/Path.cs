using Microsoft.Xna.Framework;
using System.Collections.Generic;

namespace CSharp_Steering_Behavior
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
            return _path;
        } 
    }
}