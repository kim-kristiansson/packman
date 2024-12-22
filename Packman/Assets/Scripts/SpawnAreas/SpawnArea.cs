using System.Collections.Generic;
using UnityEngine;

namespace SpawnAreas
{
    [RequireComponent(typeof(MeshFilter), typeof(MeshCollider))]
    public class SpawnArea : MonoBehaviour
    {
        private MeshCollider _collider;
        private Mesh _mesh;
        private Vector3[] _vertices;
        private List<Vector3> _precomputedPoints;

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

            // Offset inward for cylinder radius
            maxXPoint.x -= cylinderRadius;
            return maxXPoint;
        }

        public bool IsPointInside(Vector3 point)
        {
            if (_collider == null) return false;

            // Bounds check
            if (!_collider.bounds.Contains(point)) return false;

            // Raycast fallback
            var rayOrigin = point + Vector3.up * 10;
            var rayDirection = Vector3.down;

            if (Physics.Raycast(rayOrigin, rayDirection, out var hit, 20f, 1 << gameObject.layer))
                return hit.collider == _collider;

            return false;
        }

        public List<Vector3> PrecomputeValidSpawnPoints(float cylinderDiameter)
        {
            _precomputedPoints = new List<Vector3>();
            var bounds = _collider.bounds;
            var step = cylinderDiameter * 0.1f; // Adjust granularity

            for (var x = bounds.min.x; x <= bounds.max.x; x += step)
            {
                for (var z = bounds.min.z; z <= bounds.max.z; z += step)
                {
                    var point = new Vector3(x, bounds.min.y, z);
                    if (IsPointInside(point))
                        _precomputedPoints.Add(point);
                }
            }

            Debug.Log($"Precomputed {_precomputedPoints.Count} valid points.");
            return _precomputedPoints;
        }

        public List<Vector3> GetPrecomputedPoints() => _precomputedPoints;
    }
}
