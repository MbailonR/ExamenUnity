using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

public class Player : MonoBehaviour
{
    [SerializeField] private float speed;
    /*
    Los límites definidos con bound nos hacen falta debido a que el jugador se puede salir de la pantalla
    debido a que su rigidbody es quinemático, por lo que no se ve afectado por la gravedad ni puede colisionar
    con objetos estáticos.
    */
    [SerializeField] private float bound = 4.5f; // x axis bound 
    public float Timer, TimerAux;
    private Collider2D cl;
    private Boolean esGrande;

    private Vector2 startPos; // Posición inicial del jugador




    // Start is called before the first frame update
    void Start()
    {
        cl = GetComponent<Collider2D>();
        Timer = 0f;
        TimerAux = 10f;
        esGrande = false;
        startPos = transform.position; // Guardamos la posición inicial del jugador
    }

    // Update is called once per frame
    void Update()
    {
       PlayerMovement();

        if (Timer <= 0)
        {
            if(esGrande == true){
                transform.localScale /= 2;
                esGrande = false;
            }
            Timer = TimerAux;
        }
        if (esGrande)
        {
            Timer -= Time.deltaTime;           
        }
        
    }

    void PlayerMovement()
    {
         float moveInput = Input.GetAxisRaw("Horizontal");
        // Controlaríamo el movimiento de la siguiente forma de no ser el rigidbody quinemático
        // transform.position += new Vector3(moveInput * speed * Time.deltaTime, 0f, 0f);

        Vector2 playerPosition = transform.position;
        // Mathf.Clamp nos permite limitar un valor entre un mínimo y un máximo
        playerPosition.x = Mathf.Clamp(playerPosition.x + moveInput * speed * Time.deltaTime, -bound, bound);
        transform.position = playerPosition;
       
    }

    public void ResetPlayer()
    {
        transform.position = startPos; // Posición inicial del jugador
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("powerUp")) // Si colisionamos con un powerUp
        {
            Destroy(collision.gameObject); // Lo destruimos
            GameManager.Instance.AddLife(); // Añadimos una vida
        }

        if (collision.CompareTag("powerDown"))
        {
            Destroy(collision.gameObject);
            GameManager.Instance.LoseLifepowerDown();
        }


        if(collision.CompareTag("growUp"))
        {
            Destroy(collision.gameObject);
            if(esGrande == false)
            {
                transform.localScale *= 2;

            }else if(esGrande == true)
            {
                Timer = TimerAux;
            }
            esGrande = true;
            //GameManager.Instance.GrowUp(); MAYBE para decirle si está grande en el salto de escena
        }
    }
}
