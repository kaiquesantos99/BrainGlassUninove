using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.Video;

public class BrainLoading : MonoBehaviour
{
    private VideoPlayer loadingBrain;
    // Start is called before the first frame update
    void Start()
    {
        loadingBrain = GetComponent<VideoPlayer>();

        loadingBrain.loopPointReached += OnLoadingStop;
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    void OnLoadingStop(VideoPlayer obj)
    {
        loadingBrain.loopPointReached -= OnLoadingStop;
        SceneManager.LoadScene("SampleScene");
    }
}
