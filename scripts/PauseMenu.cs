using UnityEngine;

public class PauseMenu : MonoBehaviour
{
  private static PauseMenu _currentInstance;

  private void Awake()
  {
    if ((Object) PauseMenu._currentInstance != (Object) null && (Object) PauseMenu._currentInstance != (Object) this)
    {
      Object.Destroy((Object) this.gameObject);
    }
    else
    {
      PauseMenu._currentInstance = this;
      Object.DontDestroyOnLoad((Object) this.gameObject);
    }
  }
}
