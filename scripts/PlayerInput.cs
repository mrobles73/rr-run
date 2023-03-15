using UnityEngine;

[DefaultExecutionOrder(-100)]
public class PlayerInput : MonoBehaviour
{
  [HideInInspector]
  public float horizontal;
  [HideInInspector]
  public bool jumpHeld;
  [HideInInspector]
  public bool jumpPressed;
  [HideInInspector]
  public bool crouchHeld;
  [HideInInspector]
  public bool crouchPressed;
  private bool readyToClear;

  private void Update()
  {
    this.ClearInput();
    if (GameManager.IsGameOver() || GameManager.IsLevelOver() || GameManager.isGamePaused())
      return;
    this.ProcessInputs();
    this.horizontal = Mathf.Clamp(this.horizontal, -1f, 1f);
  }

  private void FixedUpdate() => this.readyToClear = true;

  private void ClearInput()
  {
    if (!this.readyToClear)
      return;
    this.horizontal = 0.0f;
    this.jumpPressed = false;
    this.jumpHeld = false;
    this.crouchPressed = false;
    this.crouchHeld = false;
    this.readyToClear = false;
  }

  private void ProcessInputs()
  {
    this.horizontal += Input.GetAxis("Horizontal");
    this.jumpPressed = this.jumpPressed || Input.GetButtonDown("Jump");
    this.jumpHeld = this.jumpHeld || Input.GetButton("Jump");
    this.crouchPressed = this.crouchPressed || Input.GetButtonDown("Crouch");
    this.crouchHeld = this.crouchHeld || Input.GetButton("Crouch");
  }
}
