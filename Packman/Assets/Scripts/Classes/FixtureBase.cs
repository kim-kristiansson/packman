using System;
using System.Collections.Generic;
using UnityEngine;

namespace Classes
{
    public class FixtureBase : PhysicalEntity
    {
        [SerializeField]
        private MountPoint[] mountPoints = new MountPoint[14]; 
        public MountPoint[] MountPoints => mountPoints;
    }
}
