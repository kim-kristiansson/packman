using UnityEngine;

namespace Core
{
    public abstract class BaseEntity : MonoBehaviour
    {
        public Vector3 Position
        {
            get => transform.position;
            set => transform.position = value;
        }

        public Quaternion Rotation
        {
            get => transform.rotation;
            set => transform.rotation = value;
        }
        
        private Renderer ObjectRenderer => GetComponent<Renderer>();

        public Vector3 Bottom
        {
            get
            {
                if (ObjectRenderer == null)
                {
                    Debug.LogWarning($"Renderer not found on {name}. Returning object's position as Bottom.");
                    return transform.position;
                }

                var bounds = ObjectRenderer.bounds;
                return new Vector3(bounds.center.x, bounds.min.y, bounds.center.z);
            }
        }

        public Vector3 Top
        {
            get
            {
                if (ObjectRenderer == null)
                {
                    Debug.LogWarning($"Renderer not found on {name}. Returning object's position as Top.");
                    return transform.position;
                }

                var bounds = ObjectRenderer.bounds;
                return new Vector3(bounds.center.x, bounds.max.y, bounds.center.z);
            }
        }
    }
}