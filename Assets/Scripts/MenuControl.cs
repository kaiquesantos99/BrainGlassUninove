using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MenuControl : MonoBehaviour
{
    private AudioSource[] audio;
    public AudioClip backgroundSong;
    public AudioClip aboutSong;


    // Paineis de opções
    public GameObject panInicio;
    public GameObject panTipo;
    public GameObject panAbout;

    void Start()
    {
        // Config Audio
        audio = GetComponents<AudioSource>();

        audio[0].clip = backgroundSong;
        audio[1].clip = aboutSong;
        audio[0].Play();
    }

    public void CarregarJogo(int value)
    {
        switch(value)
        {
            case 1:
                DadosJogo.tipoPerguntas = "perguntasPesquisaOrdenacao";
                break;
            case 2:
                DadosJogo.tipoPerguntas = "perguntasProjeto";
                break;
            case 3:
                DadosJogo.tipoPerguntas = "perguntasTecnicasProgramacao";
                break;
            case 4:
                DadosJogo.tipoPerguntas = "perguntasMetodologiasAgeis";
                break;
            case 5:
                DadosJogo.tipoPerguntas = "perguntasProcessoNegocio";
                break;
        }

        
        SceneManager.LoadScene("Loading");
    }

    public void CarregarPainelTipos()
    {
        panInicio.SetActive(false);
        panTipo.SetActive(true);
    }

    public void OnAboutClick()
    {
        panInicio.SetActive(false);
        panAbout.SetActive(true);
        audio[0].Stop();
        audio[1].Play();
    }

    public void CloseAbout()
    {
        panInicio.SetActive(true);
        panAbout.SetActive(false);
        audio[1].Stop();
        audio[0].Play();
    }

    public void FecharJogo()
    {
        Application.Quit();
    }
}
