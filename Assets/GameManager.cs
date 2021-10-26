using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Com.LuisPedroFonseca.ProCamera2D;
using UnityEngine.SceneManagement;
using System;

public class GameManager : MonoBehaviour
{

    public AudioClip[] waltzSongs;

    public AudioClip playerDiedSfx, playerDeadMusic, playerRespawnSfx, checkpointFanfare, checkpointGotSfx, checkpointFanfareEndSfx;

    public GameObject playerPrefab;
    public static GameManager gameManager;
    public GameObject deathText, respawnText, blackoutScreen;

    GameObject interactables;

    AudioSource sfx, music, sawBuzz;

    bool playerIsDead = false;
    GameObject player;
    Vector3 spawn, initialSpawn;
    List<GameObject> checkpoints;

    float songTime = 0;
    int currentSongIndex = 0;

    Vector3[] allSawPositions;
    public GameObject sawParent;

    bool canRespawn = false;

    public float respawnTimeAfterDeath = 1;

    public Text percentageDisplay, timerDisplay;

    public GameObject shelteredWaterMark;

    public float secondsSpentPlaying;
    bool countTime = true;

    public GameObject optionsMenu;
    

    void Start()
    {
        player = GameObject.FindGameObjectWithTag("Player");
        spawn = player.transform.position;
        checkpoints = new List<GameObject>();

        sfx = GetComponents<AudioSource>()[0];
        music = GetComponents<AudioSource>()[1];
        sawBuzz = GetComponents<AudioSource>()[2];

        initialSpawn = player.transform.position;

        //Start our first song
        music.clip = waltzSongs[currentSongIndex];
        music.Play();

        Camera.main.gameObject.GetComponent<ProCamera2DTransitionsFX>().TransitionEnter();

        if (PlayerPrefs.GetString("gamemode") == "sheltered")
            shelteredWaterMark.SetActive(true);

        allSawPositions = new Vector3[sawParent.transform.childCount];

        for (int i = 0; i < sawParent.transform.childCount; i++)
        {
            allSawPositions[i] = (sawParent.transform.GetChild(i).position);
        }

    }

    public float GetTime()
    {
        return secondsSpentPlaying;
    }

    private void Awake()
    {
        if (gameManager == null)
        {
            gameManager = this;
        }
        else return;


        interactables = GameObject.Find("_INTERACTABLES");

    }

    private void FixedUpdate()
    {
        secondsSpentPlaying = Time.timeSinceLevelLoad;

        TimeSpan time = TimeSpan.FromSeconds(secondsSpentPlaying);

        timerDisplay.text = time.ToString("hh':'mm':'ss");

        float closestSaw = 25;

        for (int i = 0; i < allSawPositions.Length; i++)
        {
            float dist = Vector3.Distance(player.transform.position, allSawPositions[i]);

            if (dist < closestSaw)
            {
                closestSaw = dist;
            }
        }

        if (closestSaw < 25)
            sawBuzz.volume = 1f / closestSaw;
        else
            sawBuzz.volume = 0;

    }


    public void PlayerDied()
    {
        canRespawn = false;
        StartCoroutine(DeathSequence());
    }

    IEnumerator DeathSequence()
    {
        deathText.SetActive(true);
        playerIsDead = true;

        sfx.PlayOneShot(playerDiedSfx);

        songTime = music.time;

        music.Stop();

        music.time = 0;
        music.clip = playerDeadMusic;
        music.Play();

        yield return new WaitForSeconds(respawnTimeAfterDeath);

        canRespawn = true;
        respawnText.SetActive(true);
    }

    void Update()
    {

        if (playerIsDead && canRespawn)
        {
            if (Input.GetKeyDown(KeyCode.R))
                Respawn();
        }

        if (Input.GetKeyDown(KeyCode.Escape))
        {
            optionsMenu.SetActive(!optionsMenu.activeSelf);
        }
    }

