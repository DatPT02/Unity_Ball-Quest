using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class BallController : MonoBehaviour, IPointerClickHandler
{
    [SerializeField] Color clickColor;

    GameController myGameController;
    MenuController myMenuController;

    // Start is called before the first frame update
    void Start()
    {
        myGameController = FindObjectOfType<GameController>();
        myMenuController = FindObjectOfType<MenuController>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    

    public void OnPointerClick(PointerEventData eventData)
    {
        if(myMenuController.gamePaused) return;

        if(transform.childCount > 0 && transform.GetChild(0).GetComponent<Ball>().State == Ball.BallState.preSpawn) return;
        myGameController.HandleBallMovement(gameObject.transform.GetComponent<RectTransform>(), clickColor);
    }
    
}
