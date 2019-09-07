using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

//estados del juego
public enum GameState { Idle, Playing, Ended, Ready }

public class GameController : MonoBehaviour
{
    [Range(0f, 0.2f)]
    public float parallaxSpeed = 0.02f;
    public RawImage background;
    public RawImage platform;
    public GameState gameState = GameState.Idle;
    public GameObject uiIdle;
    public GameObject uiScore;
    public Text pointsText;
    public Text recordText;
    public GameObject player;
    public GameObject enemyGenerator;
    public float scaleTime = 6f;//escala de tiempo para añadir dificultad al juego
    public float scaleInc = .25f;//cada cuanto incrementa la escala de tiempo
    private AudioSource musicPlayer;
    private int points = 0;

    // Start is called before the first frame update
    void Start()
    {
        musicPlayer = GetComponent<AudioSource>();
        recordText.text = "Record: " + GetMaxScore().ToString();
    }

    // Update is called once per frame
    void Update()
    {
        bool userAction = Input.GetKeyDown("up") || Input.GetMouseButtonDown(0);
        //si el estado es idle y presiono la tecla "arriba" o hago click con el mouse, cambio el estado a playing
        if (gameState == GameState.Idle && userAction)
        {
            gameState = GameState.Playing;
            uiIdle.SetActive(false);
            uiScore.SetActive(true);
            //le mando el mensaje con el nombre del método que quiero ejecutar y como segundo argumento, la animacion a la que quiero cambiar
            player.SendMessage("UpdateState", "PlayerRun");
            //comienzan las particulas al correr
            player.SendMessage("DustPlay");
            enemyGenerator.SendMessage("StartGenerator");
            //arrando la musica de fondo
            musicPlayer.Play();
            //comienza a correr el timeScale (los parametros son: retardo y cada cuanto se ejecuta)
            InvokeRepeating("GameTimeScale", scaleTime, scaleTime);
        }
        else if (gameState == GameState.Playing)
        {
            //si el estado es playing, activo el parallax
            Parallax();
        }
        //juego preparado para reiniciarse
        else if (gameState == GameState.Ready)
        {
            if (userAction)
            {
                RestartGame();
            }
        }
    }

    void Parallax()
    {
        float finalSpeed = parallaxSpeed * Time.deltaTime;
        background.uvRect = new Rect(background.uvRect.x + finalSpeed, 0f, 1f, 1f);
        platform.uvRect = new Rect(platform.uvRect.x + finalSpeed * 4, 0f, 1f, 1f);
    }

    public void RestartGame()
    {
        ResetTimeScale();
        SceneManager.LoadScene("Principal");
    }

    void GameTimeScale()
    {
        Time.timeScale += scaleInc;
        Debug.Log("Ritmo Incrementado: " + Time.timeScale.ToString());
    }

    public void ResetTimeScale(float newTimeScale = 1f)
    {
        //cancelo el invoke y reseteo el timeScale
        CancelInvoke("GameTimeScale");
        Time.timeScale = newTimeScale;
        Debug.Log("Ritmo reestablecido: " + Time.timeScale.ToString());
    }

    public void IncreasePoints()
    {
        points++;
        pointsText.text = points.ToString();
        if (points > GetMaxScore())
        {
            recordText.text = "Record: " + points;
            SaveMaxScore(points);
        }
    }

    public int GetMaxScore()
    {
        //con el cero pongo un valor por defecto, en caso de que todavía no se haya escrito el archivo PlayerPrefs
        return PlayerPrefs.GetInt("Max Points", 0);
    }

    public void SaveMaxScore(int currentPoints)
    {
        //con el cero pongo un valor por defecto, en caso de que todavía no se haya escrito el archivo PlayerPrefs
        PlayerPrefs.SetInt("Max Points",currentPoints);
    }
}
