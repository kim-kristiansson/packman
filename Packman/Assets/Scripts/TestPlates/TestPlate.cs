using SpawnAreas;
using UnityEngine;

namespace TestPlates
{
    public class TestPlate : MonoBehaviour
    {
        [Header("Spawn Area Settings")] [Tooltip("Prefab of the spawn area to be instantiated.")]
        public SpawnArea spawnAreaPrefab;

        private SpawnArea _spawnAreaInstance;

        private void Awake()
        {
            SpawnSpawnArea();
        }

        private void SpawnSpawnArea()
        {
            if (spawnAreaPrefab == null)
            {
                Debug.LogError("Spawn area prefab is not assigned!");
                return;
            }

            // If the spawn area already exists, destroy it
            if (_spawnAreaInstance != null) Destroy(_spawnAreaInstance.gameObject);

            // Spawn the spawn area in the center of the TestPlate
            _spawnAreaInstance = Instantiate(spawnAreaPrefab, transform.localPosition, Quaternion.identity, transform);

            Debug.Log($"SpawnArea spawned at {_spawnAreaInstance.transform.position}.");
        }

        public SpawnArea GetSpawnAreaInstance()
        {
            return _spawnAreaInstance;
        }
    }
}