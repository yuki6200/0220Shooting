using System.Collections;
using System.Collections.Generic;
using System.Net.Http.Headers;
using Unity.VisualScripting;
using UnityEngine;

public class Enemy : MonoBehaviour
{
    public float speed;
    public float health;

    public Sprite[] sprites;
    SpriteRenderer spriteRender;

    Rigidbody2D rd;

    public GameObject enemyBulletPrefab;

    public float curBulletDelay = 0f;
    public float maxBulletDelay = 1f;

    public GameObject playerObject;

    public static int getScore = 0;

    public int enemyScore;

    bool isDead = false;

    public int ranItem;
    public GameObject[] itemprefabs;

    public float curItemDelay;
    public float nextItemDelay;

    private void Awake()
    {
        rd = GetComponent<Rigidbody2D>();

        spriteRender = GetComponent<SpriteRenderer>();
    }

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        Fire();
        ReloadBullet();
        curItemDelay += Time.deltaTime;
    }

    void Fire()
    {     
        if (curBulletDelay > maxBulletDelay)
        {
            Power();
            curBulletDelay = 0;
        }
    }

    void Power()
    {
        if (gameObject.name.Contains("Enemy B"))
            return;

        GameObject bulletObj = Instantiate(enemyBulletPrefab,
            transform.position, Quaternion.identity);
        Rigidbody2D rdBullet = bulletObj.GetComponent<Rigidbody2D>();
        Vector3 dirVec = playerObject.transform.position - transform.position;
        rdBullet.AddForce(dirVec.normalized * 3, ForceMode2D.Impulse);

    }
    void ReloadBullet()
    {
        curBulletDelay += Time.deltaTime;
    }

    public void Move(int nPoint)
    {
        if (nPoint == 3 || nPoint == 4) // 오른쪽에 있는 스폰 포인트 배열 인덱스값
        {
            transform.Rotate(Vector3.back * 90);
            rd.velocity = new Vector2(speed * (-1), -1);
        }
        else if (nPoint == 5 || nPoint == 6) // 왼쪽에 있는 스폰 포인트 배열 인덱스값
        {
            transform.Rotate(Vector3.forward * 90);
            rd.velocity = new Vector2(speed, -1);
        }
        else
        {
            rd.velocity = Vector2.down * speed;
        }
    }     
    
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.tag == "Border")
        {
            Destroy(gameObject);
        }

        else if (collision.gameObject.tag == "PlayerBullet")
        {
            Bullet bullet = collision.gameObject.GetComponent<Bullet>();
            OnHit(bullet.power);
            Destroy(collision.gameObject); // 총알 지우기
        }
    }

    public void OnHit(float BulletPower)
    {
        health -= BulletPower;
        spriteRender.sprite = sprites[1];
        Invoke("ReturnSprite", 0.1f);

        if (health <= 0 && isDead == false) //isDead 조건을 더 걸어서 죽을때만 점수 체크되도록
        {
            isDead = true;
            Destroy(gameObject);
            
            getScore += enemyScore;
            GameManager.gameManager.GetScore();

            if (curItemDelay > nextItemDelay)
            {
                GetItem();

                nextItemDelay = Random.Range(5.0f, 10.0f);
                curItemDelay = 0;
            }                        
        }
    }

    void GetItem()
    {
        ranItem = Random.Range(0, 3);
        GameObject getItem = Instantiate(itemprefabs[ranItem], gameObject.transform.position, Quaternion.identity);
    }

    void ReturnSprite()
    {
        spriteRender.sprite = sprites[0];
    }
}
