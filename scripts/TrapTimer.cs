using UnityEngine;

public class TrapTimer : MonoBehaviour
{
  private float _trapPlayTimeUpdate;
  private float _trapStopTimeUpdate;
  private float _animationTime;
  private int _trapBoolParamID;
  private float _trapPlayTime;
  [SerializeField]
  private Collider2D _enableCollider;
  [SerializeField]
  private Animator anim;
  private System.Random random;

  private void Start()
  {
    if ((UnityEngine.Object) this._enableCollider == (UnityEngine.Object) null)
    {
      Debug.Log((object) "You need to add a 2D collider for this to work");
    }
    else
    {
      if (this.anim.name.Equals("Spikes"))
      {
        this.random = new System.Random();
        this._enableCollider.enabled = false;
        this._trapPlayTime = this.generateRandomUpTime() + (float) this.random.Next(-2, 2);
        this._animationTime = 0.33f;
        this._trapBoolParamID = Animator.StringToHash("SpikeUp");
        this.random = (System.Random) null;
      }
      if (this.anim.name.Equals("Crusher"))
      {
        this.random = new System.Random();
        this._enableCollider.enabled = false;
        this._trapPlayTime = this.generateRandomUpTime() + (float) this.random.Next(-2, 4);
        this._animationTime = 0.5f;
        this._trapBoolParamID = Animator.StringToHash("isCrushing");
        this.random = (System.Random) null;
      }
      if (!this.anim.name.Equals("RoofDrill"))
        return;
      this.random = new System.Random();
      this._enableCollider.enabled = false;
      this._trapPlayTime = this.generateRandomUpTime() + (float) this.random.Next(0, 2);
      this._animationTime = 1.8f;
      this._trapBoolParamID = Animator.StringToHash("isDrilling");
      this.random = (System.Random) null;
    }
  }

  private void Update()
  {
    if (GameManager.IsGameOver() || GameManager.IsLevelOver())
      return;
    this._trapPlayTimeUpdate += Time.deltaTime;
    if ((double) this._trapPlayTimeUpdate >= (double) this._trapPlayTime)
    {
      this._trapStopTimeUpdate += Time.deltaTime;
      this.anim.SetBool(this._trapBoolParamID, true);
      this._enableCollider.enabled = true;
    }
    if ((double) this._trapStopTimeUpdate <= (double) this._animationTime)
      return;
    this._enableCollider.enabled = false;
    this.anim.SetBool(this._trapBoolParamID, false);
    this._trapPlayTimeUpdate = 0.0f;
    this._trapStopTimeUpdate = 0.0f;
    this._trapPlayTime = this.generateRandomUpTime();
  }

  private float generateRandomUpTime()
  {
    if (this.random != null)
      this.random = (System.Random) null;
    this.random = new System.Random();
    return (float) this.random.Next(2, 6);
  }

  public void PlayDrillAudio() => AudioManager.PlayRoofDrillAudio();

  public void SpikeAudio() => AudioManager.PlaySpikeAudio();

  public void CrusherAudio() => AudioManager.PlayCrusherAudio();
}
