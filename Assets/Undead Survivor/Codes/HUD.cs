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
                    if(GameManager.instance.Level >= GameManager.instance.NextExp.Length)
                    {
                        slider.value = 1f;
                    }
                    else
                    {
                        float exp = (float)GameManager.instance.Exp;
                        float expMax = (float)GameManager.instance.NextExp[GameManager.instance.Level];
                        slider.value = exp / expMax;
                    }
                }
                break;
            case HUD_TYPE.Level:
                break;
            case HUD_TYPE.Kill:
                break;
            case HUD_TYPE.Time:
                break;
            case HUD_TYPE.Health:
                break;
        }

    }

}
