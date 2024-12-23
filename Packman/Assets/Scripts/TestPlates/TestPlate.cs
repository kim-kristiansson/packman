using SpawnAreas;
using UnityEngine;

namespace TestPlates
{
    public class TestPlate : MonoBehaviour
    {
        [Header("Spawn Area Settings")] [Tooltip("Prefab of the spawn area to be instantiated.")]
        public SpawnArea spawnAreaPrefab;

        private int _layerMask;

        private SpawnArea _spawnAreaInstance;

        private void Awake()
        {
            _layerMask = 1 << gameObject.layer;
            SpawnSpawnArea();
        }

        private void SpawnSpawnArea()
        {
            if (spawnAreaPrefab == null)
            {
                Debug.LogError("Spawn area prefab is not assigned!");
                return;
            }

            if (_spawnAreaInstance != null) Destroy(_spawnAreaInstance.gameObject);

            _spawnAreaInstance = Instantiate(spawnAreaPrefab, transform.localPosition, Quaternion.identity, transform);

            Debug.Log($"SpawnArea spawned at {_spawnAreaInstance.transform.position}.");
        }

        public SpawnArea GetSpawnAreaInstance()
        {
            return _spawnAreaInstance;
        }

        public bool IsColliding(Vector3 position, float radius)
        {
            var overlaps = Physics.OverlapSphere(position, radius, _layerMask);

            if (overlaps.Length <= 0) return false;
            Debug.Log($"Collision detected with TestPlate at {position}.");
            return true;
        }
    }
}