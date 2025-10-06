using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class PlayerController : MonoBehaviour
{
    public float speed = 5f;              // скорость движения персонажа
    public float jumpForce = 12f;         // сила прыжка
    private float direction = 0f;         // направление движения (влево/вправо)
    private Rigidbody2D player;           // Rigidbody2D игрока

    public Transform groundCheck;         // точка проверки земли
    public float groundCheckRadius;       // радиус проверки земли
    public LayerMask groundLayer;         // слой земли
    private bool isTouchingGround;        // стоит ли игрок на земле

    private Animator playerAnimation;     // аниматор игрока

    private Vector3 respawnPoint;         // точка возрождения
    public GameObject fallDetector;       // объект для отслеживания падения

    public Text scoreText;                // UI для очков
    public HealthBar healthBar;           // UI для здоровья
    private Vector3 initialPosition;      // стартовая позиция
    private bool canUsePortals = true;


    void Start()
    {
        player = GetComponent<Rigidbody2D>();
        playerAnimation = GetComponent<Animator>();

        if (GameManager.lastEntrance == "SpawnFromLevel2")
        {
            GameObject spawnPoint = GameObject.Find("SpawnFromLevel2");
            if (spawnPoint != null)
            {
                transform.position = spawnPoint.transform.position;
            }
            GameManager.lastEntrance = "";
        }
        else
        {
            initialPosition = transform.position;
            respawnPoint = initialPosition;
        }

        scoreText.text = "Score: " + Scoring.totalScore;

        // блокируем порталы на 0.2 секунды после загрузки сцены
        canUsePortals = false;
        Invoke("EnablePortals", 0.2f);
    }

    void EnablePortals()
    {
        canUsePortals = true;
    }



    void Update()
    {
        // проверка касания земли
        isTouchingGround = Physics2D.OverlapCircle(groundCheck.position, groundCheckRadius, groundLayer);

        // движение по горизонтали
        direction = Input.GetAxis("Horizontal");
        Vector2 movement = new Vector2(direction * speed, player.linearVelocity.y);
        player.linearVelocity = movement;

        // разворот спрайта
        float scaleX = direction > 0 ? 0.3764759f : direction < 0 ? -0.3764759f : transform.localScale.x;
        transform.localScale = new Vector3(scaleX, 0.3764759f, 1f);

        // прыжок
        if (Input.GetButtonDown("Jump") && isTouchingGround)
        {
            player.AddForce(new Vector2(0f, jumpForce), ForceMode2D.Impulse);
        }

        // анимации
        playerAnimation.SetFloat("Speed", Mathf.Abs(player.linearVelocity.x));
        playerAnimation.SetBool("OnGround", isTouchingGround);

        // fallDetector двигается за игроком по X
        fallDetector.transform.position = new Vector2(transform.position.x, fallDetector.transform.position.y);
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (!canUsePortals) return; // если портал ещё заблокирован — ничего не делаем

        if (collision.tag == "fallDetector")
        {
            Restart();
        }
        else if (collision.tag == "Checkpoint")
        {
            respawnPoint = transform.position;
        }
        else if (collision.tag == "NextLevel")
        {
            SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 1);
        }
        else if (collision.tag == "LastLevel")
        {
            GameManager.lastEntrance = "SpawnFromLevel2";
            SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex - 1);
        }
        else if (collision.tag == "Crystal")
        {
            Scoring.totalScore += 1;
            scoreText.text = "Score: " + Scoring.totalScore;
            collision.gameObject.SetActive(false);
        }
    }


    public void Restart()
    {
        // перезапуск текущей сцены
        SceneManager.LoadScene("SampleScene");

        Health.totalHealth = 1f;
        Scoring.totalScore = 0;
    }

    private void OnTriggerStay2D(Collider2D collision)
    {
        if (collision.tag == "Spike") // если игрок стоит на шипах
        {
            healthBar.Damage(0.002f);
        }
    }
}
