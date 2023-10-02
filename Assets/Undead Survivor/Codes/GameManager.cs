using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public static GameManager instance;
    
    public StageLevelData[] levelDatas;
    [Header(" # Game Control ")]
    public float gameTime;
    public float maxGameTime;
    [Header(" # Player Info ")]
    public int Level;
    public int Kill;
    public int Exp;
    public int[] NextExp = {5, 15, 30, };
    [Header(" # Game Object ")]
    public PoolManager pool;
    public Player player;

    float timer;

    private void Awake()
    {
        instance = this;
        maxGameTime = levelDatas.Length * 15f - 1f;
    }

    private void Update()
    {

        gameTime += Time.deltaTime;

        if (gameTime > maxGameTime)
        {
            gameTime = maxGameTime;
        }

        timer += Time.deltaTime;

        if (timer > levelDatas[Level].createTime)
        {
            timer = 0;
            createEmpty(levelDatas[Level]);
        }

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

        Exp++;

        if(Level < NextExp.Length && Exp >= NextExp[Level])
        {
            Exp = 0;
            Level++;
        }

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
