using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class Player : MonoBehaviour
{

    public Vector2 inputVec;
    public float speed;

    private Rigidbody2D rigid;

    // Start is called before the first frame update
    void Start()
    {
        rigid = GetComponent<Rigidbody2D>();
    }

    // Update is called once per frame
    //void Update()
    //{

    //    inputVec.x = Input.GetAxisRaw("Horizontal");
    //    inputVec.y = Input.GetAxisRaw("Vertical");

    //}

    void FixedUpdate()
    {
        //力を加える
        //rigid.AddForce(inputVec);

        //移動速度を設定
        //rigid.velocity = inputVec;

        //移動距離を設定
        //rigid.MovePosition(rigid.position + inputVec);

        Vector2 nextVec = inputVec.normalized * speed * Time.fixedDeltaTime;
        rigid.MovePosition(rigid.position + nextVec);

    }

    void OnMove(InputValue value)
    {

        inputVec = value.Get<Vector2>();

    }

    // PlayerInputから通知されるコールバック
    public void OnNavigate(InputAction.CallbackContext context)
    {
        // performedコールバックだけをチェックする
        if (!context.performed) return;

        // スティックの2軸入力取得
        var inputValue = context.ReadValue<Vector2>();

        // 入力値をログ出力
        Debug.Log($"OnNavigate : value = {inputValue}");
    }

}
