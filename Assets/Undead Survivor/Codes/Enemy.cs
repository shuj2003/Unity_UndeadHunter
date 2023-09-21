using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : MonoBehaviour
{

    public Rigidbody2D target;
    public float speed;

    private Rigidbody2D rigid;
    private SpriteRenderer sprite;

    private bool isLive = true;

    // Start is called before the first frame update
    void Start()
    {

        rigid = GetComponent<Rigidbody2D>();
        sprite = GetComponent<SpriteRenderer>();
        speed = 2;

    }

    // Update is called once per frame
    void Update()
    {
        
    }

    void LateUpdate()
    {

        if(!isLive)
        {
            return;
        }

        sprite.flipX = target.position.x < rigid.position.x;
    }

    void FixedUpdate()
    {

        if (!isLive)
        {
            return;
        }

        Vector2 inputVec =(target.position - rigid.position).normalized;
        Vector2 nextVec = inputVec * speed * Time.fixedDeltaTime;
        rigid.MovePosition(rigid.position + nextVec);

    }

    private void OnEnable()
    {

        target = GameManager.instance.player.GetComponent<Rigidbody2D>();

    }

}
