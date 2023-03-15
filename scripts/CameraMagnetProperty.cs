using UnityEngine;

[ExecuteInEditMode]
public class CameraMagnetProperty : MonoBehaviour
{
  [Range(0.1f, 50f)]
  public float MagnetStrength = 5f;
  [Range(0.1f, 50f)]
  public float Proximity = 5f;
  public Transform ProximityVisualization;
  [HideInInspector]
  public Transform myTransform;

  private void Start() => this.myTransform = this.transform;

  private void Update()
  {
    if (!((Object) this.ProximityVisualization != (Object) null))
      return;
    this.ProximityVisualization.localScale = new Vector3(this.Proximity * 2f, this.Proximity * 2f, 1f);
  }
}
