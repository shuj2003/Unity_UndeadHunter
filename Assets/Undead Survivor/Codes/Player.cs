using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

//メモ：isTrigger付けたら、当たり判定除外されます。
//OnTriggerExit2D等関数の活性化に関しては関係ないです、別物です。

public class Player : MonoBehaviour
{

    [SerializeField] VariableJoystick variableJoystick;
    public Vector2 inputVec;
    public Scanner scanner;
    public float speed;
    public float health;
    public float healthMax;
    public Hand[] hands;

    private Rigidbody2D rigid;
    private SpriteRenderer sprite;
    private Animator anim;

    // Start is called before the first frame update

    private void Awake()
    {
        rigid = GetComponent<Rigidbody2D>();
        sprite = GetComponent<SpriteRenderer>();
        anim = GetComponent<Animator>();
        scanner = GetComponent<Scanner>();
        //includeInactive : 非アクティブのコンポーネントも含めるかどうか
        hands = GetComponentsInChildren<Hand>(true);

        speed = 3f;

        healthMax = 100;
        health = healthMax;
    }

    // 毎フレーム呼ばれる基本処理を書くところ
    void Update()
    {
        if (!GameManager.instance.isLive) return;

        inputVec = variableJoystick.Direction;

        //inputVec.x = Input.GetAxisRaw("Horizontal");
        //inputVec.y = Input.GetAxisRaw("Vertical");

    }

    // 様々な計算が終わった後の最終調整を書くところ。
    void LateUpdate()
    {
        if (!GameManager.instance.isLive) return;

        anim.SetFloat("Speed", inputVec.magnitude);

        if (inputVec.x != 0)
        {
            sprite.flipX = inputVec.x < 0;
        }

    }

    //独自のスレートで行います。物理演算に関係する処理を書くところ（ゲーム内時間に関係するもの）。
    //デフォルトは0.02秒単位で呼ばれます。時間設定できます。
    void FixedUpdate()
    {
        if (!GameManager.instance.isLive) return;

        //力を加える
        //rigid.AddForce(inputVec);

        //移動速度を設定
        //rigid.velocity = inputVec;

        //移動距離を設定
        //rigid.MovePosition(rigid.position + inputVec);

        Vector2 nextVec = inputVec * speed * Time.fixedDeltaTime;
        rigid.MovePosition(rigid.position + nextVec);

    }
    
    private void OnCollisionStay2D(Collision2D collision)
    {

        if (!GameManager.instance.isLive) return;

        if (collision.gameObject.tag != "Enemy") return;

        health -= Time.deltaTime * 10f;

        if(health < 0)
        {
            for(int i = 2 ; i < transform.childCount ; i++)
            {
                transform.GetChild(i).gameObject.SetActive(false);
            }

            anim.SetTrigger("Dead");
            GameManager.instance.GameOver();
            
        }

    }
    

}
