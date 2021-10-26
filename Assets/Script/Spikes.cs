using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum State
{
    still, moving
}

public class Spikes : MonoBehaviour
{

    public State Type;
    public float speed, downTime, upTime = 1;
    public bool playSound = true;
    public AudioClip spikeUp, spikeDown;
    Animator anim;

    // Use this for initialization
    void Start()
    {
        if (Type == State.still)
            return;

        anim = GetComponent<Animator>();
        anim.speed = speed;
        StartCoroutine(Move());
    }

    // Update is called once per frame
    IEnumerator Move()
    {
        while (true)
        {
            yield return new WaitForSeconds(upTime);
            if (playSound)
                GameManager.gameManager.PlayEnviornmentSFX(spikeDown, transform.position);
            anim.SetTrigger("move");
            yield return new WaitForSeconds(downTime);
            if (playSound)
                GameManager.gameManager.PlayEnviornmentSFX(spikeUp, transform.position);
            anim.SetTrigger("move");
        }

    }
}
