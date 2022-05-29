using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class BallSpawner : MonoBehaviour
{

    //[SerializeField] GameObject[] Ball;
    [SerializeField] int InitialAmount = 10;
    [SerializeField] int newTurnSpawnAmount = 3;

    GameController myGameController;
    BallPool myBallPool;
    List<GameObject> pooledObjects = new List<GameObject>();

    List<GameObject> NodeList = new List<GameObject>();
    List<Sprite> newBallImages = new List<Sprite>();
    // Start is called before the first frame update
    void Start()
    {
        myGameController = FindObjectOfType<GameController>();
        myBallPool = GetComponent<BallPool>();
        StartCoroutine(Initialize());
    }

    IEnumerator Initialize()
    {
        yield return new WaitForEndOfFrame();

        NodeList = myGameController.getNodeList();

        SpawnBallsAtRandom(InitialAmount);
        SpawnIndicators(newTurnSpawnAmount);
    }

    void SpawnBallsAtRandom(int amount)
    {
        pooledObjects = myBallPool.PooledObjects();
        for(int i = 0; i < amount; i ++)
        {
            int ballIndex = Random.Range(0, pooledObjects.Count);
            //int ballIndex = Random.Range(0, Ball.Length);
            int nodeIndex = Random.Range(0,NodeList.Count);

            if(NodeList[nodeIndex].transform.childCount <= 0){
                GameObject ball = pooledObjects[ballIndex];
                ball.SetActive(true);
                ball.transform.SetParent(NodeList[nodeIndex].transform);
                ball.GetComponent<RectTransform>().anchoredPosition = new Vector2(0,0);  
            }
            else i--;
        }
    }

    public int getEmptyNodeAmount()
    {
        int emptyAmount = 0;

        for(int i = 0; i < NodeList.Count; i ++)
        {
            if(NodeList[i].transform.childCount <= 0)
            {
                emptyAmount++;
            }
        }

        return emptyAmount;
    }

    public void SpawnNewTurnBalls()
    {
        SpawnIndicators(newTurnSpawnAmount);
    }

    void SpawnIndicators(int amount)
    {
        pooledObjects = myBallPool.PooledObjects();
        newBallImages.Clear();

        if(amount > getEmptyNodeAmount()) amount = getEmptyNodeAmount();
        for(int i = 0; i < amount; i ++)
        {
            int ballIndex = Random.Range(0, pooledObjects.Count);
            int nodeIndex = Random.Range(0,NodeList.Count);

            if(NodeList[nodeIndex].transform.childCount <= 0)
            {
                GameObject ball = pooledObjects[ballIndex];
                ball.SetActive(true);
                newBallImages.Add(ball.GetComponent<Image>().sprite);
                ball.GetComponent<Ball>().SetSpriteToIndicator();
                ball.transform.SetParent(NodeList[nodeIndex].transform);
                ball.GetComponent<RectTransform>().anchoredPosition = new Vector2(0,0);  
            }
            else i--;
        }
    }

    public void ChangeIndicatorToBall()
    {
        foreach(var obj in NodeList)
        {
            if(obj.transform.childCount > 0)
            {
                Ball ball = obj.GetComponentInChildren<Ball>();
                if(ball.State == global::Ball.BallState.preSpawn)
                {
                    ball.State = global::Ball.BallState.Spawned;

                    if(Random.Range(0,5) == 0)
                    {
                        ball.ghosted();
                    }
                }

            }
        }
    }

    public List<Sprite> NewBallSprites
    {
        get
        {
            return newBallImages;
        }
    }
}
