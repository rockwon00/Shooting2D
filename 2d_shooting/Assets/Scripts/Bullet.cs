﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bullet : MonoBehaviour
{
    public int dmg;

    void OnTriggerEnter2D(Collider2D collision)
    {
     if(collision.gameObject.tag == "Border Bullet")
        {
            gameObject.SetActive(false);
            //Destroy(gameObject);
        }   
    }
}
