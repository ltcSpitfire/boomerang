using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class GameManager : MonoBehaviour
{
    public bool gameHasEnded = false;
    public GameObject gameOverUI;

    public void GameOver()
    {
        if (gameHasEnded == false)
        {
            gameHasEnded = true;
            gameOverUI.SetActive(true);
            Debug.Log("Game Over");
        }
    }

    public void Restart()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
        Debug.Log("Game Restarted");
    }

    public void QuitGame()
    {
        Application.Quit();
    }
}
