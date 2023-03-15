using Cinemachine;
using UnityEngine;

public class ActivateOnKeypress : MonoBehaviour
{
  public KeyCode ActivationKey = KeyCode.LeftControl;
  public int PriorityBoostAmount = 10;
  public GameObject Reticle;
  private CinemachineVirtualCameraBase vcam;
  private bool boosted = false;

  private void Start() => this.vcam = this.GetComponent<CinemachineVirtualCameraBase>();

  private void Update()
  {
    if ((Object) this.vcam != (Object) null)
    {
      if (Input.GetKey(this.ActivationKey))
      {
        if (!this.boosted)
        {
          this.vcam.Priority += this.PriorityBoostAmount;
          this.boosted = true;
        }
      }
      else if (this.boosted)
      {
        this.vcam.Priority -= this.PriorityBoostAmount;
        this.boosted = false;
      }
    }
    if (!((Object) this.Reticle != (Object) null))
      return;
    this.Reticle.SetActive(this.boosted);
  }
}
