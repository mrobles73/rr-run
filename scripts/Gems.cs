using UnityEngine;

public class Gems : MonoBehaviour
{
  [SerializeField]
  private GameObject gemExplosionPrefab;

  public void ExplodeGem()
  {
    AudioManager.PlayGemCollectAudio();
    Object.Instantiate<GameObject>(this.gemExplosionPrefab, this.transform.position, this.transform.rotation);
  }
}