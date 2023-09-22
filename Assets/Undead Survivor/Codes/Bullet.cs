using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bullet : MonoBehaviour
{
    public int power;

    // Start is called before the first frame update
    private void OnTriggerEnter2D(Collider2D collision)
    {

        if (!collision.CompareTag("Enemy")) return;

        collision.GetComponent<Enemy>().damage(power);

    }
}
