using UnityEngine;
using UnityEngine.UI;

public class DisplayPercent : MonoBehaviour
{
    [SerializeField] Text text;

    public void Refresh(float value)
    {
        text.text = (value * 100f).ToString("F0") + "%";
    }
}
