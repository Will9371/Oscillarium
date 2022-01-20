using UnityEngine.SceneManagement;
using UnityEngine;

public class Respawn : MonoBehaviour
{
    Player player = null;
    public bool isInvulnerable = false;
    private readonly int invulnerabilityTime = 3;
    private readonly float blinkInterval = 0.25f;
    private float lastBlink = 0;
    private float invulnerabilityTimer = 0;
    private void Start()
    {
        player = this.GetComponent<Player>();
    }

    private void Update()
    {

        RespawnPlayer();
        BlinkPlayer();
    }

    private void RespawnPlayer()
    {
        if (player.healthManager.killPlayer == false) return;
        transform.position = new Vector3(-0.0729999989f, 0, 10.4119997f);
        player.healthManager.killPlayer = false;
        isInvulnerable = true;
        SceneManager.LoadScene(0);
    }
    private void BlinkPlayer()
    {
        if (isInvulnerable == false) return;
        lastBlink += Time.deltaTime;
        invulnerabilityTimer += Time.deltaTime;
        if (lastBlink < blinkInterval) return;
        if (invulnerabilityTime > invulnerabilityTimer)
        {
            this.GetComponent<Renderer>().enabled = !this.GetComponent<Renderer>().enabled;
            transform.GetChild(0).GetComponent<Renderer>().enabled = !this.GetComponent<Renderer>().enabled;
            lastBlink = 0;
        }
        else
        {
            invulnerabilityTimer = 0;
            lastBlink = 0;
            isInvulnerable = false;
            this.GetComponent<Renderer>().enabled = true;
            transform.GetChild(0).GetComponent<Renderer>().enabled = true;
        }


    }
}
