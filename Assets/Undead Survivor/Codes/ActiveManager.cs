using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class AchiveManager : MonoBehaviour
{
    public GameObject[] lockCharacter;
    public GameObject[] unlockCharacter;
    public GameObject notice;
    WaitForSecondsRealtime wait;

    enum Achive { UnlockBoss, UnlockMother }
    Achive[] achives;

    private void Awake()
    {
        achives = (Achive[])Enum.GetValues(typeof(Achive));
        wait = new WaitForSecondsRealtime(5f);

        if (!PlayerPrefs.HasKey("MyData"))
        {
            Init();
        }
        
    }

    void Init()
    {
        PlayerPrefs.SetInt("MyData", 1);

        foreach(Achive achive in achives)
        {
            PlayerPrefs.SetInt(achive.ToString(), 0);
        }
    }

    // Start is called before the first frame update
    void Start()
    {
        UnlockCharacter();
    }

    void UnlockCharacter()
    {
        for(int index = 0 ; index < lockCharacter.Length; index++)
        {
            string achiveName = achives[index].ToString();
            bool isUnlock = PlayerPrefs.GetInt(achiveName) == 1;
            lockCharacter[index].SetActive(!isUnlock);
            unlockCharacter[index].SetActive(isUnlock);
        }
    }

    private void LateUpdate()
    {
        foreach (Achive achive in achives)
        {
            CheckAchive(achive);
        }
    }

    void CheckAchive(Achive achive)
    {

        bool isAchive = false;

        switch (achive)
        {
            case Achive.UnlockBoss:
                isAchive = GameManager.instance.Kill >= 200;
                break;

            case Achive.UnlockMother:
                isAchive = GameManager.instance.gameTime >= GameManager.instance.maxGameTime;
                break;
        }

        if (isAchive && PlayerPrefs.GetInt(achive.ToString()) == 0)
        {
            PlayerPrefs.SetInt(achive.ToString(), 1);

            for(int index = 0 ; index < notice.transform.childCount ; index++)
            {
                notice.transform.GetChild(index).gameObject.SetActive(index == (int)achive);
            }

            StartCoroutine(NoticeRoutine());

        }

    }

    IEnumerator NoticeRoutine()
    {
        notice.SetActive(true);

        AudioManager.instance.PlaySfx(AudioManager.Sfx.LevelUp);

        yield return wait;

        notice.SetActive(false);

    }

}
