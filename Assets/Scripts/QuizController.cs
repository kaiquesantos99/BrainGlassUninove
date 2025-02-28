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


public class QuizController : MonoBehaviour
{
    // Atributos =======================================================================================================================================================================================
    public int correta;
    public int questaoAtual;
    public int totErros;
    public int totAcertos;
    public int totQuestoes;
    public bool quebrado;

    // Lista de Quest�es
    List<Questions> questoes = new List<Questions>();

    // Objeto - Vidro inteiro
    public GameObject vidro;
    public GameObject vidroQuebrado;
    public GameObject vidroQuebrando;

    // Objeto que exibe a quest�o e seu componente
    public GameObject questao; private TextMeshProUGUI textoQuestao;

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

    // Quiz Options
    public GameObject quizOptions;

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

    // Insere as quest�es no ArrayList
    void CarregarQuestoes()
    {
        // Criando as quest�es e armazenando na lista
        Questions q1 = new Questions("Qual dos seguintes bancos de dados � relacional?", "MySQL", "MongoDB", "Redis", "Cassandra");
        Questions q2 = new Questions("O que significa a sigla IDE na programa��o?", "Integrated Development Environment", "Internet Development Engine", "Interface Data Execution", "Interactive Debugging Environment");
        Questions q3 = new Questions("Qual das op��es a seguir � um servi�o de computa��o em nuvem da AWS?", "EC2", "Azure Functions", "Google Cloud Run", "Firebase");
        Questions q4 = new Questions("Qual linguagem � mais utilizada no desenvolvimento back-end com ASP.NET?", "C#", "Java", "Python", "Ruby");
        Questions q5 = new Questions("No PHP, qual s�mbolo � usado para indicar vari�veis?", "$", "@", "#", "&");
        Questions q6 = new Questions("No DevOps, qual ferramenta � amplamente usada para automa��o de pipelines de CI/CD?", "Jenkins", "XAMPP", "Unity", "SQLite");
        Questions q7 = new Questions("Qual comando Git � usado para enviar as altera��es locais para um reposit�rio remoto?", "git push", "git commit", "git clone", "git merge");
        Questions q8 = new Questions("Qual � a fun��o do comando 'SELECT' no SQL?", "Recuperar dados de um banco", "Inserir novos dados", "Atualizar dados existentes", "Excluir uma tabela");
        Questions q9 = new Questions("Em programa��o orientada a objetos, qual termo se refere ao reuso de c�digo atrav�s da cria��o de subclasses?", "Heran�a", "Encapsulamento", "Polimorfismo", "Composi��o");
        Questions q10 = new Questions("No C#, qual palavra-chave � usada para definir uma interface?", "interface", "abstract", "implements", "define");
        Questions q11 = new Questions("O que significa a sigla API?", "Application Programming Interface", "Advanced Process Integration", "Automated Programming Interface", "Application Program Input");
        Questions q12 = new Questions("No modelo OSI, qual camada � respons�vel pelo roteamento dos pacotes?", "Rede", "Transporte", "Enlace de Dados", "Aplica��o");
        Questions q13 = new Questions("No Docker, qual comando � usado para criar e iniciar um cont�iner?", "docker run", "docker build", "docker pull", "docker compose");
        Questions q14 = new Questions("O que significa DRY na programa��o?", "Don't Repeat Yourself", "Data Recovery Yield", "Dynamic Runtime Yield", "Development Resource Year");
        Questions q15 = new Questions("No JavaScript, qual � a fun��o do m�todo 'addEventListener'?", "Adicionar um evento a um elemento", "Criar uma nova vari�vel", "Definir um timeout", "Encerrar um loop");
        Questions q16 = new Questions("Qual dos seguintes protocolos � usado para transfer�ncia segura de arquivos?", "SFTP", "FTP", "HTTP", "SMTP");
        Questions q17 = new Questions("Em SQL, qual comando � usado para alterar a estrutura de uma tabela?", "ALTER TABLE", "UPDATE TABLE", "MODIFY TABLE", "CHANGE TABLE");
        Questions q18 = new Questions("Qual � a principal funcionalidade do Docker?", "Criar ambientes isolados para execu��o de aplica��es", "Gerenciar reposit�rios de c�digo", "Compilar programas em diferentes linguagens", "Automatizar testes unit�rios");
        Questions q19 = new Questions("Qual � a fun��o do `foreach` em C#?", "Percorrer elementos de uma cole��o", "Criar um novo array", "Declarar uma vari�vel", "Concatenar strings");
        Questions q20 = new Questions("No React, qual hook � usado para gerenciar estado em componentes funcionais?", "useState", "useEffect", "useContext", "useReducer");

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
        questoes.Add(q14);
        questoes.Add(q15);
        questoes.Add(q16);
        questoes.Add(q17);
        questoes.Add(q18);
        questoes.Add(q19);
        questoes.Add(q20);

        Questions q21 = new Questions("Qual estrutura de dados segue o princ�pio LIFO (Last In, First Out)?", "Pilha", "Fila", "Lista Encadeada", "�rvore Bin�ria");
        Questions q22 = new Questions("Qual das op��es abaixo � um banco de dados NoSQL?", "MongoDB", "PostgreSQL", "MySQL", "Oracle SQL");
        Questions q23 = new Questions("Qual dos seguintes comandos � usado para criar uma chave estrangeira no SQL?", "FOREIGN KEY", "PRIMARY KEY", "CONSTRAINT UNIQUE", "INDEX");
        Questions q24 = new Questions("No C#, qual palavra-chave � usada para indicar que uma classe n�o pode ser instanciada diretamente?", "abstract", "sealed", "static", "readonly");
        Questions q25 = new Questions("O que significa a sigla ORM?", "Object-Relational Mapping", "Optimized Runtime Memory", "Object Reactive Management", "Open Relational Model");
        Questions q26 = new Questions("Qual linguagem � mais comumente usada para criar scripts de automa��o em servidores Linux?", "Bash", "C#", "Java", "Swift");
        Questions q27 = new Questions("No PHP, qual fun��o � usada para conectar-se a um banco de dados MySQL?", "mysqli_connect()", "connect_mysql()", "open_db()", "db_link()");
        Questions q28 = new Questions("Qual das seguintes op��es � uma pr�tica recomendada para seguran�a em banco de dados?", "Uso de Prepared Statements para evitar SQL Injection", "Armazenamento de senhas em texto plano", "Desativar logs de auditoria", "Uso de chaves prim�rias duplicadas");
        Questions q29 = new Questions("Qual ferramenta � usada para orquestra��o de cont�ineres no DevOps?", "Kubernetes", "Jenkins", "Docker Compose", "Terraform");
        Questions q30 = new Questions("Em um banco de dados relacional, qual � o objetivo da normaliza��o?", "Reduzir redund�ncia e melhorar integridade dos dados", "Aumentar a velocidade das consultas", "Criar mais �ndices automaticamente", "Adicionar novas colunas �s tabelas");
        Questions q31 = new Questions("Qual operador l�gico representa 'E' em C#?", "&&", "||", "!", "==");
        Questions q32 = new Questions("No Git, qual comando � usado para criar um novo branch?", "git branch nome-do-branch", "git commit -b", "git checkout nome-do-branch", "git new branch");
        Questions q33 = new Questions("Qual das op��es abaixo **n�o** � um tipo de dado primitivo em C#?", "Array", "int", "char", "double");
        Questions q34 = new Questions("O que faz o m�todo 'Dispose()' em C#?", "Libera recursos n�o gerenciados", "Fecha o programa", "Exclui um arquivo", "Libera mem�ria automaticamente pelo Garbage Collector");
        Questions q35 = new Questions("Qual dos seguintes conceitos est� diretamente relacionado ao DevOps?", "Integra��o Cont�nua", "Modelo em Cascata", "Programa��o Funcional", "Cria��o de Firewalls");
        Questions q36 = new Questions("O que � o conceito de 'Encapsulamento' na Programa��o Orientada a Objetos?", "Esconder detalhes internos de uma classe e expor apenas o necess�rio", "Criar m�ltiplos objetos de uma classe", "Permitir que m�todos tenham o mesmo nome", "Herdar atributos de uma classe pai");
        Questions q37 = new Questions("No SQL, qual comando � usado para remover uma tabela do banco de dados?", "DROP TABLE", "DELETE TABLE", "REMOVE TABLE", "TRUNCATE TABLE");
        Questions q38 = new Questions("No AWS, qual servi�o � usado para armazenar arquivos na nuvem?", "S3", "EC2", "Lambda", "RDS");
        Questions q39 = new Questions("O que � um 'Commit' no Git?", "Salvar altera��es no reposit�rio local", "Baixar um reposit�rio remoto", "Unir dois branches", "Enviar altera��es para o reposit�rio remoto");
        Questions q40 = new Questions("No PHP, qual fun��o � usada para iniciar uma sess�o?", "session_start()", "start_session()", "begin_session()", "init_session()");

        questoes.Add(q21);
        questoes.Add(q22);
        questoes.Add(q23);
        questoes.Add(q24);
        questoes.Add(q25);
        questoes.Add(q26);
        questoes.Add(q27);
        questoes.Add(q28);
        questoes.Add(q29);
        questoes.Add(q30);
        questoes.Add(q31);
        questoes.Add(q32);
        questoes.Add(q33);
        questoes.Add(q34);
        questoes.Add(q35);
        questoes.Add(q36);
        questoes.Add(q37);
        questoes.Add(q38);
        questoes.Add(q39);
        questoes.Add(q40);

        Questions q41 = new Questions("Qual estrutura de dados utiliza o conceito FIFO (First In, First Out)?", "Fila", "Pilha", "Lista Encadeada", "�rvore AVL");
        Questions q42 = new Questions("No MySQL, qual comando � usado para atualizar um registro em uma tabela?", "UPDATE", "MODIFY", "CHANGE", "ALTER");
        Questions q43 = new Questions("Qual das op��es abaixo � um servi�o de Banco de Dados gerenciado pela AWS?", "Amazon RDS", "Google Firestore", "Microsoft SQL Server", "PostgreSQL Local");
        Questions q44 = new Questions("O que significa a sigla CRUD na programa��o?", "Create, Read, Update, Delete", "Convert, Restore, Upload, Delete", "Compile, Run, Update, Deploy", "Cache, Render, Update, Download");
        Questions q45 = new Questions("Em redes de computadores, qual protocolo � usado para atribui��o din�mica de IPs?", "DHCP", "TCP", "UDP", "ARP");
        Questions q46 = new Questions("No C#, qual � a diferen�a entre `==` e `Equals()`?", "`==` compara valores primitivos, `Equals()` compara objetos", "`==` sempre retorna falso em objetos", "`Equals()` � apenas para strings", "`==` � usado apenas em structs");
        Questions q47 = new Questions("Qual � a fun��o do comando `git fetch`?", "Baixar altera��es do reposit�rio remoto sem mesclar", "Enviar altera��es para o reposit�rio remoto", "Criar um novo branch", "Mesclar mudan�as locais e remotas");
        Questions q48 = new Questions("No Docker, qual comando � usado para listar todos os cont�ineres ativos?", "docker ps", "docker list", "docker status", "docker show");
        Questions q49 = new Questions("Qual tipo de dado � imut�vel em Python?", "Tuple", "List", "Dictionary", "Set");
        Questions q50 = new Questions("No C#, qual � o operador usado para declarar um evento em uma classe?", "event", "delegate", "action", "handler");
        Questions q51 = new Questions("Qual conceito de POO permite que uma classe herde caracter�sticas de outra?", "Heran�a", "Polimorfismo", "Encapsulamento", "Abstra��o");
        Questions q52 = new Questions("No SQL, qual comando � usado para criar um �ndice?", "CREATE INDEX", "NEW INDEX", "SET INDEX", "INDEX ADD");
        Questions q53 = new Questions("O que significa JSON na programa��o?", "JavaScript Object Notation", "Java Standard Object Network", "Java System Operation Name", "Java Syntax Optimization");
        Questions q54 = new Questions("Qual � a principal vantagem de utilizar REST em APIs?", "Utiliza protocolos padr�o como HTTP", "Usa apenas servidores dedicados", "� baseado exclusivamente em XML", "N�o necessita de autentica��o");
        Questions q55 = new Questions("No Linux, qual comando � usado para exibir o conte�do de um arquivo?", "cat", "ls", "pwd", "rm");
        Questions q56 = new Questions("Qual das op��es abaixo � um protocolo de comunica��o segura na internet?", "HTTPS", "FTP", "POP3", "Telnet");
        Questions q57 = new Questions("No modelo OSI, em qual camada o protocolo TCP opera?", "Transporte", "Rede", "Sess�o", "Aplica��o");
        Questions q58 = new Questions("Qual � o objetivo do Garbage Collector em C#?", "Liberar mem�ria automaticamente", "Compilar o c�digo mais r�pido", "Gerenciar a execu��o de loops", "Evitar erros de sintaxe");
        Questions q59 = new Questions("Em DevOps, qual ferramenta � usada para gerenciar infraestrutura como c�digo?", "Terraform", "Docker", "Kubernetes", "Jenkins");
        Questions q60 = new Questions("O que � o conceito de 'Shadowing' em programa��o?", "Quando uma vari�vel local esconde uma vari�vel de escopo maior", "Quando um c�digo se torna obsoleto", "Quando um m�todo n�o retorna valor", "Quando duas classes compartilham o mesmo nome");

        questoes.Add(q41);
        questoes.Add(q42);
        questoes.Add(q43);
        questoes.Add(q44);
        questoes.Add(q45);
        questoes.Add(q46);
        questoes.Add(q47);
        questoes.Add(q48);
        questoes.Add(q49);
        questoes.Add(q50);
        questoes.Add(q51);
        questoes.Add(q52);
        questoes.Add(q53);
        questoes.Add(q54);
        questoes.Add(q55);
        questoes.Add(q56);
        questoes.Add(q57);
        questoes.Add(q58);
        questoes.Add(q59);
        questoes.Add(q60);

        totQuestoes = questoes.Count;
    }

