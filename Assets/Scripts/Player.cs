using System.Collections;
using System.Collections.Generic;
using System.Threading;
using UnityEditor.U2D.Path;
using UnityEngine;

public class Player : MonoBehaviour
{
    public float speed = 0f;

    public float power = 0f;

    public float life = 3f;
    public int score;

    public bool isHit = false;
    public static string gameState;
    //Rigidbody2D rBody;

    public bool isTouchTop = false;
    public bool isTouchBottom = false;
    public bool isTouchRight = false;
    public bool isTouchLeft = false;

    Animator anim;

    public GameObject bulletPrefabA;
    public GameObject bulletPrefabB;

    public float curBulletDelay = 0f;
    public float maxBulletDelay = 1f;

    public static int playerScore;
    public GameObject boomEffect;

    SpriteRenderer spriteRenderer;
    PolygonCollider2D poly;
    Rigidbody2D rBody;

    public bool isRespawnTime;

    float axisH;
    float axisV;
    bool isMoving = false;

    private void Start()
    {
        anim = GetComponent<Animator>();
        spriteRenderer = GetComponent<SpriteRenderer>();
        poly = GetComponent<PolygonCollider2D>();
        rBody = GetComponent<Rigidbody2D>();

        gameState = "Playing";

    }

    // Update is called once per frame
    void Update()
    {
        Move();
        Fire();
        ReloadBullet();
        PadMove();
    }

    private void FixedUpdate()
    {
        if (gameState != "Playing")
            return;

        if (isHit)
        {
            float val = Mathf.Sin(Time.time * 50);
            if (val > 0)
            {
                gameObject.GetComponent<SpriteRenderer>().enabled = true;

            }
            else
            {
                gameObject.GetComponent<SpriteRenderer>().enabled = false;
            }
            return;
        }
        rBody.velocity = new Vector2(axisH, axisV) * speed;       

    }
    public void SetAxis(float axisX, float axisY)
    {
        axisH = axisX;
        axisV = axisY;

        if (axisH == 0 && axisV == 0)
            isMoving = false;
        else
            isMoving = true;
    }

    void PadMove()
    {
        Vector3 worldpos = Camera.main.WorldToViewportPoint(this.transform.position);
        if (worldpos.x < 0f) worldpos.x = 0f;
        if (worldpos.y < 0f) worldpos.y = 0f;
        if (worldpos.x > 1f) worldpos.x = 1f;
        if (worldpos.y > 1f) worldpos.y = 1f;
        this.transform.position = Camera.main.ViewportToWorldPoint(worldpos);
    }
    void Move()
    {
        float h = Input.GetAxisRaw("Horizontal");
        float v = Input.GetAxisRaw("Vertical");

        if ((isTouchRight && h == 1) || (isTouchLeft && h == -1))
        {
            h = 0;
        }
        if ((isTouchTop && v == 1) || (isTouchBottom && v == -1))
        {
            v = 0;
        }

        Vector3 curPos = transform.position;
        Vector3 nextPos = new Vector3(h, v, 0) * speed * Time.deltaTime;

        transform.position = curPos + nextPos;

        anim.SetInteger("Input", (int)h);
    }

    void Fire()
    {
        if (!Input.GetButton("Fire1"))
            return;

        if (curBulletDelay < maxBulletDelay)
            return;

        Power();

        curBulletDelay = 0;
    }

    void ReloadBullet()
    {
        curBulletDelay += Time.deltaTime;
    }

