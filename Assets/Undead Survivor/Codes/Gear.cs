using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Gear : MonoBehaviour
{
    public ItemData.ItemType type;
    public int rate;

    public void Init(ItemData data)
    {
        name = "Gear " + data.itemId;
        transform.parent = GameManager.instance.player.transform;
        transform.localPosition = Vector3.zero;

        type = data.itemType;
        rate = data.basePower;

        ApplyGear();

    }
    public void LevelUp(int rate)
    {
        this.rate = rate;
        ApplyGear();
    }

    void ApplyGear()
    {
        switch (type)
        {
            case ItemData.ItemType.Glove:
                RateUp();
                break;
            case ItemData.ItemType.Shoe:
                SpeedUp();
                break;
        }
    }

    void RateUp()
    {
        Weapon[] weapons = transform.parent.GetComponentsInChildren<Weapon>();

        foreach (Weapon weapon in weapons)
        {
            switch (weapon.id)
            {
                case 0:
                    {
                        weapon.speed = (float)rate / 100f * 100f;
                    }
                    break;
                case 1:
                    {
                        weapon.speed = 1f - (float)rate / 100f * 0.25f;
                    }
                    break;
            }
        }

    }

    void SpeedUp()
    {

        float speed = 3f;
        GameManager.instance.player.speed = (float)rate / 100f * speed;

    }

}
