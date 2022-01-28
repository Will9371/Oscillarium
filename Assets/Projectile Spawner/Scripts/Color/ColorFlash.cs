using System;
using System.Collections;
using UnityEngine;

[Serializable]
public class ColorFlash
{
    [SerializeField] Renderer rend;
    [SerializeField] float flashTime = .1f;
    [SerializeField] int flashCount = 3;
    [SerializeField] Color defaultColor;
    
    public IEnumerator Process() { yield return Process(defaultColor); }

    public IEnumerator Process(Color color)
    {
        if (!rend) yield break;
        var flashDelay = new WaitForSeconds(flashTime);
        rend.material.color = color;
        
        for (int i = 0; i < flashCount; i++)
        {
            rend.enabled = true;
            yield return flashDelay;
            rend.enabled = false;
            yield return flashDelay;
        }
    }    
}
