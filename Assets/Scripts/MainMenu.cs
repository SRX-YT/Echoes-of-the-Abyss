using UnityEngine;
using UnityEngine.SceneManagement;

public class Menu : MonoBehaviour
{
    private void Start()
    {
        Application.targetFrameRate = 300;
    }

    public void StartGame()
    {
        SceneManager.LoadScene(1);
    }

    public void LoadGame()
    {

    }

    public void Settings()
    {

    }

    public void Quit()
    {
        Application.Quit();
    }
}
