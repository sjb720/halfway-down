using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SlipKnot : MonoBehaviour
{

    public AudioClip hang;

    private void OnTriggerExit2D(Collider2D collision)
    {


        if(collision.tag == "Player")
        {
            collision.gameObject.SetActive(false);
            GetComponent<AudioSource>().PlayOneShot(hang);
            GameManager.gameManager.BlackoutScreen(true);

            PlayerPrefs.SetFloat("final time",GameManager.gameManager.GetTime());

            StartCoroutine(ChangeScene());
        }


        IEnumerator ChangeScene()
        {
            yield return new WaitForSeconds(4);

            SceneManager.LoadScene("outro");

        }
    }
}
