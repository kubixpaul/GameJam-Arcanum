using UnityEngine;
using UnityEngine.SceneManagement;

public class MenuButtons : MonoBehaviour
{
    [SerializeField] private GameObject creditsUI;

    private bool _activeCredits;
    public void PlayGame()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 1);
    }

    public void QuitGame()
    {
        Application.Quit();
    }

    public void Credits()
    {
        if (!_activeCredits)
        {
            creditsUI.SetActive(true);
            _activeCredits = true;
        }
        else
        {
            creditsUI.SetActive(false);
            _activeCredits  = false;
        }
    }
}
