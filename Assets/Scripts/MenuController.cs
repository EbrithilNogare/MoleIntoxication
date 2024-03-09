using UnityEngine;
using UnityEngine.SceneManagement;

public class MenuController : MonoBehaviour
{
    public void OnGameClick()
    {
        SceneManager.LoadScene("SampleScene", LoadSceneMode.Single);
    }

    public void OnTutorialClick()
    {
        SceneManager.LoadScene("Tutorial", LoadSceneMode.Single);
    }

    public void OnCreditsClick()
    {
        SceneManager.LoadScene("Credits", LoadSceneMode.Single);
    }
    public void OnMenuClick()
    {
        Debug.Log("here");
        SceneManager.LoadScene("Menu", LoadSceneMode.Single);
    }
}
