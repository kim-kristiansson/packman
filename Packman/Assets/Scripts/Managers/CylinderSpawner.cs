using System.Collections.Generic;
using System.Linq;
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

        private readonly List<Vector3> _spawnedCylinderPositions = new();

        private void Start()
        {
            SpawnCylinderOnMaxX();
        }

        private void SpawnCylinderOnMaxX()
        {
            if (cylinderPrefab == null)
            {
                Debug.LogError("Spawn area or cylinder prefab is not assigned!");
                return;
            }

            var spawnArea = testPlate.GetSpawnAreaInstance();
            var maxXPoint = spawnArea.GetMaxXPoint();

            var firstPosition = PlaceWhenNoCollision(maxXPoint, Vector3.left, cylinderPrefab.Radius, spawnArea);

            if (!firstPosition.HasValue)
            {
                Debug.LogWarning("No valid position found to spawn the cylinder.");
                return;
            }

            var stepSize = 1.001f * cylinderPrefab.Diameter;
            var newColumnUpDirection = Quaternion.Euler(0, -120f, 0) * Vector3.left;
            var newColumnDownDirection = Quaternion.Euler(0, -120f, 0) * Vector3.right;
            var newRowDirection = Vector3.left;

            var rowPosition = firstPosition;

            for (var i = 0; i < 100; i++)
            {
                if (!rowPosition.HasValue)
                {
                    Debug.LogWarning("No valid position found to spawn the cylinder.");
                    return;
                }

                PlaceCylindersAlongPath(rowPosition.Value, newColumnUpDirection, cylinderPrefab.Radius,
                    spawnArea, stepSize);
                PlaceCylindersAlongPath(rowPosition.Value, newColumnDownDirection, cylinderPrefab.Radius,
                    spawnArea, stepSize);

                rowPosition = TakeOneStep(rowPosition.Value, newRowDirection, cylinderPrefab.Radius, spawnArea,
                    stepSize);
            }
        }

        private Vector3? TakeOneStep(Vector3 currentPosition, Vector3 direction, float radius, SpawnArea spawnArea,
            float stepSize)
        {
            var nextPosition = GetNextStep(currentPosition, direction, stepSize);

            if (IsValidStep(nextPosition, radius, spawnArea))
            {
                InstantiateCylinder(nextPosition);
                return nextPosition;
            }

            Debug.LogWarning($"Next position {nextPosition} is not valid.");
            return null;
        }

        private void PlaceCylindersAlongPath(Vector3 startPosition, Vector3 direction, float radius,
            SpawnArea spawnArea, float stepSize)
        {
            var currentPosition = startPosition;

            for (var i = 0; i < 1000; i++)
            {
                var nextPosition = GetNextStep(currentPosition, direction, stepSize);

                if (!IsValidStep(nextPosition, radius, spawnArea))
                {
                    Debug.LogWarning("No valid position found, stopping placement.");
                    return;
                }

                InstantiateCylinder(nextPosition);
                Debug.Log($"Spawned cylinder at position: {nextPosition}");

                currentPosition = nextPosition;
            }

            Debug.LogError("Maximum steps reached! Stopping to prevent infinite loop.");
        }

        private Vector3? PlaceWhenNoCollision(Vector3 startPoint, Vector3 direction, float radius, SpawnArea spawnArea)
        {
            var currentPosition = startPoint;

            for (var step = 0; step < 10000; step++)
            {
                currentPosition = GetNextStep(currentPosition, direction, 0.01f);

                if (!IsValidStep(currentPosition, radius, spawnArea)) continue;

                InstantiateCylinder(currentPosition);
                Debug.Log($"Cylinder placed at {currentPosition} after {step + 1} steps.");
                return currentPosition;
            }

            Debug.LogError(
                $"Maximum steps reached. Could not place a cylinder starting from {startPoint} in direction {direction}.");
            return null;
        }

        private bool IsCollidingWithOtherCylinders(Vector3 position, float radius)
        {
            foreach (var spawnedPosition in _spawnedCylinderPositions.Where(spawnedPosition =>
                         Vector3.Distance(position, spawnedPosition) < radius * 2))
            {
                Debug.Log($"Position {position} is colliding with existing cylinder at {spawnedPosition}.");
                return true;
            }

            return false;
        }

        private void InstantiateCylinder(Vector3 position)
        {
            Instantiate(cylinderPrefab, position, Quaternion.identity, parentContainer);
            _spawnedCylinderPositions.Add(position);
        }

        private static Vector3 GetNextStep(Vector3 currentPosition, Vector3 direction, float stepSize)
        {
            Debug.DrawRay(currentPosition, direction.normalized * stepSize, Color.green, 60f);
            var nextPosition = currentPosition + direction.normalized * stepSize;
            return nextPosition;
        }

        private bool IsValidStep(Vector3 position, float radius, SpawnArea spawnArea)
        {
            if (!spawnArea.IsPointInside(position))
            {
                Debug.LogWarning($"Position {position} is outside the spawn area bounds.");
                return false;
            }

            if (!testPlate.IsColliding(position, radius) && !IsCollidingWithOtherCylinders(position, radius))
                return true;
            Debug.Log($"Position {position} is colliding. " +
                      $"TestPlate: {testPlate.IsColliding(position, radius)}, " +
                      $"Other Cylinders: {IsCollidingWithOtherCylinders(position, radius)}");

            return false;
        }
    }
}