using Core;
using UnityEngine;

namespace Fixtures
{
    public class FixtureStickPrefab : PhysicalEntity
    {
        private void Start()
        {
            Debug.Log($"FixtureStick instantiated at position: {transform.position}");
        }
    }
}