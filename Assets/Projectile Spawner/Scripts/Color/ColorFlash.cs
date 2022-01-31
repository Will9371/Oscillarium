using System;
using System.Collections;
using UnityEngine;

[Serializable]
public class ColorFlash
{
    public Renderer rend;
    public Renderer secondRend;
    public float flashTime = .1f;
    public int flashCount = 3;
    public Color defaultColor;
    
    public float totalFlashTime => flashTime * flashCount * 2;
    
    public IEnumerator Process() { yield return Process(defaultColor); }

    public IEnumerator Process(Color color)
    {
        if (!rend) yield break;
        var flashDelay = new WaitForSeconds(flashTime);
        rend.material.color = color;
        
        for (int i = 0; i < flashCount; i++)
        {
            rend.enabled = true;
            if (secondRend) secondRend.enabled = true;
            yield return flashDelay;
            rend.enabled = false;
            if (secondRend) secondRend.enabled = false;
            yield return flashDelay;
        }
    }    
}
