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
    public gamemanager gamemanager;

    public float maxShotDelay;
    public float curShotDelay;

    SpriteRenderer spriteRenderer;
    Animator anim;
    //Rigidbody2D rigid;

    public int patternIndex;
    public int curPatternCount;
    public int[] maxPatternCount;


    void Awake()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();
        //rigid = GetComponent<Rigidbody2D>();
        //rigid.velocity = Vector2.down * speed;

        if (enemyName == "B")
        {
            anim = GetComponent<Animator>();
        }

    }

    void OnEnable()
    {
        switch (enemyName)
        {
            case "B":
                health = 3000;
                Invoke("Stop", 2);
                break;
            case "L":
            health = 40;
                break;
            case "M":
            health = 10;
                break;
            case "S":
            health = 3;
                break;

        }
    }

    void Stop()
    {
        if (!gameObject.activeSelf)
            return;

        Rigidbody2D rigid = GetComponent<Rigidbody2D>();
        rigid.velocity = Vector2.zero;

        Invoke("Think", 2);
    }

    void Think()
    {
        patternIndex = patternIndex == 3 ? 0 : patternIndex + 1;
        curPatternCount = 0;
        switch (patternIndex)
        {
            case 0:
                FireFoward();
                break;
            case 1:
                FireShot();
                break;
            case 2:
                FireArc();
                break;
            case 3:
                FireAround();
                break;
        }
    }

    void FireFoward()
    {
        //Debug.Log("앞으로 4발 발사.");
        GameObject bulletR = objectManager.MakeObj("BulletBossAPrefab");
        bulletR.transform.position = transform.position + Vector3.right * 0.3f;
        GameObject bulletRR = objectManager.MakeObj("BulletBossAPrefab");
        bulletRR.transform.position = transform.position + Vector3.right * 0.45f;
        GameObject bulletL = objectManager.MakeObj("BulletBossAPrefab");
        bulletL.transform.position = transform.position + Vector3.left * 0.3f;
        GameObject bulletLL = objectManager.MakeObj("BulletBossAPrefab");
        bulletLL.transform.position = transform.position + Vector3.left * 0.45f;

        Rigidbody2D rigidR = bulletR.GetComponent<Rigidbody2D>();
        Rigidbody2D rigidRR = bulletRR.GetComponent<Rigidbody2D>();
        Rigidbody2D rigidL = bulletL.GetComponent<Rigidbody2D>();
        Rigidbody2D rigidLL = bulletLL.GetComponent<Rigidbody2D>();

        rigidR.AddForce(Vector2.down * 8, ForceMode2D.Impulse);
        rigidRR.AddForce(Vector2.down * 8, ForceMode2D.Impulse);
        rigidL.AddForce(Vector2.down * 8, ForceMode2D.Impulse);
        rigidLL.AddForce(Vector2.down * 8, ForceMode2D.Impulse);

        curPatternCount++;

        if (curPatternCount < maxPatternCount[patternIndex])
            Invoke("FireFoward", 2);
        else
        Invoke("Think", 3); 
    }

    void FireShot()
    {
        //Debug.Log("플레이어 방향으로 샷건.");
        for (int index = 0; index < 5; index++)
        {
            GameObject bullet = objectManager.MakeObj("BulleteEnemyB");
            bullet.transform.position = transform.position;

            Rigidbody2D rigid = bullet.GetComponent<Rigidbody2D>();
            Vector2 dirVec = player.transform.position - transform.position;
            Vector2 ranVec = new Vector2(Random.Range(-0.5f, 0.5f), Random.Range(0f, 2f));
            dirVec = dirVec + ranVec;
            rigid.AddForce(dirVec.normalized * 3, ForceMode2D.Impulse);
        }

        curPatternCount++;

        if (curPatternCount < maxPatternCount[patternIndex])
            Invoke("FireShot", 3.5f);
        else
            Invoke("Think", 3);
    }

    void FireArc()
    {
        Debug.Log("부채모양으로 발사.");

        GameObject bullet = objectManager.MakeObj("BulletEnemyA");
        bullet.transform.position = transform.position;
        bullet.transform.rotation = Quaternion.identity;
        Rigidbody2D rigid = bullet.GetComponent<Rigidbody2D>();
        Vector2 dirVec = new Vector2(Mathf.Sin(Mathf.PI*2*curPatternCount/maxPatternCount[patternIndex]),-1);
 
        rigid.AddForce(dirVec.normalized * 5, ForceMode2D.Impulse);


        curPatternCount++;

        if (curPatternCount < maxPatternCount[patternIndex])
            Invoke("FireArc", 0.15f);
        else
            Invoke("Think", 3);
    }

    void FireAround()
    {
        Debug.Log("원 형태로 전체 공격.");

        int roundNumA = 50;
        int roundNumB = 40;
        int roundNum = curPatternCount%2 ==0? roundNumA : roundNumB;

        for (int index = 0; index < roundNumA; index++)
        {
            GameObject bullet = objectManager.MakeObj("BulletBossB");
            bullet.transform.position = transform.position;
            bullet.transform.rotation = Quaternion.identity;

            Rigidbody2D rigid = bullet.GetComponent<Rigidbody2D>();
            Vector2 dirVec = new Vector2(Mathf.Sin(Mathf.PI * 2 * index / roundNum), 
                                         Mathf.Cos(Mathf.PI * 2 * index / roundNum));

            rigid.AddForce(dirVec.normalized * 2, ForceMode2D.Impulse);

            Vector3 rotVec = (Vector3.forward * 360 * index / roundNumA) + Vector3.forward * 90;
            bullet.transform.Rotate(rotVec);
        }

        

        curPatternCount++;

        if (curPatternCount < maxPatternCount[patternIndex])
            Invoke("FireAround", 0.7f);
        else
            Invoke("Think", 3);
    }

    void Update()
    {
        if (enemyName == "B")
            return;
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
        if (enemyName == "B")
        {
            anim.SetTrigger("OnHit");
        }
        else
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
            int ran = (enemyName == "B") ? 0 : ran = Random.Range(0, 10);
            if(ran<3) //30%
            {
                Debug.Log("Not Item");
            }
            else if (ran < 6) //30%
            {
                //코인
                GameObject itemCoin = objectManager.MakeObj("ItemCoin");
                itemCoin.transform.position = transform.position;
                //Rigidbody2D rigid = itemCoin.GetComponent<Rigidbody2D>();
                //rigid.velocity = Vector2.down * 1.5f;
                //Instantiate(itemCoin, transform.position, itemCoin.transform.rotation);
            }
            else if (ran < 8) //20%
            {
                //파워
                GameObject itemPower = objectManager.MakeObj("ItemPower");
                itemPower.transform.position = transform.position;
                //Rigidbody2D rigid = itemPower.GetComponent<Rigidbody2D>();
                //rigid.velocity = Vector2.down * 1.5f;
                //Instantiate(itemPower, transform.position, itemPower.transform.rotation);
            }
            else if (ran < 10) //20%
            {
                //폭탄
                GameObject itemBoom = objectManager.MakeObj("ItemBoom");
                itemBoom.transform.position = transform.position;
                //Rigidbody2D rigid = itemBoom.GetComponent<Rigidbody2D>();
                //rigid.velocity = Vector2.down * 1.5f;
                //Instantiate(itemBoom, transform.position, itemBoom.transform.rotation);
            }

            gameObject.SetActive(false);
            transform.rotation = Quaternion.identity;
            gamemanager.CallExplosion(transform.position, enemyName);
            //Destroy(gameObject);
            
        }
    }

    void ReturnSprite()
    {
        spriteRenderer.sprite = sprites[0];
    }

    void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.tag == "Border Bullet" && enemyName != "B")
        {
            gameObject.SetActive(false);
            transform.rotation = Quaternion.identity; //각도를 원래대로 돌려놓는다
        }
        //Destroy(gameObject);    
        else if (collision.gameObject.tag == "PlayerBullet")
        {
            Bullet bullet = collision.gameObject.GetComponent<Bullet>();
            OnHit(bullet.dmg);
            bullet.gameObject.SetActive(false);
            //Destroy(collision.gameObject);

        }
            

    }

}
