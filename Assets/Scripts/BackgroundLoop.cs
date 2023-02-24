using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BackgroundLoop : MonoBehaviour
{
    float hight;

    private void Awake()
    {
        BoxCollider2D backgroundCollider = GetComponent<BoxCollider2D>();
        hight = backgroundCollider.size.y * -1f;
    }
    
    // Update is called once per frame
    void Update()
    {
        if (transform.position.y <= hight)
        {
            Reposition();
        }
    }

    void Reposition()
    {
        Vector2 offset = new Vector2(0, hight * -2f);
        transform.position = (Vector2)transform.position + offset;
    }
}
