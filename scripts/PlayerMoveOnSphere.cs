using Cinemachine.Utility;
using UnityEngine;

public class PlayerMoveOnSphere : MonoBehaviour
{
  public SphereCollider Sphere;
  public float speed = 5f;
  public bool rotatePlayer = true;
  public float rotationDamping = 0.5f;

  private void Update()
  {
    Vector3 vector3_1 = new Vector3(Input.GetAxis("Horizontal"), 0.0f, Input.GetAxis("Vertical"));
    if ((double) vector3_1.magnitude > 0.0)
    {
      Vector3 vector3_2 = Camera.main.transform.rotation * vector3_1;
      if ((double) vector3_2.magnitude > 1.0 / 1000.0)
      {
        this.transform.position += vector3_2 * (this.speed * Time.deltaTime);
        if (this.rotatePlayer)
        {
          float t = Damper.Damp(1f, this.rotationDamping, Time.deltaTime);
          this.transform.rotation = Quaternion.Slerp(this.transform.rotation, Quaternion.LookRotation(vector3_2.normalized, this.transform.up), t);
        }
      }
    }
    if (!((Object) this.Sphere != (Object) null))
      return;
    Vector3 normalized = (this.transform.position - this.Sphere.transform.position).normalized;
    Vector3 forward = this.transform.forward.ProjectOntoPlane(normalized);
    this.transform.position = this.Sphere.transform.position + normalized * (this.Sphere.radius + this.transform.localScale.y / 2f);
    this.transform.rotation = Quaternion.LookRotation(forward, normalized);
  }
}
