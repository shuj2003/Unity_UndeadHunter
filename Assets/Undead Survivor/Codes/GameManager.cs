using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    public static GameManager instance;
    
    public StageLevelData[] levelDatas;
    List<StageLevelData> enemyDatas;
    List<float> timers;
    [Header(" # Game Control ")]
    public bool isLive;
    public float gameTime;
    public float maxGameTime;
    [Header(" # Player Info ")]
    public int playerID;
    public int Level;
    public int Kill;
    public int Exp;
    public int[] NextExp;
    [Header(" # Game Object ")]
    public PoolManager pool;
    public Player player;
    public LevelUp uiLevelUp;
    public Result uiResult;
    public GameObject gameCleaner;
    public int gameLevel;

    float levelTime;

    private void Awake()
    {
        instance = this;
        maxGameTime = 180f;
        gameTime = 0f;
        levelTime = maxGameTime / (float)levelDatas.Length;

        enemyDatas = new List<StageLevelData>();
        enemyDatas.Add(levelDatas[0]);
        timers = new List<float>();
        timers.Add(0f);

    }

    private void Update()
    {
        if (!isLive) return;

        gameTime += Time.deltaTime;
        int nextGameLevel = (int)(gameTime / levelTime);
        if (nextGameLevel > gameLevel)
        {
            gameLevel = nextGameLevel;
            StageLevelData nextData = levelDatas[Mathf.Min(gameLevel, levelDatas.Length - 1)];

            bool needAdd = true;
            for (int i = 0; i < enemyDatas.Count; i++)
            {
                StageLevelData enemyData = enemyDatas[i];
                if (enemyData.prefabID == nextData.prefabID)
                {
                    enemyDatas[i] = nextData;
                    needAdd = false;
                    break;
                }
            }
            if (needAdd)
            {
                enemyDatas.Add(nextData);
                timers.Add(0f);
            }

        }

        if (gameTime >= maxGameTime)
        {
            gameTime = maxGameTime;
            GameWin();
        }

        for(int i = 0; i < timers.Count ; i++)
        {
            float timer = timers[i];
            timer += Time.deltaTime;
            timers[i] = timer;
        }

        for (int i = 0; i < enemyDatas.Count; i++)
        {
            StageLevelData enemyData = enemyDatas[i];
            float timer = timers[i];
            if (timer > enemyData.createTime)
            {
                timer = 0;
                timers[i] = timer;
                createEmpty(enemyData);
            }
        }

    }

    public void GameStart(int id)
    {
        playerID = id;
        isLive = true;

        player.gameObject.SetActive(true);
        uiLevelUp.Select(playerID % 2);
        Resume();

        AudioManager.instance.PlayBgm(true);
        AudioManager.instance.PlaySfx(AudioManager.Sfx.Select);

    }

    public void GameQuite()
    {
        Application.Quit();
    }

    public void GameOver()
    {
        StartCoroutine(GameOverRoutine());
    }

    IEnumerator GameOverRoutine()
    {
        isLive = false;

        yield return new WaitForSeconds(0.5f);

        uiResult.gameObject.SetActive(true);
        uiResult.Lose();
        Stop();

        AudioManager.instance.PlayBgm(false);
        AudioManager.instance.PlaySfx(AudioManager.Sfx.Lose);
    }

    public void GameWin()
    {
        StartCoroutine(GameWinRoutine());
    }

    IEnumerator GameWinRoutine()
    {
        isLive = false;
        gameCleaner.SetActive(true);

        yield return new WaitForSeconds(0.5f);

        uiResult.gameObject.SetActive(true);
        uiResult.Win();
        Stop();

        AudioManager.instance.PlayBgm(false);
        AudioManager.instance.PlaySfx(AudioManager.Sfx.Win);
    }

    public void GameRetry()
    {
        SceneManager.LoadScene(0);
    }

    void createEmpty(StageLevelData data)
    {
        GameObject enemy = pool.Get(0);
        enemy.GetComponent<Enemy>().Init(data);
        SpriteRenderer sp = enemy.GetComponent<SpriteRenderer>();
        Vector2 pos = Vector2.zero;

        Vector2 LB = Camera.main.ViewportToWorldPoint(Vector2.zero);
        Vector2 RU = Camera.main.ViewportToWorldPoint(Vector2.one);
        float width = RU.x - LB.x;
        float height = RU.y - LB.y;

        Vector2 pLB = new Vector2(
            player.transform.position.x - width / 2.0f - sp.size.x
            , player.transform.position.y - height / 2.0f - sp.size.y
            );

        Vector2 pLU = new Vector2(
            player.transform.position.x - width / 2.0f - sp.size.x
            , player.transform.position.y + height / 2.0f + sp.size.y
            );

        Vector2 pRB = new Vector2(
            player.transform.position.x + width / 2.0f + sp.size.x
            , player.transform.position.y - height / 2.0f - sp.size.y
            );

        Vector2 pRU = new Vector2(
            player.transform.position.x + width / 2.0f + sp.size.x
            , player.transform.position.y + height / 2.0f + sp.size.y
            );

        switch (Random.Range(0, 4))
        {
            case 0:
                pos = new Vector2(pLB.x, Random.Range(pLB.y, pLU.y));
                break;
            case 1:
                pos = new Vector2(pRB.x, Random.Range(pRB.y, pRU.y));
                break;
            case 2:
                pos = new Vector2(Random.Range(pRU.x, pLU.x), pRU.y);
                break;
            case 3:
                pos = new Vector2(Random.Range(pRB.x, pLB.x), pRB.y);
                break;
        }
        enemy.transform.position = pos;

    }

    public void getExp()
    {
        if (!isLive) return;

        Exp++;

        if(Exp >= NextExp[Mathf.Min(Level, NextExp.Length - 1)])
        {
            Exp = 0;
            Level++;
            uiLevelUp.Show();
        }

    }

    public void Stop()
    {
        isLive = false;
        Time.timeScale = 0;
    }

    public void Resume()
    {
        isLive = true;
        Time.timeScale = 1;
    }

}

[System.Serializable]
public class StageLevelData
{

    public int prefabID;
    public float createTime;
    public float hp;
    public float speed;

}
