using UnityEngine;
using UnityEngine.UI;

public class HealthBar : MonoBehaviour
{
  public Slider slider;
  public Gradient gradient;
  public Image fill;

  public void SetMaxHealthTime(float health)
  {
    this.slider.maxValue = health;
    this.slider.value = health;
    this.gradient.Evaluate(1f);
    this.fill.color = this.gradient.Evaluate(1f);
  }

  public void SetHealthTime(float health)
  {
    this.slider.value = health;
    this.fill.color = this.gradient.Evaluate(this.slider.normalizedValue);
  }
}