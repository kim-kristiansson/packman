using UnityEngine;

namespace Fixtures
{
    public class FixtureController : MonoBehaviour
    {
        private FixtureBasePrefab _fixtureBase;
        [SerializeField] private FixtureStickPrefab stickPrefab;

        public void Initialize(FixtureBasePrefab fixtureBase)
        {
            _fixtureBase = fixtureBase;
        }

        public void GenerateSticks(MountPointTag tagFilter)
        {
            if (_fixtureBase == null)
            {
                Debug.LogError("FixtureBase is not initialized!");
                return;
            }

            if (stickPrefab == null)
            {
                Debug.LogError("StickPrefab is not assigned!");
                return;
            }

            var activeMountPoints = _fixtureBase.GetActiveMountPoints(tagFilter);

            foreach (var mountPoint in activeMountPoints)
            {
                Debug.Log(mountPoint.Top);
                Instantiate(stickPrefab, mountPoint.Top, mountPoint.Rotation, _fixtureBase.transform);
                Debug.Log($"Stick generated at MountPoint: {mountPoint.name}");
            }
        }
    }
}