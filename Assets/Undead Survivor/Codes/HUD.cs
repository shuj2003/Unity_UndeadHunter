using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class HUD : MonoBehaviour
{

    public enum HUD_TYPE {
        Exp,
        Level,
        Kill,
        Time,
        Health,
    };
    public HUD_TYPE type;

    Text text;
    Slider slider;

    private void Awake()
    {
        slider = GetComponent<Slider>();
        text = GetComponent<Text>();
    }

    private void LateUpdate()
    {

        switch (type)
        {
            case HUD_TYPE.Exp:
                {
                    float exp = (float)GameManager.instance.Exp;
                    float expMax = (float)GameManager.instance.NextExp[Mathf.Min(GameManager.instance.Level, GameManager.instance.NextExp.Length - 1)];
                    slider.value = exp / expMax;
                }
                break;
            case HUD_TYPE.Level:
                text.text = "Lv." + GameManager.instance.Level.ToString();
                break;
            case HUD_TYPE.Kill:
                text.text = GameManager.instance.Kill.ToString();
                break;
            case HUD_TYPE.Time:
                int time = (int)(GameManager.instance.maxGameTime - GameManager.instance.gameTime);
                text.text = string.Format("{0:D2}:{1:D2}", time / 60 , time % 60);
                break;
            case HUD_TYPE.Health:
                float health = (float)GameManager.instance.player.health;
                float healthMax = (float)GameManager.instance.player.healthMax;
                slider.value = health / healthMax;
                break;
        }

    }

}
