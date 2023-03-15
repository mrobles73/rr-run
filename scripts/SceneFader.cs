using UnityEngine;

public class SceneFader : MonoBehaviour
{
  private Animator _anim;
  private int _fadeStartParamID;

  private void Start()
  {
    this._anim = this.GetComponent<Animator>();
    this._fadeStartParamID = Animator.StringToHash(nameof (Start));
    GameManager.RegisterSceneFader(this);
  }

  public void FadeSceneOut() => this._anim.SetTrigger(this._fadeStartParamID);
}