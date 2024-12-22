using Items;
using SpawnAreas;
using TestPlates;
using UnityEngine;

namespace Managers
{
    public class CylinderSpawner : MonoBehaviour
    {
        [Header("Test Plate")] public TestPlate testPlate;

        [Header("Cylinder Settings")] public Cylinder cylinderPrefab;

        [Header("Parent Container")] public Transform parentContainer;

        [Header("Debug Settings")] public bool enableDebugMode = true;

        private SpawnArea _spawnArea;

        private void Start()
        {
            if (testPlate == null)
            {
                Debug.LogError("TestPlate is not assigned!");
                return;
            }

            _spawnArea = testPlate.GetSpawnAreaInstance();
            if (_spawnArea == null)
            {
                Debug.LogError("No SpawnArea found on the TestPlate.");
                return;
            }

            SpawnCylinder();
        }

        private void DebugLog(string message)
        {
            if (enableDebugMode)
                Debug.Log(message);
        }

        private void SpawnCylinder()
        {
            if (_spawnArea == null || cylinderPrefab == null)
            {
                Debug.LogError("Spawn area or cylinder prefab is not assigned!");
                return;
            }

            // Get the initial maxX point
            var cylinderDiameter = cylinderPrefab.Diameter;
            var maxXPoint = _spawnArea.GetMaxXPoint(cylinderDiameter / 2);
            DebugLog($"Starting from maxX point: {maxXPoint}");

            // Move along -x axis and find a valid position
            var validPosition = FindValidPosition(maxXPoint, Vector3.left, cylinderDiameter);

            if (validPosition.HasValue)
            {
                DebugLog($"Found valid position at: {validPosition.Value}");
                InstantiateCylinder(validPosition.Value);
            }
            else
            {
                Debug.LogError("No valid position found to spawn the cylinder.");
            }
        }

        private Vector3? FindValidPosition(Vector3 startPoint, Vector3 direction, float cylinderDiameter)
        {
            var currentPosition = startPoint;
            var radius = cylinderDiameter / 2;

            while (_spawnArea.IsPointInside(currentPosition))
            {
                // Check for collisions
                if (IsPositionClear(currentPosition, radius))
                {
                    DebugLog($"Position {currentPosition} is clear and within bounds.");
                    return currentPosition; // Return the first valid position found
                }

                DebugLog($"Position {currentPosition} is not valid. Moving in direction {direction}.");
                currentPosition += direction * (cylinderDiameter * 0.1f); // Increment position
            }

            DebugLog("No valid position found within the spawn area.");
            return null; // No valid position found
        }

        private bool IsPositionClear(Vector3 position, float radius)
        {
            var collisionLayer = LayerMask.GetMask("TestPlate");

            // Use Physics.OverlapSphere to check for collisions at the position
            var overlaps = Physics.OverlapSphere(position, radius, collisionLayer);

            if (overlaps.Length > 0)
            {
                DebugLog($"Collision detected at {position}. Overlap count: {overlaps.Length}");
                return false;
            }

            DebugLog($"No collision detected at {position}.");
            return true;
        }

        private void InstantiateCylinder(Vector3 position)
        {
            if (parentContainer == null)
            {
                Debug.LogError("Parent container is not assigned!");
                return;
            }

            // Adjust Y position to place the cylinder above the TestPlate
            var cylinderHeight = cylinderPrefab.Height;
            position.y = _spawnArea.GetComponent<MeshCollider>().bounds.max.y + cylinderHeight / 2;

            cylinderPrefab.Spawn(position, parentContainer);
            DebugLog($"Spawned cylinder at {position}.");
        }
    }
}