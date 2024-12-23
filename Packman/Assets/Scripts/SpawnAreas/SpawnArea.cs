using System.Collections.Generic;
using UnityEngine;

namespace SpawnAreas
{
    [RequireComponent(typeof(MeshFilter), typeof(MeshCollider))]
    public class SpawnArea : MonoBehaviour
    {
        private MeshCollider _collider;
        private Mesh _mesh;
        private List<Vector3> _precomputedPoints;
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

        public Vector3 GetMaxXPoint()
        {
            var maxX = float.MinValue;
            var maxXPoint = Vector3.zero;

            foreach (var vertex in _vertices)
            {
                var worldVertex = transform.TransformPoint(vertex);

                if (!(worldVertex.x > maxX)) continue;
                maxX = worldVertex.x;
                maxXPoint = worldVertex;
            }

            return maxXPoint;
        }

        public bool IsPointInside(Vector3 point)
        {
            if (_collider == null)
            {
                Debug.LogError("SpawnArea is missing a collider.");
                return false;
            }

            var bounds = _collider.bounds;
            if (!bounds.Contains(point))
            {
                Debug.Log($"Point {point} is outside the spawn area bounds: {bounds}");
                return false;
            }

            Debug.Log($"Point {point} is inside the spawn area bounds.");
            return true;
        }
    }
}