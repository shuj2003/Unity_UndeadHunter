using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Weapon : MonoBehaviour
{
    List<GameObject> bullets;
    public int id;
    public int prefabId;
    public int power;
    public int count;
    public float speed;
    float timer;

    void Awake()
    {

    }
    public void Init(ItemData data)
    {
        name = "Weapon " + data.itemId;
        transform.parent = GameManager.instance.player.transform;
        transform.localPosition = Vector3.zero;

        power = data.basePower;
        count = data.baseCount;

        id = data.itemId;

        for (int i = 0; i < GameManager.instance.pool.prefabs.Length ; i++)
        {
            var obj = GameManager.instance.pool.prefabs[i];
            if(obj == data.projectile)
            {
                prefabId = i;
                break;
            }
        }

        switch (id)
        {
            case 0:
                {
                    speed = 100f;
                    if (bullets == null) bullets = new List<GameObject>();
                    createBullet01();
                }
                break;
            case 1:
                {
                    speed = 1f;
                }
                break;
        }

        Hand hand = GameManager.instance.player.hands[(int)data.itemType];
        hand.sprite.sprite = data.hand;
        hand.gameObject.SetActive(true);

        GameManager.instance.player.BroadcastMessage("ApplyGear", SendMessageOptions.DontRequireReceiver);
        
    }

    public void LevelUp(int power, int count)
    {
        this.power = power;
        this.count = count;

        switch (id)
        {
            case 0:
                {
                    if (bullets == null) bullets = new List<GameObject>();
                    createBullet01();
                }
                break;
            case 1:
                {

                }
                break;
        }

        GameManager.instance.player.BroadcastMessage("ApplyGear", SendMessageOptions.DontRequireReceiver);
    }

    void Update()
    {
        if (!GameManager.instance.isLive) return;

        switch (id)
        {
            case 0:
                {
                    transform.Rotate(Vector3.back * speed * Time.deltaTime);
                }
                break;
            case 1:
                {
                    timer += Time.deltaTime;

                    Transform target = GameManager.instance.player.scanner.nearestTarget;
                    if (timer > speed && target)
                    {
                        timer = 0;
                        createBullet02();
                    }
                }
                break;
        }

    }

    void createBullet01()
    {

        foreach (GameObject bullet in bullets)
        {
            bullet.SetActive(false);
        }
        bullets.Clear();

        for (int i = 0; i < count; i++)
        {
            var obj = GameManager.instance.pool.Get(prefabId);
            bullets.Add(obj);
        }

        float r = 360f / (float)bullets.Count;
        for (int i = 0; i < bullets.Count; i++)
        {
            GameObject bullet = bullets[i];
            bullet.transform.parent = transform;
            bullet.transform.localPosition = Vector3.zero;
            bullet.transform.localRotation = Quaternion.identity;

            bullet.transform.Rotate(Vector3.forward * r * i);
            bullet.transform.Translate(bullet.transform.up * 1.5f, Space.World);

            bullet.GetComponent<Bullet01>().Init(power);

        }

    }

    void createBullet02()
    {
        //position と　localPosition　の違い、positionはワールド座標、localPositionは相対座標
        var obj = GameManager.instance.pool.Get(prefabId);
        obj.transform.position = transform.position;
        Transform target = GameManager.instance.player.scanner.nearestTarget;
        Vector2 normal = (target.position - obj.transform.position).normalized;
        obj.transform.rotation = Quaternion.FromToRotation(Vector3.up, normal);
        obj.GetComponent<Bullet02>().Init(power, count, normal);
    }

}