using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Video;
using UnityEngine.SceneManagement;

public class LoadingController : MonoBehaviour
{
    private VideoPlayer loading;
    // Start is called before the first frame update
    void Start()
    {
        loading = GetComponent<VideoPlayer>();

        loading.loopPointReached += OnLoadingEnd; // Quando loopPointReached for falso, ele executa o método e envia o objeto loading do tipo VideoPlayer por parametro
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    void OnLoadingEnd(VideoPlayer video)
    {
        loading.loopPointReached -= OnLoadingEnd;
        SceneManager.LoadScene("MainMenu");
    }
}
