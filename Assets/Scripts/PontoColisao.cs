using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PontoColisao : MonoBehaviour
{

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
        switch(consequencia)
        {
            case -1:
                Debug.Log(gameObject.name + ": trinca ou morre!");
                break;
            case 0:
                Debug.Log(gameObject.name + ": perde R$");
                break;
            case 1:
                Debug.Log(gameObject.name + ": nada");
                break;
            case 2:
                Debug.Log(gameObject.name + ": ganha R$");
                break;
        }
    }
}
