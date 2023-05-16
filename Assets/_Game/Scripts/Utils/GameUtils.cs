using UnityEngine;

namespace Utils{
    public static class GameUtils{
        /// <param name="position">The Vector3 used to calculate the distance from the player.</param>
        /// <returns>A Vector2 containing the difference between the Player and the given position;</returns>
        public static Vector2 GetPlayerDistance(Vector3 position){
            return Player.Instance.transform.position - position;
        }

        public static Vector2 OrientVelocityToGround(Vector2 velocity, Vector2 groundNormal){
            return Vector3.ProjectOnPlane(velocity, -groundNormal);
        }
    }
}