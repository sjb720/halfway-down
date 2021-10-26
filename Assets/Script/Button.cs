using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class Button : MonoBehaviour
{
    Animator anim;
    AudioSource aus;

    public AudioClip flipSound;

    // Start is called before the first frame update
    void Awake()
    {
        anim = GetComponent<Animator>();
        aus = GetComponent<AudioSource>();
    }


    public void FlipButton()
    {
        anim.SetTrigger("flip");
        aus.PlayOneShot(flipSound);
    } 

    public void ShowMenu(GameObject menu)
    {
        menu.SetActive(true);
        transform.parent.gameObject.SetActive(false);
    }

    public void LoadScene(string sceneName)
    {
        SceneManager.LoadScene(sceneName);
    }

    public void SetGamemode(string gm)
    {
        PlayerPrefs.SetString("gamemode", gm);
    }

    public void LoadGameWithGamemode (string gm)
    {
        SetGamemode(gm);
        LoadScene("intro");
    }

    public void QuitApplication()
    {
        Application.Quit();
    }
}
