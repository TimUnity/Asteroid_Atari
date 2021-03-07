using UnityEngine; 

public class EnemyShip : MonoBehaviour
{
    #region Parameters

    private Rigidbody rb;
    public Vector3 direction;
    public float shipMooveSpeed;
    public float shootingDelay;
    public float lastTimeShot = 0;
    public float bulletSpeed;
    public GameObject projectile;
    public GameObject explosion; 
    private Transform player; 

    #endregion

    private void Start()
    {
        rb = GetComponent<Rigidbody>();
        player = GameObject.FindGameObjectWithTag("Player").transform;
    }

    private void Update()
    {
        // Shooting at the PlayerShip
        if (Time.time > lastTimeShot + shootingDelay)
        {
            // Getting direction vector
            float angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg - 90f;
            Quaternion q = Quaternion.AngleAxis(angle, Vector3.forward);

            // Creating bullet
            GameObject newBullet = Instantiate(projectile, transform.position, q);
            newBullet.GetComponent<Rigidbody>().AddRelativeForce(new Vector2(0f, bulletSpeed));
            newBullet.GetComponent<EnemyProjectile>().KillOldBullet();
            lastTimeShot = Time.time;
        }
    } 

    private void FixedUpdate()
    {
        // Moving AlienShip to PlayerShip
        direction = (player.position - transform.position).normalized;
        rb.MovePosition(rb.position + (direction * shipMooveSpeed * Time.fixedDeltaTime)); 
    }

    private void OnCollisionEnter(Collision collidedObject)
    {
        //Disabling collision with anybody except player
        Physics.IgnoreLayerCollision(12, 8, true);
        Physics.IgnoreLayerCollision(12, 11, true);

        //Collide with bullets
        if (collidedObject.collider.name == "Projectile(Clone)")
        {
            //make an explosion
            Instantiate(explosion, transform.position, transform.rotation); 
            Destroy(gameObject, 0.01f);
        } 
    } 
}
