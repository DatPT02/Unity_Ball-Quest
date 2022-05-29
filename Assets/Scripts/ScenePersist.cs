using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ScenePersist : MonoBehaviour
{
    public static ScenePersist Instance;

    int finalScore;
    int playgroundWidth, playgroundHeight;

    void Awake()
    {
        if (Instance != null)
        {
            Destroy(gameObject);
            return;
        }

        Instance = this;
        DontDestroyOnLoad(gameObject);
    }

    void Update()
    {
        
    }

    public int TotalScore
    {
        get
        {
            return ScenePersist.Instance.finalScore;
        }
        set 
        {
            ScenePersist.Instance.finalScore = value;
        }
    }

    public int PlaygroundWidth
    {
        get
        {
            return ScenePersist.Instance.playgroundWidth;
        }
        set
        {
            ScenePersist.Instance.playgroundWidth = value;
        }
    }

    public int PlaygroundHeight
    {
        get
        {
            return ScenePersist.Instance.playgroundHeight;
        }
        set
        {
            ScenePersist.Instance.playgroundHeight = value;
        }
    }
}
