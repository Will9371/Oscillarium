using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ColorChange : MonoBehaviour
{
    public int color = 0;
    [SerializeField] private Material white = null, purple = null, red = null;
    [SerializeField] private Renderer ObjectToChange = null;
    [SerializeField] public Light flashLight = null;
    [SerializeField] private ParticleSystem particle = null;
    [SerializeField] private bool startWithRandomColor = false;
    [SerializeField] private bool changeColorRandomly = false;
    private float changeColorEverySeconds = 0;
    private float colorTimer = 0;

    private void Start()
    {
        if (startWithRandomColor)
        {
            RandomColor();
        }
    }

    private void Update()
    {
        ColorSwitcher();
        ColorChangeTimer();
    }

    private void LightChange(Color color)
    {
        if (flashLight == null) return;
        flashLight.color = color;
    }

    private void ParticleChange(Color color)
    {
        if (particle == null) return;
        var main = particle.main;
        main.startColor = color;
    }
    private void RandomColor()
    {
        color = Random.Range(0, 2);
    }
    private void ColorSwitcher()
    {
        switch (color)
        {
            case 0:
                ObjectToChange.material = white;
                LightChange(Color.white);
                ParticleChange(Color.white);
                break;

            case 1:
                ObjectToChange.material = purple;
                LightChange(Color.magenta);
                ParticleChange(Color.magenta);
                break;

            case 2:
                ObjectToChange.material = red;
                LightChange(Color.red);
                ParticleChange(Color.red);
                break;
        }
    }
    private void ColorChangeTimer()
    {
        if (!changeColorRandomly) return;
        colorTimer += Time.deltaTime;
        if(colorTimer >= changeColorEverySeconds)
        {
            changeColorEverySeconds = Random.Range(2, 10);
            color = Random.Range(0, 2);
            colorTimer = 0;
        }
    }
}
