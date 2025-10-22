using UnityEngine;
using UnityEngine.UI;

public class HUDSlider : MonoBehaviour
{
    public Image timeSlider;

    public void SetTime(float currentTime, float maxTime)
    {
        SetTime(currentTime / maxTime);
    }

    public void SetTime(float calculatedTime)
    {
        timeSlider.fillAmount = calculatedTime;
    }
}
