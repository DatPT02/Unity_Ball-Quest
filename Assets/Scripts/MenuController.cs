using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MenuController : MonoBehaviour
{

    [SerializeField] Canvas PauseMenu;
    [SerializeField] Canvas TutorialMenu;
    [SerializeField] Canvas SaveMenu;
    bool pauseButtonPressed = false;
    bool tutorialButtonPressed = false;
    bool saveButtonPressed = false;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void PauseButton()
    {
        if (tutorialButtonPressed) ChangeTutorialMenuState();
        if(saveButtonPressed) changeSaveMenuState();

        ChangePauseMenuState();
    }

    void ChangePauseMenuState()
    {
        pauseButtonPressed = !pauseButtonPressed;
        PauseMenu.gameObject.SetActive(pauseButtonPressed);
        PauseGame(pauseButtonPressed);
    }

    public void TutorialButton()
    {
        if (pauseButtonPressed) ChangePauseMenuState();
        if(saveButtonPressed) changeSaveMenuState();

        ChangeTutorialMenuState();
    }

    void ChangeTutorialMenuState()
    {
        tutorialButtonPressed = !tutorialButtonPressed;
        TutorialMenu.gameObject.SetActive(tutorialButtonPressed);
        PauseGame(tutorialButtonPressed);
    }

    public void SaveMenuButton()
    {
        if(pauseButtonPressed) ChangePauseMenuState();
        if(tutorialButtonPressed) ChangeTutorialMenuState();

        changeSaveMenuState();
    }

    void changeSaveMenuState()
    {
        saveButtonPressed = !saveButtonPressed;
        SaveMenu.gameObject.SetActive(saveButtonPressed);
        PauseGame(saveButtonPressed);
    }

    public void PauseGame(bool paused)
    {
        if(paused)
        {
            Time.timeScale = 0;
        }
        else 
        {
            Time.timeScale = 1;
        }
    }

    public bool gamePaused
    {
        get
        {
            return pauseButtonPressed;
        }
    }
}
