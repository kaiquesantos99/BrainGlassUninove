using System.Collections;
using System.Collections.Generic;
using System.Globalization;
using System.Security.Cryptography;
using UnityEngine;
using TMPro;
using UnityEngine.UI;
using UnityEngine.Playables;
using UnityEngine.Video;
using UnityEngine.SceneManagement;
using System.Collections.Specialized;
using System;
using System.Runtime.InteropServices;


public class QuizController : MonoBehaviour
{
    // Atributos =======================================================================================================================================================================================
    public int correta;
    public int questaoAtual;
    public int currentQuestion = 0;
    public int totErros;
    public int totAcertos;
    public int totQuestoes;
    public bool quebrado;
    public int dinheiro;

    // Lista de Questões
    List<Questions> questoes = new List<Questions>();

    // Virtual Cameras
    public GameObject objCamTheWall;
    public GameObject objCamBall;
    private Cinemachine.CinemachineVirtualCamera camTheWall;

    // Objetos - The Wall
    public GameObject ball; private float ballX = 366.816f, ballY = 37.522f;
    public GameObject[] objResults;

    // Objeto - Vidro inteiro
    public GameObject vidro;
    public GameObject vidroQuebrado;
    public GameObject vidroQuebrando;

    // Objeto que exibe a questão e seu componente
    public GameObject questao; private TextMeshProUGUI textoQuestao;

    // Objeto que exibe o dinheiro
    public GameObject cash;

    // Objeto - Vídeo de morte
    public GameObject objDeadVideo;
    private VideoPlayer deadVideo;

    // Timeline
    public GameObject timeline;
    private PlayableDirector cutsceneMorteDePrimeira;
    public GameObject timelineIntroduction;
    private PlayableDirector sceneIntroduction;

    public GameObject timelineTrincando;
    public GameObject timelineSusto;
    public GameObject timelineMorteLonga;
    private PlayableDirector cutsceneTrincando;
    private PlayableDirector cutsceneSusto;
    private PlayableDirector cutsceneMorteLonga;

    // Efeitos Sonoros
    private AudioSource[] aud;
    public AudioClip somX;
    public AudioClip somO;
    public AudioClip mingleInstrumental;
    public AudioClip bolaCaindo;
    public AudioClip ganharDinheiro;
    public AudioClip perderDinheiro;

    // Screens options
    public GameObject quizOptions;
    public GameObject wallOptions;

    // Botão de pular introdução
    public GameObject btnSkip;

    // Botões das alternativas
    public GameObject btnAlt1;
    public GameObject btnAlt2;
    public GameObject btnAlt3;
    public GameObject btnAlt4;
    public GameObject btnNext;

    // Imagens dos botões
    public Sprite xSprite;
    public Sprite oSprite;
    public Sprite padrao;

    // Componentes das alternativas
    private Image imageBtn1;
    private Image imageBtn2;
    private Image imageBtn3;
    private Image imageBtn4;

    private Button buttonBtn1;
    private Button buttonBtn2;
    private Button buttonBtn3;
    private Button buttonBtn4;

    // Texto das alternativas
    private Transform childTextoAlt1;
    private Transform childTextoAlt2;
    private Transform childTextoAlt3;
    private Transform childTextoAlt4;

    // Componente Text dos filhos
    private TextMeshProUGUI textoAlt1;
    private TextMeshProUGUI textoAlt2;
    private TextMeshProUGUI textoAlt3;
    private TextMeshProUGUI textoAlt4;

    // Texto da questão atual
    public GameObject objQtdQuestions;
    private TextMeshProUGUI txtQtdQuestions;

    // Texto do total de questões
    public GameObject objTxtTotQuestions;
    private TextMeshProUGUI txtTotQuestions;



    // Métodos =======================================================================================================================================================================================

