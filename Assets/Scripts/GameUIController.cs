using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class GameUIController : MonoBehaviour
{

    [SerializeField] TextMeshProUGUI scoreText;
    [SerializeField] TextMeshProUGUI timerText;
    [SerializeField] TextMeshProUGUI finalScoreText;
    [SerializeField] TextMeshProUGUI highScoreText;
    [SerializeField] Image[] ballImages;
    GameController myGameController;
    BallSpawner ballSpawner;

    // Start is called before the first frame update
    void Start()
    {
        myGameController = FindObjectOfType<GameController>();
        ballSpawner = FindObjectOfType<BallSpawner>();
    }

    // Update is called once per frame
    void Update()
    {
        UpdateScore();
        UpdateTimer();
        UpdateNewBallsDisplay();
        UpdateFinalScore();
    }

    void UpdateTimer()
    {
        if(timerText)
        {
            float minutes = Mathf.FloorToInt(Time.timeSinceLevelLoad / 60);
            float seconds = Mathf.FloorToInt(Time.timeSinceLevelLoad % 60);
            timerText.text = string.Format("{0:00}:{1:00}", minutes, seconds);
        }
    }

    void UpdateScore()
    {
        if(scoreText) scoreText.text = myGameController.TotalScore.ToString();
    }

    void UpdateFinalScore()
    {
        if(finalScoreText && ScenePersist.Instance != null) 
        {
            finalScoreText.text = "Your score is : " + ScenePersist.Instance.TotalScore.ToString();
            highScoreText.text = "High Score : " + PlayerPrefs.GetInt("HighScore", 0).ToString();
        }
    }

    void UpdateNewBallsDisplay()
    {
        if(ballImages.Length > 0)
        {
            List<Sprite> newBallImages = ballSpawner.NewBallSprites;
            for(int i = 0; i < newBallImages.Count; i++)
            {
                ballImages[i].sprite = newBallImages[i];
            }

        }
    }
}
