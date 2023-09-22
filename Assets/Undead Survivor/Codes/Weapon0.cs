using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Weapon0 : MonoBehaviour
{
    public BulletData data;

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
    private int levelMax = 3;
    private List<GameObject> bullets;

    float speed;
    int count;
    int power;

    // Start is called before the first frame update
    void Awake()
    {
        bullets = new List<GameObject>();
        level = 1;

        data.count = 5;
        data.power = 1;
        data.speed = 90;
    }

    // Update is called once per frame
    void Update() {

        if(data.count != count || data.speed != speed || data.power != power)
        {
            Init(data);
            transform.localRotation = Quaternion.identity;
        }

        transform.Rotate(Vector3.back * speed * Time.deltaTime);

    }

    public void LvUp()
    {

        if(level < levelMax)
        {
            level++;
        }
        ReLoadLevel();

    }

    void ReLoadLevel()
    {

        foreach (GameObject bullet in bullets)
        {
            bullet.SetActive(false);
        }
        bullets.Clear();

        /*
        switch (level)
        {
            case 0:
                { }
                break;
            case 1:
                {
                    var obj = GameManager.instance.pool.Get(1);
                    bullets.Add(obj);
                }
                break;
            case 2:
                {
                    for (int i = 0; i < 3; i++)
                    {
                        var obj = GameManager.instance.pool.Get(1);
                        bullets.Add(obj);
                    }
                }
                break;
            case 3:
                {
                    for (int i = 0; i < 6; i++)
                    {
                        var obj = GameManager.instance.pool.Get(1);
                        bullets.Add(obj);
                    }
                }
                break;
        }
        */

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

            bullet.GetComponent<Bullet>().power = power;

        }

    }

    public void Init(BulletData data)
    {
        speed = data.speed;
        power = data.power;
        count = data.count;
        ReLoadLevel();
    }

}

[System.Serializable]
public class BulletData
{
    public float speed;
    public int power;
    public int count;
}
