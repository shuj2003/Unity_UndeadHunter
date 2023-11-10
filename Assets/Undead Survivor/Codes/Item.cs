using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Item : MonoBehaviour
{
    // Start is called before the first frame update
    public ItemData data;
    public int level;
    public Weapon weapon;
    public Gear gear;

    Image icon;
    Text textLevel;
    Text textTitle;
    Text textDesc;

    private void Awake()
    {

        icon = GetComponentsInChildren<Image>()[1];
        icon.sprite = data.itemIcon;

        Text[] texts = GetComponentsInChildren<Text>();
        textLevel = texts[0];
        textTitle = texts[1];
        textDesc = texts[2];
        textTitle.text = data.itemName;

    }

    private void OnEnable()
    {

        textLevel.text = "Lv." + (level + 1);

        switch (data.itemType)
        {
            case ItemData.ItemType.Melee:
            case ItemData.ItemType.Range:
                {
                    if (level == 0)
                    {
                        textDesc.text = string.Format(data.itemDesc, data.basePower, data.baseCount);
                    }
                    else
                    {
                        int nextPower = data.basePower + data.powers[level - 1];
                        int nextCount = data.baseCount + data.counts[level - 1];

                        textDesc.text = string.Format(data.itemDesc, nextPower, nextCount);
                    }
                    
                }
                break;
            case ItemData.ItemType.Glove:
            case ItemData.ItemType.Shoe:
                {
                    if (level == 0)
                    {
                        textDesc.text = string.Format(data.itemDesc, data.basePower);
                    }
                    else
                    {
                        int nextPower = data.basePower + data.powers[level - 1];

                        textDesc.text = string.Format(data.itemDesc, nextPower);
                    }

                }
                break;
            case ItemData.ItemType.Heal:
                {
                    textDesc.text = data.itemDesc;
                }
                break;
        }

    }

    public void onClick()
    {

        if ((level - 1) < data.powers.Length || data.itemType == ItemData.ItemType.Heal)
        {
            LoadItemData();
        }

        level++;
        if((level - 1) == data.powers.Length && data.itemType != ItemData.ItemType.Heal)
        {
            GetComponent<Button>().interactable = false;
        }

    }

    void LoadItemData()
    {
        
        switch (data.itemType)
        {
            case ItemData.ItemType.Melee:
            case ItemData.ItemType.Range:
                {
                    if (level == 0)
                    {
                        GameObject newWeapon = new GameObject();
                        weapon = newWeapon.AddComponent<Weapon>();
                        weapon.Init(data);
                    }
                    else
                    {
                        int nextPower = data.basePower + data.powers[level - 1];
                        int nextCount = data.baseCount + data.counts[level - 1];

                        weapon.LevelUp(nextPower, nextCount);
                    }
                    
                }
                break;
            case ItemData.ItemType.Glove:
            case ItemData.ItemType.Shoe:
                {
                    if (level == 0)
                    {
                        GameObject newGear = new GameObject();
                        gear = newGear.AddComponent<Gear>();
                        gear.Init(data);
                    }
                    else
                    {
                        int nextPower = data.basePower + data.powers[level - 1];

                        gear.LevelUp(nextPower);
                    }
                }
                break;
            case ItemData.ItemType.Heal:
                {
                }
                break;
        }
    }

}
