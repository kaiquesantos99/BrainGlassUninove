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

    // Insere as questões no ArrayList
    void CarregarQuestoes()
    {
        // Criando as questões e armazenando na lista
        Questions q1 = new Questions("Qual a principal diferença entre o desenvolvimento tradicional e ágil?",
    "O modelo ágil é mais flexível e iterativo, enquanto o tradicional segue um planejamento rígido",
    "O modelo tradicional é mais rápido que o ágil",
    "O desenvolvimento ágil não precisa de planejamento",
    "O modelo tradicional não exige documentação");

        Questions q2 = new Questions("Qual é um dos principais problemas do modelo tradicional de desenvolvimento de software?",
            "Dificuldade em adaptar mudanças ao longo do projeto",
            "A falta de um planejamento detalhado",
            "Excesso de flexibilidade",
            "Menos custo para a empresa");

        Questions q3 = new Questions("No desenvolvimento ágil, como os requisitos do projeto são tratados?",
            "Eles podem ser modificados ao longo do desenvolvimento de acordo com a necessidade do cliente",
            "São definidos no início e não podem ser alterados",
            "São estabelecidos apenas pelo gerente do projeto",
            "Não há necessidade de requisitos");

        Questions q4 = new Questions("Qual das seguintes opções é uma característica do desenvolvimento ágil?",
            "Entrega incremental de funcionalidades",
            "Foco apenas na documentação",
            "Fases rígidas e sequenciais",
            "Apenas o gerente do projeto pode tomar decisões");

        Questions q5 = new Questions("Qual é a abordagem do modelo tradicional em relação às fases do projeto?",
            "As fases são sequenciais e rígidas, sem possibilidade de voltar para etapas anteriores",
            "O desenvolvimento ocorre em ciclos iterativos",
            "O cliente pode intervir a qualquer momento",
            "As equipes trabalham sem planejamento prévio");

        Questions q6 = new Questions("Por que o desenvolvimento ágil é considerado mais eficiente para projetos com requisitos dinâmicos?",
            "Porque permite ajustes contínuos e priorização do que é mais importante",
            "Porque elimina completamente a necessidade de documentação",
            "Porque não precisa do envolvimento do cliente",
            "Porque reduz a quantidade de reuniões entre a equipe");

        Questions q7 = new Questions("No modelo ágil, como o cliente participa do projeto?",
            "Ativamente, dando feedback constante e participando de reuniões frequentes",
            "Apenas no início, para definir os requisitos",
            "O cliente só participa na fase de testes",
            "O cliente não tem contato com a equipe durante o desenvolvimento");

        Questions q8 = new Questions("No modelo tradicional, qual é o principal objetivo antes de iniciar o desenvolvimento?",
            "Definir um planejamento detalhado e fixo",
            "Criar protótipos rapidamente",
            "Realizar testes antes do desenvolvimento",
            "Liberar o código antes de finalizar os requisitos");

        Questions q9 = new Questions("Qual dos seguintes modelos de desenvolvimento segue uma abordagem mais rígida e estruturada?",
            "Modelo Cascata",
            "Scrum",
            "Kanban",
            "Extreme Programming (XP)");

        Questions q10 = new Questions("Por que as empresas estão cada vez mais adotando o desenvolvimento ágil?",
            "Porque ele melhora a adaptação a mudanças e entrega valor rapidamente",
            "Porque reduz a necessidade de planejamento",
            "Porque não exige reuniões com o cliente",
            "Porque não precisa de documentação");

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

        Questions q11 = new Questions("O que é o Scrum no contexto de desenvolvimento ágil?",
    "Um framework para gerenciamento de projetos baseado em ciclos iterativos e incrementais",
    "Um método tradicional de desenvolvimento de software",
    "Um software de gestão de equipes",
    "Uma ferramenta para controle de versionamento");

        Questions q12 = new Questions("Qual é o nome do evento no Scrum onde a equipe planeja o trabalho para o próximo sprint?",
            "Sprint Planning",
            "Daily Scrum",
            "Sprint Review",
            "Sprint Retrospective");

        Questions q13 = new Questions("O que é um Product Backlog no Scrum?",
            "Uma lista priorizada de funcionalidades e requisitos do produto",
            "Um documento detalhado com todos os planos do projeto",
            "Um relatório de desempenho da equipe",
            "Um contrato de requisitos fixos entre o cliente e a equipe");

        Questions q14 = new Questions("Qual é o papel do Product Owner no Scrum?",
            "Definir e priorizar os itens do Product Backlog",
            "Gerenciar a equipe de desenvolvimento diretamente",
            "Testar o software após cada sprint",
            "Executar as tarefas técnicas de desenvolvimento");

        Questions q15 = new Questions("O que acontece na Daily Scrum?",
            "Os membros da equipe compartilham o que fizeram, o que farão e os impedimentos",
            "A equipe revisa e aprova o código desenvolvido",
            "O Scrum Master apresenta métricas de produtividade",
            "O cliente aprova as entregas do sprint");

        Questions q16 = new Questions("Qual é a principal responsabilidade do Scrum Master?",
            "Facilitar o processo Scrum e remover impedimentos da equipe",
            "Definir os requisitos do projeto",
            "Criar o código-fonte do software",
            "Decidir as tecnologias que serão utilizadas");

        Questions q17 = new Questions("O que acontece na Sprint Review?",
            "A equipe apresenta as entregas do sprint para os stakeholders",
            "A equipe define as tarefas do próximo sprint",
            "Os desenvolvedores revisam o código uns dos outros",
            "O Product Owner define novos requisitos");

        Questions q18 = new Questions("Qual a duração recomendada de uma Sprint no Scrum?",
            "Entre 1 a 4 semanas",
            "1 dia",
            "3 meses",
            "6 meses");

        Questions q19 = new Questions("O que é um Sprint no Scrum?",
            "Um ciclo de desenvolvimento de tempo fixo onde um conjunto de funcionalidades é implementado",
            "Uma reunião de planejamento de projetos",
            "Um evento para revisar retrospectivas do time",
            "Uma ferramenta de controle de tarefas");

        Questions q20 = new Questions("O que é um Definition of Done (DoD) no Scrum?",
            "Critérios que definem quando um item do backlog é considerado completo",
            "Uma lista de pendências para o próximo sprint",
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

        Questions q21 = new Questions("O que são técnicas ágeis no desenvolvimento de software?",
    "Conjuntos de práticas para otimizar processos e melhorar a colaboração no desenvolvimento",
    "Um conjunto de regras fixas para desenvolvimento de software",
    "Apenas metodologias de gerenciamento de projetos",
    "Ferramentas automatizadas para desenvolvimento");

        Questions q22 = new Questions("O que é Pair Programming?",
            "Uma técnica onde dois desenvolvedores trabalham juntos no mesmo código",
            "Uma prática onde um desenvolvedor programa e outro revisa posteriormente",
            "Uma forma de alternar entre dois métodos de programação",
            "Uma técnica para programar em linguagens diferentes simultaneamente");

        Questions q23 = new Questions("O que é Test-Driven Development (TDD)?",
            "Uma técnica onde os testes são escritos antes do código",
            "Uma prática onde os testes são feitos apenas no final do projeto",
            "Um método que descarta a necessidade de testes automatizados",
            "Uma abordagem que foca apenas em testes de interface");

        Questions q24 = new Questions("Qual a principal vantagem do TDD?",
            "Melhora a qualidade do código e reduz bugs",
            "Elimina a necessidade de revisão de código",
            "Acelera o desenvolvimento sem comprometer a qualidade",
            "Permite que o código seja escrito sem planejamento prévio");

        Questions q25 = new Questions("O que significa Continuous Integration (CI)?",
            "Prática onde mudanças no código são integradas frequentemente ao repositório principal",
            "Uma técnica para desenvolver software sem branches",
            "Uma estratégia para evitar testes durante o desenvolvimento",
            "Uma abordagem para reduzir a necessidade de deploys");

        Questions q26 = new Questions("O que é Refatoração de Código?",
            "Melhoria no código sem alterar seu comportamento funcional",
            "Remoção de funcionalidades antigas do sistema",
            "Processo de migração do código para uma nova linguagem",
            "Criação de novos módulos sem reutilizar código");

        Questions q27 = new Questions("Qual é um dos principais objetivos da refatoração?",
            "Melhorar a legibilidade e manutenção do código",
            "Remover código não utilizado e arquivos temporários",
            "Diminuir a velocidade do desenvolvimento para garantir qualidade",
            "Tornar o código mais complexo para evitar cópias");

        Questions q28 = new Questions("O que é um Spike no desenvolvimento ágil?",
            "Uma investigação rápida para entender ou testar uma solução técnica",
            "Uma reunião para avaliar a velocidade do time",
            "Uma técnica para estimar tarefas futuras",
            "Um método para medir o desempenho da equipe");

        Questions q29 = new Questions("O que é a prática de Code Review?",
            "Revisão do código por outro desenvolvedor para garantir qualidade e boas práticas",
            "Avaliação do código feita pelo Product Owner",
            "Uma análise feita apenas após o software ser entregue",
            "Uma reunião para decidir quais tecnologias usar");

        Questions q30 = new Questions("O que é a técnica de Kanban?",
            "Um sistema visual para gerenciamento de fluxo de trabalho",
            "Uma metodologia de desenvolvimento de software",
            "Uma ferramenta exclusiva do Scrum",
            "Uma prática usada apenas para testes automatizados");

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

        Questions q31 = new Questions("O que são ferramentas de auxílio no desenvolvimento ágil?",
    "Softwares e práticas que ajudam a gerenciar, automatizar e melhorar o fluxo de trabalho",
    "Apenas ferramentas de controle de versão",
    "Softwares usados exclusivamente para testes automatizados",
    "Qualquer ferramenta utilizada para desenvolver código");

        Questions q32 = new Questions("Qual das seguintes opções é um exemplo de ferramenta de controle de versão?",
            "Git",
            "Jira",
            "Trello",
            "Slack");

        Questions q33 = new Questions("Para que serve uma ferramenta de integração contínua (CI/CD)?",
            "Automatizar a integração de código e a entrega contínua de software",
            "Gerenciar reuniões de equipe",
            "Acompanhar o tempo gasto por cada desenvolvedor",
            "Realizar testes de interface do usuário");

        Questions q34 = new Questions("Qual ferramenta é frequentemente usada para gerenciamento de projetos ágeis?",
            "Jira",
            "MySQL",
            "Visual Studio",
            "Notepad++");

        Questions q35 = new Questions("Qual das seguintes ferramentas é utilizada para comunicação e colaboração entre equipes ágeis?",
            "Slack",
            "Oracle",
            "Power BI",
            "Docker");

        Questions q36 = new Questions("O que é o Trello e como ele auxilia equipes ágeis?",
            "Uma ferramenta baseada em Kanban para organizar tarefas e fluxos de trabalho",
            "Uma IDE para desenvolvimento de software",
            "Um banco de dados relacional",
            "Uma linguagem de programação usada em métodos ágeis");

        Questions q37 = new Questions("Qual das opções abaixo é uma vantagem de usar ferramentas de versionamento como o Git?",
            "Permite o trabalho colaborativo e rastreamento de mudanças no código",
            "Evita a necessidade de realizar testes",
            "Elimina a necessidade de planejamento",
            "Substitui completamente a necessidade de reuniões");

        Questions q38 = new Questions("Qual é a principal vantagem de usar ferramentas de automação de testes?",
            "Aumentam a confiabilidade e reduzem o tempo de testes manuais",
            "Eliminam a necessidade de desenvolvedores",
            "Permitem desenvolver sem precisar testar o código",
            "Evita que mudanças no código precisem ser revisadas");

        Questions q39 = new Questions("Qual ferramenta pode ser usada para gerenciar repositórios Git remotamente?",
            "GitHub",
            "PostgreSQL",
            "Power BI",
            "Jira");

        Questions q40 = new Questions("O que é o Docker e como ele auxilia no desenvolvimento ágil?",
            "Uma plataforma que permite criar e gerenciar containers para aplicações",
            "Um framework exclusivo para testes unitários",
            "Uma ferramenta de automação para escrever código",
            "Uma linguagem de programação usada para definir requisitos");

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
    "De um termo técnico de desenvolvimento de software",
    "De um conceito matemático aplicado à gestão de projetos",
    "De um framework específico criado pelo PMI");

        Questions q42 = new Questions("Quem são os principais criadores do Scrum?",
            "Jeff Sutherland e Ken Schwaber",
            "Nonaka e Takeuchi",
            "Martin Fowler e Robert C. Martin",
            "Kent Beck e Ward Cunningham");

        Questions q43 = new Questions("Qual é a principal abordagem do Scrum?",
            "Empírica, baseada em adaptação e inspeção contínua",
            "Prescritiva, com regras rígidas para desenvolvimento",
            "Cascata, com fases definidas e sequenciais",
            "Baseada em documentações extensivas antes do início do projeto");

        Questions q44 = new Questions("Qual é o principal objetivo do Scrum no desenvolvimento de software?",
            "Permitir flexibilidade e adaptação a mudanças frequentes",
            "Eliminar a necessidade de planejamento prévio",
            "Acelerar entregas sem comprometer qualidade",
            "Evitar interação entre membros da equipe para maior produtividade");

        Questions q45 = new Questions("O que é o Scrum Guide?",
            "Um documento que descreve as boas práticas e valores do Scrum",
            "Uma ferramenta de software para gerenciar times ágeis",
            "Uma certificação para desenvolvedores Scrum",
            "Um relatório sobre produtividade em projetos ágeis");

        Questions q46 = new Questions("Quais são os cinco valores do Scrum?",
            "Coragem, Foco, Comprometimento, Respeito e Abertura",
            "Planejamento, Execução, Controle, Documentação e Testes",
            "Autonomia, Flexibilidade, Criatividade, Velocidade e Adaptação",
            "Objetividade, Eficiência, Qualidade, Rigor e Comunicação");

        Questions q47 = new Questions("O que significa 'Controle Empírico de Processo' no Scrum?",
            "O desenvolvimento baseado em transparência, inspeção e adaptação",
            "A centralização das decisões no Scrum Master",
            "O gerenciamento rígido de cada tarefa para evitar erros",
            "A separação completa entre planejamento e execução");

        Questions q48 = new Questions("O que significa Time-Boxing no Scrum?",
            "A definição de um tempo fixo para reuniões e eventos Scrum",
            "A organização do backlog do produto por tempo de desenvolvimento",
            "O tempo máximo permitido para mudanças no projeto",
            "O período necessário para planejar um projeto Scrum");

        Questions q49 = new Questions("Qual é a função do Product Owner?",
            "Maximizar o valor do produto e priorizar o Product Backlog",
            "Gerenciar diretamente a equipe de desenvolvimento",
            "Definir a arquitetura do software",
            "Escrever os testes automatizados do sistema");

        Questions q50 = new Questions("Qual é o papel do Scrum Master?",
            "Facilitar a aplicação do Scrum, removendo impedimentos para a equipe",
            "Definir os requisitos técnicos do projeto",
            "Gerenciar os prazos e custos do projeto",
            "Ser o responsável pelo desenvolvimento do código-fonte");

        Questions q51 = new Questions("Qual é o principal papel do Scrum Team?",
            "Ser auto-organizado e responsável pelo desenvolvimento do produto",
            "Executar tarefas apenas conforme orientações do gerente de projetos",
            "Definir os objetivos do negócio sem consultar o cliente",
            "Criar o planejamento inicial e repassá-lo ao Product Owner");

        Questions q52 = new Questions("O que acontece na Daily Scrum?",
            "Os membros da equipe compartilham o que fizeram, o que farão e impedimentos",
            "O Product Owner apresenta novas funcionalidades para a equipe",
            "A equipe revisa e aprova o código desenvolvido",
            "O Scrum Master define as próximas tarefas dos desenvolvedores");

        Questions q53 = new Questions("O que é um Sprint no Scrum?",
            "Um ciclo de desenvolvimento de tempo fixo onde funcionalidades são implementadas",
            "Um evento de revisão do código do time",
            "Uma ferramenta para medir a produtividade individual dos desenvolvedores",
            "Um método de estimativa para definir prazos");

        Questions q54 = new Questions("O que acontece na Sprint Review?",
            "A equipe apresenta as entregas do Sprint para os stakeholders",
            "A equipe define as tarefas do próximo Sprint",
            "Os desenvolvedores revisam o código uns dos outros",
            "O Scrum Master revisa o desempenho individual de cada membro");

        Questions q55 = new Questions("Qual é a função da Sprint Retrospective?",
            "Analisar o que foi bom e o que pode ser melhorado para o próximo Sprint",
            "Definir os requisitos para o próximo Sprint",
            "Aprovar ou rejeitar funcionalidades desenvolvidas",
            "Fazer ajustes na arquitetura do software");

        Questions q56 = new Questions("Qual é a duração recomendada de uma Sprint no Scrum?",
            "Entre 1 a 4 semanas",
            "1 dia",
            "3 meses",
            "6 meses");

        Questions q57 = new Questions("O que é o Definition of Done (DoD) no Scrum?",
            "Critérios que definem quando um item do backlog é considerado completo",
            "Uma lista de pendências para o próximo Sprint",
            "Uma regra para aprovar Sprints",
            "Um documento fixo estabelecido pelo gerente do projeto");

        Questions q58 = new Questions("Qual das seguintes opções é um princípio do Scrum?",
            "Colaboração entre os membros do time",
            "Planejamento fixo e rígido antes do início do projeto",
            "Documentação extensiva antes da implementação",
            "Separação entre as fases de desenvolvimento e testes");

        Questions q59 = new Questions("Como o Scrum trata mudanças nos requisitos?",
            "Aceita mudanças durante o desenvolvimento para entregar mais valor ao cliente",
            "Rejeita mudanças após o início do projeto",
            "Permite mudanças apenas no início do projeto",
            "Exige um novo planejamento completo para cada mudança");

        Questions q60 = new Questions("Qual é a importância da auto-organização no Scrum?",
            "Permite que o time tome decisões e aumente a produtividade",
            "Garante que cada membro trabalhe isoladamente",
            "Evita a necessidade de reuniões e planejamento",
            "Substitui a função do Product Owner");

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

        Questions q61 = new Questions("Qual é a importância da comunicação no Scrum?",
    "Promover transparência, colaboração e melhoria contínua",
    "Garantir que apenas o Scrum Master tenha informações do projeto",
    "Evitar mudanças durante o desenvolvimento",
    "Manter os requisitos fixos até a entrega final");

        Questions q62 = new Questions("Quais são os cinco eventos principais no Scrum?",
            "Sprint, Reunião de Planejamento do Sprint, Reuniões Diárias, Revisão do Sprint e Retrospectiva do Sprint",
            "Sprint, Code Review, Daily Scrum, Teste Unitário e Planejamento",
            "Sprint, Documentação, Reuniões Semanais, Aprovação Final e Retrospectiva",
            "Planejamento Geral, Sprint, Reuniões de Aprovação, Revisão Final e Testes de Aceitação");

        Questions q63 = new Questions("O que é um Sprint no Scrum?",
            "Um ciclo de desenvolvimento de tempo fixo para entrega de incrementos do produto",
            "Uma reunião para definir requisitos do projeto",
            "Uma etapa onde apenas testes são realizados",
            "Uma fase para documentar os processos antes da entrega final");

        Questions q64 = new Questions("O que é Time-Boxing no Scrum?",
            "Definição de um tempo fixo para execução de eventos e atividades",
            "Um prazo final obrigatório para todo o projeto",
            "Um tempo extra para correções e retrabalho",
            "Uma ferramenta para estimar o tempo de cada tarefa individualmente");

        Questions q65 = new Questions("Qual é o objetivo da Reunião de Planejamento do Sprint?",
            "Definir quais itens do backlog serão trabalhados e como serão desenvolvidos",
            "Apresentar o produto finalizado para os stakeholders",
            "Fazer um retrospecto do Sprint anterior",
            "Criar documentação detalhada antes de iniciar o Sprint");

        Questions q66 = new Questions("Quem participa da Reunião de Planejamento do Sprint?",
            "Product Owner, Scrum Master e Scrum Team",
            "Apenas o Scrum Master",
            "Somente o Product Owner e a gerência",
            "Todos os stakeholders da empresa");

        Questions q67 = new Questions("Quais são as três perguntas básicas da Reunião Diária (Daily Scrum)?",
            "O que fiz ontem? O que farei hoje? Há algum impedimento?",
            "Quais bugs encontrei? Quando será o próximo Sprint? O que foi planejado?",
            "Quais mudanças podemos fazer? Quem é o responsável pelo código? O que está atrasado?",
            "Quantas horas trabalhamos? Quem lidera o time? O que pode ser descartado?");

        Questions q68 = new Questions("Qual é a duração recomendada para a Reunião Diária?",
            "15 minutos",
            "1 hora",
            "45 minutos",
            "30 minutos");

        Questions q69 = new Questions("O que acontece na Revisão do Sprint?",
            "A equipe apresenta o que foi desenvolvido para stakeholders e Product Owner",
            "Os desenvolvedores revisam o código uns dos outros",
            "A equipe analisa problemas técnicos que surgiram durante o Sprint",
            "O Scrum Master decide o que será entregue na próxima Sprint");

        Questions q70 = new Questions("Quem aprova ou rejeita as entregas da Revisão do Sprint?",
            "Product Owner",
            "Scrum Master",
            "Scrum Team",
            "Cliente");

        Questions q71 = new Questions("Qual é o objetivo da Retrospectiva do Sprint?",
            "Analisar o que deu certo, o que pode melhorar e definir ações para otimizar o próximo Sprint",
            "Fazer correções no código antes da entrega final",
            "Avaliar individualmente cada membro da equipe",
            "Criar um novo planejamento detalhado para os próximos seis meses");

        Questions q72 = new Questions("Qual a diferença entre Revisão do Sprint e Retrospectiva do Sprint?",
            "A Revisão foca no produto entregue, enquanto a Retrospectiva foca na melhoria do processo",
            "A Retrospectiva serve para revisar código, e a Revisão para validar requisitos",
            "A Revisão acontece no início do Sprint e a Retrospectiva no final",
            "Não há diferença, ambos têm o mesmo objetivo");

        Questions q73 = new Questions("Quem deve participar da Retrospectiva do Sprint?",
            "Toda a equipe Scrum",
            "Apenas o Product Owner",
            "Apenas o Scrum Master",
            "A equipe de testes e o Scrum Master");

        Questions q74 = new Questions("Qual a importância do time-boxing na Retrospectiva do Sprint?",
            "Garante que a reunião tenha um tempo fixo e objetivo",
            "Evita que a equipe tome decisões sem o Scrum Master",
            "Determina a quantidade de tarefas que devem ser realizadas",
            "Define um limite de tempo para cada participante falar");

        Questions q75 = new Questions("Como as reuniões diárias ajudam a equipe Scrum?",
            "Facilitam a comunicação, identificam impedimentos e alinham o trabalho diário",
            "Servem para registrar a produtividade de cada membro",
            "Substituem a necessidade de reuniões de planejamento",
            "Eliminam a necessidade de um Product Owner");

        Questions q76 = new Questions("Qual evento do Scrum é voltado para avaliar e otimizar os processos internos da equipe?",
            "Retrospectiva do Sprint",
            "Revisão do Sprint",
            "Planejamento do Sprint",
            "Daily Scrum");

        Questions q77 = new Questions("Qual evento é responsável por definir as tarefas do Sprint?",
            "Reunião de Planejamento do Sprint",
            "Daily Scrum",
            "Revisão do Sprint",
            "Retrospectiva do Sprint");

        Questions q78 = new Questions("O que acontece se um time-boxing for muito curto?",
            "A equipe pode não conseguir finalizar as entregas a tempo",
            "O Scrum Master precisará replanejar todo o Sprint",
            "A velocidade de desenvolvimento será dobrada",
            "O Product Owner precisará redefinir as prioridades");

        Questions q79 = new Questions("Qual é o principal objetivo de um Sprint no Scrum?",
            "Criar um incremento funcional do produto que possa ser entregue",
            "Finalizar toda a documentação do projeto",
            "Definir novos requisitos com a equipe",
            "Garantir que o código esteja 100% otimizado");

        Questions q80 = new Questions("Por que a Revisão do Sprint é importante para o cliente?",
            "Permite que o cliente veja e valide o progresso do produto",
            "É um momento para revisar a documentação técnica",
            "Ajuda a definir o orçamento para a próxima Sprint",
            "Evita que a equipe precise fazer testes de aceitação");

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

        Questions q81 = new Questions("O que são os artefatos do Scrum?",
    "Indutores de informação que capturam o entendimento compartilhado da equipe",
    "Documentos formais para controle de qualidade do projeto",
    "Ferramentas de automação para desenvolvimento ágil",
    "Regras fixas definidas pelo Scrum Master");

        Questions q82 = new Questions("Quais são os três principais artefatos do Scrum?",
            "Backlog do Produto, Backlog do Sprint e Incremento do Produto",
            "Sprint, Retrospectiva e Product Owner",
            "Daily Scrum, Scrum Board e Burndown Chart",
            "Planejamento, Execução e Monitoramento");

        Questions q83 = new Questions("O que é o Backlog do Produto?",
            "Uma lista de requisitos priorizados que evolui ao longo do projeto",
            "Um documento fixo de requisitos fechado no início do projeto",
            "Uma lista de tarefas concluídas durante o Sprint",
            "Um conjunto de regras estabelecidas pelo Scrum Master");

        Questions q84 = new Questions("Quem é responsável por gerenciar o Backlog do Produto?",
            "Product Owner",
            "Scrum Master",
            "Scrum Team",
            "Cliente");

        Questions q85 = new Questions("O que é o Backlog do Sprint?",
            "Um subconjunto do Backlog do Produto que contém tarefas do Sprint atual",
            "Uma lista de funcionalidades aprovadas pelo cliente",
            "O planejamento geral do projeto Scrum",
            "Uma lista de impedimentos encontrados no Sprint");

        Questions q86 = new Questions("Quem pode modificar o Backlog do Sprint durante o Sprint?",
            "A própria equipe de desenvolvimento",
            "O Product Owner",
            "O Scrum Master",
            "Os Stakeholders");

        Questions q87 = new Questions("O que é o Incremento do Produto?",
            "A soma de todas as funcionalidades concluídas durante o Sprint",
            "Uma lista de funcionalidades que ainda precisam ser implementadas",
            "Uma ferramenta de controle de tempo no Scrum",
            "Uma estimativa de custos do projeto");

        Questions q88 = new Questions("Qual é a relação entre o Incremento do Produto e a Definição de Pronto (DoD)?",
            "O incremento precisa atender aos critérios da Definição de Pronto para ser aceito",
            "A Definição de Pronto define as tarefas que devem ser feitas no Sprint",
            "A Definição de Pronto é um relatório gerado no final do projeto",
            "O Incremento do Produto não precisa seguir critérios de qualidade");

        Questions q89 = new Questions("O que é um Gráfico Burndown no Scrum?",
            "Um gráfico que mostra a evolução do trabalho realizado ao longo do Sprint",
            "Uma ferramenta para estimar custos do projeto",
            "Uma lista de impedimentos encontrados durante o Sprint",
            "Uma forma de medir a produtividade individual dos membros da equipe");

        Questions q90 = new Questions("O que o Gráfico Burndown ajuda a visualizar?",
            "O progresso do Sprint e a quantidade de trabalho restante",
            "A velocidade de entrega de cada desenvolvedor",
            "A lista de tarefas que precisam ser revisadas",
            "Os erros de código encontrados durante os testes");

        Questions q91 = new Questions("O que é um Quadro Scrum (Scrum Board)?",
            "Uma ferramenta visual para acompanhar o progresso das tarefas no Sprint",
            "Uma planilha usada para documentar reuniões diárias",
            "Um relatório de desempenho dos desenvolvedores",
            "Uma ferramenta usada apenas pelo Scrum Master");

        Questions q92 = new Questions("Qual das opções é um formato comum para o Quadro Scrum?",
            "Colunas 'To Do', 'Doing' e 'Done'",
            "Lista de impedimentos e lista de riscos",
            "Relatório de progresso e lista de erros",
            "Tarefas aprovadas e tarefas pendentes");

        Questions q93 = new Questions("Como o Quadro Scrum pode ser representado?",
            "Fisicamente (quadro branco) ou digitalmente (ferramentas como Trello, Jira)",
            "Apenas como um documento físico",
            "Somente como um relatório gerado no final do Sprint",
            "Como um checklist entregue ao cliente");

        Questions q94 = new Questions("Quem pode visualizar e atualizar o Quadro Scrum?",
            "Toda a equipe de desenvolvimento",
            "Apenas o Scrum Master",
            "Somente o Product Owner",
            "Apenas a gerência");

        Questions q95 = new Questions("Por que o Backlog do Produto é considerado um artefato vivo?",
            "Porque pode ser atualizado continuamente conforme novas necessidades surgem",
            "Porque é fechado no início do projeto e não muda até o final",
            "Porque é um documento formal que precisa de aprovação para qualquer alteração",
            "Porque é um contrato fixo que não pode ser alterado");

        Questions q96 = new Questions("Qual é a relação entre o Backlog do Produto e a Revisão do Sprint?",
            "O feedback do cliente na Revisão do Sprint pode gerar novas demandas para o Backlog do Produto",
            "O Backlog do Produto deve ser fechado antes da Revisão do Sprint",
            "A Revisão do Sprint define novas prioridades para o Product Owner",
            "O Backlog do Produto só pode ser atualizado antes do início de um novo Sprint");

        Questions q97 = new Questions("O que acontece com os itens do Backlog do Sprint que não foram concluídos no Sprint atual?",
            "Podem ser movidos para o próximo Sprint, caso ainda sejam relevantes",
            "São automaticamente descartados",
            "Devem ser concluídos obrigatoriamente antes do próximo Sprint",
            "Precisam ser aprovados novamente pelo cliente");

        Questions q98 = new Questions("O que define a prioridade dos itens no Backlog do Produto?",
            "O valor de negócio e as necessidades do cliente",
            "A ordem de chegada das solicitações",
            "A decisão do Scrum Master",
            "A quantidade de tempo necessária para desenvolver cada item");

        Questions q99 = new Questions("Como os artefatos do Scrum ajudam no desenvolvimento ágil?",
            "Garantem transparência, alinhamento e acompanhamento contínuo do progresso",
            "Substituem a necessidade de reuniões da equipe",
            "Eliminam a necessidade de um Product Owner",
            "Impedem que mudanças sejam feitas no escopo do projeto");

        Questions q100 = new Questions("Qual é a principal função do Product Owner em relação aos artefatos do Scrum?",
            "Gerenciar e priorizar o Backlog do Produto",
            "Definir os critérios técnicos do Incremento do Produto",
            "Realizar a Revisão do Sprint sozinho",
            "Controlar o tempo de execução das tarefas no Quadro Scrum");

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

        Questions q101 = new Questions("Qual é o objetivo do processo de planejamento no Scrum?",
    "Definir expectativas das partes interessadas e sincronizá-las com a equipe",
    "Criar um plano rígido e inalterável para o projeto",
    "Garantir que todas as entregas sejam feitas sem atrasos",
    "Definir todas as funcionalidades do projeto antes do desenvolvimento");

        Questions q102 = new Questions("Quais são as três perguntas centrais do planejamento no Scrum?",
            "O que mudou financeiramente? Que progresso será feito a cada Sprint? Por que o projeto é um investimento valioso?",
            "Qual será o orçamento final? Quem é responsável pelo sucesso? Quando o projeto termina?",
            "Quantos membros compõem a equipe? Quem aprova cada etapa? Como os clientes interagem?",
            "Quais ferramentas serão usadas? Como será feita a documentação? Quem define os prazos?");

        Questions q103 = new Questions("O que é necessário para iniciar um projeto Scrum?",
            "Uma visão clara do projeto e um Backlog do Produto",
            "Um plano detalhado com todas as funcionalidades já definidas",
            "Um orçamento fechado e um cronograma fixo",
            "Aprovação de todas as partes interessadas antes do desenvolvimento");

        Questions q104 = new Questions("Qual é o papel da visão no planejamento de um projeto Scrum?",
            "Descrever os objetivos do projeto e o estado final desejado",
            "Definir o orçamento e prazos rígidos para o desenvolvimento",
            "Estabelecer os detalhes técnicos antes da implementação",
            "Definir as tecnologias que serão usadas no projeto");

        Questions q105 = new Questions("Quem é responsável por manter o Product Backlog?",
            "Product Owner",
            "Scrum Master",
            "Scrum Team",
            "Stakeholders");

        Questions q106 = new Questions("Quais elementos podem ser incluídos no Product Backlog?",
            "Características, funções, correções de bugs, melhorias e atualizações tecnológicas",
            "Apenas funcionalidades confirmadas pelo cliente",
            "Somente requisitos definidos no início do projeto",
            "Somente as tarefas a serem executadas no primeiro Sprint");

        Questions q107 = new Questions("Quem pode contribuir com novos itens no Product Backlog?",
            "Cliente, equipe de projeto, marketing, vendas, gerência e suporte ao cliente",
            "Apenas o Product Owner",
            "Somente os desenvolvedores",
            "Apenas a equipe de testes");

        Questions q108 = new Questions("O que é a Estimativa de Esforço no Scrum?",
            "Um processo iterativo para estimar o esforço necessário para implementar um item do Backlog",
            "Um prazo fixo para completar cada tarefa do projeto",
            "Uma avaliação financeira do custo do projeto",
            "Um relatório gerado pelo Scrum Master após cada Sprint");

        Questions q109 = new Questions("Quem é responsável por executar a Estimativa de Esforço?",
            "Product Owner e Scrum Team",
            "Apenas o Scrum Master",
            "Somente os desenvolvedores",
            "A gerência do projeto");

        Questions q110 = new Questions("Qual é a principal vantagem do Scrum em comparação com métodos tradicionais de planejamento?",
            "Maior flexibilidade para lidar com mudanças e adaptar o projeto continuamente",
            "Maior controle sobre cada etapa do desenvolvimento",
            "Menos reuniões e menor necessidade de comunicação",
            "Documentação detalhada antes do início do projeto");

        Questions q111 = new Questions("Por que o Scrum requer menos planejamento detalhado do que métodos tradicionais?",
            "Porque o progresso é monitorado a cada Sprint e as mudanças são incorporadas conforme necessário",
            "Porque a equipe já sabe tudo o que precisa ser feito antes de começar",
            "Porque o Product Owner toma todas as decisões sozinho",
            "Porque o Scrum Master controla rigidamente todas as tarefas");

        Questions q112 = new Questions("O que acontece no final de cada Sprint em relação ao planejamento?",
            "O progresso real do projeto é comparado ao plano inicial",
            "O Product Owner decide se o projeto continua ou não",
            "O Scrum Master apresenta um relatório finalizado para os stakeholders",
            "A equipe entrega um documento formal com as mudanças previstas");

        Questions q113 = new Questions("Qual é a relação entre o Scrum e o financiamento do projeto?",
            "O plano Scrum ajuda a justificar o investimento ao mostrar progresso contínuo e adaptação",
            "O Scrum elimina a necessidade de aprovação de orçamento",
            "O financiamento do projeto no Scrum é fixo e imutável",
            "Os stakeholders não têm influência no planejamento financeiro");

        Questions q114 = new Questions("Por que a transparência é essencial no gerenciamento de projetos Scrum?",
            "Permite que as partes interessadas acompanhem o progresso e façam ajustes quando necessário",
            "Evita a necessidade de reuniões diárias",
            "Ajuda a reduzir custos ao eliminar documentação",
            "Mantém os desenvolvedores focados apenas na implementação");

        Questions q115 = new Questions("Quais eventos do Scrum garantem a inspeção e adaptação contínua do projeto?",
            "Planejamento da Sprint, Reuniões Diárias, Revisão da Sprint e Retrospectiva da Sprint",
            "Daily Scrum, Revisão Financeira, Aprovação Final e Fechamento do Projeto",
            "Reuniões Quinzenais, Reunião de Gerência, Testes Unitários e Codificação",
            "Sprint Zero, Planejamento Mensal, Relatório de Progresso e Feedback do Cliente");

        Questions q116 = new Questions("Como o Product Backlog ajuda no alinhamento das partes interessadas?",
            "Mantendo uma lista clara e priorizada dos requisitos do projeto",
            "Definindo um plano rígido que não pode ser alterado",
            "Servindo como um documento finalizado no início do projeto",
            "Sendo atualizado apenas no final de cada Sprint");

        Questions q117 = new Questions("O que diferencia o planejamento do Scrum de métodos tradicionais como o modelo em cascata?",
            "O planejamento é contínuo e ajustável a cada Sprint",
            "O Scrum exige que todas as funcionalidades sejam detalhadas antes de começar",
            "O Scrum elimina a necessidade de reuniões de planejamento",
            "O planejamento no Scrum é fixo e não pode ser alterado após a primeira Sprint");

        Questions q118 = new Questions("Por que a estimativa de esforço no Scrum é considerada iterativa?",
            "Porque as estimativas são refinadas conforme mais informações ficam disponíveis",
            "Porque o tempo para cada tarefa é fixado desde o início",
            "Porque o Product Owner decide o esforço necessário para cada item",
            "Porque os desenvolvedores estimam apenas no início do projeto");

        Questions q119 = new Questions("Como o Scrum aborda mudanças nos requisitos do projeto?",
            "Aceita mudanças continuamente para maximizar o valor do produto",
            "Evita mudanças após o início do projeto",
            "Exige um novo planejamento completo para cada mudança",
            "As mudanças só podem ser feitas no final do projeto");

        Questions q120 = new Questions("Qual é a principal vantagem da abordagem ágil do Scrum em relação a projetos complexos?",
            "Permite adaptação contínua e entrega de valor incremental",
            "Elimina a necessidade de reuniões frequentes",
            "Acelera o desenvolvimento reduzindo a interação com o cliente",
            "Evita retrabalho ao definir todos os requisitos no início do projeto");

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
