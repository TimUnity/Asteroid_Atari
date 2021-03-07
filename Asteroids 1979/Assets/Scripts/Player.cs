using UnityEngine;

public class Player : MonoBehaviour
{
    #region Parameters

    private float thrust = 6f;
    private float rotationSpeed = 180f;
    private float maxSpeed = 4.5f;
    private float timer;
    public float bulletFireRate = 0.1f;
    public GameObject Bullet;
    private Camera mainCam;
    private Rigidbody rb;
    public GamePlay gamePlay;

    #endregion 

    private void Start()
    {
        mainCam = Camera.main;
        rb = GetComponent<Rigidbody>();
        Bullet.SetActive(false);
    }

    private void FixedUpdate()
    {  
        PlayerShipMove();
        CheckPosition();

        // Shooting by timer rate
        if (Input.GetButton("Fire1"))
        {
            if (timer > bulletFireRate)
            {
                Shoot();
                timer = 0f;
            }
        }

        //timers Refreshing
        if (!Input.GetButton("Fire1") && !Input.GetButton("Fire2"))
        {
            timer = 0.6f; 
        }
    }

    private void Update()
    {
        // Updating fire timers
        timer = timer + Time.deltaTime;
    }

    private void OnTriggerEnter(Collider other)
    {
        // reducing lives of PlayerShip
        if (other.CompareTag("EnemyBullet"))
        {
            gamePlay.ReduceLives();  
        }
    }

    private void PlayerShipMove()
    {
        // PlayerShip movement
        transform.Rotate(0, 0, Input.GetAxis("Horizontal") * rotationSpeed * Time.deltaTime);
        rb.AddForce(transform.up * thrust * Input.GetAxis("Vertical"));
        rb.velocity = new Vector2(Mathf.Clamp(rb.velocity.x, -maxSpeed, maxSpeed), Mathf.Clamp(rb.velocity.y, -maxSpeed, maxSpeed));
    }

    private void CheckPosition()
    { 
        // Moving PlayerShip from opposite side of screen
        float sceneWidth = mainCam.orthographicSize * 2 * mainCam.aspect;
        float sceneHeight = mainCam.orthographicSize * 2;

        float sceneRightEdge = sceneWidth / 2;
        float sceneLeftEdge = sceneRightEdge * -1;
        float sceneTopEdge = sceneHeight / 2;
        float sceneBottomEdge = sceneTopEdge * -1;

        if (transform.position.x > sceneRightEdge) { transform.position = new Vector2(sceneLeftEdge, transform.position.y); }
        if (transform.position.x < sceneLeftEdge) { transform.position = new Vector2(sceneRightEdge, transform.position.y); }
        if (transform.position.y > sceneTopEdge) { transform.position = new Vector2(transform.position.x, sceneBottomEdge); }
        if (transform.position.y < sceneBottomEdge) { transform.position = new Vector2(transform.position.x, sceneTopEdge); }
    } 

    private void Shoot()
    {
        // Creating a bullet with fly direction
        GameObject BulletClone = Instantiate(Bullet, new Vector2(Bullet.transform.position.x, Bullet.transform.position.y), transform.rotation);
        BulletClone.SetActive(true);
        BulletClone.GetComponent<Projectile>().KillOldBullet();
        BulletClone.GetComponent<Rigidbody>().AddForce(transform.up * 350); 
    } 
}
