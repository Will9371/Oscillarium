using System.Collections;
using UnityEngine;
using UnityEngine.Events;

public class ColorFlashMono : MonoBehaviour
{
    [SerializeField] ColorFlash process;
    [SerializeField] UnityEvent onComplete;
    
    public void Begin() { StartCoroutine(Routine()); }
    
    IEnumerator Routine()
    {
        yield return StartCoroutine(process.Process()); 
        onComplete.Invoke();
    }
}
