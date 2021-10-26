using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Checkpoint : MonoBehaviour
{
    public string roseName;

    AudioSource aus;

    public AudioClip fanfare;
    public GameObject drip;
    Animator anim;

    bool currentlyActive = false;
    int uses = 1;

    // Start is called before the first frame update
    void Awake()
    {
        anim = GetComponent<Animator>();
        drip = transform.Find("drip").gameObject;
        aus = GetComponent<AudioSource>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public int getUses()
    {
        return uses;
    }

    public void useCheckpoint()
    {
        uses--;

        if (uses == 0)
        {
            anim.SetTrigger("wilt");
            drip.SetActive(false);
        }
    }

    void ActivateCheckpoint()
    {
        anim.SetTrigger("revived");
        drip.SetActive(true);
        GameObject.Find("RoseSplashText").GetComponent<Animator>().SetTrigger("roseGot");
        GameObject.Find("RoseSplashText").transform.Find("Rose Name").GetComponent<Text>().text = roseName;
        currentlyActive = true;
        uses = 1;

        aus.PlayOneShot(fanfare);

        GameManager.gameManager.AddCheckpointToRespawnList(gameObject);
    }

    public void DeactivateCheckpoint()
    {
        anim.SetTrigger("killed");
        drip.SetActive(false);
        currentlyActive = false;
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.tag == "Player")
        {
            if(!currentlyActive)
            ActivateCheckpoint();
        }
    }
}
