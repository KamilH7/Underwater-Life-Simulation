using UnityEngine;

namespace Shaders
{
    public class Boid : MonoBehaviour
    {
        [SerializeField] private bool debug;

        [SerializeField] private BoidSettings settings;

        public Vector3 avgDisplacementVector;

        public Vector3 avgAvoidanceDisplacementVector;

        public Vector3 avgPosition;

        public void UpdateBoid()
        {
            Vector3 moveVector = Vector3.zero;

            moveVector += AlignmentBehaviour();
            moveVector += CohesionBehaviour();
            moveVector += SeparationBehaviour();

            transform.forward = Vector3.Lerp(transform.forward, moveVector, Time.deltaTime);

            var hit = GetCollisionInfo(transform.forward);

            if (hit.collider)
                transform.forward = Vector3.Lerp(transform.forward, GetDirectionToAvoidCollision(),
                    Time.deltaTime * 1 / hit.distance);

            transform.position += transform.forward * settings.maxSpeed * Time.deltaTime;
        }

        private Vector3 AlignmentBehaviour()
        {
            return avgDisplacementVector * settings.alignmentBehaviour;
        }

        private Vector3 CohesionBehaviour()
        {
            if (avgPosition != Vector3.zero)
            {
                var direction = avgPosition - transform.position;
                return direction * settings.cohesionBehaviour;
            }

            return Vector3.zero;
        }

        private Vector3 SeparationBehaviour()
        {
            return avgAvoidanceDisplacementVector * settings.separationBehaviour;
        }

        private Vector3 GetDirectionToAvoidCollision()
        {
            return ObstacleRays();
        }

        private RaycastHit GetCollisionInfo(Vector3 direction)
        {
            RaycastHit hit;

            Physics.Raycast(transform.position, direction, out hit, settings.collisionAvoidDst, settings.obstacleLayer);

            return hit;
        }

        private Vector3 ObstacleRays()
        {
            var rayDirections = Values.Instance.GoldenRatioDirections;

            for (var i = 0; i < rayDirections.Length; i++)
            {
                var dir = transform.TransformDirection(rayDirections[i]);
                var ray = new Ray(transform.position, dir);

                if (!Physics.Raycast(ray, settings.collisionAvoidDst, settings.obstacleLayer)) return dir;
            }

            return transform.forward;
        }
    }
}