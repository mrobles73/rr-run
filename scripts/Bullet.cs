using UnityEngine;

public class Bullet : MonoBehaviour
{
  [SerializeField]
  private float speed = 20f;
  [SerializeField]
  private int damage = 40;
  [SerializeField]
  private Rigidbody2D _rb;
  [SerializeField]
  private GameObject _bulletImpact;

  private void Start() => this._rb.velocity = (Vector2) (this.transform.right * this.speed);

  private void OnTriggerEnter2D(Collider2D collision)
  {
    Enemy component = collision.GetComponent<Enemy>();
    if ((Object) component != (Object) null)
      component.TakeDamage(this.damage);
    Object.Instantiate<GameObject>(this._bulletImpact, this.transform.position, this.transform.rotation);
    Object.Destroy((Object) this.gameObject);
  }
}
