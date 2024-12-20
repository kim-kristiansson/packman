using System.Collections.Generic;
using System.Linq;
using Core;
using UnityEngine;

namespace Fixtures
{
    public class FixtureBasePrefab : PhysicalEntity
    {
        private readonly List<MountPoint> _mountPoints = new();

        public void Awake()
        {
            _mountPoints.Clear();
            foreach (Transform child in transform)
            {
                if (!child.TryGetComponent<MountPoint>(out var mountPoint)) continue;
                _mountPoints.Add(mountPoint);
                Debug.Log($"MountPoint '{mountPoint.name}' added");
            }

            Debug.Log($"FixtureBase initialized with {_mountPoints.Count} MountPoints.");
        }
        
        public List<MountPoint> GetActiveMountPoints(MountPointTag filter)
        {
            var activeMountPoints = _mountPoints.Where(mountPoint => mountPoint.IsInTag(filter)).ToList();
            Debug.Log($"GetActiveMountPoints() found {activeMountPoints.Count} active MountPoints for filter: {filter}");
            return activeMountPoints;
        }
    }
}