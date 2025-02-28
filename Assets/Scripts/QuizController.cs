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

    // Lista de Questões
    List<Questions> questoes = new List<Questions>();

    // Objeto - Vidro inteiro
    public GameObject vidro;
    public GameObject vidroQuebrado;
    public GameObject vidroQuebrando;

    // Objeto que exibe a questão e seu componente
    public GameObject questao; private TextMeshProUGUI textoQuestao;

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

    // Quiz Options
    public GameObject quizOptions;

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



    // Métodos =======================================================================================================================================================================================

    void Start()
    {
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

    // Insere as questões no ArrayList
    void CarregarQuestoes()
    {
        // Criando as questões e armazenando na lista
        Questions q1 = new Questions("Qual dos seguintes bancos de dados é relacional?", "MySQL", "MongoDB", "Redis", "Cassandra");
        Questions q2 = new Questions("O que significa a sigla IDE na programação?", "Integrated Development Environment", "Internet Development Engine", "Interface Data Execution", "Interactive Debugging Environment");
        Questions q3 = new Questions("Qual das opções a seguir é um serviço de computação em nuvem da AWS?", "EC2", "Azure Functions", "Google Cloud Run", "Firebase");
        Questions q4 = new Questions("Qual linguagem é mais utilizada no desenvolvimento back-end com ASP.NET?", "C#", "Java", "Python", "Ruby");
        Questions q5 = new Questions("No PHP, qual símbolo é usado para indicar variáveis?", "$", "@", "#", "&");
        Questions q6 = new Questions("No DevOps, qual ferramenta é amplamente usada para automação de pipelines de CI/CD?", "Jenkins", "XAMPP", "Unity", "SQLite");
        Questions q7 = new Questions("Qual comando Git é usado para enviar as alterações locais para um repositório remoto?", "git push", "git commit", "git clone", "git merge");
        Questions q8 = new Questions("Qual é a função do comando 'SELECT' no SQL?", "Recuperar dados de um banco", "Inserir novos dados", "Atualizar dados existentes", "Excluir uma tabela");
        Questions q9 = new Questions("Em programação orientada a objetos, qual termo se refere ao reuso de código através da criação de subclasses?", "Herança", "Encapsulamento", "Polimorfismo", "Composição");
        Questions q10 = new Questions("No C#, qual palavra-chave é usada para definir uma interface?", "interface", "abstract", "implements", "define");
        Questions q11 = new Questions("O que significa a sigla API?", "Application Programming Interface", "Advanced Process Integration", "Automated Programming Interface", "Application Program Input");
        Questions q12 = new Questions("No modelo OSI, qual camada é responsável pelo roteamento dos pacotes?", "Rede", "Transporte", "Enlace de Dados", "Aplicação");
        Questions q13 = new Questions("No Docker, qual comando é usado para criar e iniciar um contêiner?", "docker run", "docker build", "docker pull", "docker compose");
        Questions q14 = new Questions("O que significa DRY na programação?", "Don't Repeat Yourself", "Data Recovery Yield", "Dynamic Runtime Yield", "Development Resource Year");
        Questions q15 = new Questions("No JavaScript, qual é a função do método 'addEventListener'?", "Adicionar um evento a um elemento", "Criar uma nova variável", "Definir um timeout", "Encerrar um loop");
        Questions q16 = new Questions("Qual dos seguintes protocolos é usado para transferência segura de arquivos?", "SFTP", "FTP", "HTTP", "SMTP");
        Questions q17 = new Questions("Em SQL, qual comando é usado para alterar a estrutura de uma tabela?", "ALTER TABLE", "UPDATE TABLE", "MODIFY TABLE", "CHANGE TABLE");
        Questions q18 = new Questions("Qual é a principal funcionalidade do Docker?", "Criar ambientes isolados para execução de aplicações", "Gerenciar repositórios de código", "Compilar programas em diferentes linguagens", "Automatizar testes unitários");
        Questions q19 = new Questions("Qual é a função do `foreach` em C#?", "Percorrer elementos de uma coleção", "Criar um novo array", "Declarar uma variável", "Concatenar strings");
        Questions q20 = new Questions("No React, qual hook é usado para gerenciar estado em componentes funcionais?", "useState", "useEffect", "useContext", "useReducer");

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

        Questions q21 = new Questions("Qual estrutura de dados segue o princípio LIFO (Last In, First Out)?", "Pilha", "Fila", "Lista Encadeada", "Árvore Binária");
        Questions q22 = new Questions("Qual das opções abaixo é um banco de dados NoSQL?", "MongoDB", "PostgreSQL", "MySQL", "Oracle SQL");
        Questions q23 = new Questions("Qual dos seguintes comandos é usado para criar uma chave estrangeira no SQL?", "FOREIGN KEY", "PRIMARY KEY", "CONSTRAINT UNIQUE", "INDEX");
        Questions q24 = new Questions("No C#, qual palavra-chave é usada para indicar que uma classe não pode ser instanciada diretamente?", "abstract", "sealed", "static", "readonly");
        Questions q25 = new Questions("O que significa a sigla ORM?", "Object-Relational Mapping", "Optimized Runtime Memory", "Object Reactive Management", "Open Relational Model");
        Questions q26 = new Questions("Qual linguagem é mais comumente usada para criar scripts de automação em servidores Linux?", "Bash", "C#", "Java", "Swift");
        Questions q27 = new Questions("No PHP, qual função é usada para conectar-se a um banco de dados MySQL?", "mysqli_connect()", "connect_mysql()", "open_db()", "db_link()");
        Questions q28 = new Questions("Qual das seguintes opções é uma prática recomendada para segurança em banco de dados?", "Uso de Prepared Statements para evitar SQL Injection", "Armazenamento de senhas em texto plano", "Desativar logs de auditoria", "Uso de chaves primárias duplicadas");
        Questions q29 = new Questions("Qual ferramenta é usada para orquestração de contêineres no DevOps?", "Kubernetes", "Jenkins", "Docker Compose", "Terraform");
        Questions q30 = new Questions("Em um banco de dados relacional, qual é o objetivo da normalização?", "Reduzir redundância e melhorar integridade dos dados", "Aumentar a velocidade das consultas", "Criar mais índices automaticamente", "Adicionar novas colunas às tabelas");
        Questions q31 = new Questions("Qual operador lógico representa 'E' em C#?", "&&", "||", "!", "==");
        Questions q32 = new Questions("No Git, qual comando é usado para criar um novo branch?", "git branch nome-do-branch", "git commit -b", "git checkout nome-do-branch", "git new branch");
        Questions q33 = new Questions("Qual das opções abaixo **não** é um tipo de dado primitivo em C#?", "Array", "int", "char", "double");
        Questions q34 = new Questions("O que faz o método 'Dispose()' em C#?", "Libera recursos não gerenciados", "Fecha o programa", "Exclui um arquivo", "Libera memória automaticamente pelo Garbage Collector");
        Questions q35 = new Questions("Qual dos seguintes conceitos está diretamente relacionado ao DevOps?", "Integração Contínua", "Modelo em Cascata", "Programação Funcional", "Criação de Firewalls");
        Questions q36 = new Questions("O que é o conceito de 'Encapsulamento' na Programação Orientada a Objetos?", "Esconder detalhes internos de uma classe e expor apenas o necessário", "Criar múltiplos objetos de uma classe", "Permitir que métodos tenham o mesmo nome", "Herdar atributos de uma classe pai");
        Questions q37 = new Questions("No SQL, qual comando é usado para remover uma tabela do banco de dados?", "DROP TABLE", "DELETE TABLE", "REMOVE TABLE", "TRUNCATE TABLE");
        Questions q38 = new Questions("No AWS, qual serviço é usado para armazenar arquivos na nuvem?", "S3", "EC2", "Lambda", "RDS");
        Questions q39 = new Questions("O que é um 'Commit' no Git?", "Salvar alterações no repositório local", "Baixar um repositório remoto", "Unir dois branches", "Enviar alterações para o repositório remoto");
        Questions q40 = new Questions("No PHP, qual função é usada para iniciar uma sessão?", "session_start()", "start_session()", "begin_session()", "init_session()");

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

        Questions q41 = new Questions("Qual estrutura de dados utiliza o conceito FIFO (First In, First Out)?", "Fila", "Pilha", "Lista Encadeada", "Árvore AVL");
        Questions q42 = new Questions("No MySQL, qual comando é usado para atualizar um registro em uma tabela?", "UPDATE", "MODIFY", "CHANGE", "ALTER");
        Questions q43 = new Questions("Qual das opções abaixo é um serviço de Banco de Dados gerenciado pela AWS?", "Amazon RDS", "Google Firestore", "Microsoft SQL Server", "PostgreSQL Local");
        Questions q44 = new Questions("O que significa a sigla CRUD na programação?", "Create, Read, Update, Delete", "Convert, Restore, Upload, Delete", "Compile, Run, Update, Deploy", "Cache, Render, Update, Download");
        Questions q45 = new Questions("Em redes de computadores, qual protocolo é usado para atribuição dinâmica de IPs?", "DHCP", "TCP", "UDP", "ARP");
        Questions q46 = new Questions("No C#, qual é a diferença entre `==` e `Equals()`?", "`==` compara valores primitivos, `Equals()` compara objetos", "`==` sempre retorna falso em objetos", "`Equals()` é apenas para strings", "`==` é usado apenas em structs");
        Questions q47 = new Questions("Qual é a função do comando `git fetch`?", "Baixar alterações do repositório remoto sem mesclar", "Enviar alterações para o repositório remoto", "Criar um novo branch", "Mesclar mudanças locais e remotas");
        Questions q48 = new Questions("No Docker, qual comando é usado para listar todos os contêineres ativos?", "docker ps", "docker list", "docker status", "docker show");
        Questions q49 = new Questions("Qual tipo de dado é imutável em Python?", "Tuple", "List", "Dictionary", "Set");
        Questions q50 = new Questions("No C#, qual é o operador usado para declarar um evento em uma classe?", "event", "delegate", "action", "handler");
        Questions q51 = new Questions("Qual conceito de POO permite que uma classe herde características de outra?", "Herança", "Polimorfismo", "Encapsulamento", "Abstração");
        Questions q52 = new Questions("No SQL, qual comando é usado para criar um índice?", "CREATE INDEX", "NEW INDEX", "SET INDEX", "INDEX ADD");
        Questions q53 = new Questions("O que significa JSON na programação?", "JavaScript Object Notation", "Java Standard Object Network", "Java System Operation Name", "Java Syntax Optimization");
        Questions q54 = new Questions("Qual é a principal vantagem de utilizar REST em APIs?", "Utiliza protocolos padrão como HTTP", "Usa apenas servidores dedicados", "É baseado exclusivamente em XML", "Não necessita de autenticação");
        Questions q55 = new Questions("No Linux, qual comando é usado para exibir o conteúdo de um arquivo?", "cat", "ls", "pwd", "rm");
        Questions q56 = new Questions("Qual das opções abaixo é um protocolo de comunicação segura na internet?", "HTTPS", "FTP", "POP3", "Telnet");
        Questions q57 = new Questions("No modelo OSI, em qual camada o protocolo TCP opera?", "Transporte", "Rede", "Sessão", "Aplicação");
        Questions q58 = new Questions("Qual é o objetivo do Garbage Collector em C#?", "Liberar memória automaticamente", "Compilar o código mais rápido", "Gerenciar a execução de loops", "Evitar erros de sintaxe");
        Questions q59 = new Questions("Em DevOps, qual ferramenta é usada para gerenciar infraestrutura como código?", "Terraform", "Docker", "Kubernetes", "Jenkins");
        Questions q60 = new Questions("O que é o conceito de 'Shadowing' em programação?", "Quando uma variável local esconde uma variável de escopo maior", "Quando um código se torna obsoleto", "Quando um método não retorna valor", "Quando duas classes compartilham o mesmo nome");

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

    // Usado no botão de saltar introdução
    public void IntroductionSkip()
    {
        quizOptions.SetActive(true);
        vidro.SetActive(false);
        sceneIntroduction.Stop();
        sceneIntroduction.stopped -= OnIntroductionFinished;
        btnSkip.SetActive(false);
    }

    // Usado quando a cena de introdução terminar
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

            // Bloqueia os botões
            buttonBtn1.enabled = false;
            buttonBtn2.enabled = false;
            buttonBtn3.enabled = false;
            buttonBtn4.enabled = false;

            totAcertos++;

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

            HideQuizWithDelay();
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

        if (!quebrado) // Se não tiver trincado
        {
            if (numero%2!=0) // Não morre
            {
                quebrado = true;
                vidro.SetActive(true);
                vidroQuebrado.SetActive(true);
                vidroQuebrando.SetActive(false);
                cutsceneTrincando.Play();

                // Listener para exibir o botão de próxima questão
                cutsceneTrincando.stopped += OnTrincandoFinished;
            }
            else // Morre
            {
                vidro.SetActive(true);
                vidroQuebrando.SetActive(true);
                cutsceneMorteDePrimeira.Play(); // Essa já está certo

                // Cria um listener
                cutsceneMorteDePrimeira.stopped += OnDeadFinished;
            }
            
        }
        else // Se já tiver trincado
        {
            if (numero%2!=0) // Não morre
            {
                vidro.SetActive(false);
                vidroQuebrado.SetActive(true);
                vidroQuebrando.SetActive(false);
                cutsceneSusto.Play();

                // Listener para exibir o botão de próxima questão
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
