using UnityEngine;

namespace SpawnAreas
{
    [RequireComponent(typeof(MeshFilter), typeof(MeshCollider))]
    public class SpawnArea : MonoBehaviour
    {
        private MeshCollider _collider;
        private Mesh _mesh;
        private Vector3[] _vertices;

        private void Awake()
        {
            _mesh = GetComponent<MeshFilter>().mesh;
            _collider = GetComponent<MeshCollider>();

            if (_collider == null)
            {
                Debug.LogError("SpawnArea is missing a MeshCollider!");
                return;
            }

            _vertices = _mesh.vertices;
            Debug.Log($"Spawn area initialized with {_vertices.Length} vertices.");
        }

        public Vector3 GetMaxXPoint(float cylinderRadius)
        {
            var maxX = float.MinValue;
            var maxXPoint = Vector3.zero;

            foreach (var vertex in _vertices)
            {
                var worldVertex = transform.TransformPoint(vertex);

                if (worldVertex.x > maxX)
                {
                    maxX = worldVertex.x;
                    maxXPoint = worldVertex;
                }
            }

            // Offset the max point inward to ensure the cylinder's edge stays inside
            maxXPoint.x -= cylinderRadius;
            Debug.Log($"Adjusted Max X point of the spawn area: {maxXPoint}");
            return maxXPoint;
        }


        public bool IsPointInside(Vector3 point)
        {
            if (_collider == null)
            {
                Debug.LogError("No collider found on SpawnArea.");
                return false;
            }

            // Check if the point is within the collider's bounding box
            var bounds = _collider.bounds;
            if (!bounds.Contains(point))
            {
                Debug.Log($"Point {point} is outside the collider bounds: {bounds}");
                return false;
            }

            // Fallback to raycasting for additional validation
            var rayOrigin = point + Vector3.up * 10;
            var rayDirection = Vector3.down;

            Debug.DrawRay(rayOrigin, rayDirection * 20f, Color.cyan, 5.0f);

            if (Physics.Raycast(rayOrigin, rayDirection, out var hit, 20f, 1 << gameObject.layer))
            {
                var isInside = hit.collider == _collider;
                Debug.Log($"Raycast hit at {hit.point}, Is inside: {isInside}");
                return isInside;
            }

            Debug.Log("Raycast did not hit the spawn area.");
            return false;
        }
    }
}