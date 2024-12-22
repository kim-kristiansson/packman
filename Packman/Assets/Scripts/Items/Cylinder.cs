using UnityEngine;

namespace Items
{
    [RequireComponent(typeof(CapsuleCollider))]
    public class Cylinder : MonoBehaviour
    {
        private CapsuleCollider _collider;

        private CapsuleCollider Collider => _collider != null ? _collider : _collider = GetComponent<CapsuleCollider>();

        public float Diameter => Collider.radius * 2;

        public float Radius => Collider.radius;

        public float Height => Collider.height; // Use this property instead of GetHeight()

        private void Awake()
        {
            _collider = GetComponent<CapsuleCollider>();
        }

        public void Spawn(Vector3 position, Transform parent = null)
        {
            Instantiate(this, position, Quaternion.identity, parent);
        }

        public void SetScale(float newDiameter, float newHeight)
        {
            Collider.radius = newDiameter / 2;
            Collider.height = newHeight;
            transform.localScale = Vector3.one; // Reset scale issues
        }
    }
}