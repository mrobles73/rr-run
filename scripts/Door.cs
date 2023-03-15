using UnityEngine;

public class Door : MonoBehaviour
{
  private Animator _anim;
  private int _openParameterID;
  private int _playerLayer;
  private bool _isOpen;
  [SerializeField]
  private GameObject doorSparklesPrefab;
  private Vector3 _doorTopRight;
  private Vector3 _doorTopLeft;

  private void Start()
  {
    this._playerLayer = LayerMask.NameToLayer("Player");
    this._anim = this.GetComponent<Animator>();
    this._openParameterID = Animator.StringToHash("isOpen");
    GameManager.RegisterDoor(this);
    this._doorTopRight = new Vector3(this.transform.position.x + 0.7f, this.transform.position.y + 1.2f);
    this._doorTopLeft = new Vector3(this.transform.position.x - 0.7f, this.transform.position.y + 1.2f);
  }

  public void OpenClose() => this._anim.SetBool(this._openParameterID, this._isOpen);

  private void OnCollisionEnter2D(Collision2D collision)
  {
    if (collision.gameObject.layer != this._playerLayer)
      return;
    this._isOpen = true;
    Object.Instantiate<GameObject>(this.doorSparklesPrefab, this._doorTopLeft, this.transform.rotation);
    Object.Instantiate<GameObject>(this.doorSparklesPrefab, this._doorTopRight, this.transform.rotation);
    this.OpenClose();
    Debug.Log((object) "Player FinishedLevel");
    GameManager.PlayerFinishedLevel();
  }

  private void OnCollisionExit2D(Collision2D collision)
  {
    this._isOpen = false;
    this.OpenClose();
  }

  public bool GetIsOpen() => this._isOpen;
}
