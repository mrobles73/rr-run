using UnityEngine;

public class CharacterMovementNoCamera : MonoBehaviour
{
  public Transform InvisibleCameraOrigin;
  public float StrafeSpeed = 0.1f;
  public float TurnSpeed = 3f;
  public float Damping = 0.2f;
  public float VerticalRotMin = -80f;
  public float VerticalRotMax = 80f;
  public KeyCode sprintJoystick = KeyCode.JoystickButton2;
  public KeyCode sprintKeyboard = KeyCode.Space;
  private bool isSprinting;
  private Animator anim;
  private float currentStrafeSpeed;
  private Vector2 currentVelocity;

  private void OnEnable()
  {
    this.anim = this.GetComponent<Animator>();
    this.currentVelocity = Vector2.zero;
    this.currentStrafeSpeed = 0.0f;
    this.isSprinting = false;
  }

  private void FixedUpdate()
  {
    Vector2 vector2_1 = new Vector2(Input.GetAxis("Horizontal"), Input.GetAxis("Vertical"));
    float target = Mathf.Clamp(vector2_1.y, -1f, 1f);
    float num = Mathf.SmoothDamp(this.anim.GetFloat("Speed"), target, ref this.currentVelocity.y, this.Damping);
    this.anim.SetFloat("Speed", num);
    this.anim.SetFloat("Direction", num);
    this.isSprinting = (Input.GetKey(this.sprintJoystick) || Input.GetKey(this.sprintKeyboard)) && (double) num > 0.0;
    this.anim.SetBool("isSprinting", this.isSprinting);
    this.currentStrafeSpeed = Mathf.SmoothDamp(this.currentStrafeSpeed, vector2_1.x * this.StrafeSpeed, ref this.currentVelocity.x, this.Damping);
    this.transform.position += this.transform.TransformDirection(Vector3.right) * this.currentStrafeSpeed;
    Vector2 vector2_2 = new Vector2(Input.GetAxis("Mouse X"), Input.GetAxis("Mouse Y"));
    Vector3 eulerAngles = this.transform.eulerAngles;
    eulerAngles.y += vector2_2.x * this.TurnSpeed;
    this.transform.rotation = Quaternion.Euler(eulerAngles);
    if (!((Object) this.InvisibleCameraOrigin != (Object) null))
      return;
    eulerAngles = this.InvisibleCameraOrigin.localRotation.eulerAngles;
    eulerAngles.x -= vector2_2.y * this.TurnSpeed;
    if ((double) eulerAngles.x > 180.0)
      eulerAngles.x -= 360f;
    eulerAngles.x = Mathf.Clamp(eulerAngles.x, this.VerticalRotMin, this.VerticalRotMax);
    this.InvisibleCameraOrigin.localRotation = Quaternion.Euler(eulerAngles);
  }
}
