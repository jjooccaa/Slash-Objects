using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    public List<GameObject> targets;
    public TextMeshProUGUI scoreText;
    public TextMeshProUGUI gameOverText;
    public TextMeshProUGUI livesText;
    public GameObject Panel;
    private AudioSource gameMusic;
    public Button restartButton;
    public Slider volumeSlider;
    public GameObject titleScreen;
    private int score;
    private int lives;
    private float spawnRate = 1.0f;
    public bool isGameActive;
    public bool gameIsPaused;

    // Start is called before the first frame update
    void Start()
    {
        // ignroe collision between boxes, they are all at layer 0
        //Physics.IgnoreLayerCollision(0, 0, true);

        gameMusic = GetComponent<AudioSource>();
        gameMusic.Play();
        gameMusic.volume = volumeSlider.value;
    }

    // Update is called once per frame
    void Update()
    {
        if(Input.GetKeyDown(KeyCode.Escape))
        {
            gameIsPaused = !gameIsPaused;
            PauseGame();
        }
    }

    IEnumerator SpawnTarget()
    {
        while(isGameActive)
        {
            yield return new WaitForSeconds(spawnRate);
            int index = Random.Range(0, targets.Count);
            Instantiate(targets[index]);

        }
    }

    public void UpdateScore(int scoreToAdd)
    {
        score += scoreToAdd;
        scoreText.text = "Score: " + score;
    }

    public void UpdateLives(int livesToChange)
    {
        lives += livesToChange;
        livesText.text = "Lives: " + lives;
        if (lives <= 0)
        {
            GameOver();
        }

    }

    public void GameOver()
    {
        gameOverText.gameObject.SetActive(true);
        restartButton.gameObject.SetActive(true);
        isGameActive = false;
    }

    public void RestartGame()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }

    public void ChangeVolume()
    {
        gameMusic.volume = volumeSlider.value;

    }

    void PauseGame()
    {
        if (gameIsPaused)
        {
            Time.timeScale = 0f;
            Panel.SetActive(true);
            
        }
        else
        {
            Time.timeScale = 1.0f;
            Panel.SetActive(false);
        }
    }

    void ResumeGame()
    {
        Time.timeScale = 1;
    }

    public void StartGame(int difficulty)
    {
        isGameActive = true;
        score = 0;

        spawnRate /= difficulty;


        StartCoroutine(SpawnTarget());
        UpdateScore(0);
        UpdateLives(3);

        titleScreen.gameObject.SetActive(false);
    }
}