    void Start()
    {
        Debug.Log(DadosJogo.tipoPerguntas);
        // Pegando os filhos (textos) dos botões das alternativas || Pegando o componente Text Pro de cada filho e salvando em seus atributos textoAlt
        childTextoAlt1 = btnAlt1.transform.Find("TxtAlt1"); textoAlt1 = childTextoAlt1.GetComponent<TextMeshProUGUI>();
        childTextoAlt2 = btnAlt2.transform.Find("TxtAlt2"); textoAlt2 = childTextoAlt2.GetComponent<TextMeshProUGUI>();
        childTextoAlt3 = btnAlt3.transform.Find("TxtAlt3"); textoAlt3 = childTextoAlt3.GetComponent<TextMeshProUGUI>();
        childTextoAlt4 = btnAlt4.transform.Find("TxtAlt4"); textoAlt4 = childTextoAlt4.GetComponent<TextMeshProUGUI>();

        textoQuestao = questao.GetComponent<TextMeshProUGUI>();

        aud = GetComponents<AudioSource>();

        imageBtn1 = btnAlt1.GetComponent<Image>();
        imageBtn2 = btnAlt2.GetComponent<Image>();
        imageBtn3 = btnAlt3.GetComponent<Image>();
        imageBtn4 = btnAlt4.GetComponent<Image>();

        buttonBtn1 = btnAlt1.GetComponent<Button>();
        buttonBtn2 = btnAlt2.GetComponent<Button>();
        buttonBtn3 = btnAlt3.GetComponent<Button>();
        buttonBtn4 = btnAlt4.GetComponent<Button>();


        sceneIntroduction = timelineIntroduction.GetComponent<PlayableDirector>();
        cutsceneTrincando = timelineTrincando.GetComponent<PlayableDirector>();
        cutsceneSusto = timelineSusto.GetComponent<PlayableDirector>();
        cutsceneMorteDePrimeira = timeline.GetComponent<PlayableDirector>();
        cutsceneMorteLonga = timelineMorteLonga.GetComponent<PlayableDirector>();

        deadVideo = objDeadVideo.GetComponent<VideoPlayer>();

        camTheWall = objCamTheWall.GetComponent<Cinemachine.CinemachineVirtualCamera>();

        txtQtdQuestions = objQtdQuestions.GetComponent<TextMeshProUGUI>();

        txtTotQuestions = objTxtTotQuestions.GetComponent<TextMeshProUGUI>();

        // Criar listerner para o fim da cena de introdução e partir para o quiz
        if (sceneIntroduction != null)
        {
            sceneIntroduction.stopped += OnIntroductionFinished; // Cria um listerner pra ficar ouvindo quando a introdução for parada, e executa o método OnIntroductionFinished
        }

        // Toca a música de fundo
        aud[1].clip = mingleInstrumental;
        aud[1].loop = true;

        CarregarQuestoes();

        NextQuestion();
    }


    void Update()
    {

    }

    // Armazena em um list as questões do arquivo Json
    void CarregarQuestoes()
    {
        TextAsset jsonFile = Resources.Load<TextAsset>(DadosJogo.tipoPerguntas); // Pega o arquivo JSON

        if (jsonFile != null)
        {
            Questions[] arrayQuestoes = JsonHelper.FromJson<Questions>(jsonFile.text); // Desserializa o json e carrega em um array chamado arrayQuestoes

            questoes = new List<Questions>(arrayQuestoes); // Carrega as questões do list temporário no list das questões
            totQuestoes = questoes.Count;
            txtTotQuestions.text = totQuestoes.ToString();
        }
        else
        {
            Debug.LogError("Arquivo JSON não encontrado na pasta Resources!");
        }
        
    }

    // Usado no botão de saltar introdução
    public void IntroductionSkip()
    {
        quizOptions.SetActive(true);
        vidro.SetActive(false);
        currentQuestion = 1; txtQtdQuestions.text = currentQuestion.ToString();
        sceneIntroduction.Stop();
        sceneIntroduction.stopped -= OnIntroductionFinished;
        btnSkip.SetActive(false);
    }

    // Usado quando a cena de introdução terminar
    void OnIntroductionFinished(PlayableDirector obj)
    {
        quizOptions.SetActive(true);
        btnSkip.SetActive(false);
        currentQuestion = 1; txtQtdQuestions.text = currentQuestion.ToString();
        vidro.SetActive(false);
        sceneIntroduction.stopped -= OnIntroductionFinished; // Remove o listener para evitar chamadas duplicadas
    }

