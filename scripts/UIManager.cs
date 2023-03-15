using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class UIManager : MonoBehaviour
{
  private static UIManager _currentInstance;
  public TextMeshProUGUI hpText;
  public TextMeshProUGUI timeText;
  public TextMeshProUGUI gameOverText;
  public TextMeshProUGUI yourTimeText;
  public TextMeshProUGUI finalTimeText;
  public Image backgroundWonImage;

  private void Awake()
  {
    if ((Object) UIManager._currentInstance != (Object) null && (Object) UIManager._currentInstance != (Object) this)
    {
      Object.Destroy((Object) this.gameObject);
    }
    else
    {
      UIManager._currentInstance = this;
      Object.DontDestroyOnLoad((Object) this.gameObject);
    }
  }

  public static void UpdateHpUI(int hp)
  {
    if ((Object) UIManager._currentInstance == (Object) null)
      return;
    if (hp < 0)
      hp = 0;
    UIManager._currentInstance.hpText.text = hp.ToString();
  }

  public static void UpdateTimeUI(float time)
  {
    if ((Object) UIManager._currentInstance == (Object) null)
      return;
    UIManager._currentInstance.timeText.text = UIManager._currentInstance.TimeToString(time);
  }

  public static void DisplayGameOverText(float time)
  {
    if ((Object) UIManager._currentInstance == (Object) null)
      return;
    UIManager._currentInstance.backgroundWonImage.enabled = true;
    UIManager._currentInstance.gameOverText.enabled = true;
    UIManager._currentInstance.yourTimeText.enabled = true;
    UIManager._currentInstance.finalTimeText.text = UIManager._currentInstance.TimeToString(time);
    UIManager._currentInstance.finalTimeText.enabled = true;
  }

  private string TimeToString(float time)
  {
    int num1 = (int) ((double) time / 1000.0 / 60.0);
    float num2 = (float) (int) ((double) time / 1000.0 % 60.0);
    float num3 = time % 1000f;
    return num1.ToString("00") + ":" + num2.ToString("00") + ":" + num3.ToString("000");
  }
}
