using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BallController : MonoBehaviour
{

    public float rotacaoIntensidade = 5f;
    public Texture[] texturasBola;
    public Texture texturaTemporaria;
    private Renderer ballRender;

    // Start is called before the first frame update
    void Start()
    {
        Vector3 torqueAleatorio = new Vector3(UnityEngine.Random.Range(1.5f, 1.5f), UnityEngine.Random.Range(1.5f, 1.5f), UnityEngine.Random.Range(1.5f, 1.5f)) * rotacaoIntensidade;

        // Pegando componentes sem armazenar
        GetComponent<Rigidbody>().AddTorque(torqueAleatorio, ForceMode.Impulse);

        // Salvando componentes
        ballRender = GetComponent<Renderer>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void ChangeBallTexture(int value)
    {
        if (ballRender == null)
        {
            ballRender = GetComponent<Renderer>();
        }

        switch(value)
        {
            case 1:
                ballRender.material.mainTexture = texturasBola[0];
                break;
            case 2:
                ballRender.material.mainTexture = texturasBola[1];
                break;
            case 3:
                ballRender.material.mainTexture = texturasBola[2];
                break;
            case 4:
                ballRender.material.mainTexture = texturasBola[3];
                break;
            case 5:
                ballRender.material.mainTexture = texturasBola[4];
                break;
            case 6:
                ballRender.material.mainTexture = texturasBola[5];
                break;
            case 7:
                ballRender.material.mainTexture = texturasBola[6];
                break;
            case 8:
                ballRender.material.mainTexture = texturasBola[7];
                break;
            case 9:
                ballRender.material.mainTexture = texturasBola[8];
                break;
            case 10:
                ballRender.material.mainTexture = texturasBola[9];
                break;
            case 11:
                ballRender.material.mainTexture = texturasBola[10];
                break;
            case 12:
                ballRender.material.mainTexture = texturasBola[11];
                break;
            case 13:
                ballRender.material.mainTexture = texturasBola[12];
                break;
            case 14:
                ballRender.material.mainTexture = texturasBola[13];
                break;
            case 15:
                ballRender.material.mainTexture = texturasBola[14];
                break;
        }
    }
}
