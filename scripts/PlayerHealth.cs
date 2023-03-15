using UnityEngine;

public class PlayerHealth : MonoBehaviour
{
  private Animator anim;
  private bool isAlive = true;
  private bool cannotDie;
  private bool _isHit;
  private int _trapsLayer;
  private int _collectablesLayer;
  private int _bulletsLayer;
  private int _deathParamID;
  private int _hurtParamID;
  [SerializeField]
  private int _hitPoints = 1;
  private float _disappearTime = 1.5f;
  public float _currentCannotDieTime;
  [SerializeField]
  private GameObject fallOffVFXPrefab;
  private float _maxTime;
  private float _currentTime;
  public HealthBar healthBar;

  private void Start()
  {
    this._trapsLayer = LayerMask.NameToLayer("Traps");
    this._collectablesLayer = LayerMask.NameToLayer("Collectables");
    this._bulletsLayer = LayerMask.NameToLayer("Bullets");
    this._deathParamID = Animator.StringToHash("isDead");
    this._hurtParamID = Animator.StringToHash("isHurt");
    this.anim = this.GetComponent<Animator>();
    this._maxTime = this.GenerateTime();
    this._currentTime = this._maxTime;
    this.healthBar.SetMaxHealthTime(this._maxTime);
    this._currentCannotDieTime = 5f;
  }

  private void Update()
  {
    if (GameManager.IsGameOver() || GameManager.IsLevelOver())
      return;
    UIManager.UpdateHpUI(this._hitPoints);
    if ((double) this.transform.position.y < -6.0 || (double) this._currentTime <= 0.0)
    {
      AudioManager.PlayPlayerExplosionAudio();
      UnityEngine.Object.Instantiate<GameObject>(this.fallOffVFXPrefab, new Vector3(this.transform.position.x, this.transform.position.y + 1f), this.transform.rotation);
      this.DeactivatePlayerObject();
    }
    this.LowerTime(Time.deltaTime);
    if (this.cannotDie)
      this._currentCannotDieTime -= Time.deltaTime;
    if ((double) this._currentCannotDieTime > 0.0)
      return;
    this.cannotDie = false;
    this._currentCannotDieTime = 5f;
  }

  private void OnTriggerEnter2D(Collider2D collision)
  {
    if (!this.isAlive)
      return;
    if (collision.gameObject.layer == this._trapsLayer || collision.gameObject.layer == this._bulletsLayer)
    {
      AudioManager.PlayHitAudio();
      if (!this.cannotDie)
      {
        if (collision.gameObject.name.Equals("Crusher") || collision.gameObject.name.Equals("Bomb(Clone)"))
          this._hitPoints -= 3;
        else if (collision.gameObject.name.Equals("RoofDrill"))
          this._hitPoints -= 2;
        else
          --this._hitPoints;
        if (this._hitPoints <= 0)
        {
          AudioManager.PlayDeathAudio();
          this.isAlive = false;
          this.anim.SetBool(this._deathParamID, true);
          this.Invoke("DeactivatePlayerObject", this._disappearTime);
        }
        this._isHit = true;
        this.anim.SetBool(this._hurtParamID, this._isHit);
        this.Invoke("ResetHitBool", 0.25f);
      }
    }
    if (collision.gameObject.layer != this._collectablesLayer)
      return;
    if (collision.gameObject.name.Equals("GemOne"))
      this._currentTime = this._maxTime;
    else if (collision.gameObject.name.Equals("GemTwo"))
    {
      if (!this.cannotDie)
      {
        this.anim.SetBool(this._deathParamID, true);
        this.Invoke("DeactivatePlayerObject", this._disappearTime);
      }
      else
        this.cannotDie = false;
    }
    else if (collision.gameObject.name.Equals("GemThree"))
      ++this._hitPoints;
    else if (collision.gameObject.name.Equals("GemFour"))
    {
      if (this.cannotDie)
        this._currentCannotDieTime += 3f;
      this.cannotDie = true;
    }
    else if (collision.gameObject.name.Equals("GemFive"))
      ++this._currentTime;
    else if (collision.gameObject.name.Equals("GemSix") || collision.gameObject.name.Equals("GemSix(Clone)"))
      this._hitPoints += 5;
    collision.gameObject.GetComponent<Gems>().ExplodeGem();
    collision.gameObject.SetActive(false);
  }

  private void DeactivatePlayerObject()
  {
    this.isAlive = false;
    this.gameObject.SetActive(false);
    GameManager.PlayerDied();
  }

  private void LowerTime(float timeDamage)
  {
    this._currentTime -= timeDamage;
    this.healthBar.SetHealthTime(this._currentTime);
  }

  private float GenerateTime() => (float) new System.Random().Next(10, 60);

  private void ResetHitBool()
  {
    this._isHit = false;
    this.anim.SetBool(this._hurtParamID, this._isHit);
  }
}
