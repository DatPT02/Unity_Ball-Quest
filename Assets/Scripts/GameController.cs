using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GameController : MonoBehaviour
{
    [SerializeField] GameObject Node;
    [SerializeField] int ballCombination = 5;
    [SerializeField] float gameOverDelay = 1f;

    [SerializeField] AudioClip pickupSFX;
    [SerializeField] AudioClip laydownSFX;
    [SerializeField] AudioClip ballExplodeSFX;
    [SerializeField] Scrollbar volumeScrollbar;

    AudioSource backgroundAudio;

    int Width;
    int Height;

    private RectTransform rectTrans;

    List<GameObject> NodeList = new List<GameObject>();
    List<GameObject> movableNodes = new List<GameObject>();

    int totalRows, totalCols;
    int totalNodeAmounts;

    GameObject Ball;
    bool ballSelected = false;
    BallSpawner ballSpawner;
    MenuController myMenuController;

    int totalScore = 0;
    bool haveCombination = false;

    // Start is called before the first frame update
    void Start()
    {
        rectTrans = GetComponent<RectTransform>();
        ballSpawner = FindObjectOfType<BallSpawner>();
        myMenuController = FindObjectOfType<MenuController>();

        Width = ScenePersist.Instance.PlaygroundWidth;
        Height = ScenePersist.Instance.PlaygroundHeight;
        if(Width > 0 && Height > 0) 
        {
            rectTrans.sizeDelta = new Vector2(Width * 100, Height * 100);
        }
        InstantiateGrid();

        backgroundAudio = GameObject.FindGameObjectWithTag("Background Audio").GetComponent<AudioSource>();
        if(backgroundAudio) volumeScrollbar.value = backgroundAudio.volume;
    }

    // Update is called once per frame
    void Update()
    {
        checkValidBallCombination();
        ScenePersist.Instance.TotalScore = totalScore;
        backgroundAudio.volume = volumeScrollbar.value;
    }

    public List<GameObject> getNodeList()
    {
        return NodeList;
    }

    void InstantiateGrid()
    {
        //get dimensions
        RectTransform nodeTrans = Node.GetComponent<RectTransform>();
        int nodeDimension = (int)nodeTrans.rect.width;
        int playgroundWidth = (int)rectTrans.rect.width;
        int playgroundHeight = (int)rectTrans.rect.height;

        //calculate amount of nodes in the grid
        totalCols = playgroundWidth / nodeDimension;
        totalRows = playgroundHeight / nodeDimension;
        totalNodeAmounts = totalRows * totalCols;

        //instantiate the grid
        for(int i = 0; i < totalRows; i++)
        {
            for(int j = 0; j < totalCols; j++)
            {
                //create node
                GameObject node = Instantiate(Node);
                node.transform.SetParent(this.transform);

                //set node position
                Vector2 nodePos = new Vector2(j * nodeDimension, -i * nodeDimension);
                node.GetComponent<RectTransform>().anchoredPosition = nodePos;
                NodeList.Add(node);
            }
        }
    }

    public void HandleBallMovement(RectTransform obj, Color clickColor) 
    {
        foreach(var list_obj in NodeList)
        {
            list_obj.GetComponent<Image>().color = Color.white;
        }

        if(obj.childCount > 0)
        {
            if(ballSelected)
            {
                if(Ball.GetComponent<Ball>().State != global::Ball.BallState.Ghost)
                {
                    Ball.GetComponent<Image>().color = Color.white;
                }
                AudioSource.PlayClipAtPoint(laydownSFX, Camera.main.transform.position);
                ballSelected = false;
            }
            else if(!ballSelected)
            {
                movableNodes = new List<GameObject>();
                Ball = obj.transform.GetChild(0).gameObject;
                if(Ball.GetComponent<Ball>().State != global::Ball.BallState.Ghost)
                {
                    Ball.GetComponent<Image>().color = clickColor;
                    GetMovableNodes(obj.gameObject);
                }
                else
                {
                    movableNodes = GetEmptyNodes();
                }
                AudioSource.PlayClipAtPoint(pickupSFX, Camera.main.transform.position);
                ballSelected = true;
            }
        }
        else
        {
            if(movableNodes.Contains(obj.gameObject))
            {
                if(ballSelected)
                {
                    FindObjectOfType<BallSpawner>().ChangeIndicatorToBall();
                    Ball.transform.SetParent(obj.transform, false);
                    if(Ball.GetComponent<Ball>().State != global::Ball.BallState.Ghost)
                    {
                        Ball.GetComponent<Image>().color = Color.white;
                    }
                    ballSelected = false;
                    ballSpawner.SpawnNewTurnBalls();
                }
            }
            else 
            {
                Ball.GetComponent<Image>().color = Color.white;
                ballSelected = false;
            }
            AudioSource.PlayClipAtPoint(laydownSFX, Camera.main.transform.position);
        }
        StartCoroutine(CheckGameOver());
    }


    void checkValidBallCombination()
    {
        for(int i = 0; i < NodeList.Count; i++)
        {
            CheckHorizontalCombination(i);
            CheckVerticalCombination(i);
            CheckDiagonalCombination(i);
            if(haveCombination)
            {
                haveCombination = false;
                break;
            }
        }
    }

    void PopBall(GameObject obj)
    {
        GameObject ballObj = obj.transform.GetChild(0).gameObject;
        Ball ball = ballObj.GetComponent<Ball>();
        totalScore += ball.Score;
        ball.Explode();
        AudioSource.PlayClipAtPoint(ballExplodeSFX, Camera.main.transform.position);
    }

    public int TotalScore
    {
        get
        {
            return totalScore;
        }
    }

    void CheckHorizontalCombination(int index)
    {
        List<GameObject> horizontalList = checkHorizontal(index);
        if (horizontalList.Count >= ballCombination)
        {
            foreach (var obj in horizontalList)
            {
                PopBall(obj);
            }
            haveCombination = true;
        }
    }

    List<GameObject> checkHorizontal(int index)
    {
        GameObject rightNode = getRightNode(index);

        List<GameObject> horizontallyConnectedNodes = new List<GameObject>();
        horizontallyConnectedNodes.Add(NodeList[index]);

        if(rightNode != null) 
        {
            if(NodeList[index].transform.childCount > 0 && rightNode.transform.childCount > 0)
            {
                if(getSpriteInNode(NodeList[index]) == getSpriteInNode(rightNode))
                {
                    horizontallyConnectedNodes.AddRange(checkHorizontal(index + 1));
                }
            }
        }

        return horizontallyConnectedNodes;
    }

    void CheckVerticalCombination(int index)
    {
        List<GameObject> verticalList = checkVertical(index);
        if (verticalList.Count >= ballCombination)
        {
            foreach (var obj in verticalList)
            {
                PopBall(obj);
            }
            haveCombination = true;
        }
    }

    List<GameObject> checkVertical(int index)
    {
        GameObject bottomNode = getBottomNode(index);

        List<GameObject> verticallyConnectedNodes = new List<GameObject>();
        verticallyConnectedNodes.Add(NodeList[index]);

        if(bottomNode != null) 
        {
            if(NodeList[index].transform.childCount > 0 && bottomNode.transform.childCount > 0)
            {
                if(getSpriteInNode(NodeList[index]) == getSpriteInNode(bottomNode))
                {
                    verticallyConnectedNodes.AddRange(checkVertical(index + totalCols));
                }
            }
        }

        return verticallyConnectedNodes;
    }

    void CheckDiagonalCombination(int index)
    {
        List<GameObject> diagonalRightList = checkDiagonalRight(index);
        if (diagonalRightList.Count >= ballCombination)
        {
            foreach (var obj in diagonalRightList)
            {
                PopBall(obj);
            }
            haveCombination = true;
        }

        List<GameObject> diagonalLeftList = checkDiagonalLeft(index);
        if (diagonalLeftList.Count >= ballCombination)
        {
            foreach (var obj in diagonalLeftList)
            {
                PopBall(obj);
            }
        }
    }

    List<GameObject> checkDiagonalRight(int index)
    {
        GameObject bottomRightNode = getBottomNode(index + 1);

        List<GameObject> verticallyConnectedNodes = new List<GameObject>();
        verticallyConnectedNodes.Add(NodeList[index]);

        if(bottomRightNode != null) 
        {
            if(NodeList[index].transform.childCount > 0 && bottomRightNode.transform.childCount > 0)
            {
                if(getSpriteInNode(NodeList[index]) == getSpriteInNode(bottomRightNode))
                {
                    verticallyConnectedNodes.AddRange(checkDiagonalRight(index + totalCols + 1));
                }
            }
        }

        return verticallyConnectedNodes;
    }

    List<GameObject> checkDiagonalLeft(int index)
    {
        GameObject bottomLeftNode = getBottomNode(index - 1);

        List<GameObject> verticallyConnectedNodes = new List<GameObject>();
        verticallyConnectedNodes.Add(NodeList[index]);

        if(bottomLeftNode != null) 
        {
            if(NodeList[index].transform.childCount > 0 && bottomLeftNode.transform.childCount > 0)
            {
                if(getSpriteInNode(NodeList[index]) == getSpriteInNode(bottomLeftNode))
                {
                    verticallyConnectedNodes.AddRange(checkDiagonalLeft(index + totalCols - 1));
                }
            }
        }

        return verticallyConnectedNodes;
    }

    GameObject getLeftNode(int index)
    {
        return (index + totalCols) % totalCols != 0 ? NodeList[index - 1] : null;
    }

    GameObject getRightNode(int index)
    {
        return (index + 1) % totalCols != 0 ? NodeList[index + 1] : null;
    }

    GameObject getTopNode(int index)
    {
        return index >= totalCols ? NodeList[index - totalCols] : null;
    }

    GameObject getBottomNode(int index)
    {
        return index < (totalNodeAmounts - totalCols) ? NodeList[index + totalCols] : null;
    }

    Sprite getSpriteInNode(GameObject Node)
    {
        return Node.transform.GetChild(0).GetComponent<Image>().sprite;
    }

    void GetMovableNodes(GameObject obj)
    {
        List<GameObject> neighbourNodes = GetNeighbourNodes(obj);

        foreach (var list_obj in neighbourNodes)
        {
            if (!movableNodes.Contains(list_obj))
            {
                if (isMovable(list_obj))
                {
                    movableNodes.Add(list_obj);
                    GetMovableNodes(list_obj);
                }
            }
        }

        foreach (var list_obj in movableNodes)
        {
            if(list_obj.transform.childCount <= 0) list_obj.GetComponent<Image>().color = Color.grey;
        }
    }

    List<GameObject> GetNeighbourNodes(GameObject obj)
    {
        int objIndex = NodeList.IndexOf(obj);
        List<GameObject> neighbourNodes = new List<GameObject>();

        GameObject leftNode = getLeftNode(objIndex);
        if(leftNode != null) neighbourNodes.Add(leftNode);
        GameObject rightNode = getRightNode(objIndex);
        if(rightNode != null) neighbourNodes.Add(rightNode);
        GameObject topNode = getTopNode(objIndex);
        if(topNode != null)neighbourNodes.Add(topNode);
        GameObject bottomNode = getBottomNode(objIndex);
        if(bottomNode != null) neighbourNodes.Add(bottomNode);

        return neighbourNodes;
    }

    bool isMovable(GameObject obj)
    {
        return ObjectIsEmpty(obj) 
                || obj.GetComponentInChildren<Ball>().State == global::Ball.BallState.preSpawn;
    }
    
    bool ObjectIsEmpty(GameObject obj)
    {
        return obj.transform.childCount <= 0;
    }

    IEnumerator CheckGameOver()
    {
        yield return new WaitForSeconds(gameOverDelay);

        if(ballSpawner.getEmptyNodeAmount() <= 0)
        {
            sceneManager mySceneManager = FindObjectOfType<sceneManager>();
            if(mySceneManager != null)
            {
                mySceneManager.LoadGameOver();
            }
        }

        if(PlayerPrefs.GetInt("HighScore", 0) < totalScore)
        {
            PlayerPrefs.SetInt("HighScore", totalScore);
        }
    }


    List<GameObject> GetEmptyNodes()
    {
        List<GameObject> emptyNodes = new List<GameObject>();
        foreach(var obj in NodeList)
        {
            if(ObjectIsEmpty(obj))
            {
                emptyNodes.Add(obj);
            }
        }
        
        foreach(var obj in emptyNodes)
        {
            obj.GetComponent<Image>().color = Color.grey;
        }

        return emptyNodes;
    }
}
