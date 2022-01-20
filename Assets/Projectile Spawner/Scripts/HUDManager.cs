using UnityEngine;
using UnityEngine.UI;

public class HUDManager : MonoBehaviour
{
    [SerializeField] private Canvas HUD = null;
    private Text fpsText;
    private float frameCounter = 0;
    private void Start()
    {
        fpsText = HUD.transform.GetChild(0).GetChild(0).GetComponent<Text>();
    }

    private void Update()
    {
        TextUpdate();
    }

    private void TextUpdate()
    {
        frameCounter++;
        if (frameCounter < 100) return;
        float fps = 1 / Time.unscaledDeltaTime;
        fpsText.text = "FPS: " + fps;
        frameCounter = 0;
    }
}
