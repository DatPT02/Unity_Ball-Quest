using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GameSettingMenu : MonoBehaviour
{
    
    [SerializeField] Canvas settingCanvas;
    [SerializeField] Scrollbar volumeScrollBar;
    bool settingEntered = false;
    AudioSource backgroundAudio;
    // Start is called before the first frame update
    void Start()
    {
        backgroundAudio = GameObject.FindGameObjectWithTag("Background Audio").GetComponent<AudioSource>();
        if(backgroundAudio) volumeScrollBar.value = backgroundAudio.volume;
    }

    // Update is called once per frame
    void Update()
    {
        backgroundAudio.volume = volumeScrollBar.value;
    }

    
    public void openSettings()
    {
        settingEntered = !settingEntered;
        settingCanvas.gameObject.SetActive(settingEntered);
    }
}
