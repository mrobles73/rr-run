using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
  public bool drawDebugRaycasts = true;
  [Header("Movement Properties")]
  public float speed = 8f;
  public float crouchSpeedDivisor = 3f;
  public float coyoteDuration = 0.05f;
  public float maxFallSpeed = -25f;
  [Header("Jump Properties")]
  public float jumpForce = 6.3f;
  public float crouchJumpBoost = 2.5f;
  public float jumpHoldForce = 1.9f;
  public float jumpHoldDuration = 0.1f;
  [Header("Environment Check Properties")]
  public float footOffset = 0.4f;
  public float eyeHeight = 1.5f;
  public float reachOffset = 0.7f;
  public float headClearance = 0.5f;
  public float groundDistance = 0.2f;
  public float grabDistance = 0.4f;
  public LayerMask groundLayer;
  [Header("Status Flags")]
  public bool isOnGround;
  public bool isJumping;
  public bool isCrouching;
  public bool isHeadBlocked;
  private PlayerInput input;
  private CapsuleCollider2D bodyCollider;
  private Rigidbody2D rigidBody;
  private Animator anim;
  private float jumpTime;
  private float coyoteTime;
  private float playerHeight;
  private float originalXScale;
  private int direction = 1;
  private Vector2 colliderStandSize;
  private Vector2 colliderStandOffset;
  private Vector2 colliderCrouchSize;
  private Vector2 colliderCrouchOffset;
  private const float smallAmount = 0.05f;
  private int velocityXParamID;
  private int velocityYParamID;
  private int jumpParamID;
  private int crouchParamID;
  private int groundParamID;

  private void Start()
  {
    this.velocityXParamID = Animator.StringToHash("velocityX");
    this.velocityYParamID = Animator.StringToHash("velocityY");
    this.jumpParamID = Animator.StringToHash("isJumping");
    this.crouchParamID = Animator.StringToHash("isCrouching");
    this.groundParamID = Animator.StringToHash("isGrounded");
    this.input = this.GetComponent<PlayerInput>();
    this.rigidBody = this.GetComponent<Rigidbody2D>();
    this.bodyCollider = this.GetComponent<CapsuleCollider2D>();
    this.anim = this.GetComponent<Animator>();
    this.originalXScale = this.transform.localScale.x;
    this.playerHeight = this.bodyCollider.size.y;
    this.colliderStandSize = this.bodyCollider.size;
    this.colliderStandOffset = this.bodyCollider.offset;
    this.colliderCrouchSize = new Vector2(this.bodyCollider.size.x, 2.047029f);
    this.colliderCrouchOffset = new Vector2(this.bodyCollider.offset.x, 1.016061f);
  }

  private void Update() => this.updateAnimator();

  private void FixedUpdate()
  {
    this.PhysicsCheck();
    this.GroundMovement();
    this.MidAirMovement();
  }

  private void updateAnimator()
  {
    this.anim.SetFloat(this.velocityXParamID, Mathf.Abs(this.input.horizontal));
    this.anim.SetFloat(this.velocityYParamID, this.rigidBody.velocity.y);
    this.anim.SetBool(this.jumpParamID, this.isJumping);
    this.anim.SetBool(this.crouchParamID, this.isCrouching);
    this.anim.SetBool(this.groundParamID, this.isOnGround);
  }

  private void PhysicsCheck()
  {
    this.isOnGround = false;
    this.isHeadBlocked = false;
    RaycastHit2D raycastHit2D1 = this.Raycast(new Vector2(-this.footOffset, 0.0f), Vector2.down, this.groundDistance);
    RaycastHit2D raycastHit2D2 = this.Raycast(new Vector2(this.footOffset, 0.0f), Vector2.down, this.groundDistance);
    if ((bool) raycastHit2D1 || (bool) raycastHit2D2)
      this.isOnGround = true;
    if ((bool) this.Raycast(new Vector2(0.0f, this.bodyCollider.size.y), Vector2.up, this.headClearance))
      this.isHeadBlocked = true;
    Vector2 rayDirection = new Vector2((float) this.direction, 0.0f);
    this.Raycast(new Vector2(this.footOffset * (float) this.direction, this.playerHeight), rayDirection, this.grabDistance);
    this.Raycast(new Vector2(this.reachOffset * (float) this.direction, this.playerHeight), Vector2.down, this.grabDistance);
    this.Raycast(new Vector2(this.footOffset * (float) this.direction, this.eyeHeight), rayDirection, this.grabDistance);
  }

  private void GroundMovement()
  {
    if (this.input.crouchHeld && !this.isCrouching && this.isOnGround)
      this.Crouch();
    else if (!this.input.crouchHeld && this.isCrouching)
      this.StandUp();
    else if (!this.isOnGround && this.isCrouching)
      this.StandUp();
    float x = this.speed * this.input.horizontal;
    if ((double) x * (double) this.direction < 0.0)
      this.FlipCharacterDirection();
    if (this.isCrouching)
      x /= this.crouchSpeedDivisor;
    this.rigidBody.velocity = new Vector2(x, this.rigidBody.velocity.y);
    if (!this.isOnGround)
      return;
    this.coyoteTime = Time.time + this.coyoteDuration;
  }

  private void MidAirMovement()
  {
    if (this.input.jumpPressed && !this.isJumping && (this.isOnGround || (double) this.coyoteTime > (double) Time.time))
    {
      if (this.isCrouching && !this.isHeadBlocked)
      {
        this.StandUp();
        this.rigidBody.AddForce(new Vector2(0.0f, this.crouchJumpBoost), ForceMode2D.Impulse);
      }
      this.isOnGround = false;
      this.isJumping = true;
      this.jumpTime = Time.time + this.jumpHoldDuration;
      this.rigidBody.AddForce(new Vector2(0.0f, this.jumpForce), ForceMode2D.Impulse);
      AudioManager.PlayJumpAudio();
    }
    else if (this.isJumping)
    {
      if (this.input.jumpHeld)
        this.rigidBody.AddForce(new Vector2(0.0f, this.jumpHoldForce), ForceMode2D.Impulse);
      if ((double) this.jumpTime <= (double) Time.time)
        this.isJumping = false;
    }
    if ((double) this.rigidBody.velocity.y >= (double) this.maxFallSpeed)
      return;
    this.rigidBody.velocity = new Vector2(this.rigidBody.velocity.x, this.maxFallSpeed);
  }

  private void FlipCharacterDirection()
  {
    this.direction *= -1;
    this.transform.Rotate(0.0f, 180f, 0.0f);
  }

  private void Crouch()
  {
    this.isCrouching = true;
    this.bodyCollider.size = this.colliderCrouchSize;
    this.bodyCollider.offset = this.colliderCrouchOffset;
  }

  private void StandUp()
  {
    if (this.isHeadBlocked)
      return;
    this.isCrouching = false;
    this.bodyCollider.size = this.colliderStandSize;
    this.bodyCollider.offset = this.colliderStandOffset;
  }

  private RaycastHit2D Raycast(Vector2 offset, Vector2 rayDirection, float length) => this.Raycast(offset, rayDirection, length, this.groundLayer);

  private RaycastHit2D Raycast(Vector2 offset, Vector2 rayDirection, float length, LayerMask mask)
  {
    Vector2 position = (Vector2) this.transform.position;
    RaycastHit2D raycastHit2D = Physics2D.Raycast(position + offset, rayDirection, length, (int) mask);
    if (this.drawDebugRaycasts)
    {
      Color color = (bool) raycastHit2D ? Color.red : Color.green;
      Debug.DrawRay((Vector3) (position + offset), (Vector3) (rayDirection * length), color);
    }
    return raycastHit2D;
  }

  public void StepAudio() => AudioManager.PlayFootstepAudio();

  public void CrouchStepAudio() => AudioManager.PlayCrouchFootstepAudio();
}
