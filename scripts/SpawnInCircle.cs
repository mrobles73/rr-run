using UnityEngine;

public class SpawnInCircle : MonoBehaviour
{
  public GameObject Prefab;
  public float Radius = 1000f;
  public float Amount = 10000f;
  public bool DoIt;

  private void Update()
  {
    if (this.DoIt && (Object) this.Prefab != (Object) null)
    {
      Vector3 position1 = this.transform.position;
      Quaternion rotation = this.transform.rotation;
      for (int index = 0; (double) index < (double) this.Amount; ++index)
      {
        int f = Random.Range(0, 360);
        Vector3 position2 = new Vector3(Mathf.Cos((float) f), 0.0f, Mathf.Sin((float) f));
        position2 = position1 + position2 * Mathf.Sqrt(Random.Range(0.0f, 1f)) * this.Radius;
        Object.Instantiate<GameObject>(this.Prefab, position2, rotation, this.transform.parent);
      }
    }
    this.DoIt = false;
  }
}