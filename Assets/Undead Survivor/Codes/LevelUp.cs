using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LevelUp : MonoBehaviour
{
    RectTransform rect;
    public Item[] items;

    private void Awake()
    {
        rect = GetComponent<RectTransform>();
        items = GetComponentsInChildren<Item>(true);
        
    }

    public void Show()
    {
        Next();
        rect.localScale = Vector3.one;
        GameManager.instance.Stop();
    }

    public void Hide()
    {
        rect.localScale = Vector3.zero;
        GameManager.instance.Resume();
    }

    public void Select(int index)
    {
        items[index].onClick();
    }

    void Next()
    {
        //一旦全部非活性化します
        foreach(var item in items)
        {
            item.gameObject.SetActive(false);
        }

        //ランダムでボタンを表示
        int[] ran = new int[3];
        while (true)
        {
            ran[0] = Random.Range(0, items.Length);
            ran[1] = Random.Range(0, items.Length);
            ran[2] = Random.Range(0, items.Length);

            if (ran[0] != ran[1] && ran[1] != ran[2] && ran[2] != ran[0])
                break;
        }

        foreach(int r in ran)
        {
            Item item = items[r];

            if((item.level - 1) == item.data.powers.Length)
            {
                items[4].gameObject.SetActive(true);
            }
            else
            {
                item.gameObject.SetActive(true);
            }
        }

    }

}
