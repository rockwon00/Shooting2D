using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : MonoBehaviour
{
    public float speed;
    public float health;
    public Sprite[] sprites;
    public string enemyName;
    public int enemyScore;


    public GameObject bulletObjA;
    public GameObject bulletObjB;
    public GameObject itemCoin;
    public GameObject itemPower;
    public GameObject itemBoom;
    public GameObject player;
    //#.Enemy같은 경우는 프리펩이기 때문에 바로 매니저를 끌고 올수가 없다
    public ObjectManager objectManager;

    public float maxShotDelay;
    public float curShotDelay;

    SpriteRenderer spriteRenderer;
    //Rigidbody2D rigid;

    void Awake()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();
        //rigid = GetComponent<Rigidbody2D>();
        //rigid.velocity = Vector2.down * speed;
    }

    void Update()
    {
        Fire();
        Reload();
    }

    void Fire()
    {
       

        if (curShotDelay < maxShotDelay)
            return;

        if (enemyName == "S")
        {
            GameObject bullet = objectManager.MakeObj("BulleteEnemyA");
            bullet.transform.position = transform.position;
            //GameObject bullet = Instantiate(bulletObjA, transform.position, transform.rotation);
            Rigidbody2D rigid = bullet.GetComponent<Rigidbody2D>();

            Vector3 dirVec = player.transform.position - transform.position;
            //목표물로 방향 = 목표물 위치 - 자신의 위치
            rigid.AddForce(dirVec.normalized * 3, ForceMode2D.Impulse);
        }
        else if (enemyName == "L")
        {
            GameObject bulletR = objectManager.MakeObj("BulleteEnemyB");
            bulletR.transform.position = transform.position + Vector3.right * 0.3f;
            GameObject bulletL = objectManager.MakeObj("BulleteEnemyB");
            bulletL.transform.position = transform.position + Vector3.left * 0.3f;
            //GameObject bulletR = Instantiate(bulletObjB, transform.position + Vector3.right*0.3f, transform.rotation);
            //GameObject bulletL = Instantiate(bulletObjB, transform.position + Vector3.left* 0.3f, transform.rotation);

            Rigidbody2D rigidR = bulletR.GetComponent<Rigidbody2D>();
            Rigidbody2D rigidL = bulletL.GetComponent<Rigidbody2D>();

            Vector3 dirVecR = player.transform.position - transform.position;
            Vector3 dirVecL = player.transform.position - transform.position;

            rigidR.AddForce(dirVecR.normalized * 5, ForceMode2D.Impulse);
            rigidL.AddForce(dirVecL.normalized * 5, ForceMode2D.Impulse);
        }
 
        curShotDelay = 0;
    }

    void Reload()
    {
        curShotDelay += Time.deltaTime;
    }


    public void OnHit(int dmg)
    {
        health -= dmg;
        if (health > 0)
        {
            spriteRenderer.sprite = sprites[1];
            Invoke("ReturnSprite", 0.1f);
        }

        if(health <= 0)
        {
            Player playerLogic = player.GetComponent<Player>();
            playerLogic.score += enemyScore;
            //적기가 파괴되면 플레이어에게 점수를 넘겨주어야함

            //#.랜덤으로 아이템 드랍
            int ran = Random.Range(0, 10);
            if(ran<3) //30%
            {
                Debug.Log("꽝");
            }
            else if (ran < 6) //30%
            {
                //코인
                GameObject itemCoin = objectManager.MakeObj("ItemCoin");
                itemCoin.transform.position = transform.position;
                //Instantiate(itemCoin, transform.position, itemCoin.transform.rotation);
            }
            else if (ran < 8) //20%
            {
                //파워
                GameObject itemPower = objectManager.MakeObj("ItemPower");
                itemPower.transform.position = transform.position;
                //Instantiate(itemPower, transform.position, itemPower.transform.rotation);
            }
            else if (ran < 10) //20%
            {
                //폭탄
                GameObject itemBoom = objectManager.MakeObj("ItemBoom");
                itemBoom.transform.position = transform.position;
                //Instantiate(itemBoom, transform.position, itemBoom.transform.rotation);
            }

            gameObject.SetActive(false);
            //Destroy(gameObject);
            
        }
    }

    void ReturnSprite()
    {
        spriteRenderer.sprite = sprites[0];
    }

    void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.tag == "Border Bullet")
            gameObject.SetActive(false);
        //Destroy(gameObject);    
        else if(collision.gameObject.tag == "PlayerBullet")
        {
            Bullet bullet = collision.gameObject.GetComponent<Bullet>(); 
            OnHit(bullet.dmg);
            //gameObject.SetActive(false);
            //Destroy(collision.gameObject);

        }
            

    }
}
