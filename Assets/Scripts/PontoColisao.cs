using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class PontoColisao : MonoBehaviour
{
    public GameObject gameManager;
    public int consequencia; // -1 (trinca, morre) 0 (perde R$) 1 (nada) 2 (ganha R$)

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void AlterarFundo()
    {
        TextMeshPro texto = this.transform.Find("TxtResult").GetComponent<TextMeshPro>();

        switch (consequencia)
        {
            case -1: // Trinca ou morre
                texto.text = "D\nE\nA\nD";
                break;
            case 0: // Perde R$
                texto.text = "-\nR\n$\n1\n0\n0";
                break;
            case 1: // Nada acontece
                texto.text = "S\nE\nG\nU\nI\nR";
                break;
            case 2: // Ganha R$
                texto.text = "R\n$\n5\n0";
                break;
        }
    }

    public void OnTriggerEnter(Collider other)
    {
        if (other.name == "bola")
        {
            StartCoroutine(gameManager.GetComponent<QuizController>().PlayConsequence(2f, consequencia));
        }
    }
}
