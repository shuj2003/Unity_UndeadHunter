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

    // Start is called before the first frame update
    void Start()
    {
        rigid = GetComponent<Rigidbody2D>();
        speed = 3;
    }

    // Update is called once per frame
    void Update()
    {
        inputVec = variableJoystick.Direction;

        //inputVec.x = Input.GetAxisRaw("Horizontal");
        //inputVec.y = Input.GetAxisRaw("Vertical");

    }

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
