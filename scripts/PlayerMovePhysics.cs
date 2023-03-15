using System;
using UnityEngine;

public class PlayerMovePhysics : MonoBehaviour
{
  public float speed = 5f;
  public bool worldDirection = true;
  public bool rotatePlayer = true;
  public Action spaceAction;
  public Action enterAction;
  private Rigidbody rb;

  private void Start() => this.rb = this.GetComponent<Rigidbody>();

  private void OnEnable() => this.transform.position += new Vector3(10f, 0.0f, 0.0f);

  private void FixedUpdate()
  {
    Vector3 vector3_1 = new Vector3(Input.GetAxis("Horizontal"), 0.0f, Input.GetAxis("Vertical"));
    if ((double) vector3_1.magnitude > 0.0)
    {
      Vector3 forward = (this.worldDirection ? Vector3.forward : this.transform.position - Camera.main.transform.position) with
      {
        y = 0.0f
      };
      forward = forward.normalized;
      if ((double) forward.magnitude > 1.0 / 1000.0)
      {
        Vector3 vector3_2 = Quaternion.LookRotation(forward, Vector3.up) * vector3_1;
        if ((double) vector3_2.magnitude > 1.0 / 1000.0)
        {
          this.rb.AddForce(this.speed * vector3_2);
          if (this.rotatePlayer)
            this.transform.rotation = Quaternion.LookRotation(vector3_2.normalized, Vector3.up);
        }
      }
    }
    if (Input.GetKeyDown(KeyCode.Space) && this.spaceAction != null)
      this.spaceAction();
    if (!Input.GetKeyDown(KeyCode.Return) || this.enterAction == null)
      return;
    this.enterAction();
  }
}
