using Items;
using SpawnAreas;
using UnityEngine;

namespace Managers
{
    public class CylinderSpawner : MonoBehaviour
    {
        [Header("Spawn Area")] public SpawnArea spawnArea;
        [Header("Cylinder Settings")] public Cylinder cylinderPrefab;
        [Header("Parent Container")] public Transform parentContainer;

        public void Start()
        {
            SpawnCylinderAtMaxXPointAndMove();
        }

        public void SpawnCylinderAtMaxXPointAndMove()
        {
            if (spawnArea == null || cylinderPrefab == null)
            {
                Debug.LogError("Spawn area or cylinder prefab is not assigned!");
                return;
            }

            // Retrieve the cylinder's diameter and spawn area's max X point
            var cylinderDiameter = cylinderPrefab.Diameter;
            var maxXPoint = spawnArea.GetMaxXPoint(cylinderDiameter / 2);

            Debug.Log($"Attempting to place cylinder at Max X Point: {maxXPoint}");

            // Check if the cylinder fits at the starting point
            if (DoesCylinderFit(maxXPoint, cylinderDiameter))
            {
                Debug.Log($"Cylinder fits at the starting point {maxXPoint}. Spawning cylinder.");
                InstantiateCylinder(maxXPoint);
                return;
            }

            // Try moving in the -Z direction first
            if (!MoveAndSpawn(maxXPoint, Vector3.back, cylinderDiameter))
            {
                Debug.LogWarning("No valid position found in -Z direction. Trying -X direction...");
                if (!MoveAndSpawn(maxXPoint, Vector3.left, cylinderDiameter))
                    Debug.LogError("No valid position found in either -Z or -X direction.");
            }
        }

        private bool MoveAndSpawn(Vector3 startPosition, Vector3 direction, float cylinderDiameter)
        {
            var currentPosition = startPosition;

            while (spawnArea.IsPointInside(currentPosition))
            {
                // Visualize raycast scanning position
                Debug.DrawRay(currentPosition + Vector3.up * 10, Vector3.down * 20, Color.red, 5.0f);
                Debug.Log($"Checking position: {currentPosition} in direction {direction}");

                if (DoesCylinderFit(currentPosition, cylinderDiameter))
                {
                    Debug.Log($"Cylinder fits at {currentPosition}. Spawning cylinder.");
                    InstantiateCylinder(currentPosition);
                    return true;
                }

                currentPosition += direction * (cylinderDiameter * 0.1f); // Increment position along the direction
            }

            return false; // No valid position found
        }

        private bool DoesCylinderFit(Vector3 position, float diameter)
        {
            var radius = diameter / 2;
            Vector3[] offsets =
            {
                Vector3.right * radius,
                Vector3.left * radius,
                Vector3.forward * radius,
                Vector3.back * radius
            };

            foreach (var offset in offsets)
            {
                var edgePosition = position + offset;

                // Check if each edge point is within the spawn area
                if (!spawnArea.IsPointInside(edgePosition))
                {
                    Debug.Log($"Edge point {edgePosition} is outside the spawn area. Cylinder does not fit.");
                    Debug.DrawRay(edgePosition, Vector3.up * 2, Color.red, 2f); // Visualize failing edge
                    return false;
                }

                Debug.DrawRay(edgePosition, Vector3.up * 2, Color.green, 2f); // Visualize successful edge
            }

            Debug.Log("All edge points are within the spawn area. Cylinder fits.");
            return true;
        }

        private void InstantiateCylinder(Vector3 position)
        {
            if (parentContainer == null)
            {
                Debug.LogError("Parent container is not assigned!");
                return;
            }

            // Clamp position to ensure the cylinder fits within the spawn area's boundaries
            var bounds = spawnArea.GetComponent<MeshCollider>().bounds;
            position.x = Mathf.Clamp(position.x, bounds.min.x + cylinderPrefab.Radius,
                bounds.max.x - cylinderPrefab.Radius);
            position.z = Mathf.Clamp(position.z, bounds.min.z + cylinderPrefab.Radius,
                bounds.max.z - cylinderPrefab.Radius);

            // Instantiate the cylinder
            cylinderPrefab.InstantiateAt(position, parentContainer);
            Debug.Log($"Spawned cylinder at {position}.");
        }
    }
}