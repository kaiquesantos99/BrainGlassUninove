using System.Collections;
using System.Collections.Generic;
using System.Reflection;
using System.Threading;
using UnityEngine;
using UnityEngine.UI;

public class AboutController : MonoBehaviour
{
    private int IndexCriador { get; set; } = 0;
    private float ChangeTime { get; set; } = 5f;

    public GameObject criadorObj;
    private Image criadorImg;

    public Sprite[] criadores;

    // Start is called before the first frame update
    void Start()
    {
        criadorImg = criadorObj.GetComponent<Image>();

        StartCoroutine(ChangePic(5f));
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    
    private IEnumerator ChangePic(float duration)
    {
        while(true)
        {
            if (criadores.Length == 0 || criadorImg == null) yield break;

            criadorImg.sprite = criadores[IndexCriador];

            IndexCriador = (IndexCriador + 1) % criadores.Length;

            yield return new WaitForSeconds(ChangeTime);
        }
    } 
}
