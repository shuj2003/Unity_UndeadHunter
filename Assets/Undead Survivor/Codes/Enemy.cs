using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : MonoBehaviour
{

    public Rigidbody2D target;
    public float speed;
    public int hp;
    public int hpMax;
    public RuntimeAnimatorController[] aniCons;

    private Rigidbody2D rigid;
    private SpriteRenderer sprite;
    private Animator animator;

    private bool isLive;

    // Start is called before the first frame update
    void Awake()
    {

        rigid = GetComponent<Rigidbody2D>();
        sprite = GetComponent<SpriteRenderer>();
        animator = GetComponent<Animator>();

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

    public void Init(StageLevelData data)
    {
        isLive = true;

        hpMax = data.hp;
        hp = hpMax;
        speed = data.speed;
        animator.runtimeAnimatorController = aniCons[data.prefabID];

        
        sprite.transform.localScale = new Vector2(.25f, .25f) * (float)(hpMax / 2) + Vector2.one;

    }

    public void damage(int d)
    {
        hp -= d;
        if(hp <= 0)
        {
            isLive = false;
            gameObject.SetActive(false);
        }
    }

}
