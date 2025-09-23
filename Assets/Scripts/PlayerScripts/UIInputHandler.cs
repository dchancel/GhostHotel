using UnityEngine;

public class UIInputHandler : MonoBehaviour
{
    public static UIInputHandler instance;

    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
        }
        else
        {
            Destroy(this.gameObject);
            return;
        }
    }
}
