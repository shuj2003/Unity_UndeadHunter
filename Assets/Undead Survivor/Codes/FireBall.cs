using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FireBall : MonoBehaviour
{
    public int power;
    public int hitCount;
    public float speed;

    private SpriteRenderer sprite;
    private Rigidbody2D rigid;

    void Awake()
    {

        sprite = GetComponent<SpriteRenderer>();
        rigid = GetComponent<Rigidbody2D>();

    }

    // Start is called before the first frame update
    private void OnTriggerEnter2D(Collider2D collision)
    {

        if (!collision.CompareTag("Enemy")) return;

        collision.GetComponent<Enemy>().damage(power);

        hitCount--;

        if(hitCount == 0)
        {
            rigid.velocity = Vector2.zero;
            gameObject.SetActive(false);
        }

    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (!collision.CompareTag("Area"))
            return;

        gameObject.SetActive(false);

    }

    public void Init(FireBallData data, Vector2 dir)
    {

        power = data.power;
        hitCount = data.hitCount;
        speed =  data.speed;
        rigid.velocity = dir * speed;

        //sprite.transform.localScale = new Vector2(.25f, .25f) * (float)(power / 3) + Vector2.one;

    }
}
