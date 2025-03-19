using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System.Collections;

public class GameManager : SingletonMonoBehavior<GameManager>
{
    [SerializeField] private int maxLives = 3;
    [SerializeField] private Ball ball;
    [SerializeField] private Transform bricksContainer;
    [SerializeField] private Text livesText;
    [SerializeField] private GameObject gameOverScreen;

    AudioManager audioManager;

    private int currentBrickCount;
    private int totalBrickCount;

    public int currentBrick;

    public TextMeshProUGUI brickText;

    private void Start()
    {
        UpdateLivesUI();
        if (gameOverScreen != null)
        {
            gameOverScreen.SetActive(false);
        }
    }

    private void OnEnable()
    {
        audioManager = GameObject.FindGameObjectWithTag("Audio").GetComponent<AudioManager>();
        InputHandler.Instance.OnFire.AddListener(FireBall);
        ball.ResetBall();
        totalBrickCount = bricksContainer.childCount;
        currentBrickCount = bricksContainer.childCount;
    }

    private void OnDisable()
    {
        InputHandler.Instance.OnFire.RemoveListener(FireBall);
    }

    private void FireBall()
    {
        ball.FireBall();
    }

    public void OnBrickDestroyed(Vector3 position)
    {
        // fire audio here
        audioManager.PlaySFX(audioManager.hitBlock);
        // implement particle effect here
        // add camera shake here
        currentBrickCount--;
        Debug.Log($"Destroyed Brick at {position}, {currentBrickCount}/{totalBrickCount} remaining");
        if(currentBrickCount == 0) SceneHandler.Instance.LoadNextScene();
    }

    public void KillBall()
    {
        maxLives--;
        // update lives on HUD here
        UpdateLivesUI();
        // game over UI if maxLives < 1, then exit to main menu after delay
        if (maxLives < 1)
        {
            StartCoroutine(GameOverSequence());
        }
        else
        {
            ball.ResetBall();
        }
    }

    public void AddBrick(int brickToAdd)
    {
            currentBrick += brickToAdd;

            brickText.text = "Score: " + currentBrick;
    }

    private void UpdateLivesUI()
{
    if (livesText != null)
    {
        livesText.text = "Lives: " + maxLives;
    }
}
    private IEnumerator GameOverSequence()
{
    Debug.Log("Game Over!");
    Time.timeScale = 0;
   if (gameOverScreen != null)
{
    gameOverScreen.SetActive(true);
}


    yield return new WaitForSecondsRealtime(1.5f);

    if (SceneHandler.Instance == null)
    {
        Debug.LogError("SceneHandler instance is null! Make sure SceneHandler exists in the scene.");
        yield break;
    }

    Time.timeScale = 1;
    Debug.Log("Loading Menu Scene...");
    SceneHandler.Instance.LoadMenuScene();
}
}
