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
    //Awake()が呼び出された後に、Start()が呼び出されます。
    //GetComponent系や自身のクラス内での初期化をAwake()でしておいて、
    //他のクラスを呼び出す処理などはStart()に記述すれば、呼び出し先のクラスでまだ初期化できてない！みたいなことがなくなると思います。
    //どのクラスのAwake()が呼び出されるかという順番は一定ではないので、Awake()内に他のクラスを利用する処理を書いてしまうとうまくいかないことが多々あるので注意。
    //OnEnable()が呼び出されるタイミングですが、Unityの公式マニュアルによると「Awake()と同じタイミング」だそうです。
    //アクティブ化した時に実行したい処理がある場合はOnEnable()
    //イベント関数の実行順：https://docs.unity3d.com/jp/530/Manual/ExecutionOrder.html

    void Awake()
    {

        rigid = GetComponent<Rigidbody2D>();
        sprite = GetComponent<SpriteRenderer>();
        animator = GetComponent<Animator>();
        coll = GetComponent<Collider2D>();

        //次のFixedUpdateが発生するまでの時間
        wait = new WaitForFixedUpdate();
    }

    // Update is called once per frame
    void Update()
    {
        if (!GameManager.instance.isLive) return;
    }

    void LateUpdate()
    {
        if (!GameManager.instance.isLive) return;

        if (!isLive)
        {
            return;
        }

        sprite.flipX = target.position.x < rigid.position.x;
    }

    void FixedUpdate()
    {
        if (!GameManager.instance.isLive) return;

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

        
        sprite.transform.localScale = new Vector2(.15f, .15f) * (float)(hpMax / 2) + Vector2.one;

    }

    public void damage(int d)
    {
        if (!GameManager.instance.isLive) return;

        hp -= d;
        StartCoroutine(KnockBack());

        if(hp <= 0)
        {
            isLive = false;
            coll.enabled = false;
            //Simulatedは物理演算と当たり判定をOFFにする
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
        //waitの秒数分を待ってくれます、その後下の処理を実行します
        Vector3 playerPos = GameManager.instance.player.transform.position;
        Vector3 dir = transform.position - playerPos;
        rigid.AddForce(dir.normalized * 3, ForceMode2D.Impulse);

    }

    private void Dead()
    {
        gameObject.SetActive(false);
    }

}
