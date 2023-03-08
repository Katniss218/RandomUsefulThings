using Geometry;
using System;
using System.Collections.Generic;
using System.Text;

namespace Miscellaneous
{
    public static class Directions
    {
        /// <param name="youPos">Position of the player</param>
        /// <param name="youRight">Position of the target</param>
        /// <param name="targetPos">The 'transform.right' vector of the player (vector pointing directly right from the direction the player is facing).</param>
        public static void TurnLeftOrRight( Vector3 youPos, Vector3 youRight, Vector3 targetPos )
        {
            Vector3 towardsTarget = targetPos - youPos;

            float dotProduct = Vector3.Dot( youRight, towardsTarget );

            if( dotProduct > 0f )
            {
                // "Turn right"; right vector is pointing towards the target.
            }
            else
            {
                // "Turn left"; right vector is pointing away from the target.
            }
        } 
        
        public static void TurnLeftOrRightDir( Vector3 youRight, Vector3 targetDir )
        {
            float dotProduct = Vector3.Dot( youRight, targetDir );

            if( dotProduct > 0f )
            {
               // "Turn right"; right vector is pointing towards the target.
            }
            else
            {
                // "Turn left"; right vector is pointing away from the target.
            }
        }


    }
}
