using Geometry;
using System;

namespace Physics
{
    // AI-generated

    [Obsolete( "Unconfirmed" )]
    struct Spring
    {
        public float Mass;
        public float SpringConstant;
        public float DampingConstant;
        public float RestPosition;

        public Spring( float mass, float springConstant, float dampingConstant, float restPosition )
        {
            Mass = mass;
            SpringConstant = springConstant;
            DampingConstant = dampingConstant;
            RestPosition = restPosition;
        }

        public static Vector3 GetNewPosition( Spring spring, Vector3 position, Vector3 velocity, float deltaTime )
        {
            // Calculate the spring force
            Vector3 springForce = -spring.SpringConstant * (position - spring.RestPosition);

            // Calculate the damping force
            Vector3 dampingForce = -spring.DampingConstant * velocity;

            // Calculate the total force
            Vector3 force = springForce + dampingForce;

            // Calculate the acceleration
            Vector3 acceleration = force / spring.Mass;

            // Calculate the new velocity
            Vector3 newVelocity = velocity + acceleration * deltaTime;

            // Calculate the new position
            Vector3 newPosition = position + newVelocity * deltaTime;

            return newPosition;
        }
    };

}
