using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AudioManager : MonoBehaviour
{
    [SerializeField]AudioSource shootingSource = null;
    [SerializeField] AudioSource energyPickupSource = null;
    [SerializeField] AudioSource otherSoundEffects = null;
    [SerializeField] AudioSource GameMusic = null;
    [SerializeField] AudioClip Damage1x = null;
    [SerializeField] AudioClip Damage4x = null;
    [SerializeField] AudioClip BossMusic = null;

    public void PlayShoot()
    {
        if (shootingSource.isPlaying) return;
        shootingSource.Play();
    }
    public void StopShoot()
    {
        if (!shootingSource.isPlaying) return;
        shootingSource.Stop();
    }
    public void PickUp()
    {
        energyPickupSource.Play();
    }
    public void Hit1x()
    {
        if (otherSoundEffects.isPlaying) return;
        otherSoundEffects.clip = Damage1x;
        otherSoundEffects.Play();
    }
    public void Hit4x()
    {
        otherSoundEffects.clip = Damage4x;
        otherSoundEffects.Play();
    }

    public void PlayBossMusic()
    {
        GameMusic.clip = BossMusic;
        GameMusic.Play();
    }

}
