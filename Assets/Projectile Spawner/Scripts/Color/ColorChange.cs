using System.Collections;
using UnityEngine;
using Playcraft;
using BulletHell;

public class ColorChange : MonoBehaviour
{
    public int colorIndex;
    [SerializeField] Renderer rend;
    [SerializeField] bool startWithRandomColor;
    [SerializeField] bool changeColorRandomly;
    [SerializeField] Vector2 timeRange = new Vector2(2f, 10f);
    [SerializeField] ColorFlash flash;

    public ColorInfo colorInfo;
    public ColorData colorData => colorInfo.data[colorIndex];
    public Color color => colorData.color;
    public SO colorId => colorData.id;
    int colorCount => colorInfo.data.Length;

    void Start()
    {
        if (startWithRandomColor)
            RandomizeColor();
    }
    
    void OnEnable()
    {
        if (changeColorRandomly)
            StartCoroutine(nameof(ColorChangeRoutine));
    }
    
    void OnDisable() { StopAllCoroutines(); }
    
    
    public void RandomizeColor() { SetColor(Random.Range(0, colorCount)); }
    
    public void SetColor(int index)
    {
        colorIndex = index;
        rend.material.color = color;
    }

    IEnumerator ColorChangeRoutine()
    {
        while (true)
        {
            yield return new WaitForSeconds(Random.Range(timeRange.x, timeRange.y));
            var nextIndex = Random.Range(0, colorCount);
            
            if (nextIndex != colorIndex)
                yield return StartCoroutine(flash.Process(colorInfo.data[nextIndex].color));

            SetColor(nextIndex);
        }
    }
}