    public void Power()
    {
        switch (power)
        {
            case 1:
                {
                    GameObject bullet = Instantiate(bulletPrefabA, transform.position, Quaternion.identity);
                    Rigidbody2D rd = bullet.GetComponent<Rigidbody2D>();
                    rd.AddForce(Vector2.up * 10, ForceMode2D.Impulse);
                }
                break;
            case 2:
                {
                    GameObject bulletR = Instantiate(bulletPrefabA,
                        transform.position + Vector3.right * 0.1f,
                        Quaternion.identity);
                    Rigidbody2D rdR = bulletR.GetComponent<Rigidbody2D>();
                    rdR.AddForce(Vector2.up * 10, ForceMode2D.Impulse);

                    GameObject bulletL = Instantiate(bulletPrefabA,
                        transform.position + Vector3.left * 0.1f,
                        Quaternion.identity);
                    Rigidbody2D rdL = bulletL.GetComponent<Rigidbody2D>();
                    rdL.AddForce(Vector2.up * 10, ForceMode2D.Impulse);
                }
                break;
            case 3:
                {
                    GameObject bulletR = Instantiate(bulletPrefabA,
                        transform.position + Vector3.right * 0.25f,
                        Quaternion.identity);
                    Rigidbody2D rdR = bulletR.GetComponent<Rigidbody2D>();
                    rdR.AddForce(Vector2.up * 10, ForceMode2D.Impulse);

                    GameObject bulletC = Instantiate(bulletPrefabB,
                        transform.position,
                        Quaternion.identity);
                    Rigidbody2D rdC = bulletC.GetComponent<Rigidbody2D>();
                    rdC.AddForce(Vector2.up * 10, ForceMode2D.Impulse);

                    GameObject bulletL = Instantiate(bulletPrefabA,
                        transform.position + Vector3.left * 0.25f,
                        Quaternion.identity);
                    Rigidbody2D rdL = bulletL.GetComponent<Rigidbody2D>();
                    rdL.AddForce(Vector2.up * 10, ForceMode2D.Impulse);
                }
                break;
        }

    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.tag == "PlayerBorder")
        {
            switch (collision.gameObject.name)
            {
                case "Top":
                    isTouchTop = true;
                    break;
                case "Bottom":
                    isTouchBottom = true;
                    break;
                case "Right":
                    isTouchRight = true;
                    break;
                case "Left":
                    isTouchLeft = true;
                    break;
            }
        }
        if (collision.gameObject.tag == "EnemyBullet" || collision.gameObject.tag == "Enemy")
        {
            if (isRespawnTime)
                return;

            if (isHit)
                return;

            isHit = true;
            life--;

            if (life == 0)
            {
                GameManager.gameManager.GameOver();
                gameObject.SetActive(false);
            }
            else
            {
                GameManager.gameManager.RespawnPlayer();
                gameObject.SetActive(false);
                Invoke("Appear", 1.0f);
            }
            
        }

        if (collision.gameObject.tag == "Item")
        {
            Item item = collision.gameObject.GetComponent<Item>();
            switch (item.type)
            {
                case ItemType.Coin:
                    playerScore += 100;
                    GameManager.gameManager.GetScore();
                    break;
                case ItemType.Power:
                    power++;
                    if (power >= 3)
                        power = 3;
                    break;
                case ItemType.Boom:
                    {
                        boomEffect.SetActive(true);
                        Invoke("OffBoomEffect", 3.0f);

                        GameObject[] enemies = GameObject.FindGameObjectsWithTag("Enemy");
                        for (int i = 0; i < enemies.Length; i++)
                        {
                            Enemy enemyLogic = enemies[i].GetComponent<Enemy>();
                            enemyLogic.OnHit(1000);
                            Destroy(enemies[i]);
                        }
                        GameObject[] enemyBullets = GameObject.FindGameObjectsWithTag("EnemyBullet");
                        for (int i = 0; i < enemyBullets.Length; i++)
                        {
                            Destroy(enemyBullets[i]);
                        }
                    }
                    break;
            }
            Destroy(collision.gameObject);
        }        
    }

    void OffBoomEffect()
    {
        boomEffect.SetActive(false);
    }

    void Appear()
    {
        PowerUp();
        Invoke("PowerUp", 3f);
        gameObject.SetActive(true);
        transform.position = Vector3.down * 4.2f;
        isHit = false;        
    }
    
    void PowerUp()
    {
        isRespawnTime = !isRespawnTime;
        if (isRespawnTime)
        {
            spriteRenderer.color = new Color(1, 1, 1, 0.5f);
            poly.isTrigger = true;
        }
        else
        {
            spriteRenderer.color = new Color(1,1,1,1);
            poly.isTrigger = false;
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.gameObject.tag == "PlayerBorder")
        {
            switch (collision.gameObject.name)
            {
                case "Top":
                    isTouchTop = false;
                    break;
                case "Bottom":
                    isTouchBottom = false;
                    break;
                case "Right":
                    isTouchRight = false;
                    break;
                case "Left":
                    isTouchLeft = false;
                    break;
            }
        }
    }
}
