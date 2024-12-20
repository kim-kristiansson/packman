using System;
using Core;
using UnityEngine;

namespace Fixtures
{
    [Flags]
    public enum MountPointTag
    {
        Default = 1 << 0,
        Rings = 1 << 1,
        Special = 1 << 2
    }

    public class MountPoint : BaseEntity
    {
        [Header("MountPoint Tags")]
        [SerializeField] private MountPointTag tags;

        public bool IsInTag(MountPointTag filter)
        {
            Debug.Log($"MountPoint '{name}' with Tags: {tags}, Filter: {filter}, Match: {(tags & filter) != 0}");
            return (tags & filter) != 0;
        }
    }
}