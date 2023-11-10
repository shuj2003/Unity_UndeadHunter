using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    public static GameManager instance;
    
    public StageLevelData[] levelDatas;
    [Header(" # Game Control ")]
    public bool isLive;
    public float gameTime;
    public float maxGameTime;
    [Header(" # Player Info ")]
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

    float timer;

    private void Awake()
    {
        instance = this;
        maxGameTime = 10f;
        gameTime = maxGameTime;
    }

    private void Update()
    {
        if (!isLive) return;

        gameTime -= Time.deltaTime;

        if (gameTime <= 0)
        {
            gameTime = 0;
            GameWin();
        }

        timer += Time.deltaTime;

        if (timer > levelDatas[Mathf.Min(Level, levelDatas.Length - 1)].createTime)
        {
            timer = 0;
            createEmpty(levelDatas[Mathf.Min(Level, levelDatas.Length - 1)]);
        }

    }

    public void GameStart()
    {
        isLive = true;
        uiLevelUp.Select(0);
      
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
    public int hp;
    public float speed;

}
