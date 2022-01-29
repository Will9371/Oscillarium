using UnityEngine;

public class OscillariumMusic : MonoBehaviour
{
    [SerializeField] AudioSource audio;
    [SerializeField] AudioClip gameMusic;

    public void BeginGame(float startTime = 0f)
    {
        audio.clip = gameMusic;
        audio.time = startTime;
        audio.Play();
    }
}
