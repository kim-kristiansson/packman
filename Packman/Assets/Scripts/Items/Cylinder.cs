using System;
using UnityEngine;

namespace Items
{
    [RequireComponent(typeof(CapsuleCollider))]
    public class Cylinder : MonoBehaviour
    {
        private CapsuleCollider _collider;

        // Use a property to ensure the collider is always properly referenced
        private CapsuleCollider Collider
        {
            get
            {
                if (_collider != null) return _collider;
                _collider = GetComponent<CapsuleCollider>();
                if (_collider == null)
                    throw new Exception("Cylinder prefab does not have a CapsuleCollider component!");

                return _collider;
            }
        }

        public float Diameter => Collider.radius * 2;

        public float Radius => Collider.radius;

        public float Height => Collider.height;

        private void Awake()
        {
            // Ensure the CapsuleCollider component is initialized
            _collider = GetComponent<CapsuleCollider>();
        }

        public void InstantiateAt(Vector3 position, Transform parent = null)
        {
            Instantiate(this, position, Quaternion.identity, parent);
        }

        public void SetScale(float newDiameter, float newHeight)
        {
            // Adjust the collider and the transform scale to fit the desired dimensions
            Collider.radius = newDiameter / 2;
            Collider.height = newHeight;

            transform.localScale = Vector3.one; // Ensure no unwanted scaling issues
        }
    }
}