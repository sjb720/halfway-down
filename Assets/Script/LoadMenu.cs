using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LoadMenu : MonoBehaviour
{
    public GameObject light, mainMenuGui;

    public AudioClip lightswitch;

    private void Awake()
    {
        light.SetActive(false);
        mainMenuGui.SetActive(false);
    }

    // Start is called before the first frame update
    void Start()
    {
        StartCoroutine(BeingLoadingMainMenu());

        // Trying for web players
        Screen.SetResolution(1440, 1080, true);
    }

    IEnumerator BeingLoadingMainMenu()
    {
        AudioSource aus = GetComponent<AudioSource>();

        yield return new WaitForSeconds(3);

        aus.PlayOneShot(lightswitch);

        light.SetActive(true);
        mainMenuGui.SetActive(true);
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
