using UnityEngine;

namespace Assets.Scripts.System
{
    public class PhysicsBoxController : MonoBehaviour
    {
        public Collider2D Collider { get; private set; }

        void Awake()
        {
            Collider = transform.GetComponent<Collider2D>();
        }
        
    }
}