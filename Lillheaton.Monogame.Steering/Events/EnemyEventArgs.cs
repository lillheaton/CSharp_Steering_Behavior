using System;

namespace Lillheaton.Monogame.Steering.Events
{
    public class EnemyEventArgs : EventArgs
    {
        public IBoid Enemy { get; set; }

        public EnemyEventArgs(IBoid enemy)
        {
            this.Enemy = enemy;
        }
    }
}
