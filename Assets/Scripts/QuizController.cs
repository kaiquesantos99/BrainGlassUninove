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
    public int bolaDaVez;

    

    // Lista de Quest�es
    List<Questions> questoes = new List<Questions>();

    // Virtual Cameras
    public GameObject objCamTheWall;
    public GameObject objCamBall;
    private Cinemachine.CinemachineVirtualCamera camTheWall;

    // Objetos - The Wall
    public GameObject ball; private float ballZ = 11.918f, ballY = 37.48f;
    private Renderer ballRender; // Render da bola
    public GameObject[] objResults;


    // Objeto - Vidro inteiro
    public GameObject vidro;
    public GameObject vidroQuebrado;
    public GameObject vidroQuebrando;

    // Objeto que exibe a quest�o e seu componente
    public GameObject questao; private TextMeshProUGUI textoQuestao;

    // Objeto que exibe o dinheiro
    public GameObject cash;

    // Objeto que exibe a bola da vez
    public GameObject objCurrentBall; private Image currentBall;
    public Sprite[] balls;

    // Objeto - V�deo de morte
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

    // Bot�o de pular introdu��o
    public GameObject btnSkip;

    // Bot�es das alternativas
    public GameObject btnAlt1;
    public GameObject btnAlt2;
    public GameObject btnAlt3;
    public GameObject btnAlt4;
    public GameObject btnNext;

    // Imagens dos bot�es
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

    // Texto da quest�o atual
    public GameObject objQtdQuestions;
    private TextMeshProUGUI txtQtdQuestions;

    // Texto do total de quest�es
    public GameObject objTxtTotQuestions;
    private TextMeshProUGUI txtTotQuestions;



    // M�todos =======================================================================================================================================================================================

    void Start()
    {
        // Pegando os filhos (textos) dos bot�es das alternativas || Pegando o componente Text Pro de cada filho e salvando em seus atributos textoAlt
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

        ballRender = ball.GetComponent<Renderer>();

        currentBall = objCurrentBall.GetComponent<Image>();


        // Criar listerner para o fim da cena de introdu��o e partir para o quiz
        if (sceneIntroduction != null)
        {
            sceneIntroduction.stopped += OnIntroductionFinished; // Cria um listerner pra ficar ouvindo quando a introdu��o for parada, e executa o m�todo OnIntroductionFinished
        }

        // Toca a m�sica de fundo
        aud[1].clip = mingleInstrumental;
        aud[1].loop = true;

        CarregarQuestoes();

        NextQuestion();
    }


    void Update()
    {

    }

    // Armazena em um list as quest�es do arquivo Json
    void CarregarQuestoes()
    {
        TextAsset jsonFile = Resources.Load<TextAsset>(DadosJogo.tipoPerguntas); // Pega o arquivo JSON

        if (jsonFile != null)
        {
            Questions[] arrayQuestoes = JsonHelper.FromJson<Questions>(jsonFile.text); // Desserializa o json e carrega em um array chamado arrayQuestoes

            questoes = new List<Questions>(arrayQuestoes); // Carrega as quest�es do list tempor�rio no list das quest�es
            totQuestoes = questoes.Count;
            txtTotQuestions.text = totQuestoes.ToString();
        }
        else
        {
            Debug.LogError("Arquivo JSON n�o encontrado na pasta Resources!");
        }
        
    }

    // Usado no bot�o de saltar introdu��o
    public void IntroductionSkip()
    {
        quizOptions.SetActive(true);
        vidro.SetActive(false);
        currentQuestion = 1; txtQtdQuestions.text = currentQuestion.ToString();
        sceneIntroduction.Stop();
        sceneIntroduction.stopped -= OnIntroductionFinished;
        btnSkip.SetActive(false);
    }

    // Usado quando a cena de introdu��o terminar
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

        if (correta == value) // Acertou
        {

            switch (value)
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

            // Bloqueia os bot�es
            buttonBtn1.enabled = false;
            buttonBtn2.enabled = false;
            buttonBtn3.enabled = false;
            buttonBtn4.enabled = false;

            totAcertos++;

            dinheiro += 50;
            cash.GetComponent<TextMeshProUGUI>().text = "R$" + dinheiro.ToString();

            if (bolaDaVez == DadosJogo.mainBall) // Se a bola principal for igual a bola atual
            {
                StartCoroutine(HideQuestionsAfterToHit(2f));
            }
            else
            {
                // Exibir bot�o next
                btnNext.SetActive(true);
            }

        }
        else // Errou
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

            // Verifica qual a quest�o correta
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

            // Bloqueia os bot�es
            buttonBtn1.enabled = false;
            buttonBtn2.enabled = false;
            buttonBtn3.enabled = false;
            buttonBtn4.enabled = false;

            totErros++;

            StartCoroutine(HideQuestionsAfterError(2f));


        }
    }

    // Carrega a pr�xima quest�o ou exibe o final do jogo
    public void NextQuestion()
    {
        if ((totAcertos + totErros) == totQuestoes)
        {
            Debug.Log("Fim de jogo!");
        }
        else
        {
            // Desbloquear bot�es
            btnNext.SetActive(false);
            buttonBtn1.enabled = true;
            buttonBtn2.enabled = true;
            buttonBtn3.enabled = true;
            buttonBtn4.enabled = true;

            // Define a imagem padr�o para os 4 bot�es
            imageBtn1.sprite = padrao;
            imageBtn2.sprite = padrao;
            imageBtn3.sprite = padrao;
            imageBtn4.sprite = padrao;

            // Atualiza a quest�o atual
            currentQuestion+=1;
            txtQtdQuestions.text = currentQuestion.ToString();

            // Sortear a quest�o da vez
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

            // Insere a quest�o correta na posi��o definida pelo random
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

            // Remove a quest�o atual
            questoes.RemoveAt(questaoAtual);

            // Sorteia a bola da vez
            System.Random random2 = new System.Random();
            bolaDaVez = random2.Next(1, 15);

            switch(bolaDaVez)
            {
                case 1:
                    currentBall.sprite = balls[0];
                    break;
                case 2:
                    currentBall.sprite = balls[1];
                    break;
                case 3:
                    currentBall.sprite = balls[2];
                    break;
                case 4:
                    currentBall.sprite = balls[3];
                    break;
                case 5:
                    currentBall.sprite = balls[4];
                    break;
                case 6:
                    currentBall.sprite = balls[5];
                    break;
                case 7:
                    currentBall.sprite = balls[6];
                    break;
                case 8:
                    currentBall.sprite = balls[7];
                    break;
                case 9:
                    currentBall.sprite = balls[8];
                    break;
                case 10:
                    currentBall.sprite = balls[9];
                    break;
                case 11:
                    currentBall.sprite = balls[10];
                    break;
                case 12:
                    currentBall.sprite = balls[11];
                    break;
                case 13:
                    currentBall.sprite = balls[12];
                    break;
                case 14:
                    currentBall.sprite = balls[13];
                    break;
                case 15:
                    currentBall.sprite = balls[14];
                    break;
            }
        }
    }

    public void BallPosition(int position)
    {
        float xAleatorio = 0.0f;

        // Scale
        float xyzScale = UnityEngine.Random.Range(0.1367179f, 0.3197531f);

        switch (position)
        {
            case 1:
                xAleatorio = UnityEngine.Random.Range(325.249f, 325.697f);
                break;
            case 2:
                xAleatorio = UnityEngine.Random.Range(326.144f, 326.605f);
                break;
            case 3:
                xAleatorio = UnityEngine.Random.Range(327.03f, 327.491f);
                break;
            case 4:
                xAleatorio = UnityEngine.Random.Range(327.931f, 328.364f);
                break;
            case 5:
                xAleatorio = UnityEngine.Random.Range(328.822f, 329.274f);
                break;
            case 6:
                xAleatorio = UnityEngine.Random.Range(329.71f, 330.162f);
                break;
            case 7:
                xAleatorio = UnityEngine.Random.Range(330.614f, 331.062f);
                break;
        }

        ball.transform.position = new Vector3(xAleatorio, ballY, ballZ);
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

        // Define a textura da bola sorteada
        ball.GetComponent<BallController>().ChangeBallTexture(bolaDaVez);

    }
    
    public IEnumerator PlayConsequence(float delay, int conseq)
    {
        yield return new WaitForSeconds(delay);
        camTheWall.Priority = 9;
        wallOptions.SetActive(false);
        aud[0].Stop();

        // Quebrado: -1 quebra, 0 perde dinheiro, 1 nada, 2 ganha dinheiro
        // N�o quebrado: -1 trinca, 0 perde dinheiro, 1 nada, 2 ganha dinheiro

        if (!quebrado) // Se n�o tiver trincado
        {
            if (conseq == -1) // trinca
            {
                quebrado = true;
                vidro.SetActive(true);
                vidroQuebrado.SetActive(true);
                vidroQuebrando.SetActive(false);
                cutsceneTrincando.Play();

                // Listener para exibir o bot�o de pr�xima quest�o
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
        else // Se j� tiver trincado
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

        List<int> conseq = new List<int>();
        if (DadosJogo.teamLisa) // Team Lisa
        {
            if (bolaDaVez == DadosJogo.mainBall) // Se errou e a bola era a principal
            {
                conseq.AddRange(new int[] { -1, -1, -1, -1, -1, -1, -1, -1, -1, 0, 0, 0, 0, 1, 2 }); // 80% desvantagem 20% vantagem
            }
            else
            {
                if (bolaDaVez > 0 && bolaDaVez < 8) // Bola da vez = time lisa
                {
                    conseq.AddRange(new int[] { -1, -1, -1, -1, 0, 0, 1, 1, 1, 1, 1, 2, 2, 2, 2 }); // 60% vantagem 40% desvantagem
                }
                else if (bolaDaVez > 8 && bolaDaVez <= 15) // Bola da vez = time listrada
                {
                    conseq.AddRange(new int[] { -1, -1, -1, -1, -1, -1, 0, 0, 0, 1, 1, 1, 1, 2, 2 }); // 60% desvantagem 40% vantagem
                }
                else // Bola da vez = bola 8
                {
                    conseq.AddRange(new int[] { -1, -1, -1, -1, -1, -1, -1, -1, -1, 0, 0, 0, 0, 1, 2 }); // 80% desvantagem 20% vantagem
                }
            }

            
        }
        else // Team Listrado
        {
            if (bolaDaVez == DadosJogo.mainBall) // Se errou e a bola era a principal
            {
                conseq.AddRange(new int[] { -1, -1, -1, -1, -1, -1, -1, -1, -1, 0, 0, 0, 0, 1, 2 }); // 80% desvantagem 20% vantagem
            }
            else
            {
                if (bolaDaVez > 8 && bolaDaVez <= 15) // Bola da vez = time listrada
                {
                    conseq.AddRange(new int[] { -1, -1, -1, -1, 0, 0, 1, 1, 1, 1, 1, 2, 2, 2, 2 }); // 60% vantagem 40% desvantagem
                }
                else if (bolaDaVez > 0 && bolaDaVez < 8) // Bola da vez = time lisa
                {
                    conseq.AddRange(new int[] { -1, -1, -1, -1, -1, -1, 0, 0, 0, 1, 1, 1, 1, 2, 2 }); // 60% desvantagem 40% vantagem
                }
                else // Bola da vez = bola 8
                {
                    conseq.AddRange(new int[] { -1, -1, -1, -1, -1, -1, -1, -1, -1, 0, 0, 0, 0, 1, 2 }); // 80% desvantagem 20% vantagem
                }
            }
            
        }


        for (int c = 0; c < 15; c++)
        {
            
            System.Random random = new System.Random();
            int aleatorio = random.Next(0, conseq.Count - 1); // Sorteia um �ndice do list conseq (de 0 a 14)


            objResults[c].GetComponent<PontoColisao>().consequencia = conseq[aleatorio]; // Um ponto de colis�o da bola recebe uma das consequ�ncias da list e armazena no atributo consequencia do script
            objResults[c].GetComponent<PontoColisao>().AlterarFundo(); // Altera o fundo do ponto de colis�o
            conseq.RemoveAt(aleatorio); // Remove a consequ�ncia do list

        }
    }

    private IEnumerator HideQuestionsAfterToHit(float delay)
    {
        yield return new WaitForSeconds(delay);
        quizOptions.SetActive(false);
        camTheWall.Priority = 11;
        wallOptions.SetActive(true);

        List<int> conseq = new List<int>();
        conseq.AddRange(new int[] { 1, 1, 1, 1, 1, 1, 1, 2, 2, 2, 2, 2, 2, 2, 2 }); // 55% ganhar dinheiro 45% nada


        for (int c = 0; c < 15; c++)
        {

            System.Random random = new System.Random();
            int aleatorio = random.Next(0, conseq.Count - 1); // Sorteia um �ndice do list conseq (de 0 a 14)


            objResults[c].GetComponent<PontoColisao>().consequencia = conseq[aleatorio]; // Um ponto de colis�o da bola recebe uma das consequ�ncias da list e armazena no atributo consequencia do script
            objResults[c].GetComponent<PontoColisao>().AlterarFundo(); // Altera o fundo do ponto de colis�o
            conseq.RemoveAt(aleatorio); // Remove a consequ�ncia do list

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
    public static bool teamLisa;
    public static int mainBall;
}