    // Usado no bot�o de saltar introdu��o
    public void IntroductionSkip()
    {
        quizOptions.SetActive(true);
        vidro.SetActive(false);
        sceneIntroduction.Stop();
        sceneIntroduction.stopped -= OnIntroductionFinished;
        btnSkip.SetActive(false);
    }

    // Usado quando a cena de introdu��o terminar
    void OnIntroductionFinished(PlayableDirector obj)
    {
        quizOptions.SetActive(true);
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

            // Bloqueia os bot�es
            buttonBtn1.enabled = false;
            buttonBtn2.enabled = false;
            buttonBtn3.enabled = false;
            buttonBtn4.enabled = false;

            totAcertos++;

            // Exibir bot�o next
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

            HideQuizWithDelay();
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

    public void ShowQuizOptions()
    {
        quizOptions.SetActive(true);
        btnSkip.SetActive(false);
    }

    public void HideQuizWithDelay()
    {
        StartCoroutine(DisableAfterSeconds(2f));
    }

    private IEnumerator DisableAfterSeconds(float delay)
    {
        yield return new WaitForSeconds(delay);
        quizOptions.SetActive(false);

        System.Random aleatorio = new System.Random();
        int numero = aleatorio.Next(1,3);

        if (!quebrado) // Se n�o tiver trincado
        {
            if (numero%2!=0) // N�o morre
            {
                quebrado = true;
                vidro.SetActive(true);
                vidroQuebrado.SetActive(true);
                vidroQuebrando.SetActive(false);
                cutsceneTrincando.Play();

                // Listener para exibir o bot�o de pr�xima quest�o
                cutsceneTrincando.stopped += OnTrincandoFinished;
            }
            else // Morre
            {
                vidro.SetActive(true);
                vidroQuebrando.SetActive(true);
                cutsceneMorteDePrimeira.Play(); // Essa j� est� certo

                // Cria um listener
                cutsceneMorteDePrimeira.stopped += OnDeadFinished;
            }
            
        }
        else // Se j� tiver trincado
        {
            if (numero%2!=0) // N�o morre
            {
                vidro.SetActive(false);
                vidroQuebrado.SetActive(true);
                vidroQuebrando.SetActive(false);
                cutsceneSusto.Play();

                // Listener para exibir o bot�o de pr�xima quest�o
                cutsceneSusto.stopped += OnSustoFinished;
            }
            else // Morre
            {
                vidro.SetActive(false);
                vidroQuebrado.SetActive(false);
                vidroQuebrando.SetActive(true);
                cutsceneMorteLonga.Play();

                // Cria um listener
                cutsceneMorteLonga.stopped += OnDeadFinished;
            }
        }
    }

    void OnTrincandoFinished(PlayableDirector obj)
    {
        cutsceneTrincando.stopped -= OnTrincandoFinished;
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
