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

    // Lista de Quest�es
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

    // Objeto que exibe a quest�o e seu componente
    public GameObject questao; private TextMeshProUGUI textoQuestao;

    // Objeto que exibe o dinheiro
    public GameObject cash;

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
        Questions q1 = new Questions("Qual a principal diferen�a entre o desenvolvimento tradicional e �gil?",
    "O modelo �gil � mais flex�vel e iterativo, enquanto o tradicional segue um planejamento r�gido",
    "O modelo tradicional � mais r�pido que o �gil",
    "O desenvolvimento �gil n�o precisa de planejamento",
    "O modelo tradicional n�o exige documenta��o");

        Questions q2 = new Questions("Qual � um dos principais problemas do modelo tradicional de desenvolvimento de software?",
            "Dificuldade em adaptar mudan�as ao longo do projeto",
            "A falta de um planejamento detalhado",
            "Excesso de flexibilidade",
            "Menos custo para a empresa");

        Questions q3 = new Questions("No desenvolvimento �gil, como os requisitos do projeto s�o tratados?",
            "Eles podem ser modificados ao longo do desenvolvimento de acordo com a necessidade do cliente",
            "S�o definidos no in�cio e n�o podem ser alterados",
            "S�o estabelecidos apenas pelo gerente do projeto",
            "N�o h� necessidade de requisitos");

        Questions q4 = new Questions("Qual das seguintes op��es � uma caracter�stica do desenvolvimento �gil?",
            "Entrega incremental de funcionalidades",
            "Foco apenas na documenta��o",
            "Fases r�gidas e sequenciais",
            "Apenas o gerente do projeto pode tomar decis�es");

        Questions q5 = new Questions("Qual � a abordagem do modelo tradicional em rela��o �s fases do projeto?",
            "As fases s�o sequenciais e r�gidas, sem possibilidade de voltar para etapas anteriores",
            "O desenvolvimento ocorre em ciclos iterativos",
            "O cliente pode intervir a qualquer momento",
            "As equipes trabalham sem planejamento pr�vio");

        Questions q6 = new Questions("Por que o desenvolvimento �gil � considerado mais eficiente para projetos com requisitos din�micos?",
            "Porque permite ajustes cont�nuos e prioriza��o do que � mais importante",
            "Porque elimina completamente a necessidade de documenta��o",
            "Porque n�o precisa do envolvimento do cliente",
            "Porque reduz a quantidade de reuni�es entre a equipe");

        Questions q7 = new Questions("No modelo �gil, como o cliente participa do projeto?",
            "Ativamente, dando feedback constante e participando de reuni�es frequentes",
            "Apenas no in�cio, para definir os requisitos",
            "O cliente s� participa na fase de testes",
            "O cliente n�o tem contato com a equipe durante o desenvolvimento");

        Questions q8 = new Questions("No modelo tradicional, qual � o principal objetivo antes de iniciar o desenvolvimento?",
            "Definir um planejamento detalhado e fixo",
            "Criar prot�tipos rapidamente",
            "Realizar testes antes do desenvolvimento",
            "Liberar o c�digo antes de finalizar os requisitos");

        Questions q9 = new Questions("Qual dos seguintes modelos de desenvolvimento segue uma abordagem mais r�gida e estruturada?",
            "Modelo Cascata",
            "Scrum",
            "Kanban",
            "Extreme Programming (XP)");

        Questions q10 = new Questions("Por que as empresas est�o cada vez mais adotando o desenvolvimento �gil?",
            "Porque ele melhora a adapta��o a mudan�as e entrega valor rapidamente",
            "Porque reduz a necessidade de planejamento",
            "Porque n�o exige reuni�es com o cliente",
            "Porque n�o precisa de documenta��o");

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

        Questions q11 = new Questions("O que � o Scrum no contexto de desenvolvimento �gil?",
    "Um framework para gerenciamento de projetos baseado em ciclos iterativos e incrementais",
    "Um m�todo tradicional de desenvolvimento de software",
    "Um software de gest�o de equipes",
    "Uma ferramenta para controle de versionamento");

        Questions q12 = new Questions("Qual � o nome do evento no Scrum onde a equipe planeja o trabalho para o pr�ximo sprint?",
            "Sprint Planning",
            "Daily Scrum",
            "Sprint Review",
            "Sprint Retrospective");

        Questions q13 = new Questions("O que � um Product Backlog no Scrum?",
            "Uma lista priorizada de funcionalidades e requisitos do produto",
            "Um documento detalhado com todos os planos do projeto",
            "Um relat�rio de desempenho da equipe",
            "Um contrato de requisitos fixos entre o cliente e a equipe");

        Questions q14 = new Questions("Qual � o papel do Product Owner no Scrum?",
            "Definir e priorizar os itens do Product Backlog",
            "Gerenciar a equipe de desenvolvimento diretamente",
            "Testar o software ap�s cada sprint",
            "Executar as tarefas t�cnicas de desenvolvimento");

        Questions q15 = new Questions("O que acontece na Daily Scrum?",
            "Os membros da equipe compartilham o que fizeram, o que far�o e os impedimentos",
            "A equipe revisa e aprova o c�digo desenvolvido",
            "O Scrum Master apresenta m�tricas de produtividade",
            "O cliente aprova as entregas do sprint");

        Questions q16 = new Questions("Qual � a principal responsabilidade do Scrum Master?",
            "Facilitar o processo Scrum e remover impedimentos da equipe",
            "Definir os requisitos do projeto",
            "Criar o c�digo-fonte do software",
            "Decidir as tecnologias que ser�o utilizadas");

        Questions q17 = new Questions("O que acontece na Sprint Review?",
            "A equipe apresenta as entregas do sprint para os stakeholders",
            "A equipe define as tarefas do pr�ximo sprint",
            "Os desenvolvedores revisam o c�digo uns dos outros",
            "O Product Owner define novos requisitos");

        Questions q18 = new Questions("Qual a dura��o recomendada de uma Sprint no Scrum?",
            "Entre 1 a 4 semanas",
            "1 dia",
            "3 meses",
            "6 meses");

        Questions q19 = new Questions("O que � um Sprint no Scrum?",
            "Um ciclo de desenvolvimento de tempo fixo onde um conjunto de funcionalidades � implementado",
            "Uma reuni�o de planejamento de projetos",
            "Um evento para revisar retrospectivas do time",
            "Uma ferramenta de controle de tarefas");

        Questions q20 = new Questions("O que � um Definition of Done (DoD) no Scrum?",
            "Crit�rios que definem quando um item do backlog � considerado completo",
            "Uma lista de pend�ncias para o pr�ximo sprint",
            "Uma regra para aprovar sprints",
            "Um documento fixo estabelecido pelo gerente do projeto");

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

        Questions q21 = new Questions("O que s�o t�cnicas �geis no desenvolvimento de software?",
    "Conjuntos de pr�ticas para otimizar processos e melhorar a colabora��o no desenvolvimento",
    "Um conjunto de regras fixas para desenvolvimento de software",
    "Apenas metodologias de gerenciamento de projetos",
    "Ferramentas automatizadas para desenvolvimento");

        Questions q22 = new Questions("O que � Pair Programming?",
            "Uma t�cnica onde dois desenvolvedores trabalham juntos no mesmo c�digo",
            "Uma pr�tica onde um desenvolvedor programa e outro revisa posteriormente",
            "Uma forma de alternar entre dois m�todos de programa��o",
            "Uma t�cnica para programar em linguagens diferentes simultaneamente");

        Questions q23 = new Questions("O que � Test-Driven Development (TDD)?",
            "Uma t�cnica onde os testes s�o escritos antes do c�digo",
            "Uma pr�tica onde os testes s�o feitos apenas no final do projeto",
            "Um m�todo que descarta a necessidade de testes automatizados",
            "Uma abordagem que foca apenas em testes de interface");

        Questions q24 = new Questions("Qual a principal vantagem do TDD?",
            "Melhora a qualidade do c�digo e reduz bugs",
            "Elimina a necessidade de revis�o de c�digo",
            "Acelera o desenvolvimento sem comprometer a qualidade",
            "Permite que o c�digo seja escrito sem planejamento pr�vio");

        Questions q25 = new Questions("O que significa Continuous Integration (CI)?",
            "Pr�tica onde mudan�as no c�digo s�o integradas frequentemente ao reposit�rio principal",
            "Uma t�cnica para desenvolver software sem branches",
            "Uma estrat�gia para evitar testes durante o desenvolvimento",
            "Uma abordagem para reduzir a necessidade de deploys");

        Questions q26 = new Questions("O que � Refatora��o de C�digo?",
            "Melhoria no c�digo sem alterar seu comportamento funcional",
            "Remo��o de funcionalidades antigas do sistema",
            "Processo de migra��o do c�digo para uma nova linguagem",
            "Cria��o de novos m�dulos sem reutilizar c�digo");

        Questions q27 = new Questions("Qual � um dos principais objetivos da refatora��o?",
            "Melhorar a legibilidade e manuten��o do c�digo",
            "Remover c�digo n�o utilizado e arquivos tempor�rios",
            "Diminuir a velocidade do desenvolvimento para garantir qualidade",
            "Tornar o c�digo mais complexo para evitar c�pias");

        Questions q28 = new Questions("O que � um Spike no desenvolvimento �gil?",
            "Uma investiga��o r�pida para entender ou testar uma solu��o t�cnica",
            "Uma reuni�o para avaliar a velocidade do time",
            "Uma t�cnica para estimar tarefas futuras",
            "Um m�todo para medir o desempenho da equipe");

        Questions q29 = new Questions("O que � a pr�tica de Code Review?",
            "Revis�o do c�digo por outro desenvolvedor para garantir qualidade e boas pr�ticas",
            "Avalia��o do c�digo feita pelo Product Owner",
            "Uma an�lise feita apenas ap�s o software ser entregue",
            "Uma reuni�o para decidir quais tecnologias usar");

        Questions q30 = new Questions("O que � a t�cnica de Kanban?",
            "Um sistema visual para gerenciamento de fluxo de trabalho",
            "Uma metodologia de desenvolvimento de software",
            "Uma ferramenta exclusiva do Scrum",
            "Uma pr�tica usada apenas para testes automatizados");

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

        Questions q31 = new Questions("O que s�o ferramentas de aux�lio no desenvolvimento �gil?",
    "Softwares e pr�ticas que ajudam a gerenciar, automatizar e melhorar o fluxo de trabalho",
    "Apenas ferramentas de controle de vers�o",
    "Softwares usados exclusivamente para testes automatizados",
    "Qualquer ferramenta utilizada para desenvolver c�digo");

        Questions q32 = new Questions("Qual das seguintes op��es � um exemplo de ferramenta de controle de vers�o?",
            "Git",
            "Jira",
            "Trello",
            "Slack");

        Questions q33 = new Questions("Para que serve uma ferramenta de integra��o cont�nua (CI/CD)?",
            "Automatizar a integra��o de c�digo e a entrega cont�nua de software",
            "Gerenciar reuni�es de equipe",
            "Acompanhar o tempo gasto por cada desenvolvedor",
            "Realizar testes de interface do usu�rio");

        Questions q34 = new Questions("Qual ferramenta � frequentemente usada para gerenciamento de projetos �geis?",
            "Jira",
            "MySQL",
            "Visual Studio",
            "Notepad++");

        Questions q35 = new Questions("Qual das seguintes ferramentas � utilizada para comunica��o e colabora��o entre equipes �geis?",
            "Slack",
            "Oracle",
            "Power BI",
            "Docker");

        Questions q36 = new Questions("O que � o Trello e como ele auxilia equipes �geis?",
            "Uma ferramenta baseada em Kanban para organizar tarefas e fluxos de trabalho",
            "Uma IDE para desenvolvimento de software",
            "Um banco de dados relacional",
            "Uma linguagem de programa��o usada em m�todos �geis");

        Questions q37 = new Questions("Qual das op��es abaixo � uma vantagem de usar ferramentas de versionamento como o Git?",
            "Permite o trabalho colaborativo e rastreamento de mudan�as no c�digo",
            "Evita a necessidade de realizar testes",
            "Elimina a necessidade de planejamento",
            "Substitui completamente a necessidade de reuni�es");

        Questions q38 = new Questions("Qual � a principal vantagem de usar ferramentas de automa��o de testes?",
            "Aumentam a confiabilidade e reduzem o tempo de testes manuais",
            "Eliminam a necessidade de desenvolvedores",
            "Permitem desenvolver sem precisar testar o c�digo",
            "Evita que mudan�as no c�digo precisem ser revisadas");

        Questions q39 = new Questions("Qual ferramenta pode ser usada para gerenciar reposit�rios Git remotamente?",
            "GitHub",
            "PostgreSQL",
            "Power BI",
            "Jira");

        Questions q40 = new Questions("O que � o Docker e como ele auxilia no desenvolvimento �gil?",
            "Uma plataforma que permite criar e gerenciar containers para aplica��es",
            "Um framework exclusivo para testes unit�rios",
            "Uma ferramenta de automa��o para escrever c�digo",
            "Uma linguagem de programa��o usada para definir requisitos");

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

        Questions q41 = new Questions("De onde vem o termo 'Scrum'?",
    "De uma jogada do Rugby, representando trabalho em equipe",
    "De um termo t�cnico de desenvolvimento de software",
    "De um conceito matem�tico aplicado � gest�o de projetos",
    "De um framework espec�fico criado pelo PMI");

        Questions q42 = new Questions("Quem s�o os principais criadores do Scrum?",
            "Jeff Sutherland e Ken Schwaber",
            "Nonaka e Takeuchi",
            "Martin Fowler e Robert C. Martin",
            "Kent Beck e Ward Cunningham");

        Questions q43 = new Questions("Qual � a principal abordagem do Scrum?",
            "Emp�rica, baseada em adapta��o e inspe��o cont�nua",
            "Prescritiva, com regras r�gidas para desenvolvimento",
            "Cascata, com fases definidas e sequenciais",
            "Baseada em documenta��es extensivas antes do in�cio do projeto");

        Questions q44 = new Questions("Qual � o principal objetivo do Scrum no desenvolvimento de software?",
            "Permitir flexibilidade e adapta��o a mudan�as frequentes",
            "Eliminar a necessidade de planejamento pr�vio",
            "Acelerar entregas sem comprometer qualidade",
            "Evitar intera��o entre membros da equipe para maior produtividade");

        Questions q45 = new Questions("O que � o Scrum Guide?",
            "Um documento que descreve as boas pr�ticas e valores do Scrum",
            "Uma ferramenta de software para gerenciar times �geis",
            "Uma certifica��o para desenvolvedores Scrum",
            "Um relat�rio sobre produtividade em projetos �geis");

        Questions q46 = new Questions("Quais s�o os cinco valores do Scrum?",
            "Coragem, Foco, Comprometimento, Respeito e Abertura",
            "Planejamento, Execu��o, Controle, Documenta��o e Testes",
            "Autonomia, Flexibilidade, Criatividade, Velocidade e Adapta��o",
            "Objetividade, Efici�ncia, Qualidade, Rigor e Comunica��o");

        Questions q47 = new Questions("O que significa 'Controle Emp�rico de Processo' no Scrum?",
            "O desenvolvimento baseado em transpar�ncia, inspe��o e adapta��o",
            "A centraliza��o das decis�es no Scrum Master",
            "O gerenciamento r�gido de cada tarefa para evitar erros",
            "A separa��o completa entre planejamento e execu��o");

        Questions q48 = new Questions("O que significa Time-Boxing no Scrum?",
            "A defini��o de um tempo fixo para reuni�es e eventos Scrum",
            "A organiza��o do backlog do produto por tempo de desenvolvimento",
            "O tempo m�ximo permitido para mudan�as no projeto",
            "O per�odo necess�rio para planejar um projeto Scrum");

        Questions q49 = new Questions("Qual � a fun��o do Product Owner?",
            "Maximizar o valor do produto e priorizar o Product Backlog",
            "Gerenciar diretamente a equipe de desenvolvimento",
            "Definir a arquitetura do software",
            "Escrever os testes automatizados do sistema");

        Questions q50 = new Questions("Qual � o papel do Scrum Master?",
            "Facilitar a aplica��o do Scrum, removendo impedimentos para a equipe",
            "Definir os requisitos t�cnicos do projeto",
            "Gerenciar os prazos e custos do projeto",
            "Ser o respons�vel pelo desenvolvimento do c�digo-fonte");

        Questions q51 = new Questions("Qual � o principal papel do Scrum Team?",
            "Ser auto-organizado e respons�vel pelo desenvolvimento do produto",
            "Executar tarefas apenas conforme orienta��es do gerente de projetos",
            "Definir os objetivos do neg�cio sem consultar o cliente",
            "Criar o planejamento inicial e repass�-lo ao Product Owner");

        Questions q52 = new Questions("O que acontece na Daily Scrum?",
            "Os membros da equipe compartilham o que fizeram, o que far�o e impedimentos",
            "O Product Owner apresenta novas funcionalidades para a equipe",
            "A equipe revisa e aprova o c�digo desenvolvido",
            "O Scrum Master define as pr�ximas tarefas dos desenvolvedores");

        Questions q53 = new Questions("O que � um Sprint no Scrum?",
            "Um ciclo de desenvolvimento de tempo fixo onde funcionalidades s�o implementadas",
            "Um evento de revis�o do c�digo do time",
            "Uma ferramenta para medir a produtividade individual dos desenvolvedores",
            "Um m�todo de estimativa para definir prazos");

        Questions q54 = new Questions("O que acontece na Sprint Review?",
            "A equipe apresenta as entregas do Sprint para os stakeholders",
            "A equipe define as tarefas do pr�ximo Sprint",
            "Os desenvolvedores revisam o c�digo uns dos outros",
            "O Scrum Master revisa o desempenho individual de cada membro");

        Questions q55 = new Questions("Qual � a fun��o da Sprint Retrospective?",
            "Analisar o que foi bom e o que pode ser melhorado para o pr�ximo Sprint",
            "Definir os requisitos para o pr�ximo Sprint",
            "Aprovar ou rejeitar funcionalidades desenvolvidas",
            "Fazer ajustes na arquitetura do software");

        Questions q56 = new Questions("Qual � a dura��o recomendada de uma Sprint no Scrum?",
            "Entre 1 a 4 semanas",
            "1 dia",
            "3 meses",
            "6 meses");

        Questions q57 = new Questions("O que � o Definition of Done (DoD) no Scrum?",
            "Crit�rios que definem quando um item do backlog � considerado completo",
            "Uma lista de pend�ncias para o pr�ximo Sprint",
            "Uma regra para aprovar Sprints",
            "Um documento fixo estabelecido pelo gerente do projeto");

        Questions q58 = new Questions("Qual das seguintes op��es � um princ�pio do Scrum?",
            "Colabora��o entre os membros do time",
            "Planejamento fixo e r�gido antes do in�cio do projeto",
            "Documenta��o extensiva antes da implementa��o",
            "Separa��o entre as fases de desenvolvimento e testes");

        Questions q59 = new Questions("Como o Scrum trata mudan�as nos requisitos?",
            "Aceita mudan�as durante o desenvolvimento para entregar mais valor ao cliente",
            "Rejeita mudan�as ap�s o in�cio do projeto",
            "Permite mudan�as apenas no in�cio do projeto",
            "Exige um novo planejamento completo para cada mudan�a");

        Questions q60 = new Questions("Qual � a import�ncia da auto-organiza��o no Scrum?",
            "Permite que o time tome decis�es e aumente a produtividade",
            "Garante que cada membro trabalhe isoladamente",
            "Evita a necessidade de reuni�es e planejamento",
            "Substitui a fun��o do Product Owner");

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

        Questions q61 = new Questions("Qual � a import�ncia da comunica��o no Scrum?",
    "Promover transpar�ncia, colabora��o e melhoria cont�nua",
    "Garantir que apenas o Scrum Master tenha informa��es do projeto",
    "Evitar mudan�as durante o desenvolvimento",
    "Manter os requisitos fixos at� a entrega final");

        Questions q62 = new Questions("Quais s�o os cinco eventos principais no Scrum?",
            "Sprint, Reuni�o de Planejamento do Sprint, Reuni�es Di�rias, Revis�o do Sprint e Retrospectiva do Sprint",
            "Sprint, Code Review, Daily Scrum, Teste Unit�rio e Planejamento",
            "Sprint, Documenta��o, Reuni�es Semanais, Aprova��o Final e Retrospectiva",
            "Planejamento Geral, Sprint, Reuni�es de Aprova��o, Revis�o Final e Testes de Aceita��o");

        Questions q63 = new Questions("O que � um Sprint no Scrum?",
            "Um ciclo de desenvolvimento de tempo fixo para entrega de incrementos do produto",
            "Uma reuni�o para definir requisitos do projeto",
            "Uma etapa onde apenas testes s�o realizados",
            "Uma fase para documentar os processos antes da entrega final");

        Questions q64 = new Questions("O que � Time-Boxing no Scrum?",
            "Defini��o de um tempo fixo para execu��o de eventos e atividades",
            "Um prazo final obrigat�rio para todo o projeto",
            "Um tempo extra para corre��es e retrabalho",
            "Uma ferramenta para estimar o tempo de cada tarefa individualmente");

        Questions q65 = new Questions("Qual � o objetivo da Reuni�o de Planejamento do Sprint?",
            "Definir quais itens do backlog ser�o trabalhados e como ser�o desenvolvidos",
            "Apresentar o produto finalizado para os stakeholders",
            "Fazer um retrospecto do Sprint anterior",
            "Criar documenta��o detalhada antes de iniciar o Sprint");

        Questions q66 = new Questions("Quem participa da Reuni�o de Planejamento do Sprint?",
            "Product Owner, Scrum Master e Scrum Team",
            "Apenas o Scrum Master",
            "Somente o Product Owner e a ger�ncia",
            "Todos os stakeholders da empresa");

        Questions q67 = new Questions("Quais s�o as tr�s perguntas b�sicas da Reuni�o Di�ria (Daily Scrum)?",
            "O que fiz ontem? O que farei hoje? H� algum impedimento?",
            "Quais bugs encontrei? Quando ser� o pr�ximo Sprint? O que foi planejado?",
            "Quais mudan�as podemos fazer? Quem � o respons�vel pelo c�digo? O que est� atrasado?",
            "Quantas horas trabalhamos? Quem lidera o time? O que pode ser descartado?");

        Questions q68 = new Questions("Qual � a dura��o recomendada para a Reuni�o Di�ria?",
            "15 minutos",
            "1 hora",
            "45 minutos",
            "30 minutos");

        Questions q69 = new Questions("O que acontece na Revis�o do Sprint?",
            "A equipe apresenta o que foi desenvolvido para stakeholders e Product Owner",
            "Os desenvolvedores revisam o c�digo uns dos outros",
            "A equipe analisa problemas t�cnicos que surgiram durante o Sprint",
            "O Scrum Master decide o que ser� entregue na pr�xima Sprint");

        Questions q70 = new Questions("Quem aprova ou rejeita as entregas da Revis�o do Sprint?",
            "Product Owner",
            "Scrum Master",
            "Scrum Team",
            "Cliente");

        Questions q71 = new Questions("Qual � o objetivo da Retrospectiva do Sprint?",
            "Analisar o que deu certo, o que pode melhorar e definir a��es para otimizar o pr�ximo Sprint",
            "Fazer corre��es no c�digo antes da entrega final",
            "Avaliar individualmente cada membro da equipe",
            "Criar um novo planejamento detalhado para os pr�ximos seis meses");

        Questions q72 = new Questions("Qual a diferen�a entre Revis�o do Sprint e Retrospectiva do Sprint?",
            "A Revis�o foca no produto entregue, enquanto a Retrospectiva foca na melhoria do processo",
            "A Retrospectiva serve para revisar c�digo, e a Revis�o para validar requisitos",
            "A Revis�o acontece no in�cio do Sprint e a Retrospectiva no final",
            "N�o h� diferen�a, ambos t�m o mesmo objetivo");

        Questions q73 = new Questions("Quem deve participar da Retrospectiva do Sprint?",
            "Toda a equipe Scrum",
            "Apenas o Product Owner",
            "Apenas o Scrum Master",
            "A equipe de testes e o Scrum Master");

        Questions q74 = new Questions("Qual a import�ncia do time-boxing na Retrospectiva do Sprint?",
            "Garante que a reuni�o tenha um tempo fixo e objetivo",
            "Evita que a equipe tome decis�es sem o Scrum Master",
            "Determina a quantidade de tarefas que devem ser realizadas",
            "Define um limite de tempo para cada participante falar");

        Questions q75 = new Questions("Como as reuni�es di�rias ajudam a equipe Scrum?",
            "Facilitam a comunica��o, identificam impedimentos e alinham o trabalho di�rio",
            "Servem para registrar a produtividade de cada membro",
            "Substituem a necessidade de reuni�es de planejamento",
            "Eliminam a necessidade de um Product Owner");

        Questions q76 = new Questions("Qual evento do Scrum � voltado para avaliar e otimizar os processos internos da equipe?",
            "Retrospectiva do Sprint",
            "Revis�o do Sprint",
            "Planejamento do Sprint",
            "Daily Scrum");

        Questions q77 = new Questions("Qual evento � respons�vel por definir as tarefas do Sprint?",
            "Reuni�o de Planejamento do Sprint",
            "Daily Scrum",
            "Revis�o do Sprint",
            "Retrospectiva do Sprint");

        Questions q78 = new Questions("O que acontece se um time-boxing for muito curto?",
            "A equipe pode n�o conseguir finalizar as entregas a tempo",
            "O Scrum Master precisar� replanejar todo o Sprint",
            "A velocidade de desenvolvimento ser� dobrada",
            "O Product Owner precisar� redefinir as prioridades");

        Questions q79 = new Questions("Qual � o principal objetivo de um Sprint no Scrum?",
            "Criar um incremento funcional do produto que possa ser entregue",
            "Finalizar toda a documenta��o do projeto",
            "Definir novos requisitos com a equipe",
            "Garantir que o c�digo esteja 100% otimizado");

        Questions q80 = new Questions("Por que a Revis�o do Sprint � importante para o cliente?",
            "Permite que o cliente veja e valide o progresso do produto",
            "� um momento para revisar a documenta��o t�cnica",
            "Ajuda a definir o or�amento para a pr�xima Sprint",
            "Evita que a equipe precise fazer testes de aceita��o");

        questoes.Add(q61);
        questoes.Add(q62);
        questoes.Add(q63);
        questoes.Add(q64);
        questoes.Add(q65);
        questoes.Add(q66);
        questoes.Add(q67);
        questoes.Add(q68);
        questoes.Add(q69);
        questoes.Add(q70);
        questoes.Add(q71);
        questoes.Add(q72);
        questoes.Add(q73);
        questoes.Add(q74);
        questoes.Add(q75);
        questoes.Add(q76);
        questoes.Add(q77);
        questoes.Add(q78);
        questoes.Add(q79);
        questoes.Add(q80);

        Questions q81 = new Questions("O que s�o os artefatos do Scrum?",
    "Indutores de informa��o que capturam o entendimento compartilhado da equipe",
    "Documentos formais para controle de qualidade do projeto",
    "Ferramentas de automa��o para desenvolvimento �gil",
    "Regras fixas definidas pelo Scrum Master");

        Questions q82 = new Questions("Quais s�o os tr�s principais artefatos do Scrum?",
            "Backlog do Produto, Backlog do Sprint e Incremento do Produto",
            "Sprint, Retrospectiva e Product Owner",
            "Daily Scrum, Scrum Board e Burndown Chart",
            "Planejamento, Execu��o e Monitoramento");

        Questions q83 = new Questions("O que � o Backlog do Produto?",
            "Uma lista de requisitos priorizados que evolui ao longo do projeto",
            "Um documento fixo de requisitos fechado no in�cio do projeto",
            "Uma lista de tarefas conclu�das durante o Sprint",
            "Um conjunto de regras estabelecidas pelo Scrum Master");

        Questions q84 = new Questions("Quem � respons�vel por gerenciar o Backlog do Produto?",
            "Product Owner",
            "Scrum Master",
            "Scrum Team",
            "Cliente");

        Questions q85 = new Questions("O que � o Backlog do Sprint?",
            "Um subconjunto do Backlog do Produto que cont�m tarefas do Sprint atual",
            "Uma lista de funcionalidades aprovadas pelo cliente",
            "O planejamento geral do projeto Scrum",
            "Uma lista de impedimentos encontrados no Sprint");

        Questions q86 = new Questions("Quem pode modificar o Backlog do Sprint durante o Sprint?",
            "A pr�pria equipe de desenvolvimento",
            "O Product Owner",
            "O Scrum Master",
            "Os Stakeholders");

        Questions q87 = new Questions("O que � o Incremento do Produto?",
            "A soma de todas as funcionalidades conclu�das durante o Sprint",
            "Uma lista de funcionalidades que ainda precisam ser implementadas",
            "Uma ferramenta de controle de tempo no Scrum",
            "Uma estimativa de custos do projeto");

        Questions q88 = new Questions("Qual � a rela��o entre o Incremento do Produto e a Defini��o de Pronto (DoD)?",
            "O incremento precisa atender aos crit�rios da Defini��o de Pronto para ser aceito",
            "A Defini��o de Pronto define as tarefas que devem ser feitas no Sprint",
            "A Defini��o de Pronto � um relat�rio gerado no final do projeto",
            "O Incremento do Produto n�o precisa seguir crit�rios de qualidade");

        Questions q89 = new Questions("O que � um Gr�fico Burndown no Scrum?",
            "Um gr�fico que mostra a evolu��o do trabalho realizado ao longo do Sprint",
            "Uma ferramenta para estimar custos do projeto",
            "Uma lista de impedimentos encontrados durante o Sprint",
            "Uma forma de medir a produtividade individual dos membros da equipe");

        Questions q90 = new Questions("O que o Gr�fico Burndown ajuda a visualizar?",
            "O progresso do Sprint e a quantidade de trabalho restante",
            "A velocidade de entrega de cada desenvolvedor",
            "A lista de tarefas que precisam ser revisadas",
            "Os erros de c�digo encontrados durante os testes");

        Questions q91 = new Questions("O que � um Quadro Scrum (Scrum Board)?",
            "Uma ferramenta visual para acompanhar o progresso das tarefas no Sprint",
            "Uma planilha usada para documentar reuni�es di�rias",
            "Um relat�rio de desempenho dos desenvolvedores",
            "Uma ferramenta usada apenas pelo Scrum Master");

        Questions q92 = new Questions("Qual das op��es � um formato comum para o Quadro Scrum?",
            "Colunas 'To Do', 'Doing' e 'Done'",
            "Lista de impedimentos e lista de riscos",
            "Relat�rio de progresso e lista de erros",
            "Tarefas aprovadas e tarefas pendentes");

        Questions q93 = new Questions("Como o Quadro Scrum pode ser representado?",
            "Fisicamente (quadro branco) ou digitalmente (ferramentas como Trello, Jira)",
            "Apenas como um documento f�sico",
            "Somente como um relat�rio gerado no final do Sprint",
            "Como um checklist entregue ao cliente");

        Questions q94 = new Questions("Quem pode visualizar e atualizar o Quadro Scrum?",
            "Toda a equipe de desenvolvimento",
            "Apenas o Scrum Master",
            "Somente o Product Owner",
            "Apenas a ger�ncia");

        Questions q95 = new Questions("Por que o Backlog do Produto � considerado um artefato vivo?",
            "Porque pode ser atualizado continuamente conforme novas necessidades surgem",
            "Porque � fechado no in�cio do projeto e n�o muda at� o final",
            "Porque � um documento formal que precisa de aprova��o para qualquer altera��o",
            "Porque � um contrato fixo que n�o pode ser alterado");

        Questions q96 = new Questions("Qual � a rela��o entre o Backlog do Produto e a Revis�o do Sprint?",
            "O feedback do cliente na Revis�o do Sprint pode gerar novas demandas para o Backlog do Produto",
            "O Backlog do Produto deve ser fechado antes da Revis�o do Sprint",
            "A Revis�o do Sprint define novas prioridades para o Product Owner",
            "O Backlog do Produto s� pode ser atualizado antes do in�cio de um novo Sprint");

        Questions q97 = new Questions("O que acontece com os itens do Backlog do Sprint que n�o foram conclu�dos no Sprint atual?",
            "Podem ser movidos para o pr�ximo Sprint, caso ainda sejam relevantes",
            "S�o automaticamente descartados",
            "Devem ser conclu�dos obrigatoriamente antes do pr�ximo Sprint",
            "Precisam ser aprovados novamente pelo cliente");

        Questions q98 = new Questions("O que define a prioridade dos itens no Backlog do Produto?",
            "O valor de neg�cio e as necessidades do cliente",
            "A ordem de chegada das solicita��es",
            "A decis�o do Scrum Master",
            "A quantidade de tempo necess�ria para desenvolver cada item");

        Questions q99 = new Questions("Como os artefatos do Scrum ajudam no desenvolvimento �gil?",
            "Garantem transpar�ncia, alinhamento e acompanhamento cont�nuo do progresso",
            "Substituem a necessidade de reuni�es da equipe",
            "Eliminam a necessidade de um Product Owner",
            "Impedem que mudan�as sejam feitas no escopo do projeto");

        Questions q100 = new Questions("Qual � a principal fun��o do Product Owner em rela��o aos artefatos do Scrum?",
            "Gerenciar e priorizar o Backlog do Produto",
            "Definir os crit�rios t�cnicos do Incremento do Produto",
            "Realizar a Revis�o do Sprint sozinho",
            "Controlar o tempo de execu��o das tarefas no Quadro Scrum");

        questoes.Add(q81);
        questoes.Add(q82);
        questoes.Add(q83);
        questoes.Add(q84);
        questoes.Add(q85);
        questoes.Add(q86);
        questoes.Add(q87);
        questoes.Add(q88);
        questoes.Add(q89);
        questoes.Add(q90);
        questoes.Add(q91);
        questoes.Add(q92);
        questoes.Add(q93);
        questoes.Add(q94);
        questoes.Add(q95);
        questoes.Add(q96);
        questoes.Add(q97);
        questoes.Add(q98);
        questoes.Add(q99);
        questoes.Add(q100);

        Questions q101 = new Questions("Qual � o objetivo do processo de planejamento no Scrum?",
    "Definir expectativas das partes interessadas e sincroniz�-las com a equipe",
    "Criar um plano r�gido e inalter�vel para o projeto",
    "Garantir que todas as entregas sejam feitas sem atrasos",
    "Definir todas as funcionalidades do projeto antes do desenvolvimento");

        Questions q102 = new Questions("Quais s�o as tr�s perguntas centrais do planejamento no Scrum?",
            "O que mudou financeiramente? Que progresso ser� feito a cada Sprint? Por que o projeto � um investimento valioso?",
            "Qual ser� o or�amento final? Quem � respons�vel pelo sucesso? Quando o projeto termina?",
            "Quantos membros comp�em a equipe? Quem aprova cada etapa? Como os clientes interagem?",
            "Quais ferramentas ser�o usadas? Como ser� feita a documenta��o? Quem define os prazos?");

        Questions q103 = new Questions("O que � necess�rio para iniciar um projeto Scrum?",
            "Uma vis�o clara do projeto e um Backlog do Produto",
            "Um plano detalhado com todas as funcionalidades j� definidas",
            "Um or�amento fechado e um cronograma fixo",
            "Aprova��o de todas as partes interessadas antes do desenvolvimento");

        Questions q104 = new Questions("Qual � o papel da vis�o no planejamento de um projeto Scrum?",
            "Descrever os objetivos do projeto e o estado final desejado",
            "Definir o or�amento e prazos r�gidos para o desenvolvimento",
            "Estabelecer os detalhes t�cnicos antes da implementa��o",
            "Definir as tecnologias que ser�o usadas no projeto");

        Questions q105 = new Questions("Quem � respons�vel por manter o Product Backlog?",
            "Product Owner",
            "Scrum Master",
            "Scrum Team",
            "Stakeholders");

        Questions q106 = new Questions("Quais elementos podem ser inclu�dos no Product Backlog?",
            "Caracter�sticas, fun��es, corre��es de bugs, melhorias e atualiza��es tecnol�gicas",
            "Apenas funcionalidades confirmadas pelo cliente",
            "Somente requisitos definidos no in�cio do projeto",
            "Somente as tarefas a serem executadas no primeiro Sprint");

        Questions q107 = new Questions("Quem pode contribuir com novos itens no Product Backlog?",
            "Cliente, equipe de projeto, marketing, vendas, ger�ncia e suporte ao cliente",
            "Apenas o Product Owner",
            "Somente os desenvolvedores",
            "Apenas a equipe de testes");

        Questions q108 = new Questions("O que � a Estimativa de Esfor�o no Scrum?",
            "Um processo iterativo para estimar o esfor�o necess�rio para implementar um item do Backlog",
            "Um prazo fixo para completar cada tarefa do projeto",
            "Uma avalia��o financeira do custo do projeto",
            "Um relat�rio gerado pelo Scrum Master ap�s cada Sprint");

        Questions q109 = new Questions("Quem � respons�vel por executar a Estimativa de Esfor�o?",
            "Product Owner e Scrum Team",
            "Apenas o Scrum Master",
            "Somente os desenvolvedores",
            "A ger�ncia do projeto");

        Questions q110 = new Questions("Qual � a principal vantagem do Scrum em compara��o com m�todos tradicionais de planejamento?",
            "Maior flexibilidade para lidar com mudan�as e adaptar o projeto continuamente",
            "Maior controle sobre cada etapa do desenvolvimento",
            "Menos reuni�es e menor necessidade de comunica��o",
            "Documenta��o detalhada antes do in�cio do projeto");

        Questions q111 = new Questions("Por que o Scrum requer menos planejamento detalhado do que m�todos tradicionais?",
            "Porque o progresso � monitorado a cada Sprint e as mudan�as s�o incorporadas conforme necess�rio",
            "Porque a equipe j� sabe tudo o que precisa ser feito antes de come�ar",
            "Porque o Product Owner toma todas as decis�es sozinho",
            "Porque o Scrum Master controla rigidamente todas as tarefas");

        Questions q112 = new Questions("O que acontece no final de cada Sprint em rela��o ao planejamento?",
            "O progresso real do projeto � comparado ao plano inicial",
            "O Product Owner decide se o projeto continua ou n�o",
            "O Scrum Master apresenta um relat�rio finalizado para os stakeholders",
            "A equipe entrega um documento formal com as mudan�as previstas");

        Questions q113 = new Questions("Qual � a rela��o entre o Scrum e o financiamento do projeto?",
            "O plano Scrum ajuda a justificar o investimento ao mostrar progresso cont�nuo e adapta��o",
            "O Scrum elimina a necessidade de aprova��o de or�amento",
            "O financiamento do projeto no Scrum � fixo e imut�vel",
            "Os stakeholders n�o t�m influ�ncia no planejamento financeiro");

        Questions q114 = new Questions("Por que a transpar�ncia � essencial no gerenciamento de projetos Scrum?",
            "Permite que as partes interessadas acompanhem o progresso e fa�am ajustes quando necess�rio",
            "Evita a necessidade de reuni�es di�rias",
            "Ajuda a reduzir custos ao eliminar documenta��o",
            "Mant�m os desenvolvedores focados apenas na implementa��o");

        Questions q115 = new Questions("Quais eventos do Scrum garantem a inspe��o e adapta��o cont�nua do projeto?",
            "Planejamento da Sprint, Reuni�es Di�rias, Revis�o da Sprint e Retrospectiva da Sprint",
            "Daily Scrum, Revis�o Financeira, Aprova��o Final e Fechamento do Projeto",
            "Reuni�es Quinzenais, Reuni�o de Ger�ncia, Testes Unit�rios e Codifica��o",
            "Sprint Zero, Planejamento Mensal, Relat�rio de Progresso e Feedback do Cliente");

        Questions q116 = new Questions("Como o Product Backlog ajuda no alinhamento das partes interessadas?",
            "Mantendo uma lista clara e priorizada dos requisitos do projeto",
            "Definindo um plano r�gido que n�o pode ser alterado",
            "Servindo como um documento finalizado no in�cio do projeto",
            "Sendo atualizado apenas no final de cada Sprint");

        Questions q117 = new Questions("O que diferencia o planejamento do Scrum de m�todos tradicionais como o modelo em cascata?",
            "O planejamento � cont�nuo e ajust�vel a cada Sprint",
            "O Scrum exige que todas as funcionalidades sejam detalhadas antes de come�ar",
            "O Scrum elimina a necessidade de reuni�es de planejamento",
            "O planejamento no Scrum � fixo e n�o pode ser alterado ap�s a primeira Sprint");

        Questions q118 = new Questions("Por que a estimativa de esfor�o no Scrum � considerada iterativa?",
            "Porque as estimativas s�o refinadas conforme mais informa��es ficam dispon�veis",
            "Porque o tempo para cada tarefa � fixado desde o in�cio",
            "Porque o Product Owner decide o esfor�o necess�rio para cada item",
            "Porque os desenvolvedores estimam apenas no in�cio do projeto");

        Questions q119 = new Questions("Como o Scrum aborda mudan�as nos requisitos do projeto?",
            "Aceita mudan�as continuamente para maximizar o valor do produto",
            "Evita mudan�as ap�s o in�cio do projeto",
            "Exige um novo planejamento completo para cada mudan�a",
            "As mudan�as s� podem ser feitas no final do projeto");

        Questions q120 = new Questions("Qual � a principal vantagem da abordagem �gil do Scrum em rela��o a projetos complexos?",
            "Permite adapta��o cont�nua e entrega de valor incremental",
            "Elimina a necessidade de reuni�es frequentes",
            "Acelera o desenvolvimento reduzindo a intera��o com o cliente",
            "Evita retrabalho ao definir todos os requisitos no in�cio do projeto");

        questoes.Add(q101);
        questoes.Add(q102);
        questoes.Add(q103);
        questoes.Add(q104);
        questoes.Add(q105);
        questoes.Add(q106);
        questoes.Add(q107);
        questoes.Add(q108);
        questoes.Add(q109);
        questoes.Add(q110);
        questoes.Add(q111);
        questoes.Add(q112);
        questoes.Add(q113);
        questoes.Add(q114);
        questoes.Add(q115);
        questoes.Add(q116);
        questoes.Add(q117);
        questoes.Add(q118);
        questoes.Add(q119);
        questoes.Add(q120);


        totQuestoes = questoes.Count;
        txtTotQuestions.text = totQuestoes.ToString();
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

            dinheiro += 50;
            cash.GetComponent<TextMeshProUGUI>().text = "R$" + dinheiro.ToString();

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

        List<int> conseq = new List<int> {-1, -1, -1, -1, -1, -1, 0,0,0, 1,1,1,1, 2,2};

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
