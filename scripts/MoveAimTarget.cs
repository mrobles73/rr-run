using Cinemachine;
using UnityEngine;
using UnityEngine.Events;

public class MoveAimTarget : MonoBehaviour
{
  public CinemachineBrain Brain;
  public RectTransform ReticleImage;
  [Tooltip("How far to raycast to place the aim target")]
  public float AimDistance;
  [Tooltip("Objects on these layers will be detected")]
  public LayerMask CollideAgainst;
  [TagField]
  [Tooltip("Obstacles with this tag will be ignored.  It's a good idea to set this field to the player's tag")]
  public string IgnoreTag = string.Empty;
  [Header("Axis Control")]
  [Tooltip("The Vertical axis.  Value is -90..90. Controls the vertical orientation")]
  [AxisStateProperty]
  public AxisState VerticalAxis;
  [Tooltip("The Horizontal axis.  Value is -180..180.  Controls the horizontal orientation")]
  [AxisStateProperty]
  public AxisState HorizontalAxis;

  private void OnValidate()
  {
    this.VerticalAxis.Validate();
    this.HorizontalAxis.Validate();
    this.AimDistance = Mathf.Max(1f, this.AimDistance);
  }

  private void Reset()
  {
    this.AimDistance = 200f;
    this.ReticleImage = (RectTransform) null;
    this.CollideAgainst = (LayerMask) 1;
    this.IgnoreTag = string.Empty;
    this.VerticalAxis = new AxisState(-70f, 70f, false, false, 10f, 0.1f, 0.1f, "Mouse Y", true);
    this.VerticalAxis.m_SpeedMode = AxisState.SpeedMode.InputValueGain;
    this.HorizontalAxis = new AxisState(-180f, 180f, true, false, 10f, 0.1f, 0.1f, "Mouse X", false);
    this.HorizontalAxis.m_SpeedMode = AxisState.SpeedMode.InputValueGain;
  }

  private void OnEnable()
  {
    CinemachineCore.CameraUpdatedEvent.RemoveListener(new UnityAction<CinemachineBrain>(this.PlaceReticle));
    CinemachineCore.CameraUpdatedEvent.AddListener(new UnityAction<CinemachineBrain>(this.PlaceReticle));
  }

  private void OnDisable() => CinemachineCore.CameraUpdatedEvent.RemoveListener(new UnityAction<CinemachineBrain>(this.PlaceReticle));

  private void Update()
  {
    if ((Object) this.Brain == (Object) null)
      return;
    this.HorizontalAxis.Update(Time.deltaTime);
    this.VerticalAxis.Update(Time.deltaTime);
    this.PlaceTarget();
  }

  private void PlaceTarget()
  {
    Quaternion quaternion = Quaternion.Euler(this.VerticalAxis.Value, this.HorizontalAxis.Value, 0.0f);
    Vector3 rawPosition = this.Brain.CurrentCameraState.RawPosition;
    this.transform.position = this.GetProjectedAimTarget(rawPosition + quaternion * Vector3.forward, rawPosition);
  }

  private Vector3 GetProjectedAimTarget(Vector3 pos, Vector3 camPos)
  {
    Vector3 origin = pos;
    Vector3 normalized = (pos - camPos).normalized;
    pos += this.AimDistance * normalized;
    RaycastHit hitInfo;
    if ((int) this.CollideAgainst != 0 && this.RaycastIgnoreTag(new Ray(origin, normalized), out hitInfo, this.AimDistance, (int) this.CollideAgainst))
      pos = hitInfo.point;
    return pos;
  }

  private bool RaycastIgnoreTag(Ray ray, out RaycastHit hitInfo, float rayLength, int layerMask)
  {
    float num1 = 0.0f;
    while (Physics.Raycast(ray, out hitInfo, rayLength, layerMask, QueryTriggerInteraction.Ignore))
    {
      if (this.IgnoreTag.Length == 0 || !hitInfo.collider.CompareTag(this.IgnoreTag))
      {
        hitInfo.distance += num1;
        return true;
      }
      Ray ray1 = new Ray(ray.GetPoint(rayLength), -ray.direction);
      if (hitInfo.collider.Raycast(ray1, out hitInfo, rayLength))
      {
        float num2 = rayLength - (hitInfo.distance - 1f / 1000f);
        if ((double) num2 >= 1.0 / 1000.0)
        {
          num1 += num2;
          rayLength = hitInfo.distance - 1f / 1000f;
          if ((double) rayLength >= 1.0 / 1000.0)
            ray.origin = ray1.GetPoint(rayLength);
          else
            break;
        }
        else
          break;
      }
      else
        break;
    }
    return false;
  }

  private void PlaceReticle(CinemachineBrain brain)
  {
    if ((Object) brain == (Object) null || (Object) brain != (Object) this.Brain || (Object) this.ReticleImage == (Object) null || (Object) brain.OutputCamera == (Object) null)
      return;
    this.PlaceTarget();
    CameraState currentCameraState = brain.CurrentCameraState;
    Camera outputCamera = brain.OutputCamera;
    Vector3 screenPoint = outputCamera.WorldToScreenPoint(this.transform.position);
    this.ReticleImage.anchoredPosition = new Vector2(screenPoint.x - (float) outputCamera.pixelWidth * 0.5f, screenPoint.y - (float) outputCamera.pixelHeight * 0.5f);
  }
}