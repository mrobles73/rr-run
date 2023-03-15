using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenu : MonoBehaviour
{
  private string _firstLevel = "Level1";

  private void Awake() => GameManager.SetIsInMenu(true);

  public void PlayGame()
  {
    SceneManager.LoadScene(this._firstLevel);
    GameManager.ResetTotalGameTime();
    GameManager.SetIsInMenu(false);
    AudioManager.callLevelAudio();
  }

  public void QuitGame()
  {
    Debug.Log((object) "QUIT");
    Application.Quit();
  }
}