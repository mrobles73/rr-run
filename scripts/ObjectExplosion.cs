using UnityEngine;

public class ObjectExplosion : MonoBehaviour
{
  private void playBombSound() => AudioManager.PlayBombExplosionAudio();

  private void playFoodExplosionSound() => AudioManager.PlayFoodExplosionAudio();

  private void OnCollisionEnter2D(Collision2D collision)
  {
    if (!this.gameObject.name.Equals("Bubbles(clone") && !this.gameObject.name.Equals("Bubbles"))
      return;
    this.playBubbleSound();
  }

  private void playBubbleSound() => AudioManager.PlayBubbleAudio();
}
