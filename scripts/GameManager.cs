using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
  private static GameManager current;
  public float deathSequenceDuration = 1f;
  public float transitionTime = 3f;
  private Door _doorClosed;
  private bool _doorOpen;
  private float _totalGameTime;
  private bool _isGameOver;
  private SceneFader _sceneFader;
  private bool _isLevelOver;
  private bool _isInMenu;
  private bool _isPaused;
  public GameObject pauseMenuUI;
  private string _menuSceneName = "Menu";

  private void Awake()
  {
    if ((Object) GameManager.current != (Object) null && (Object) GameManager.current != (Object) this)
    {
      Object.Destroy((Object) this.gameObject);
    }
    else
    {
      GameManager.current = this;
      Object.DontDestroyOnLoad((Object) this.gameObject);
    }
  }

  private void Update()
  {
    if (this._isGameOver || this._isLevelOver || this._isInMenu)
      return;
    this.PauseCheck();
    if (this._isPaused)
      return;
    this._totalGameTime += Time.deltaTime * 1000f;
    UIManager.UpdateTimeUI(this._totalGameTime);
  }

  public static bool IsGameOver() => !((Object) GameManager.current == (Object) null) && GameManager.current._isGameOver;

  public static bool IsLevelOver() => !((Object) GameManager.current == (Object) null) && GameManager.current._isLevelOver;

  public static bool isGamePaused() => !((Object) GameManager.current == (Object) null) && GameManager.current._isPaused;

  public static void SetIsInMenu(bool isInMenu) => GameManager.current._isInMenu = isInMenu;

  public static void RegisterSceneFader(SceneFader fader)
  {
    if ((Object) GameManager.current == (Object) null)
      return;
    GameManager.current._sceneFader = fader;
  }

  public static void RegisterDoor(Door door)
  {
    if ((Object) GameManager.current == (Object) null)
      return;
    GameManager.current._doorClosed = door;
    GameManager.current._doorOpen = door.GetIsOpen();
  }

  public static void PlayerDied()
  {
    if ((Object) GameManager.current == (Object) null)
      return;
    if ((Object) GameManager.current._sceneFader != (Object) null)
      GameManager.current._sceneFader.FadeSceneOut();
    GameManager.current.Invoke("RestartScene", GameManager.current.deathSequenceDuration);
  }

  private void RestartScene() => SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);

  public static void PlayerFinishedLevel()
  {
    if ((Object) GameManager.current == (Object) null)
      return;
    GameManager.current._isLevelOver = true;
    AudioManager.PlayLevelFinishAudio();
    GameManager.current.StartCoroutine(GameManager.current.LoadNextLevel(SceneManager.GetActiveScene().buildIndex + 1));
  }

  public static void PlayerWon()
  {
    if ((Object) GameManager.current == (Object) null)
      return;
    GameManager.current._isGameOver = true;
    UIManager.DisplayGameOverText(GameManager.current._totalGameTime);
    AudioManager.PlayWinAudio();
  }

  public IEnumerator LoadNextLevel(int levelIndex)
  {
    yield return (object) new WaitForSeconds(GameManager.current.transitionTime);
    if (levelIndex < 8)
    {
      if ((Object) GameManager.current._sceneFader != (Object) null)
        GameManager.current._sceneFader.FadeSceneOut();
      SceneManager.LoadScene(levelIndex);
      GameManager.current._isLevelOver = false;
    }
    else
      GameManager.PlayerWon();
  }

  private void PauseCheck()
  {
    if (!Input.GetKeyDown(KeyCode.Escape))
      return;
    if (GameManager.current._isPaused)
      this.Resume();
    else
      this.Pause();
  }

  public void Resume()
  {
    GameManager.current.pauseMenuUI.SetActive(false);
    Time.timeScale = 1f;
    GameManager.current._isPaused = false;
  }

  private void Pause()
  {
    GameManager.current.pauseMenuUI.SetActive(true);
    Time.timeScale = 0.0f;
    GameManager.current._isPaused = true;
  }

  public void LoadMenu()
  {
    SceneManager.LoadScene(GameManager.current._menuSceneName);
    AudioManager.StartMenuAudio();
    this._isInMenu = true;
  }

  public void QuitGame()
  {
    Debug.Log((object) "QUIT");
    Application.Quit();
  }

  public static void ResetTotalGameTime()
  {
    GameManager.current._totalGameTime = 0.0f;
    GameManager.current.Resume();
  }
}
