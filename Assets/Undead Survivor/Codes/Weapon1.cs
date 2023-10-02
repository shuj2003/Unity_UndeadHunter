using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Weapon1 : MonoBehaviour
{
    public FireBallData[] fireBallDatas;

    private int _level = 0;
    public int level
    {
        set
        {
            _level = value;
        }
        get
        {
            return _level;
        }
    }
    private int levelMax;
    private List<GameObject> bullets;

    float gameTime;
    float levelUpTime = 5f;
    float maxGameTime;

    float timer;

    // Start is called before the first frame update
    void Awake()
    {
        bullets = new List<GameObject>();
         
        level = 0;
        levelMax = fireBallDatas.Length;
        maxGameTime = fireBallDatas.Length * levelUpTime - 1f;
    }

    // Update is called once per frame
    void Update()
    {

        gameTime += Time.deltaTime;

        if (gameTime > maxGameTime)
        {
            gameTime = maxGameTime;
        }

        int levelNext = Mathf.FloorToInt(gameTime / levelUpTime);
        if (levelNext > level) LvUp();

        timer += Time.deltaTime;

        Transform target = GameManager.instance.player.scanner.nearestTarget;
        if (timer > fireBallDatas[level].createTime && target)
        {
            timer = 0;
            createFireball(fireBallDatas[level]);
        }

    }

    public void LvUp()
    {

        if (level < levelMax)
        {
            level++;
        }

    }

    void createFireball(FireBallData data)
    {
        //position と　localPosition　の違い、positionはワールド座標、localPositionは相対座標
        var obj = GameManager.instance.pool.Get(2);
        obj.transform.position = transform.position;
        Transform target = GameManager.instance.player.scanner.nearestTarget;
        Vector2 normal = (target.position - obj.transform.position).normalized;
        obj.transform.rotation = Quaternion.FromToRotation(Vector3.up, normal);
        obj.GetComponent<FireBall>().Init(data, normal);
    }

}

[System.Serializable]
public class FireBallData
{
    public float speed;
    public int power;
    public int hitCount;
    public float createTime;
}