    // Verifica se a resposta foi correta ou errada
    public void AnswerCheck(int value)
    {

        if (correta == value)
        {
            switch(value)
            {
                case 0:
                    imageBtn1.sprite = oSprite;
                    break;
                case 1:
                    imageBtn2.sprite = oSprite;
                    break;
                case 2:
                    imageBtn3.sprite = oSprite;
                    break;
                case 3:
                    imageBtn4.sprite = oSprite;
                    break;
            }

            aud[0].clip = somO;
            aud[0].Play();

            // Bloqueia os botões
            buttonBtn1.enabled = false;
            buttonBtn2.enabled = false;
            buttonBtn3.enabled = false;
            buttonBtn4.enabled = false;

            totAcertos++;

            dinheiro += 50;
            cash.GetComponent<TextMeshProUGUI>().text = "R$" + dinheiro.ToString();

            // Exibir botão next
            btnNext.SetActive(true);
        }
        else
        {
            switch (value)
            {
                case 0:
                    imageBtn1.sprite = xSprite;
                    break;
                case 1:
                    imageBtn2.sprite = xSprite;
                    break;
                case 2:
                    imageBtn3.sprite = xSprite;
                    break;
                case 3:
                    imageBtn4.sprite = xSprite;
                    break;
            }

            // Verifica qual a questão correta
            switch (correta)
            {
                case 0:
                    imageBtn1.sprite = oSprite;
                    break;
                case 1:
                    imageBtn2.sprite = oSprite;
                    break;
                case 2:
                    imageBtn3.sprite = oSprite;
                    break;
                case 3:
                    imageBtn4.sprite = oSprite;
                    break;
            }

            aud[0].clip = somX;
            aud[0].Play();

            // Bloqueia os botões
            buttonBtn1.enabled = false;
            buttonBtn2.enabled = false;
            buttonBtn3.enabled = false;
            buttonBtn4.enabled = false;

            totErros++;

            StartCoroutine(HideQuestionsAfterError(2f));


        }
    }

    // Carrega a próxima questão ou exibe o final do jogo
    public void NextQuestion()
    {
        if ((totAcertos + totErros) == totQuestoes)
        {
            Debug.Log("Fim de jogo!");
        }
        else
        {
            // Desbloquear botões
            btnNext.SetActive(false);
            buttonBtn1.enabled = true;
            buttonBtn2.enabled = true;
            buttonBtn3.enabled = true;
            buttonBtn4.enabled = true;

            // Define a imagem padrão para os 4 botões
            imageBtn1.sprite = padrao;
            imageBtn2.sprite = padrao;
            imageBtn3.sprite = padrao;
            imageBtn4.sprite = padrao;

            // Atualiza a questão atual
            currentQuestion+=1;
            txtQtdQuestions.text = currentQuestion.ToString();

            // Sortear a questão da vez
            System.Random random = new System.Random();
            int aleatorio = random.Next(0, questoes.Count);
            questaoAtual = aleatorio;

            textoQuestao.text = questoes[questaoAtual].questao;

            // Coloco as alternativas em uma lista para sorteio
            List<string> alternativas = new List<string>();
            alternativas.Add(questoes[questaoAtual].alt1); alternativas.Add(questoes[questaoAtual].alt2); alternativas.Add(questoes[questaoAtual].alt3); alternativas.Add(questoes[questaoAtual].alt4);

            // Faz o sorteio para a alternativa correta e remove
            aleatorio = random.Next(0, alternativas.Count);
            correta = aleatorio;

            // Insere a questão correta na posição definida pelo random
            switch (correta)
            {
                case 0:
                    textoAlt1.text = alternativas[0];
                    break;
                case 1:
                    textoAlt2.text = alternativas[0];
                    break;
                case 2:
                    textoAlt3.text = alternativas[0];
                    break;
                case 3:
                    textoAlt4.text = alternativas[0];
                    break;
            }

            alternativas.RemoveAt(0);

            // Preencho as demais alternativas
            for (int c = alternativas.Count; c >= 0; c--)
            {
                if (c != correta)
                {
                    switch (c)
                    {
                        case 0:
                            textoAlt1.text = alternativas[0];
                            alternativas.RemoveAt(0);
                            break;
                        case 1:
                            textoAlt2.text = alternativas[0];
                            alternativas.RemoveAt(0);
                            break;
                        case 2:
                            textoAlt3.text = alternativas[0];
                            alternativas.RemoveAt(0);
                            break;
                        case 3:
                            textoAlt4.text = alternativas[0];
                            alternativas.RemoveAt(0);
                            break;

                    }
                }
            }

            // Remove a questão atual
            questoes.RemoveAt(questaoAtual);
        }
    }

