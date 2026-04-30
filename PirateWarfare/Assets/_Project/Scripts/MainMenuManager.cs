using UnityEditor;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class MainMenuManager : MonoBehaviour
{

    public Button newGame, loadGame, quitApp, creditsButton;
    public GameObject creditsPopup;
    public SaveManager tempShopTester;
    void Start()
    {
        newGame.onClick.AddListener(() => {
            SaveManager.ResetGame();
            Time.timeScale = 1f; //if loading from paused state, the time scale will still be 0 at this point!
            tempShopTester.LoadGame();
            SceneManager.LoadScene("World Map");
        });
        loadGame.onClick.AddListener(() => {
            Time.timeScale = 1f; //if loading from paused state, the time scale will still be 0 at this point!
            tempShopTester.LoadGame();
            SceneManager.LoadScene("World Map");
        });
        quitApp.onClick.AddListener(() => {
            #if UNITY_EDITOR
            EditorApplication.isPlaying = false;
            #endif
            Application.Quit();
        });
        creditsButton.onClick.AddListener(() =>
        {
            creditsPopup.SetActive(!creditsPopup.activeSelf);
        });
    }


}
