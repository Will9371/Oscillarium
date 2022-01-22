using UnityEngine;

public class AccessColorChange : MonoBehaviour
{
    public void Trigger(Collider2D other)
    {
        var color = other.GetComponent<ColorChange>();
        if (!color) return;
        color.RandomizeColor();
    }
}
