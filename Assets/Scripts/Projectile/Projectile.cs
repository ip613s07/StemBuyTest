namespace Projectile
{
    using Health;
    using Unity.Netcode;
    using UnityEngine;

    public class Projectile : NetworkBehaviour
    {
        [SerializeField]
        private int damage = 20;
        
        [SerializeField]
        private float speed = 10;
        
        [SerializeField]
        private Rigidbody2D body;

        private Vector2 _direction;
        
        private void FixedUpdate()
        {
            var distance = speed * Time.fixedDeltaTime;
            var delta = _direction * distance;
            var newPosition = body.position + delta;
            
            body.MovePosition(newPosition);
        }

        private void OnCollisionEnter2D(Collision2D other)
        {
            if (other.collider.TryGetComponent(out ITakeDamage takeDamage))
            {
                takeDamage.TakeDamage(damage);
            }

            if (other.collider.TryGetComponent(out IStopProjectile _))
            {
                NetworkObject.Despawn();
            }
        }

        public void Initialize(Vector2 direction) 
        {
            _direction = direction;

            if (NetworkManager.Singleton.IsServer)
            {
                NetworkObject.Spawn();
            }
        }
    }
}
