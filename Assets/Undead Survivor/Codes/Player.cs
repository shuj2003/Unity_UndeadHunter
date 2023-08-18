using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class Player : MonoBehaviour
{

    [SerializeField] VariableJoystick variableJoystick;
    public Vector2 inputVec;
    public float speed;

    private Rigidbody2D rigid;
    private SpriteRenderer sprite;
    private Animator anim;

    // Start is called before the first frame update
    void Start()
    {
        rigid = GetComponent<Rigidbody2D>();
        sprite = GetComponent<SpriteRenderer>();
        anim = GetComponent<Animator>();
        speed = 3;
    }

    // 毎フレーム呼ばれる基本処理を書くところ
    void Update()
    {
        inputVec = variableJoystick.Direction;

        //inputVec.x = Input.GetAxisRaw("Horizontal");
        //inputVec.y = Input.GetAxisRaw("Vertical");

    }

    // 様々な計算が終わった後の最終調整を書くところ。
    void LateUpdate()
    {
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
        //力を加える
        //rigid.AddForce(inputVec);

        //移動速度を設定
        //rigid.velocity = inputVec;

        //移動距離を設定
        //rigid.MovePosition(rigid.position + inputVec);

        Vector2 nextVec = inputVec * speed * Time.fixedDeltaTime;
        rigid.MovePosition(rigid.position + nextVec);

    }

}
