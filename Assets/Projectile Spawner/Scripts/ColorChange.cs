using UnityEngine;
using Playcraft;
using BulletHell;

public class ColorChange : MonoBehaviour
{
    public int colorIndex = 0;
    [SerializeField] private Renderer rend = null;
    [SerializeField] public Light flashLight = null;
    [SerializeField] private ParticleSystem particle = null;
    [SerializeField] private bool startWithRandomColor = false;
    [SerializeField] private bool changeColorRandomly = false;
    private float changeColorEverySeconds = 0;
    private float colorTimer = 0;
    
    public ColorInfo colorInfo;
    public ColorData colorData => colorInfo.data[colorIndex];
    public Color color => colorData.color;
    public SO colorId => colorData.id;
    int colorCount => colorInfo.data.Length;

    private void Start()
    {
        if (startWithRandomColor)
        {
            RandomizeColor();
        }
    }

    private void Update()
    {
        ColorSwitcher();
        ColorChangeTimer();
    }

    private void LightChange(Color color)
    {
        if (!flashLight) return;
        flashLight.color = color;
    }

    private void ParticleChange(Color color)
    {
        if (!particle) return;
        var main = particle.main;
        main.startColor = color;
    }
    
    private void RandomizeColor()
    {
        colorIndex = Random.Range(0, colorCount);
    }
    
    private void ColorSwitcher()
    {
        rend.material.color = color;
        LightChange(color);
        ParticleChange(color);
    }
    
    private void ColorChangeTimer()
    {
        if (!changeColorRandomly) return;
        colorTimer += Time.deltaTime;
        
        if (colorTimer >= changeColorEverySeconds)
        {
            changeColorEverySeconds = Random.Range(2, 10);
            colorIndex = Random.Range(0, 2);
            colorTimer = 0;
        }
    }
}
