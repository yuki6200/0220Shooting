using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class Player : MonoBehaviour
{
    public float speed = 0f;

    public float power = 0f;

    public bool isTouchTop = false;
    public bool isTouchBottom = false;
    public bool isTouchLeft = false;
    public bool isTouchRight = false;

    Animator anim;

    public GameObject bulletPrefabA;
    public GameObject bulletPrefabB;
    public float curBulletDelay = 0f;
    public float maxBulletDelay = 0.3f;

    private void Start()
    {
        anim = GetComponent<Animator>();
    }

    // Update is called once per frame
    void Update()
    {
        Move();
        Fire();
        ReloadBullet();
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

    void Power()
    {
        switch(power)
        {
            case 1:
                {
                    GameObject bullet = Instantiate(bulletPrefabA,
                        transform.position, Quaternion.identity);
                    Rigidbody2D rd = bullet.GetComponent<Rigidbody2D>();
                    rd.AddForce(Vector2.up * 10, ForceMode2D.Impulse);
                }
                break;
            case 2:
                {
                    GameObject bulletR = Instantiate(bulletPrefabA,
                        transform.position + Vector3.right * 0.1f, Quaternion.identity);
                    Rigidbody2D rdR = bulletR.GetComponent<Rigidbody2D>();
                    rdR.AddForce(Vector2.up * 10, ForceMode2D.Impulse);

                    GameObject bulletL = Instantiate(bulletPrefabA,
                        transform.position + Vector3.left * 0.1f, Quaternion.identity);
                    Rigidbody2D rdL = bulletL.GetComponent<Rigidbody2D>();
                    rdL.AddForce(Vector2.up * 10, ForceMode2D.Impulse);
                }
                break;
            case 3:
                {
                    GameObject bulletR = Instantiate(bulletPrefabA,
                        transform.position + Vector3.right * 0.25f, Quaternion.identity);
                    Rigidbody2D rdR = bulletR.GetComponent<Rigidbody2D>();
                    rdR.AddForce(Vector2.up * 10, ForceMode2D.Impulse);

                    GameObject bulletL = Instantiate(bulletPrefabA,
                        transform.position + Vector3.left * 0.25f, Quaternion.identity);
                    Rigidbody2D rdL = bulletL.GetComponent<Rigidbody2D>();
                    rdL.AddForce(Vector2.up * 10, ForceMode2D.Impulse);

                    GameObject bulletC = Instantiate(bulletPrefabB,
                        transform.position, Quaternion.identity);
                    Rigidbody2D rdC = bulletC.GetComponent<Rigidbody2D>();
                    rdC.AddForce(Vector2.up * 10, ForceMode2D.Impulse);
                }
                break;

        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.gameObject.tag == "PlayerBorder")
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
    }    
}
