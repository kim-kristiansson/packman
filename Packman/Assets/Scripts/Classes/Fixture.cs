using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace Classes
{
    public class Fixture : MonoBehaviour
    {
        public FixtureBase FixtureBase { get; set; }
        public FixtureStick FixtureStick { get; set; }
        private List<FixtureStick> GeneratedSticks { get; set; } = new List<FixtureStick>();

        public void GenerateSticks()
        {
            if (FixtureBase == null || FixtureBase.MountPoints == null || FixtureStick == null)
            {
                Debug.LogWarning("FixtureBase, MountPoints, or FixtureStickPrefab is not set!");
                return;
            }

            foreach (var stick in GeneratedSticks)
            {
                Destroy(stick);                
            }

            foreach (var stick in from mountPoint in FixtureBase.MountPoints where mountPoint == null select Instantiate(FixtureStick, mountPoint.Position, mountPoint.Rotation, transform))
            {
                GeneratedSticks.Add(stick);
            }
        }
    }
}