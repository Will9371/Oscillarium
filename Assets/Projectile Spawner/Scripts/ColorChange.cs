using System.Collections;
using UnityEngine;
using Playcraft;
using BulletHell;

// REFACTOR: delegate timer logic
public class ColorChange : MonoBehaviour
{
    public int colorIndex;
    [SerializeField] Renderer rend;
    [SerializeField] Renderer rendCue;
    [SerializeField] public Light flashLight;
    [SerializeField] ParticleSystem particle;
    [SerializeField] bool startWithRandomColor;
    [SerializeField] bool changeColorRandomly;
    [SerializeField] Vector2 timeRange = new Vector2(2f, 10f);
    [SerializeField] float flashTime = .1f;
    [SerializeField] int flashCount = 3;

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

    void LightChange(Color color)
    {
        if (!flashLight) return;
        flashLight.color = color;
    }

    void ParticleChange(Color color)
    {
        if (!particle) return;
        var main = particle.main;
        main.startColor = color;
    }
    
    public void RandomizeColor() { SetColor(Random.Range(0, colorCount)); }
    
    public void SetColor(int index)
    {
        colorIndex = index;
        rend.material.color = color;
        LightChange(color);
        ParticleChange(color);       
    }

    IEnumerator ColorChangeRoutine()
    {
        while (true)
        {
            yield return new WaitForSeconds(Random.Range(timeRange.x, timeRange.y));
            var nextIndex = Random.Range(0, colorCount);

            if (rendCue && nextIndex != colorIndex)
                yield return StartCoroutine(FlashRoutine(nextIndex));

            SetColor(nextIndex);
        }
    }
    
    IEnumerator FlashRoutine(int nextIndex)
    {
        var flashDelay = new WaitForSeconds(flashTime);
        var nextColor = colorInfo.data[nextIndex].color;
        rendCue.material.color = nextColor;
        
        for (int i = 0; i < flashCount; i++)
        {
            rendCue.enabled = true;
            yield return flashDelay;
            rendCue.enabled = false;
            yield return flashDelay;
        }
    }
}