    public void BallPosition(int position)
    {
        float zAleatorio = 0.0f;

        // Scale
        float xyzScale = UnityEngine.Random.Range(0.1367179f, 0.3197531f);

        switch (position)
        {
            case 1:
                zAleatorio = UnityEngine.Random.Range(-4.242f, -4.108f);
                break;
            case 2:
                zAleatorio = UnityEngine.Random.Range(-5.133f, -4.997f);
                break;
            case 3:
                zAleatorio = UnityEngine.Random.Range(-6.027f, -5.894f);
                break;
            case 4:
                zAleatorio = UnityEngine.Random.Range(-6.917f, -6.778f);
                break;
            case 5:
                zAleatorio = UnityEngine.Random.Range(-7.821f, -7.679f);
                break;
            case 6:
                zAleatorio = UnityEngine.Random.Range(-8.706f, -8.569f);
                break;
            case 7:
                zAleatorio = UnityEngine.Random.Range(-9.596f, -9.463f);
                break;
        }

        ball.transform.position = new Vector3(ballX, ballY, zAleatorio);
        ball.transform.localScale = new Vector3(xyzScale, xyzScale, xyzScale);
        objCamBall.GetComponent<Cinemachine.CinemachineVirtualCamera>().Priority = 11;
        wallOptions.SetActive(false);

        StartCoroutine(TheWallStart(2f));
    }

    

    public void ShowQuizOptions()
    {
        quizOptions.SetActive(true);
        btnSkip.SetActive(false);
    }

    public IEnumerator TheWallStart(float delay)
    {
        yield return new WaitForSeconds(delay);
        aud[0].clip = bolaCaindo;
        aud[0].Play();
        ball.SetActive(true);
    }
    
    public IEnumerator PlayConsequence(float delay, int conseq)
    {
        yield return new WaitForSeconds(delay);
        camTheWall.Priority = 9;
        wallOptions.SetActive(false);
        aud[0].Stop();

        // Quebrado: -1 quebra, 0 perde dinheiro, 1 nada, 2 ganha dinheiro
        // Não quebrado: -1 trinca, 0 perde dinheiro, 1 nada, 2 ganha dinheiro

        if (!quebrado) // Se não tiver trincado
        {
            if (conseq == -1) // trinca
            {
                quebrado = true;
                vidro.SetActive(true);
                vidroQuebrado.SetActive(true);
                vidroQuebrando.SetActive(false);
                cutsceneTrincando.Play();

                // Listener para exibir o botão de próxima questão
                cutsceneTrincando.stopped += OnTrincandoFinished;
            } else if (conseq == 0) // perde dinheiro
            {
                if (dinheiro-100 < 0)
                {
                    dinheiro = 0;
                    cash.GetComponent<TextMeshProUGUI>().text = "R$" + dinheiro.ToString();
                }
                else
                {
                    dinheiro -= 100;
                    cash.GetComponent<TextMeshProUGUI>().text = "R$" + dinheiro.ToString();
                }

                aud[0].clip = perderDinheiro;
                aud[0].Play();
                StartCoroutine(DoAfterSurvive(2f));
            } else if (conseq == 1) // nada
            {
                StartCoroutine(DoAfterSurvive(2f));
            } else if (conseq == 2) // ganha dinheiro
            {
                dinheiro += 50;
                cash.GetComponent<TextMeshProUGUI>().text = "R$" + dinheiro.ToString();

                aud[0].clip = ganharDinheiro;
                aud[0].Play();
                StartCoroutine(DoAfterSurvive(2f));
            }

        }
        else // Se já tiver trincado
        {

            if (conseq == -1) // quebra
            {
                vidro.SetActive(false);
                vidroQuebrado.SetActive(false);
                vidroQuebrando.SetActive(true);
                cutsceneMorteLonga.Play();

                // Cria um listener para a tela de morte
                cutsceneMorteLonga.stopped += OnDeadFinished;
            }
            else if (conseq == 0) // perde dinheiro
            {
                if (dinheiro-100 < 0)
                {
                    dinheiro = 0;
                    cash.GetComponent<TextMeshProUGUI>().text = "R$" + dinheiro.ToString();
                }
                else
                {
                    dinheiro -= 100;
                    cash.GetComponent<TextMeshProUGUI>().text = "R$" + dinheiro.ToString();
                }

                aud[0].clip = perderDinheiro;
                aud[0].Play();
                StartCoroutine(DoAfterSurvive(2f));
            }
            else if (conseq == 1) // nada
            {
                StartCoroutine(DoAfterSurvive(2f));
            }
            else if (conseq == 2) // ganha dinheiro
            {
                dinheiro += 50;
                cash.GetComponent<TextMeshProUGUI>().text = "R$" + dinheiro.ToString();

                aud[0].clip = ganharDinheiro;
                aud[0].Play();
                StartCoroutine(DoAfterSurvive(2f));
            }
            
        }
    }
    

