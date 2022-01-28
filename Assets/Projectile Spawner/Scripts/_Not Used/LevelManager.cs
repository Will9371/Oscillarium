using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LevelManager : MonoBehaviour
{
    public float scrollSpeed = 1;
    [SerializeField] private GameObject[] background = null;
    private AudioManager audioManager = null;

    private void Start()
    {
        audioManager = this.GetComponent<AudioManager>();
        audioManager.PlayBossMusic();
    }

    private void Update()
    {
        Application.targetFrameRate = 60;
        if(background[0].transform.position.z >= 30)
        {
            background[0].transform.position = new Vector3(2, 0, -39.03201f);
        }
        if (background[1].transform.position.z >= 30)
        {
            background[1].transform.position = new Vector3(2, 0, -39.03201f);
        }

        Scroll(background[0]);
        Scroll(background[1]);
    }

    public void Scroll(GameObject thingToScroll)
    {
        thingToScroll.transform.position += new Vector3(0,0,scrollSpeed);
    }
}
