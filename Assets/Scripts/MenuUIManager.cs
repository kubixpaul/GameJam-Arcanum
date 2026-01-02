using UnityEngine;
using UnityEngine.SceneManagement;

public class MenuUIManager : MonoBehaviour
{
    public GameObject pauseMenu;
    
    private static bool _isPaused = false;
    public static bool IsDead = false;
    private void Start()
    {
        pauseMenu.SetActive(false);
    }

    private void PauseGame()
    {
        pauseMenu.SetActive(true);
        Time.timeScale = 0f;
        _isPaused = true;
    }
    
    public void ResumeGame()
    {
        pauseMenu.SetActive(false);
        Time.timeScale = 1f;
        _isPaused = false;
    }
    
    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            if (IsDead)
            {
                return;
            }
            if (_isPaused)
            {
                ResumeGame();
            }
            else
            {
                PauseGame();
            }
        }
    }

    public void GoToMainMenu()
    {
        Time.timeScale = 1f;
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex - 1);
    }

    public void Respawn()
    {
        Time.timeScale = 1f;
        IsDead = false;
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }
}
