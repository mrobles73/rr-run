using UnityEngine;

public class PointAtAimTarget : MonoBehaviour
{
  [Tooltip("This object represents the aim target.  We always point toeards this")]
  public Transform AimTarget;

  private void Update()
  {
    if ((Object) this.AimTarget == (Object) null)
      return;
    Vector3 forward = this.AimTarget.position - this.transform.position;
    if ((double) forward.sqrMagnitude <= 0.0099999997764825821)
      return;
    this.transform.rotation = Quaternion.LookRotation(forward);
  }
}