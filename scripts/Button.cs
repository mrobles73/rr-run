using UnityEngine;

public class Button : MonoBehaviour
{
  [SerializeField]
  private int _pressedTime = 10;
  private int _playerLayer;
  private int _pressedParamID;
  private bool _isPressed;
  private Animator _anim;
  private float _timePassed;
  private BoxCollider2D _bCollider;
  private float _colliderYLocation;
  private float _colliderXLocation;
  private float _colliderYOffset;
  [SerializeField]
  private GameObject obstacles;

  private void Start()
  {
    this._pressedParamID = Animator.StringToHash("isPressed");
    this._playerLayer = LayerMask.NameToLayer("Player");
    this._anim = this.GetComponent<Animator>();
    this._bCollider = this.GetComponent<BoxCollider2D>();
    this._colliderYOffset = 0.1886192f;
    this._colliderYLocation = this._bCollider.offset.y - this._colliderYOffset;
    this._colliderXLocation = this._bCollider.offset.x;
  }

  private void Update()
  {
    if (!this._isPressed)
      return;
    this._timePassed += Time.deltaTime;
    if ((double) this._timePassed > (double) this._pressedTime)
    {
      this._isPressed = false;
      this._anim.SetBool(this._pressedParamID, this._isPressed);
      Object.Destroy((Object) this.gameObject);
      this.obstacles.SetActive(true);
    }
  }

  private void OnCollisionEnter2D(Collision2D collision)
  {
    if (collision.collider.gameObject.layer != this._playerLayer || (double) collision.contacts[0].normal.y >= -0.5)
      return;
    this._isPressed = true;
    this._anim.SetBool(this._pressedParamID, this._isPressed);
    this._bCollider.offset = new Vector2(this._colliderXLocation, this._colliderYLocation);
    this.obstacles.SetActive(false);
    AudioManager.PlayTrapButtonAudio();
  }
}
