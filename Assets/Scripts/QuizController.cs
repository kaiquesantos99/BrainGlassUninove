using System.Collections;
using System.Collections.Generic;
using System.Globalization;
using System.Security.Cryptography;
using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class QuizController : MonoBehaviour
{
    // Atributos da partida
    public int correta;
    public int questaoAtual;
    public int totErros;
    public int totAcertos;
    public int totQuestoes;

    // Lista de Quest�es
    List<Questions> questoes = new List<Questions>();

    // Objeto que exibe a quest�o e seu componente
    public GameObject questao; private TextMeshProUGUI textoQuestao;

    // Efeitos Sonoros
    private AudioSource[] aud;
    public AudioClip somX;
    public AudioClip somO;
    public AudioClip mingleInstrumental;

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

        // Toca a m�sica de fundo
        aud[1].clip = mingleInstrumental;
        aud[1].loop = true;
        aud[1].Play();



        // Criando as quest�es e armazenando na lista
        Questions q1 = new Questions("Qual destas linguagens pertence a gigante de tecnologia Oracle?", "Java", "PHP", "HTML", "Ruby");
        Questions q2 = new Questions("Qual � a principal linguagem utilizada para desenvolver aplicativos para iOS?", "Swift", "Kotlin", "C#", "Python");
        Questions q3 = new Questions("Em programa��o orientada a objetos, qual � o conceito que permite um m�todo ter o mesmo nome, mas comportamentos diferentes?", "Polimorfismo", "Encapsulamento", "Heran�a", "Abstra��o");
        Questions q4 = new Questions("Qual das op��es a seguir � uma linguagem de marca��o?", "HTML", "Java", "C#", "Ruby");
        Questions q5 = new Questions("O que significa a sigla SGBD?", "Sistema de Gerenciamento de Banco de Dados", "Sistema Global de Banco de Dados", "Servi�o Geral de Banco de Dados", "Software de Gerenciamento de Big Data");
        Questions q6 = new Questions("No modelo relacional de banco de dados, como chamamos uma linha de uma tabela?", "Tupla", "Atributo", "Chave Estrangeira", "Coluna");
        Questions q7 = new Questions("Qual � a palavra-chave usada em C# para criar uma classe?", "class", "create", "new", "object");
        Questions q8 = new Questions("Qual � a fun��o do operador `this` em programa��o orientada a objetos?", "Referenciar o pr�prio objeto da classe", "Criar um novo objeto", "Encapsular dados", "Indicar heran�a");
        Questions q9 = new Questions("Em sistemas operacionais, qual � a principal fun��o de um escalonador?", "Gerenciar o uso da CPU entre os processos", "Controlar o acesso � mem�ria", "Organizar o sistema de arquivos", "Gerenciar conex�es de rede");
        Questions q10 = new Questions("Qual das seguintes op��es � considerada um paradigma de programa��o?", "Orienta��o a Objetos", "SQL", "MySQL", "Data Science");
        Questions q11 = new Questions("Qual � o principal objetivo da normaliza��o em banco de dados?", "Eliminar redund�ncias e inconsist�ncias", "Aumentar a velocidade de acesso", "Organizar os �ndices", "Criar chaves prim�rias");
        Questions q12 = new Questions("Qual o maior time brasileiro?", "Santos", "Palmeiras", "Gr�mio", "Corinthians");
        Questions q13 = new Questions("Qual time possui 2 mundiais com 1 libertadores?", "Corinthians", "Santos", "S�o Paulo", "Palmeiras");

        questoes.Add(q1);
        questoes.Add(q2);
        questoes.Add(q3);
        questoes.Add(q4);
        questoes.Add(q5);
        questoes.Add(q6);
        questoes.Add(q7);
        questoes.Add(q8);
        questoes.Add(q9);
        questoes.Add(q10);
        questoes.Add(q11);
        questoes.Add(q12);
        questoes.Add(q13);
        totQuestoes = questoes.Count;

        NextQuestion();
    }


    void Update()
    {

    }


    public void AnswerCheck(int value)
    {
        

        // Exibir bot�o next
        btnNext.SetActive(true);

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

            // Bloqueia os bot�es
            buttonBtn1.enabled = false;
            buttonBtn2.enabled = false;
            buttonBtn3.enabled = false;
            buttonBtn4.enabled = false;

            totAcertos++;
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
        }
    }

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
        }

        
    }
}






public class Questions
{
    public string questao;
    public string alt1;
    public string alt2;
    public string alt3;
    public string alt4;

    public Questions(string questao, string alt1, string alt2, string alt3, string alt4)
    {
        this.questao = questao;
        this.alt1 = alt1;
        this.alt2 = alt2;
        this.alt3 = alt3;
        this.alt4 = alt4;
    }
}
