using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    [SerializeField] private GameObject gameOverUI;
    [SerializeField] private GameObject gameWinUI;
    [SerializeField] private GameObject gamePauseUI;
    [SerializeField] private GameObject playGuideUI;
    [SerializeField] private GameObject pauseBtn;
    [SerializeField] private GameObject guideBtn;
    private bool isGameOver = false;
    private bool isGameWin = false;

    //UI Control
    public GameObject BlockChain_Map2;
    public GameObject BlockChain_Map3;
    private bool Map1_Completed = false;
    private bool Map2_Completed = false;
    private bool Map3_Completed = false;
    void Start()
    {
        Map1_Completed = PlayerPrefs.GetInt("Map1_Completed", 0) == 1;
        Map2_Completed = PlayerPrefs.GetInt("Map2_Completed", 0) == 1;
        Map3_Completed = PlayerPrefs.GetInt("Map3_Completed", 0) == 1;

        if (gameOverUI != null) gameOverUI.SetActive(false);
        if (gameWinUI != null) gameWinUI.SetActive(false);
        if (gamePauseUI != null) gamePauseUI.SetActive(false);
        if (playGuideUI != null) playGuideUI.SetActive(false);

        UpdateChainUI();
    }
    private void UpdateChainUI()
    {
        if (BlockChain_Map2 != null && Map1_Completed)
            BlockChain_Map2.SetActive(false);
            
        if (BlockChain_Map3 != null && Map2_Completed)
            BlockChain_Map3.SetActive(false);
    }
    public void GamePause()
    {
        Time.timeScale = 0;
        pauseBtn?.SetActive(false);
        guideBtn?.SetActive(false);
        gamePauseUI?.SetActive(true);
    }
    public void Continue()
    {
        Time.timeScale = 1;
        pauseBtn?.SetActive(true);
        guideBtn?.SetActive(true);
        playGuideUI?.SetActive(false);
        gamePauseUI?.SetActive(false);
    }
    public void PlayGuide()
    {
        Time.timeScale = 0;
        pauseBtn?.SetActive(false);
        guideBtn?.SetActive(false);
        playGuideUI?.SetActive(true);
    }
    public void GameOver()
    {
        isGameOver = true;
        gameOverUI?.SetActive(true);
    }
    public void GameWin()
    {
        isGameWin = true;
        Time.timeScale = 0;
        gameWinUI?.SetActive(true);
    }
    public void RestartGame()
    {
        isGameOver = false;
        isGameWin = false;
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
        Time.timeScale = 1;
        SceneManager.LoadScene("ChapterSelect");
    }
    public void BackToMainMenu()
    {
        Time.timeScale = 1;
        SceneManager.LoadScene("MainMenu");
    }
    public void LoadSceneChap1()
    {
        Time.timeScale = 1;
        SceneManager.LoadScene("Chapter1");
    }
    public void LoadSceneChap2()
    {
        if (Map1_Completed)
        {
            Time.timeScale = 1;
            SceneManager.LoadScene("Chapter2");
        }
    }
    public void LoadSceneChap3()
    {
        if (Map2_Completed)
        {
            Time.timeScale = 1;
            SceneManager.LoadScene("Chapter3");
        }
    }
    public void LoadSceneChap4()
    {
        if(Map2_Completed && Map3_Completed)
        {
            Time.timeScale = 1;
            SceneManager.LoadScene("Chapter4");
        }
    }
    public void CompleteMap_1()
    {
        Map1_Completed = true;
        PlayerPrefs.SetInt("Map1_Completed", 1);
        PlayerPrefs.Save();
        UpdateChainUI();
    }
    public void CompleteMap_2()
    {
        Map2_Completed = true;
        PlayerPrefs.SetInt("Map2_Completed", 1);
        PlayerPrefs.Save();
        UpdateChainUI();
    }
    public void CompleteMap_3()
    {
        Map3_Completed = true;
        PlayerPrefs.SetInt("Map3_Completed", 1);
        PlayerPrefs.Save();
        UpdateChainUI();
    }
    public void ResetData()
    {
        PlayerPrefs.DeleteAll();
        Map1_Completed = false;
        Map2_Completed = false;
        Map3_Completed = false;
        UpdateChainUI();
    }
}
