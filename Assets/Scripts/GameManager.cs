using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    [SerializeField] private GameObject gameOverUI;
    [SerializeField] private GameObject gameWinUI;
    [SerializeField] private GameObject gamePauseUI;
    [SerializeField] private GameObject playGuideUI;
    [SerializeField] private GameObject PauseBtn;
    [SerializeField] private GameObject GuideBtn;
    private bool isGameOver = false;
    private bool isGameWin = false;
    private bool isGamePause = false;
    private bool isTouchGuide = false;
    void Start()
    {

        gameOverUI.SetActive(false);
        gameWinUI.SetActive(false);
        gamePauseUI.SetActive(false);
        playGuideUI.SetActive(false);
    }

    void Update()
    {
        if (isGameOver || isGameWin) return;
        if (isGamePause)
        {
            GamePause();
        }
        else if (isTouchGuide)
        {
            PlayGuide();
        }
        else
        {
            Continue();
        }
    }
    public void GamePause()
    {
        isGamePause = true;
        Time.timeScale = 0;
        PauseBtn.SetActive(false);
        GuideBtn.SetActive(false);
        gamePauseUI.SetActive(true);
    }
    public void Continue()
    {
        isGamePause = false;
        isTouchGuide = false;
        Time.timeScale = 1;
        PauseBtn.SetActive(true);
        GuideBtn.SetActive(true);
        playGuideUI.SetActive(false);
        gamePauseUI.SetActive(false);
    }
    public void PlayGuide()
    {
        isTouchGuide = true;
        Time.timeScale = 0;
        PauseBtn.SetActive(false);
        GuideBtn.SetActive(false);
        playGuideUI.SetActive(true);
    }
    public void GameOver()
    {
        isGameOver = true;
        gameOverUI.SetActive(true);
    }
    public void GameWin()
    {
        isGameWin = true;
        Time.timeScale = 0;
        gameWinUI.SetActive(true);
    }
    public void RestartGame()
    {
        isGameOver = false;
        Time.timeScale = 1;
        string currentScene = SceneManager.GetActiveScene().name;
        SceneManager.LoadScene(currentScene);
    }
    public bool IsGameOver()
    {
        return isGameOver;
    }
    public bool IsGameWin()
    {
        return isGameWin;
    }
    public void MenuUp()
    {
        SceneManager.LoadScene("ChapterSelect");
        Time.timeScale = 1;
    }
    public void BackToMainMenu()
    {
        SceneManager.LoadScene("MainMenu");
        Time.timeScale = 1;
    }
    public void loadSceneChap1()
    {
        SceneManager.LoadScene("Chapter1");
        Time.timeScale = 1;
    }
}
