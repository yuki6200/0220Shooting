using System.Collections;
using System.Collections.Generic;
using System.Xml.Serialization;
using UnityEngine;

public class VirtualPad : MonoBehaviour
{
    public float MaxLength = 70;
    public bool is4DPad = false;
    public GameObject player;
    
    Vector2 defPos;
    Vector2 downPos;

    // Start is called before the first frame update
    void Start()
    {
        player = GameObject.FindGameObjectWithTag("Player");

        defPos = GetComponent<RectTransform>().localPosition;
    }
    
    public void Power()
    {
        Player shoot = player.GetComponent<Player>();
        shoot.Power();
    }

    public void PadDown()
    {
        downPos = Input.mousePosition;
    }
    public void PadUp()
    {
        GetComponent<RectTransform>().localPosition = defPos;
        Player playerControl = player.GetComponent<Player>();
        playerControl.SetAxis(0, 0);
    }

    public void PadDrag()
    {
        Vector2 mousePosition = Input.mousePosition;

        Vector2 newTabPos = mousePosition - downPos;
        Vector2 axis = newTabPos.normalized;

        float len = Vector2.Distance(defPos, newTabPos);
        if (len > MaxLength)
        {
            newTabPos.x = axis.x * MaxLength;
            newTabPos.y = axis.y * MaxLength;
        }

        GetComponent<RectTransform>().localPosition = newTabPos;

        Player playerController = player.GetComponent<Player>();
        playerController.SetAxis(axis.x, axis.y);
    }
}
