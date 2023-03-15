using UnityEngine;

public class Barrel : MonoBehaviour
{
  private int _bulletsLayer;
  [SerializeField]
  private int _barrelHits = 7;
  [SerializeField]
  private GameObject _collectablePrefab;
  [SerializeField]
  private GameObject _barrelExplosionPrefab;

  private void Start() => this._bulletsLayer = LayerMask.NameToLayer("Bullets");

  private void Update()
  {
    if (this._barrelHits > 0)
      return;
    if ((Object) this._collectablePrefab != (Object) null)
      Object.Instantiate<GameObject>(this._collectablePrefab, this.transform.position, this.transform.rotation);
    AudioManager.PlayBarrelDeathAudio();
    Object.Instantiate<GameObject>(this._barrelExplosionPrefab, this.transform.position, this.transform.rotation);
    Object.Destroy((Object) this.gameObject);
  }

  private void OnTriggerEnter2D(Collider2D collision)
  {
    if (collision.gameObject.layer != this._bulletsLayer)
      return;
    AudioManager.PlayBarrelHitAudio();
    --this._barrelHits;
  }
}
