using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    public GameObject game;
    public GameObject enemyGenerator;
    public AudioClip jumpClip;
    public AudioClip dieClip;
    public AudioClip pointClip;
    public ParticleSystem dust;
    private Animator animator;
    private AudioSource audioPlayer;
    private float startY;

    // Start is called before the first frame update
    void Start()
    {
        animator = GetComponent<Animator>();
        audioPlayer = GetComponent<AudioSource>();
        //tomo la posición Y del personaje
        startY = transform.position.y;
    }

    // Update is called once per frame
    void Update()
    {
        //verifico si la posicion Y actual es la misma que al inicio
        bool isGrounded = transform.position.y == startY;
        //obtengo la accion del usuario
        bool userAction = Input.GetKeyDown("up") || Input.GetMouseButtonDown(0);
        //obtengo el estado del juego
        bool gamePlaying = game.GetComponent<GameController>().gameState == GameState.Playing;

        if (isGrounded && gamePlaying && userAction)
        {
            UpdateState("PlayerJump");
            audioPlayer.clip = jumpClip;
            audioPlayer.Play();
            DustStop();
        }
        else if (isGrounded && gamePlaying)
        { 
            DustPlay();
        }
    }

    public void UpdateState(string state = null)
    {
        if (state != null)
        {
            animator.Play(state);
        }
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        //Verifico si el enemigo colisionó con el EnemyDestroyer revisando el tag del objeto con el que colisionó
        if (other.gameObject.tag == "Enemy")
        {
            UpdateState("PlayerDie");
            game.GetComponent<GameController>().gameState = GameState.Ended;
            enemyGenerator.SendMessage("StopGenerator", true);
            game.SendMessage("ResetTimeScale", 0.5f);
            //detengo las particulas al correr
            DustStop();//Player.SendMessage("DustStop");

            //audio
            game.GetComponent<AudioSource>().Stop();
            audioPlayer.clip = dieClip;
            audioPlayer.Play();
        }
        else if (other.gameObject.tag == "Point")
        {
            game.SendMessage("IncreasePoints");
            audioPlayer.clip = pointClip;
            audioPlayer.Play();
            //DustPlay();
        }
    }
    void GameReady()
    {
        game.GetComponent<GameController>().gameState = GameState.Ready;
    }

    void DustPlay()
    {
        dust.Play();
    }

    void DustStop()
    {
        dust.Stop();
    }

}
