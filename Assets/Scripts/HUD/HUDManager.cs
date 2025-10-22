using UnityEngine;

public class HUDManager : MonoBehaviour
{
    public static HUDManager instance;

    public MarqueeManager marquee;
    public HUDSlider slider;

    private void Awake()
    {
        if(instance == null)
        {
            instance = this;
        }
        else
        {
            Destroy(this.gameObject);
        }
    }

    public void ModifySouls(int amount)
    {
        marquee.ModifySouls(amount);
    }

    public void SetTime(float currentTime, float maxTime)
    {
        SetTime(currentTime / maxTime);
    }

    public void SetTime(float calculatedTime)
    {
        slider.SetTime(calculatedTime);
    }
}
