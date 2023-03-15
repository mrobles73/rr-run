using Cinemachine;
using UnityEngine;

public class CameraMagnetTargetController : MonoBehaviour
{
  public CinemachineTargetGroup targetGroup;
  private int playerIndex;
  private CameraMagnetProperty[] cameraMagnets;

  private void Start()
  {
    this.cameraMagnets = this.GetComponentsInChildren<CameraMagnetProperty>();
    this.playerIndex = 0;
  }

  private void Update()
  {
    for (int index = 1; index < this.targetGroup.m_Targets.Length; ++index)
    {
      float magnitude = (this.targetGroup.m_Targets[this.playerIndex].target.position - this.targetGroup.m_Targets[index].target.position).magnitude;
      this.targetGroup.m_Targets[index].weight = (double) magnitude >= (double) this.cameraMagnets[index - 1].Proximity ? 0.0f : this.cameraMagnets[index - 1].MagnetStrength * (float) (1.0 - (double) magnitude / (double) this.cameraMagnets[index - 1].Proximity);
    }
  }
}
