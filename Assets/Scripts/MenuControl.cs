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

    public GameObject panPool;
    // Painéis de escolha de bola lisa
    public GameObject panLisas1;
    public GameObject panLisas2;
    public GameObject panLisas3;
    // Painéis de escolha de bola listrada
    public GameObject panListradas1;
    public GameObject panListradas2;
    public GameObject panListradas3;

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
            case 6:
                DadosJogo.tipoPerguntas = "perguntasMetodologiasUni";
                break;
        }

        panGpt.SetActive(false);
        panUninove.SetActive(false);
        panPool.SetActive(true);
    }

    public void TipoDeBola(bool lisa)
    {
        panPool.SetActive(false);

        if (lisa)
        {
            DadosJogo.teamLisa = true;
            panLisas1.SetActive(true);
        }
        else
        {
            DadosJogo.teamLisa = false;
            panListradas1.SetActive(true);
        }
    }

    public void PoolNavButtons(int value)
    {

        if (value >= 0 && value <=6) // Se for painel das lisas
        {
            if (value >= 1 && value <= 3) // Se for voltar
            {
                switch (value)
                {
                    case 1: // Voltar 1
                        panLisas1.SetActive(false);
                        panPool.SetActive(true);
                        break;
                    case 2: // Voltar 2
                        panLisas2.SetActive(false);
                        panLisas1.SetActive(true);
                        break;
                    case 3: // Voltar 3
                        panLisas3.SetActive(false);
                        panLisas2.SetActive(true);
                        break;
                }
            }
            else if (value >= 4 && value <= 5) // Se for next
            {
                switch (value)
                {
                    case 4: // Avançar 1
                        panLisas1.SetActive(false);
                        panLisas2.SetActive(true);
                        break;
                    case 5: // Avançar 2
                        panLisas2.SetActive(false);
                        panLisas3.SetActive(true);
                        break;
                }
            }
            else if (value == 6) // Se for home
            {
                panLisas3.SetActive(false);
                panInicio.SetActive(true);
            }
        }
        else // Se for painel das listradas
        {
            if (value >= 7 && value <= 9) // Se for voltar
            {
                switch (value)
                {
                    case 7: // Voltar 1
                        panListradas1.SetActive(false);
                        panPool.SetActive(true);
                        break;
                    case 8: // Voltar 2
                        panListradas2.SetActive(false);
                        panListradas1.SetActive(true);
                        break;
                    case 9: // Voltar 3
                        panListradas3.SetActive(false);
                        panListradas2.SetActive(true);
                        break;
                }
            }
            else if (value >= 10 && value <= 11) // Se for next
            {
                switch (value)
                {
                    case 10: // Avançar 1
                        panListradas1.SetActive(false);
                        panListradas2.SetActive(true);
                        break;
                    case 11: // Avançar 2
                        panListradas2.SetActive(false);
                        panListradas3.SetActive(true);
                        break;
                }
            }
            else if (value == 12) // Se for home
            {
                panListradas3.SetActive(false);
                panInicio.SetActive(true);
            }
        }

        
    }

    public void ChooseBall(int value)
    {
        DadosJogo.mainBall = value;
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
