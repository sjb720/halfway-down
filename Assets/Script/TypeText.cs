using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class TypeText : MonoBehaviour
{
    public float initialTypeDelay = 2;
    public float typeDelay = 0.4f;
    public float typeDelayError = 0.1f;
    public float indentDelay = 0.3f;

    public AudioClip[] keySounds;
    public AudioClip spaceSound;

    [TextArea(15,20)]
    public string text;

    AudioSource aus,song;
    Text typed;

    public bool outro = false;

    public GameObject timeScreen;
    

    private void Awake()
    {
        aus = GetComponent<AudioSource>();
        typed = GetComponent<Text>();
        song = GameObject.Find("music").GetComponent<AudioSource>();
    }

    void Start()
    {
        StartCoroutine(Type());
    }


    IEnumerator Type()
    {
        yield return new WaitForSeconds(initialTypeDelay);

        while(text.Length > 1)
        {
            yield return new WaitForSeconds(typeDelay + Random.Range(-typeDelayError,typeDelayError));

            string letter = text.Substring(0, 1);

            // Play a sound based on the letter
            if (letter == " ") aus.PlayOneShot(spaceSound);
            else if (letter == "\n") yield return new WaitForSeconds(indentDelay);
                else aus.PlayOneShot(keySounds[Random.Range(0, keySounds.Length)]);

            typed.text += letter;

            text = text.Substring(1);

        }

        if(!outro)
            StartCoroutine(DoneTyping());
        else
        {
            yield return new WaitForSeconds(5);
            timeScreen.SetActive(true);
            gameObject.SetActive(false);

        }


    }

    IEnumerator DoneTyping()
    {

        print("here2");
        string phrase = "My imagination isn't real. ";
        int phraseIndex = 0;

        int maxTyped = 30 * 40;
        int i = 0;

        float subtractor = 0.01f;

        float delay = 0.1f;

        while(i < maxTyped)
        {
            yield return new WaitForSeconds(delay);

            if (delay <= subtractor)
            {
                delay = subtractor;
                
            }
            else
            {
                if(i > 40)
                delay -= subtractor;

            }

            string letter = phrase.Substring(phraseIndex, 1);

            if (letter == " ") AudioSource.PlayClipAtPoint(spaceSound,Camera.main.transform.position);
            else if (letter == "\n") yield return new WaitForSeconds(indentDelay);
            else AudioSource.PlayClipAtPoint(keySounds[Random.Range(0, keySounds.Length)], Camera.main.transform.position);

            typed.text += letter;

            phraseIndex++;
            if (phraseIndex == phrase.Length) phraseIndex = 0;

            if(!song.isPlaying)
            {
                typed.color = Color.clear;
                Camera.main.backgroundColor = Color.black;
            }

            i++;
        }
        yield return new WaitForSeconds(3);

        SceneManager.LoadScene("level1");

    }
}
