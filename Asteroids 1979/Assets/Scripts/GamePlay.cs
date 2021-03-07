using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class GamePlay : MonoBehaviour
{
    #region Parameters
    public GameObject[] Asteroids;  
    public GameObject Live1;
    public GameObject Live2;
    public GameObject Live3;
    public GameObject GameOverButton; 
    public GameObject Scores;
    public GameObject AlienShip;
    public GameObject[] AlienShipStartPoints;

    public AudioClip Bip;
    public AudioClip Bop;

    public float BackSoundsDelay;
    private float LastBackSoundPlayed = 0;
    private int LastBackSoundPlayedIndex = 0; 

    private float LastShipSpawned = 0;
    public float AlienShipSpawningDelay;

    private int ScoresCount;
    private int playerLivesCount;

    private int asteroidsStartCount;
    private bool allAsteroidsOffScreen;
    private int levelAsteroidNum;

    private Camera mainCam;
    private int asteroidLife;
    public bool AsteroindsON;
    #endregion 

    private void Start()
    {
        mainCam = Camera.main;
        playerLivesCount = 3;
        ScoresCount = 0;

        // disabling asteroid samples
        foreach (var item in Asteroids) { item.SetActive(false); }   
        
        // Creating asteroids
        if (AsteroindsON) { CreateAsteroids(Asteroids.Length-2); }
        
    }

    private void Update()
    {
        if (asteroidLife <= 0)
        {
            asteroidLife = 6;

            if (AsteroindsON)
            {
                CreateAsteroids(1);
            }
        }

        float sceneWidth = mainCam.orthographicSize * 2 * mainCam.aspect;
        float sceneHeight = mainCam.orthographicSize * 2;
        float sceneRightEdge = sceneWidth / 2;
        float sceneLeftEdge = sceneRightEdge * -1;
        float sceneTopEdge = sceneHeight / 2;
        float sceneBottomEdge = sceneTopEdge * -1;

        allAsteroidsOffScreen = true;

        // Playing background sounds by timer
        if (Time.time > LastBackSoundPlayed + BackSoundsDelay)
        {
            if (LastBackSoundPlayedIndex == 0)
            {
                AudioSource.PlayClipAtPoint(Bip, gameObject.transform.position, 6f);
                LastBackSoundPlayedIndex++;
            }
            else
            {
                AudioSource.PlayClipAtPoint(Bop, gameObject.transform.position, 6f);
                LastBackSoundPlayedIndex--;
            }

            LastBackSoundPlayed = Time.time;
        }

        // Spawning AlienShips by timer
        if (Time.time > AlienShipSpawningDelay + LastShipSpawned)
        {
            AlienShipSpawn();
            LastShipSpawned = Time.time;
        }

        if (Input.GetKeyUp(KeyCode.Escape))
        {
            Application.Quit();
        }
    }

    public void PlayerLivesCounter(int value)
    {
        // Calculating PlayerShip lives count
        if (playerLivesCount > 0)
        {
            playerLivesCount += value;

            switch (playerLivesCount)
            {
                case 3:
                    Live1.SetActive(true); Live2.SetActive(true); Live3.SetActive(true);
                    break;
                case 2:
                    Live1.SetActive(true); Live2.SetActive(true); Live3.SetActive(false);
                    break;
                case 1:
                    Live1.SetActive(true); Live2.SetActive(false); Live3.SetActive(false);
                    break;
            }
        }

        // Game over operations
        if (playerLivesCount <= 0)
        {
            playerLivesCount = 0; Live1.SetActive(false); Live2.SetActive(false); Live3.SetActive(false);
            GameOverButton.SetActive(true);
            Time.timeScale = 0;
        }
    }

    private void CreateAsteroids(float asteroidsNum)
    {
        // Creating new asteroids with generation setting
        for (int i = 1; i <= asteroidsNum; i++)
        {
            GameObject AsteroidClone = Instantiate(Asteroids[i-1], new Vector2(Random.Range(-10, 10), 6f), transform.rotation);
            AsteroidClone.GetComponent<EnemyAsteroid>().SetGeneration(1);
            AsteroidClone.SetActive(true);
        }
    }

    public void ReduceLives()
    { 
        PlayerLivesCounter(-1);
    }

    public void AsteroidDestroyed()
    { 
        asteroidLife--;
        // adding scores after asteroid hited
        ScoreCounter();
    }

    public void ScoreCounter()
    {
        ScoresCount += 150;
        Scores.GetComponent<Text>().text = ScoresCount.ToString();
    }

    public void AlienShipSpawn()
    {
        // Spawning AlienShip randomly at one of the AlienShipStartPoints
        var randomPoint = Random.Range(0, 4);
        var startPosition = AlienShipStartPoints[randomPoint].transform.position;  
        Instantiate(AlienShip, startPosition, transform.rotation);
    }

    public void GameRestart()
    {
        Time.timeScale = 1;
        SceneManager.LoadScene("Level1");
    }

    public int StartLevelAsteroidsNum
    {
        get { return asteroidsStartCount; }
    }

    public bool AllAsteroidsOffScreen
    {
        get { return allAsteroidsOffScreen; }
    }
}
