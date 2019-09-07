using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyController : MonoBehaviour
{
    public float velocity = 2f;
    private Rigidbody2D rb2d;
    // Start is called before the first frame update
    void Start()
    {
        rb2d = GetComponent<Rigidbody2D>();
        rb2d.velocity = Vector2.left * velocity;
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        //Verifico si el enemigo colisionó con el EnemyDestroyer revisando el tag del objeto con el que colisionó
        if (other.gameObject.tag == "Destroyer")
        {
            //destruyo la instancia del enemigo
            Destroy(gameObject);
        }
    }

}
