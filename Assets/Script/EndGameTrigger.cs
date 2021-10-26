using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EndGameTrigger : MonoBehaviour
{

    public GameObject endGameWall;

    public AudioClip musicCut;

    bool triggered = false;

    // Start is called before the first frame update
    void Start()
    {
        endGameWall.SetActive(false);
        GetComponent<SpriteRenderer>().enabled = false;
    }

    void SetupEndGame()
    {
        endGameWall.SetActive(true);

        GameManager.gameManager.UpdatePercentage(1);
        // More dramatic without
        //GetComponent<AudioSource>().PlayOneShot(musicCut);

        // TODO make this actually fade music
        GameManager.gameManager.GetComponents<AudioSource>()[1].volume = 0;
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.tag == "Player" && !triggered)
        {
            triggered = true;
            SetupEndGame();
        }
    }
}
