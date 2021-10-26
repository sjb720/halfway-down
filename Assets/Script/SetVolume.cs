using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;

public class SetVolume : MonoBehaviour
{
    public AudioMixer am;

    public void ChangeMusicVol(float val)
    {
        am.SetFloat("musicVolume", Mathf.Log10(val) * 20);
    }

    public void ChangeSFXVol(float val)
    {
        am.SetFloat("soundVolume", Mathf.Log10(val) * 20);
    }
}
