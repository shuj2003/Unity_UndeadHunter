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
    private Collider2D coll;

    private WaitForFixedUpdate wait;
    private bool isLive;

    // Start is called before the first frame update
    void Awake()
    {

        rigid = GetComponent<Rigidbody2D>();
        sprite = GetComponent<SpriteRenderer>();
        animator = GetComponent<Animator>();
        coll = GetComponent<Collider2D>();

        wait = new WaitForFixedUpdate();
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

        if (!isLive || animator.GetCurrentAnimatorStateInfo(0).IsName("Hit"))
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

        isLive = true;
        coll.enabled = true;
        rigid.simulated = true;
        sprite.sortingOrder = 2;
        animator.SetBool("Dead", false);
        hp = hpMax;

    }

    public void Init(StageLevelData data)
    {

        hpMax = data.hp;
        hp = hpMax;
        speed = data.speed;
        animator.runtimeAnimatorController = aniCons[data.prefabID];

        
        sprite.transform.localScale = new Vector2(.25f, .25f) * (float)(hpMax / 2) + Vector2.one;

    }

    public void damage(int d)
    {
        hp -= d;
        StartCoroutine(KnockBack());

        if(hp <= 0)
        {
            isLive = false;
            coll.enabled = false;
            rigid.simulated = false;
            sprite.sortingOrder = 1;
            animator.SetBool("Dead", true);
            GameManager.instance.Kill++;
            GameManager.instance.getExp();
        }
        else
        {
            animator.SetTrigger("Hit");
        }
    }

    IEnumerator KnockBack()
    {
        yield return wait;
        Vector3 playerPos = GameManager.instance.player.transform.position;
        Vector3 dir = transform.position - playerPos;
        rigid.AddForce(dir.normalized * 3, ForceMode2D.Impulse);

    }

    private void Dead()
    {
        gameObject.SetActive(false);
    }

}
