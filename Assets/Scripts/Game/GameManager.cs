using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class GameManager : MonoBehaviour
{
    private GameState state;
    private int score = 0;
    private float totalTime = 60.0f;
    private float timeRemaining = 60.0f;
    private float timeDelta = 0.0f;

    [Header("Main Menu")]
    [SerializeField] private GameObject mainMenu = null;

    [Header("Pause Menu")]
    [SerializeField] private GameObject pauseMenu = null;

    [Header("Game GUI")]
    [SerializeField] private GameObject gameGUI = null;
    [SerializeField] private TextMeshProUGUI scoreText = null;
    [SerializeField] private TextMeshProUGUI timerText = null;
    [SerializeField] private Image timerBar = null;

    [Header("Game Over GUI")]
    [SerializeField] private GameObject gameOverMenu = null;
    [SerializeField] private TextMeshProUGUI highScoreText = null;
    [SerializeField] private TextMeshProUGUI newHighScoreText = null;
    [SerializeField] private TextMeshProUGUI gameScoreText = null;

    [Header("Game")]
    [SerializeField] private WeaponSpawner weaponSpawner = null;

    void Start()
    {
        timeRemaining = totalTime;
        SetState(GameState.MAIN_MENU);
    }

    void Update()
    {
        if (state == GameState.RUNNING)
        {
            timeDelta += Time.deltaTime;
            if (timeDelta >= 1.0f)
            {
                timeDelta = 0.0f;
                timeRemaining--;
                UpdateTimer();

                if (timeRemaining - 1 <= -1)
                {
                    SetState(GameState.GAME_OVER);
                }
            }
        }

        if (Input.GetKeyDown(KeyCode.Escape))
        {
            if (state == GameState.PAUSE_MENU)
            {
                SetState(GameState.RUNNING);
            }
            else if (state == GameState.RUNNING)
            {
                SetState(GameState.PAUSE_MENU);
            }
        }
    }

    public GameState GetState()
    {
        return state;
    }

    public void SetState(GameState state)
    {
        this.state = state;

        if (state == GameState.MAIN_MENU)
        {
            mainMenu.SetActive(true);
            pauseMenu.SetActive(false);
            gameGUI.SetActive(false);
            gameOverMenu.SetActive(false);
            Time.timeScale = 0.0f;
            Cursor.lockState = CursorLockMode.None;
            Cursor.visible = true;
        }
        else if (state == GameState.PAUSE_MENU)
        {
            mainMenu.SetActive(false);
            pauseMenu.SetActive(true);
            gameOverMenu.SetActive(false);
            Time.timeScale = 0.0f;
            Cursor.lockState = CursorLockMode.None;
            Cursor.visible = true;
        }
        else if (state == GameState.RUNNING)
        {
            mainMenu.SetActive(false);
            pauseMenu.SetActive(false);
            gameGUI.SetActive(true);
            gameOverMenu.SetActive(false);
            Time.timeScale = 1.0f;
            Cursor.lockState = CursorLockMode.Locked;
            Cursor.visible = false;
        }
        else
        {
            mainMenu.SetActive(false);
            pauseMenu.SetActive(false);
            gameGUI.SetActive(false);
            gameOverMenu.SetActive(true);
            Time.timeScale = 0.0f;
            Cursor.lockState = CursorLockMode.None;
            Cursor.visible = true;
            BuildGameOverScreen();
        }
    }

    public void AddPoints(int points)
    {
        score += points;
        scoreText.text = "Score: " + score;
    }

    public void StartGame()
    {
        SetState(GameState.RUNNING);
        weaponSpawner.SpawnWeapons();
    }

    public void ResumeGame()
    {
        SetState(GameState.RUNNING);
    }

    public void QuitGame()
    {
        Application.Quit();
    }

    private void UpdateTimer()
    {
        timerText.text = "Time Remaining: " + timeRemaining + "s";
        timerBar.fillAmount = timeRemaining / totalTime;
    }

    private void BuildGameOverScreen()
    {
        if (!PlayerPrefs.HasKey("Highscore"))
        {
            PlayerPrefs.SetInt("Highscore", score);
        }

        int highscore = PlayerPrefs.GetInt("Highscore");

        if (score > highscore)
        {
            PlayerPrefs.SetInt("Highscore", score);
        }

        highScoreText.text = "Highscore: " + highscore;
        newHighScoreText.text = (score >= highscore) ? "Score: " + score : "";
        gameScoreText.text = (score >= highscore) ? "" : "Score: " + score;
    }
}
