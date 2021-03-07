using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyAsteroid : MonoBehaviour
{
    #region Parameters

    public GameObject bigRock;
    public GamePlay gamePlay;
    private float maxRotation;
    private float rotationZ;
    private Rigidbody rb;
    private Camera mainCam;
    private int generation;
    public GameObject explosion; 

    #endregion

    private void Start()
    { 
        mainCam = Camera.main;
        maxRotation = 95f; 
        rotationZ = Random.Range(-maxRotation, maxRotation);
        rb = bigRock.GetComponent<Rigidbody>();

        // Adjusting speeds and vectors for asteroid movement
        var speedX = Random.Range(200f, 800f);
        var selectorX = Random.Range(0, 2);
        float dirX = 0;

        if (selectorX == 1) { dirX = -1; }
        else { dirX = 1; }
        
        var finalSpeedX = speedX * dirX;
        rb.AddForce(transform.right * finalSpeedX);

        var speedY = Random.Range(200f, 800f);
        var selectorY = Random.Range(0, 2);
        float dirY = 0;
        if (selectorY == 1) { dirY = -1; }
        else { dirY = 1; }
        var finalSpeedY = speedY * dirY;
        rb.AddForce(transform.up * finalSpeedY);

        // Asteroids do not collide with other asteroids
        Physics2D.IgnoreLayerCollision(8, 8, true);
    }

    public void SetGeneration(int generation)
    {
        this.generation = generation;
    }

    private void Update()
    {
        // Asteroids movements
        bigRock.transform.Rotate(new Vector3(0, 0, rotationZ) * Time.deltaTime);
        CheckPosition();
        var dynamicMaxSpeed = 3f;
        rb.velocity = new Vector2(Mathf.Clamp(rb.velocity.x, -dynamicMaxSpeed, dynamicMaxSpeed),
            Mathf.Clamp(rb.velocity.y, -dynamicMaxSpeed, dynamicMaxSpeed)); 
    }

    private void OnCollisionEnter(Collision collidedObject)
    {
        //Disabling collision between asteroids
        Physics.IgnoreLayerCollision(8, 8, true);

        //Collide with bullets
        if (collidedObject.collider.name == "Projectile(Clone)")
        {
            if (generation < 3)
            {
                CreateSmallAsteriods(2);
            }

            Destroy(); 
        }

        //Collide with Player
        if (collidedObject.collider.name == "Player")
        {
            gamePlay.ReduceLives(); 
            
            if (generation < 3)
            {
                CreateSmallAsteriods(2);
            }

            Destroy();
        } 
    }

    private void CreateSmallAsteriods(int asteroidsNum)
    {  
        var newGeneration = generation + 1;

        //make an explosion
        Instantiate(explosion, transform.position, transform.rotation);

        //Creating smaller asteroids with next generation num
        for (var i = 1; i <= asteroidsNum; i++)
        {
            const float scaleSize = 0.5f;
            var asteroid = Instantiate(bigRock, new Vector3(transform.position.x, transform.position.y, 0f), transform.rotation);
            asteroid.transform.localScale = new Vector3(asteroid.transform.localScale.x * scaleSize,
                asteroid.transform.localScale.y * scaleSize, asteroid.transform.localScale.z * scaleSize);
            asteroid.GetComponent<EnemyAsteroid>().SetGeneration(newGeneration);
            asteroid.SetActive(true);
        } 
    }

    private void CheckPosition()
    {
        // Moving asteroids from opposite side of screen
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

    public void Destroy()
    {
        // launching Acteroid destroy from GamePlay (will add scores)
        gamePlay.AsteroidDestroyed();
        Destroy(gameObject, 0.01f);
    } 
}
