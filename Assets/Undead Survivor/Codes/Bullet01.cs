using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bullet01 : MonoBehaviour
{
    public float power;

    private SpriteRenderer sprite;

    void Awake()
    {

        sprite = GetComponent<SpriteRenderer>();

    }

    // Start is called before the first frame update
    private void OnTriggerEnter2D(Collider2D collision)
    {

        if (!collision.CompareTag("Enemy")) return;

        collision.GetComponent<Enemy>().damage(power);

    }

    public void Init(float p)
    {

        power = p;

        sprite.transform.localScale = new Vector2(.25f, .25f) * (power / 3f) + Vector2.one;

    }

}
