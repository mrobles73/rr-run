using UnityEngine;

public class Weapon : MonoBehaviour
{
  [SerializeField]
  private Transform _firePoint;
  [SerializeField]
  private GameObject bulletPrefab;
  [SerializeField]
  private Animator _anim;
  private int _shootingParamID;

  private void Start() => this._shootingParamID = Animator.StringToHash("isShooting");

  private void Update()
  {
    if (GameManager.IsLevelOver() || GameManager.IsGameOver() || GameManager.isGamePaused())
      return;
    if (Input.GetButtonDown("Fire1"))
    {
      this._anim.SetBool(this._shootingParamID, true);
      this.Shoot();
    }
    if (!Input.GetButtonUp("Fire1"))
      return;
    this._anim.SetBool(this._shootingParamID, false);
  }

  private void Shoot()
  {
    Object.Instantiate<GameObject>(this.bulletPrefab, this._firePoint.position, this._firePoint.rotation);
    AudioManager.PlayShotAudio();
  }
}
