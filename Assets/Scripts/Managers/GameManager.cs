using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public static GameManager instance;
    public int loopCount = 0;
    public float completionTime = 0;
    public int killCount = 0;
    private bool _loading = false;

    void Awake()
    {
        if (instance == null)
        {
            instance = this;
        }
        else
        {
            Destroy(gameObject);
            return;
        }
        DontDestroyOnLoad(gameObject);

        LevelLoader.instance.OnLoadingLevel += () => { _loading = true; };
        LevelLoader.instance.OnLevelLoaded += () => { _loading = false; };
    }

    private void Update()
    {
        if (_loading)
            return;

        completionTime += Time.deltaTime;
    }
}