    public void AddCheckpointToRespawnList(GameObject checkpoint)
    {
        checkpoints.Add(checkpoint);

        if (music.clip == waltzSongs[currentSongIndex])
            songTime = music.time;
        music.Stop();
        music.time = 0;
        music.clip = checkpointFanfare;
        music.Play();


        sfx.PlayOneShot(checkpointGotSfx);

        UpdatePercentage(float.Parse(checkpoint.name) / 18);

        StartCoroutine(ResumeWaltzMusic(checkpointFanfare.length - 0.2f));
    }

    public void UpdatePercentage(float percent)
    {
        percentageDisplay.text = (percent*100).ToString("0.00") + "%";
    }

    public void BlackoutScreen(bool b)
    {
        blackoutScreen.SetActive(b);
    }

    float hearingThreshold = 20;

    public void PlayEnviornmentSFX(AudioClip sound, Vector3 position)
    {
        PlayEnviornmentSFX(sound, position, 1);
    }

    public void PlayEnviornmentSFX(AudioClip sound, Vector3 position, float volume)
    {
        if (player == null) return;

        if (volume > 1) volume = 1;

        // distance from origin of sound to player
        float dist = Vector3.Distance(player.transform.position, position);

        // if we are too far don't play
        if (dist > hearingThreshold) return;

        // play at volume
        sfx.PlayOneShot(sound, (1 - (dist / hearingThreshold)) * volume);
    }

    // Resumes waltz music after however many seconds
    IEnumerator ResumeWaltzMusic(float seconds)
    {
        yield return new WaitForSeconds(seconds);

        music.Stop();

        sfx.PlayOneShot(checkpointFanfareEndSfx);

        yield return new WaitForSeconds(checkpointFanfareEndSfx.length);

        music.clip = waltzSongs[currentSongIndex];
        music.time = songTime;
        music.Play();

    }

    Vector3 GetRespawnPoint()
    {
        if (checkpoints.Count == 0)
        {
            UpdatePercentage(0);
            return initialSpawn;
        }
        else
        {
            UpdatePercentage(float.Parse(checkpoints[checkpoints.Count - 1].name) / 18);
            return checkpoints[checkpoints.Count - 1].transform.position;
        }
    }

    void Respawn()
    {

        bool foundCheckpointWithRose = false;

        // Remove last checkpoint from list
        if (PlayerPrefs.GetString("gamemode") != "sheltered")
            while (!foundCheckpointWithRose && checkpoints.Count > 0)
            {
                Checkpoint cp = checkpoints[checkpoints.Count - 1].GetComponent<Checkpoint>();

                if (cp.getUses() > 0)
                {
                    cp.useCheckpoint();
                    foundCheckpointWithRose = true;
                }
                else
                {
                    cp.DeactivateCheckpoint();
                    checkpoints.RemoveAt(checkpoints.Count - 1);
                }

            }

        playerIsDead = false;
        //player.SetActive (true);
        //player.transform.position = GetRespawnPoint();
        Destroy(player);
        player = Instantiate(playerPrefab).gameObject;
        player.transform.position = GetRespawnPoint();

        Camera.main.GetComponent<ProCamera2D>().RemoveCameraTarget(GameObject.Find("bag").transform);
        GameObject.Find("RagDoll(Clone)").name = "oldbody";

        Camera.main.GetComponent<ProCamera2D>().AddCameraTarget(player.transform);

        // Hide death text
        deathText.SetActive(false);
        respawnText.SetActive(false);

        // Destroy our old body
        Destroy(GameObject.Find("oldbody").gameObject);

        player.GetComponent<Player>().dead = false;

        music.Stop();

        music.clip = waltzSongs[currentSongIndex];
        music.time = songTime;
        music.Play();

        sfx.PlayOneShot(playerRespawnSfx);

        //SceneManager.LoadScene (SceneManager.GetActiveScene ().buildIndex);
    }
}
