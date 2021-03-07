using UnityEngine;

public class EnemyProjectile : MonoBehaviour
{  
    private void OnCollisionEnter(Collision collisionInfo)
    {
        //Disabling collision with anybody except player
        Physics.IgnoreLayerCollision(11, 8, true);
        Physics.IgnoreLayerCollision(11, 12, true);
        Destroy(gameObject, 0.0f);
    }

    private void OnTriggerEnter(Collider other)
    {
        // Detecting collision with PlayerShip
        if (other.CompareTag("Player"))
        { 
            Destroy(gameObject, 0.0f); 
        }
    }

    public void KillOldBullet()
    {
        // Destroying bullet after specified time
        Destroy(gameObject, 5f);
    }
}
