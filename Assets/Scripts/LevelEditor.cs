using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class LevelEditor : MonoBehaviour
{
    [SerializeField] Canvas startMenuCanvas;

    [SerializeField] Canvas levelEditorCanvas;
    [SerializeField] Scrollbar widthScrollBar;
    [SerializeField] Scrollbar heightScrollBar;
    [SerializeField] int maxWidth = 16;
    [SerializeField] int minWidth = 8;
    [SerializeField] int maxHeight = 10;
    [SerializeField] int minHeight = 5;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        ScenePersist.Instance.PlaygroundWidth = (int)(widthScrollBar.value * maxWidth) > minWidth ? (int)(widthScrollBar.value * maxWidth) : minWidth;
        ScenePersist.Instance.PlaygroundHeight = (int)(heightScrollBar.value * maxHeight) > minHeight ? (int)(heightScrollBar.value * maxHeight) : minHeight;
    }

    public void openLevelEditor()
    {
        startMenuCanvas.gameObject.SetActive(false);
        levelEditorCanvas.gameObject.SetActive(true);
    }

    public void exitLevelEditor()
    {
        startMenuCanvas.gameObject.SetActive(true);
        levelEditorCanvas.gameObject.SetActive(false);
        resetLevelEditor();
    }

    void resetLevelEditor()
    {
        widthScrollBar.value = 0;
        heightScrollBar.value = 0 ;
        ScenePersist.Instance.PlaygroundWidth = 0;
        ScenePersist.Instance.PlaygroundHeight = 0;
    }
}
