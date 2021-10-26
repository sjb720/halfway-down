using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TurnOnLightTrigger : MonoBehaviour
{
    public GameObject inDark;
    AudioSource aus;

    public AudioClip lightsOn;

    bool triggered = false;

    // Start is called before the first frame update
    void Start()
    {
        inDark.SetActive(false);
        aus = GetComponent<AudioSource>();
        GetComponent<SpriteRenderer>().enabled = false;
    }


    private void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.tag == "Player" && !triggered)
        {
            triggered = true;
            inDark.SetActive(true);
            aus.PlayOneShot(lightsOn);
        }


    }
}
