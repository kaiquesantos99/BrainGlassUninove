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
    public GameObject panPlay; public GameObject[] panTipo; // Btn Gpt ou Btn Uninove
    public GameObject panGpt; public GameObject[] panTipoGpt; // PanGpt1 ou PanGpt2
    public GameObject panUninove;
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

    public void OnPlayClick()
    {
        panInicio.SetActive(false);
        panPlay.SetActive(true);
    }

    public void OnGptClick() // Ao apertar no botão gpt
    {
        panPlay.SetActive(false);
        panGpt.SetActive(true); // Pan GPT
        panTipoGpt[0].SetActive(true);
    }

    public void OnUninoveClick()
    {
        panPlay.SetActive(false);
        panUninove.SetActive(true); // Pan Uninove
    }

    public void OnAboutClick()
    {
        panInicio.SetActive(false);
        panAbout.SetActive(true);
        audio[0].Stop();
        audio[1].Play();
    }

    public void OnHome1Click() // Home após o play
    {
        panPlay.SetActive(false);
        panInicio.SetActive(true);
    }

    public void OnNextClick() // Next do pan gpt 1
    {
        panTipoGpt[0].SetActive(false);
        panTipoGpt[1].SetActive(true);
    }

    public void OnHome2Click() // Home do pan gpt 2
    {
        panTipoGpt[1].SetActive(false);
        panGpt.SetActive(false);
        panInicio.SetActive(true);
    }

    public void OnHome3Click() // Home do pan uninove
    {
        panUninove.SetActive(false);
        panInicio.SetActive(true);
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