    private IEnumerator DoAfterSurvive(float delay)
    {
        yield return new WaitForSeconds(delay);
        objCamBall.GetComponent<Cinemachine.CinemachineVirtualCamera>().Priority = 9;
        ball.SetActive(false);
        quizOptions.SetActive(true);
        btnNext.SetActive(true);
    }

    private IEnumerator HideQuestionsAfterError(float delay)
    {
        yield return new WaitForSeconds(delay);
        quizOptions.SetActive(false);
        camTheWall.Priority = 11;
        wallOptions.SetActive(true);

        List<int> conseq = new List<int> {-1, -1, -1, -1, -1, -1, 0,0,0, 1,1,1,1, 2,2};

        for (int c = 0; c < 15; c++)
        {
            
            System.Random random = new System.Random();
            int aleatorio = random.Next(0, conseq.Count - 1); // Sorteia um índice do list conseq (de 0 a 14)


            objResults[c].GetComponent<PontoColisao>().consequencia = conseq[aleatorio]; // Um ponto de colisão da bola recebe uma das consequências da list e armazena no atributo consequencia do script
            objResults[c].GetComponent<PontoColisao>().AlterarFundo(); // Altera o fundo do ponto de colisão
            conseq.RemoveAt(aleatorio); // Remove a consequência do list

        }
    }

    void OnTrincandoFinished(PlayableDirector obj)
    {
        cutsceneTrincando.stopped -= OnTrincandoFinished;
        objCamBall.GetComponent<Cinemachine.CinemachineVirtualCamera>().Priority = 9;
        ball.SetActive(false);
        quizOptions.SetActive(true);
        btnNext.SetActive(true);
    }

    void OnSustoFinished(PlayableDirector obj)
    {
        cutsceneSusto.stopped -= OnSustoFinished;
        btnNext.SetActive(true);
    }

    void OnDeadFinished(PlayableDirector obj)
    {
        cutsceneMorteDePrimeira.stopped -= OnDeadFinished;
        deadVideo.Play();

        deadVideo.loopPointReached += OnDeadVideoFinished;
    }

    void OnDeadVideoFinished(VideoPlayer obj)
    {
        deadVideo.loopPointReached -= OnDeadVideoFinished;
        SceneManager.LoadScene("MainMenu");
    }
    
}

// OUTRAS CLASSES ====================================================================================================================================================================================

[System.Serializable]
public class Questions
{
    public string questao;
    public string alt1;
    public string alt2;
    public string alt3;
    public string alt4;
    
}

public static class DadosJogo
{
    public static string tipoPerguntas;
}

