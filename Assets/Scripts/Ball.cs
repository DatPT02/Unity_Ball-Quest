using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Ball : MonoBehaviour
{
    public enum BallState {inactive, preSpawn, Spawned, Ghost}

    [SerializeField] Sprite indicator;

    [SerializeField] ParticleSystem explosionFX;

    [SerializeField] int score = 10;
    BallState currentState = BallState.inactive;

    Sprite ballSprite;

    void Awake()
    {
        ballSprite = GetComponent<Image>().sprite;
        State = BallState.Spawned;
    }

    // Update is called once per frame
    void Update()
    {
        switch(currentState)
        {
            case BallState.preSpawn:
                GetComponent<Image>().sprite = indicator;
                GetComponent<Animator>().enabled = false;
                break;
            case BallState.Spawned:
                GetComponent<Image>().sprite = ballSprite;
                GetComponent<Animator>().enabled = true;
                break;

        }
    }

    public void ghosted()
    {
        currentState = BallState.Ghost;
        GetComponent<Image>().sprite = ballSprite;
        Color ballColor = GetComponent<Image>().color;
        ballColor.a /= 4;
        GetComponent<Image>().color = ballColor;
        GetComponent<Animator>().enabled = true;
        score /= 2;
    }

    public BallState State
    {
        get
        {
            return currentState;
        }
        set
        {
            currentState = value;
        }
    }

    public int Score{
        get
        {
            return score;
        }
    }

    public void SetSpriteToIndicator()
    {
        State = BallState.preSpawn;
    }

    public void SetSpriteToBall()
    {
        State = BallState.Spawned;
    }

    public void Explode()
    {
        ParticleSystem explosion = Instantiate(explosionFX, transform.position, Quaternion.identity);
        float explode_time = Random.Range(explosion.main.duration + explosion.main.startLifetime.constantMin, explosion.main.duration + explosion.main.startLifetime.constantMax);
        Destroy(explosion.gameObject,explode_time);

        //Destroy(gameObject);
        GameObject pool = FindObjectOfType<BallPool>().gameObject;
        gameObject.transform.SetParent(pool.transform);
        Debug.Log(gameObject.transform.parent);
        gameObject.SetActive(false);
        gameObject.GetComponent<Image>().color = Color.white;
    }
}
