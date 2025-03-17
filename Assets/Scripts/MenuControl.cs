using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MenuControl : MonoBehaviour
{
    private AudioSource audio;
    public AudioClip backgroundSong;

    void Start()
    {
        audio = GetComponent<AudioSource>();

        audio.clip = backgroundSong;
        audio.loop = true;
        audio.Play();
    }

    public void CarregarJogo()
    {
        SceneManager.LoadScene("Loading");
    }

    public void FecharJogo()
    {
        Application.Quit();
    }
}
