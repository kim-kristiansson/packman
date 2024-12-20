using Fixtures;
using UnityEngine;

namespace Managers
{
    public class FixtureSpawner : MonoBehaviour
    {
        [Header("Prefab References")]
        [SerializeField] private FixtureBasePrefab fixtureBasePrefab;
        [SerializeField] private FixtureController fixtureControllerPrefab;

        [Header("Spawn Settings")]
        public Transform spawnLocation;

        private FixtureController _currentController;

        public void SpawnFixture()
        {
            if (_currentController != null)
            {
                Destroy(_currentController.gameObject);
                Debug.Log("Destroyed previous fixture.");
            }

            var controllerInstance = Instantiate(fixtureControllerPrefab, spawnLocation.position, spawnLocation.rotation);
            var baseInstance = Instantiate(fixtureBasePrefab, spawnLocation.position, spawnLocation.rotation, controllerInstance.transform);

            controllerInstance.Initialize(baseInstance);
            _currentController = controllerInstance;
            
            _currentController.GenerateSticks(MountPointTag.Rings);

            Debug.Log("Fixture spawned and initialized.");
        }
    }
}