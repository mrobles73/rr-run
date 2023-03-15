using Cinemachine.Utility;
using System;
using UnityEngine;

public class PlayerMove : MonoBehaviour
{
  public float Speed;
  public float VelocityDamping;
  public float JumpTime;
  public PlayerMove.ForwardMode InputForward;
  public bool RotatePlayer = true;
  public Action SpaceAction;
  public Action EnterAction;
  private Vector3 m_currentVleocity;
  private float m_currentJumpSpeed;
  private float m_restY;

  private void Reset()
  {
    this.Speed = 5f;
    this.InputForward = PlayerMove.ForwardMode.Camera;
    this.RotatePlayer = true;
    this.VelocityDamping = 0.5f;
    this.m_currentVleocity = Vector3.zero;
    this.JumpTime = 1f;
    this.m_currentJumpSpeed = 0.0f;
  }

  private void OnEnable()
  {
    this.m_currentJumpSpeed = 0.0f;
    this.m_restY = this.transform.position.y;
    this.SpaceAction -= new Action(this.Jump);
    this.SpaceAction += new Action(this.Jump);
  }

  private void Update()
  {
    Vector3 vector3_1;
    switch (this.InputForward)
    {
      case PlayerMove.ForwardMode.Camera:
        vector3_1 = Camera.main.transform.forward;
        break;
      case PlayerMove.ForwardMode.Player:
        vector3_1 = this.transform.forward;
        break;
      default:
        vector3_1 = Vector3.forward;
        break;
    }
    vector3_1.y = 0.0f;
    vector3_1 = vector3_1.normalized;
    if ((double) vector3_1.sqrMagnitude < 0.0099999997764825821)
      return;
    Vector3 vector3_2 = Quaternion.LookRotation(vector3_1, Vector3.up) * new Vector3(Input.GetAxis("Horizontal"), 0.0f, Input.GetAxis("Vertical"));
    float deltaTime = Time.deltaTime;
    this.m_currentVleocity += Damper.Damp(vector3_2 * this.Speed - this.m_currentVleocity, this.VelocityDamping, deltaTime);
    this.transform.position += this.m_currentVleocity * deltaTime;
    if (this.RotatePlayer && (double) this.m_currentVleocity.sqrMagnitude > 0.0099999997764825821)
      this.transform.rotation = Quaternion.Slerp(this.transform.rotation, Quaternion.LookRotation(this.InputForward != PlayerMove.ForwardMode.Player || (double) Vector3.Dot(vector3_1, this.m_currentVleocity) >= 0.0 ? this.m_currentVleocity : -this.m_currentVleocity), Damper.Damp(1f, this.VelocityDamping, deltaTime));
    if ((double) this.m_currentJumpSpeed != 0.0)
      this.m_currentJumpSpeed -= 10f * deltaTime;
    Vector3 position = this.transform.position;
    position.y += this.m_currentJumpSpeed * deltaTime;
    if ((double) position.y < (double) this.m_restY)
    {
      position.y = this.m_restY;
      this.m_currentJumpSpeed = 0.0f;
    }
    this.transform.position = position;
    if (Input.GetKeyDown(KeyCode.Space) && this.SpaceAction != null)
      this.SpaceAction();
    if (!Input.GetKeyDown(KeyCode.Return) || this.EnterAction == null)
      return;
    this.EnterAction();
  }

  public void Jump() => this.m_currentJumpSpeed += (float) (10.0 * (double) this.JumpTime * 0.5);

  public enum ForwardMode
  {
    Camera,
    Player,
    World,
  }
}
