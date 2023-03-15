using UnityEngine;

public class Enemy : MonoBehaviour
{
  private Animator _anim;
  private bool _seesPlayer;
  private int _playerLayer;
  private float _currentShootTime;
  private float _gDDistance = 0.5f;
  private float _rayOffset = 0.5f;
  private float _groundRayOffset = 0.6f;
  private float _rayHeightTop = 2.3f;
  private float _rayHeightMid = 1.3f;
  private float _rayHeightLow = 0.5f;
  private float _rayHeightPlayer = 1.3f;
  private float _reachDistance = 0.5f;
  private float _playerReachDistance = 10f;
  private bool _movingRight = true;
  private int _direction = 1;
  [SerializeField]
  private int _health = 100;
  [SerializeField]
  private float _shootTime = 0.2f;
  [SerializeField]
  private float _speed;
  [SerializeField]
  private Transform _firePoint;
  [SerializeField]
  private GameObject bulletPrefab;
  [SerializeField]
  private GameObject _enemyExplosionPrefab;
  [SerializeField]
  private GameObject _collectablePrefab;
  [SerializeField]
  private LayerMask _groundLayer;
  [SerializeField]
  private LayerMask _maskPlayerLayer;
  private Rigidbody2D _rigidBody;
  private bool _isShooting;
  private bool _isAttacking;
  private bool _isHurt;
  private bool _isKilled;
  private bool _isMoving = true;
  private int _shootingParamID;
  private int _attackingParamID;
  private int _killedParamID;
  private int _hurtParamID;
  private int _movingParamID;

  private void Start()
  {
    this._shootingParamID = Animator.StringToHash("isShooting");
    this._attackingParamID = Animator.StringToHash("isAttacking");
    this._killedParamID = Animator.StringToHash("isKilled");
    this._hurtParamID = Animator.StringToHash("isHurt");
    this._movingParamID = Animator.StringToHash("isMoving");
    this._anim = this.GetComponent<Animator>();
    this._rigidBody = this.GetComponent<Rigidbody2D>();
  }

  private void Update()
  {
    this.UpdatePlayerContact();
    this.UpdateAnimator();
    if (this._isHurt || !this._isShooting)
      return;
    this._currentShootTime += Time.deltaTime;
    if ((double) this._currentShootTime > 0.20000000298023224)
    {
      this.Shoot();
      this._currentShootTime = 0.0f;
    }
  }

  private void UpdatePlayerContact()
  {
    if (this._seesPlayer)
    {
      this._isMoving = false;
      this._isShooting = true;
    }
    else
    {
      if (this._seesPlayer)
        return;
      this._isMoving = true;
      this._isShooting = false;
    }
  }

  private void UpdateAnimator()
  {
    this._anim.SetBool(this._shootingParamID, this._isShooting);
    this._anim.SetBool(this._attackingParamID, this._isAttacking);
    this._anim.SetBool(this._movingParamID, this._isMoving);
  }

  private void FixedUpdate()
  {
    if (!this._seesPlayer && this._isMoving)
      this.transform.Translate((Vector3) (Vector2.right * this._speed * Time.deltaTime));
    this.PhysicsCheck();
  }

  private void PhysicsCheck()
  {
    Vector2 rayDirection = new Vector2((float) this._direction, 0.0f);
    RaycastHit2D raycastHit2D1 = this.Raycast(new Vector2(this._groundRayOffset * (float) this._direction, 0.0f), Vector2.down, this._gDDistance, this._groundLayer);
    RaycastHit2D raycastHit2D2 = this.Raycast(new Vector2(this._rayOffset * (float) this._direction, this._rayHeightTop), rayDirection, this._reachDistance, this._groundLayer);
    RaycastHit2D raycastHit2D3 = this.Raycast(new Vector2(this._rayOffset * (float) this._direction, this._rayHeightMid), rayDirection, this._reachDistance, this._groundLayer);
    RaycastHit2D raycastHit2D4 = this.Raycast(new Vector2(this._rayOffset * (float) this._direction, this._rayHeightLow), rayDirection, this._reachDistance, this._groundLayer);
    RaycastHit2D raycastHit2D5 = this.Raycast(new Vector2(this._rayOffset * (float) this._direction, this._rayHeightPlayer), rayDirection, this._playerReachDistance, this._maskPlayerLayer);
    if (!(bool) (Object) raycastHit2D1.collider || (bool) (Object) raycastHit2D2.collider || (bool) (Object) raycastHit2D3.collider || (bool) (Object) raycastHit2D4.collider)
      this.flip();
    if ((bool) (Object) raycastHit2D5.collider)
    {
      this._seesPlayer = true;
    }
    else
    {
      if ((bool) (Object) raycastHit2D5.collider)
        return;
      this._seesPlayer = false;
    }
  }

  private RaycastHit2D Raycast(Vector2 offset, Vector2 rayDirection, float length, LayerMask mask)
  {
    Vector2 position = (Vector2) this.transform.position;
    RaycastHit2D raycastHit2D = Physics2D.Raycast(position + offset, rayDirection, length, (int) mask);
    Color color = (bool) raycastHit2D ? Color.red : Color.green;
    Debug.DrawRay((Vector3) (position + offset), (Vector3) (rayDirection * length), color);
    return raycastHit2D;
  }

  private void flip()
  {
    if (this._movingRight)
    {
      this.transform.eulerAngles = new Vector3(0.0f, -180f, 0.0f);
      this._movingRight = false;
      this._direction = -1;
    }
    else
    {
      this.transform.eulerAngles = new Vector3(0.0f, 0.0f, 0.0f);
      this._movingRight = true;
      this._direction = 1;
    }
  }

  private void OnTriggerEnter2D(Collider2D collision)
  {
    if (!this._isKilled)
      ;
  }

  public void TakeDamage(int damage)
  {
    AudioManager.PlayEnemyHitAudio();
    this._health -= damage;
    this._isHurt = true;
    this._anim.SetBool(this._hurtParamID, this._isHurt);
    this.Invoke("ResetHurtBool", 0.3f);
    if (this._health > 0)
      return;
    this.Die();
  }

  private void Die()
  {
    AudioManager.PlayEnemyDeathAudio();
    this._isKilled = true;
    this._anim.SetBool(this._killedParamID, this._isKilled);
    this.Invoke("DestroyEnemy", 0.5f);
  }

  private void ResetHurtBool()
  {
    this._isHurt = false;
    this._anim.SetBool(this._hurtParamID, this._isHurt);
  }

  private void DestroyEnemy()
  {
    AudioManager.PlayEnemyExplosionAudio();
    Vector3 position = new Vector3(this.transform.position.x, this.transform.position.y + 0.5f);
    Object.Instantiate<GameObject>(this._collectablePrefab, position, this.transform.rotation);
    Object.Instantiate<GameObject>(this._enemyExplosionPrefab, position, this.transform.rotation);
    Object.Destroy((Object) this.gameObject);
  }

  private void Shoot()
  {
    Object.Instantiate<GameObject>(this.bulletPrefab, this._firePoint.position, this._firePoint.rotation);
    AudioManager.PlayShotAudio();
  }
}
