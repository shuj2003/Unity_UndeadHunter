using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Weapon0 : MonoBehaviour
{
    public BulletData[] bulletDatas;

    private int _level = 0;
    public int level
    {
        set 
        {
            _level = value;
            ReLoadLevel();
        }
        get
        {
            return _level;
        }
    }
    private int levelMax;
    private List<GameObject> bullets;

    float speed;
    int count;
    int power;

    float gameTime;
    float levelUpTime = 5f;
    float maxGameTime;

    // Start is called before the first frame update
    void Awake()
    {
        bullets = new List<GameObject>();
        level = 0;
        levelMax = bulletDatas.Length;
        Init(bulletDatas[level]);
        maxGameTime = bulletDatas.Length * levelUpTime - 1f;
    }

    // Update is called once per frame
    void Update() {

        gameTime += Time.deltaTime;

        if (gameTime > maxGameTime)
        {
            gameTime = maxGameTime;
        }

        int levelNext = Mathf.FloorToInt(gameTime / levelUpTime);
        if (levelNext > level) LvUp();

        transform.Rotate(Vector3.back * speed * Time.deltaTime);

    }

    public void LvUp()
    {

        if(level < levelMax)
        {
            level++;
        }
        Init(bulletDatas[level]);

    }

    public void Init(BulletData data)
    {
        speed = data.speed;
        power = data.power;
        count = data.count;
        ReLoadLevel();
    }

    void ReLoadLevel()
    {

        foreach (GameObject bullet in bullets)
        {
            bullet.SetActive(false);
        }
        bullets.Clear();

        for (int i = 0; i < count; i++)
        {
            var obj = GameManager.instance.pool.Get(1);
            bullets.Add(obj);
        }

        float r = 360f / (float)bullets.Count;
        for (int i = 0 ; i < bullets.Count ; i++)
        {
            GameObject bullet = bullets[i];
            bullet.transform.parent = transform;
            bullet.transform.localPosition = Vector3.zero;
            bullet.transform.localRotation = Quaternion.identity;

            bullet.transform.Rotate(Vector3.forward * r * i);
            bullet.transform.Translate(bullet.transform.up * 1.5f, Space.World);

            bullet.GetComponent<Bullet>().Init(power);

        }

    }

}

[System.Serializable]
public class BulletData
{
    public float speed;
    public int power;
    public int count;
}
