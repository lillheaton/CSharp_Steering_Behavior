
namespace Lillheaton.Monogame.Steering
{
    public class Settings
    {
        /// <summary>
        /// How much ahead the boid see ahead in the collision avoidance
        /// </summary>
        public int MaxSeeAhead { get; set; }

        /// <summary>
        /// Every update max force is used to truncate the steering
        /// </summary>
        public float MaxForce { get; set; }

        /// <summary>
        /// The radius for when the boid is start to slow down in the arrive behaviour
        /// </summary>
        public int SlowingRadius { get; set; }

        /// <summary>
        /// Used in wander, the longer the distance the smoother direction changes
        /// </summary>
        public int CircleDistance { get; set; }

        /// <summary>
        /// Used in wander, the larger radius the less "sudden moves" accoure
        /// </summary>
        public int CircleRadius { get; set; }

        /// <summary>
        /// Used in wander, setting the angle: (float)this.random.NextDouble() * AngleChange - AngleChange * 0.5f
        /// </summary>
        public int AngleChange { get; set; }

        /// <summary>
        /// Used in collision avoidance, how much force used for pushing boid away from obstacles
        /// </summary>
        public float MaxAvoidanceForce { get; set; }

        /// <summary>
        /// Used in follow, how far back unit will stay from the leader
        /// </summary>
        public int LeaderBehindDistance { get; set; }

        /// <summary>
        /// If the character is on the leader's sight, add a force to evade the route.
        /// </summary>
        public int LeaderSightRadius { get; set; }

        /// <summary>
        /// Used for separation of other boids
        /// </summary>
        public int SeparationRadius { get; set; }

        /// <summary>
        /// How much separation force from other boids 
        /// </summary>
        public float SeparationForce { get; set; }

        /// <summary>
        /// The Distance for when to start queueing
        /// </summary>
        public int MaxQueueAhead { get; set; }

        /// <summary>
        /// How much space from other boids
        /// </summary>
        public int MaxQueueRadius { get; set; }

        public Settings()
        {
            MaxSeeAhead = 50;
            MaxForce = 5.4f;
            SlowingRadius = 100;
            CircleDistance = 6;
            CircleRadius = 8;
            AngleChange = 1;
            MaxAvoidanceForce = 100;

            SeparationRadius = 30;
            SeparationForce = 2.0f;

            LeaderBehindDistance = 50;
            LeaderSightRadius = 30;

            MaxQueueAhead = 30;
            MaxQueueRadius = 30;
        }
    }
}
