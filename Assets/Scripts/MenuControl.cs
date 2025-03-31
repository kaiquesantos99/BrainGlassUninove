using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MenuControl : MonoBehaviour
{
    private AudioSource audio;
    public AudioClip backgroundSong;

    // Paineis de opções
    public GameObject panInicio;
    public GameObject panTipo;

    void Start()
    {
        audio = GetComponent<AudioSource>();

        audio.clip = backgroundSong;
        audio.loop = true;
        audio.Play();
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

    public void FecharJogo()
    {
        Application.Quit();
    }
}
