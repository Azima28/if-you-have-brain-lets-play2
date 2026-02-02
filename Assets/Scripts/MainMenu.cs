using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenu : MonoBehaviour
{
    // MAIN MENU
    public void PlayGame()
    {
        SceneManager.LoadScene("Level");
    }

    public void QuitGame()
    {
        Application.Quit();
        Debug.Log("Game Keluar");
    }

    // LEVEL MENU
    public void LoadLevel1()
    {
        SceneManager.LoadScene("SampleScene");
    }
}
