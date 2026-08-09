using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenuController : MonoBehaviour
{
    [SerializeField] private GameObject Setting;
    [SerializeField] private GameObject PlayBtn;
    [SerializeField] private GameObject OutGameBtn;
    [SerializeField] private GameObject SettingBtn;
    [SerializeField] private AudioManager audioControl;
    void Start()
    {
        Setting.SetActive(false);
        PlayBtn.SetActive(true);
        OutGameBtn.SetActive(true);
        SettingBtn.SetActive(true);
        audioControl.PlayMusic();
    }
    public void SettingUI()
    {
        PlayBtn.SetActive(false);
        OutGameBtn.SetActive(false);
        SettingBtn.SetActive(false);
        Setting.SetActive(true);
    }
    public void Close()
    {
        Setting.SetActive(false);
        PlayBtn.SetActive(true);
        OutGameBtn.SetActive(true);
        SettingBtn.SetActive(true);
    }
    public void PlayGame()
    {
        SceneManager.LoadScene("ChapterSelect");
    }
    public void QuitGame()
    {
        Application.Quit();
        #if UNITY_EDITOR
        UnityEditor.EditorApplication.isPlaying = false;
        #endif
    }
}
