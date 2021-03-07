using UnityEngine;

public class Projectile : MonoBehaviour
{
    #region Parameters

    private Camera mainCam;
    private Rigidbody rb;
    [Tooltip("If checked, bullets will fly back from opposite side")]
    public bool superBullet;

    #endregion

    private void Start()
    {
        GetComponent<Rigidbody>().AddForce(transform.up * 350);
        rb = GetComponent<Rigidbody>();
        mainCam = Camera.main;
    }

    public void KillOldBullet()
    {
        Destroy(gameObject, 1.1f);
    }

    private void OnCollisionEnter(Collision collisionInfo)
    {
        Physics.IgnoreLayerCollision(10, 9, true);
        Destroy(gameObject, 0.0f);
    }

    private void FixedUpdate()
    {
        if (superBullet)
        {
            CheckPosition();
        }
    }

    private void CheckPosition()
    {
        // Moving bullets from opposit side of screen
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
}
