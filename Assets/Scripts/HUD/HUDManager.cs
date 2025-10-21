using UnityEngine;

public class HUDManager : MonoBehaviour
{
    public static HUDManager instance;

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
}
