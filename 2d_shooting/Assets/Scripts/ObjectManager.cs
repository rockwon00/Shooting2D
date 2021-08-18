using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObjectManager : MonoBehaviour
{
    public GameObject enemyBPrefab;
    public GameObject enemyLPrefab;
    public GameObject enemyMPrefab;
    public GameObject enemySPrefab;
    public GameObject itemCoinPrefab;
    public GameObject itemPowerPrefab;
    public GameObject itemBoomPrefab;
    public GameObject BulletPlayerAPrefab;
    public GameObject BulletPlayerBPrefab;
    public GameObject BulletEnemyAPrefab;
    public GameObject BulletEnemyBPrefab;
    public GameObject BulletFollowerPrefab;
    public GameObject BulletBossAPrefab;
    public GameObject BulletBossBPrefab;
    public GameObject ExplosionPrefab;

    GameObject[] enemyB ;
    GameObject[] enemyL;
    GameObject[] enemyM;
    GameObject[] enemyS;

    GameObject[] itemCoin;
    GameObject[] itemPower;
    GameObject[] itemBoom;

    GameObject[] bulletPlayerA;
    GameObject[] bulletPlayerB;
    GameObject[] bulleteEnemyA;
    GameObject[] bulleteEnemyB;
    GameObject[] bulletFollowerPrefab;
    GameObject[] bulletBossAPrefab;
    GameObject[] bulletBossBPrefab;
    GameObject[] explosionBPrefab;

    GameObject[] targetPool;

    void Awake()
    {
        enemyB = new GameObject[1];
        enemyL = new GameObject[10];
        enemyM = new GameObject[10];
        enemyS = new GameObject[20];

        itemCoin = new GameObject[20];
        itemPower = new GameObject[10];
        itemBoom = new GameObject[10];

        bulletPlayerA = new GameObject[100];
        bulletPlayerB = new GameObject[100];
        bulleteEnemyA = new GameObject[100];
        bulleteEnemyB = new GameObject[100];
        bulletFollowerPrefab = new GameObject[100];
        bulletBossAPrefab = new GameObject[50];
        bulletBossBPrefab = new GameObject[50];
        explosionBPrefab = new GameObject[20];

        Generate();
    }

    void Generate()
    {
        //#. Enemy
        for (int index = 0; index < enemyB.Length; index++)
        {
            enemyB[index] = Instantiate(enemyBPrefab);
            enemyB[index].SetActive(false);
        }

        for (int index = 0; index < enemyL.Length; index++)
        {
            enemyL[index] = Instantiate(enemyLPrefab);
            enemyL[index].SetActive(false);
        }
        for (int index = 0; index < enemyM.Length; index++)
        {
            enemyM[index] = Instantiate(enemyMPrefab);
            enemyM[index].SetActive(false);
        }
        for (int index = 0; index < enemyS.Length; index++)
        {
            enemyS[index] = Instantiate(enemySPrefab);
            enemyS[index].SetActive(false);
        }
        //#. Item
        for (int index = 0; index < itemCoin.Length; index++)
        {
            itemCoin[index] = Instantiate(itemCoinPrefab);
            itemCoin[index].SetActive(false);
        }
        for (int index = 0; index < itemPower.Length; index++)
        {
            itemPower[index] = Instantiate(itemPowerPrefab);
            itemPower[index].SetActive(false);
        }
        for (int index = 0; index < itemBoom.Length; index++)
        {
            itemBoom[index] = Instantiate(itemBoomPrefab);
            itemBoom[index].SetActive(false);
        }

        //#. Bullet
        for (int index = 0; index < bulletPlayerA.Length; index++)
        {
            bulletPlayerA[index] = Instantiate(BulletPlayerAPrefab);
            bulletPlayerA[index].SetActive(false);
        }
        for (int index = 0; index < bulletPlayerB.Length; index++)
        {
            bulletPlayerB[index] = Instantiate(BulletPlayerBPrefab);
            bulletPlayerB[index].SetActive(false);
        }
        for (int index = 0; index < bulleteEnemyA.Length; index++)
        {
            bulleteEnemyA[index] = Instantiate(BulletEnemyAPrefab);
            bulleteEnemyA[index].SetActive(false);
        }

        for (int index = 0; index < bulleteEnemyB.Length; index++)
        {
            bulleteEnemyB[index] = Instantiate(BulletEnemyBPrefab);
            bulleteEnemyB[index].SetActive(false);
        }

        for (int index = 0; index < bulletFollowerPrefab.Length; index++)
        {
            bulletFollowerPrefab[index] = Instantiate(BulletFollowerPrefab);
            bulletFollowerPrefab[index].SetActive(false);
        }

        for (int index = 0; index < bulletBossAPrefab.Length; index++)
        {
            bulletBossAPrefab[index] = Instantiate(BulletBossAPrefab);
            bulletBossAPrefab[index].SetActive(false);
        }

        for (int index = 0; index < bulletBossBPrefab.Length; index++)
        {
            bulletBossBPrefab[index] = Instantiate(BulletBossBPrefab);
            bulletBossBPrefab[index].SetActive(false);
        }

        for (int index = 0; index < explosionBPrefab.Length; index++)
        {
            explosionBPrefab[index] = Instantiate(ExplosionPrefab);
            explosionBPrefab[index].SetActive(false);
        }

    }

    public GameObject MakeObj(string type)
    {
        switch (type)
        {
            case "EnemyB":
                targetPool = enemyB;
                break;
            case "EnemyL":
                targetPool = enemyL;
                break;
            case "EnemyM":
                targetPool = enemyM;
                break;
            case "EnemyS":
                targetPool = enemyS;
                    break;
            case "ItemCoin":
                targetPool = itemCoin;
                break;
            case "ItemPower":
                targetPool = itemPower;
                break;
            case "ItemBoom":
                targetPool = itemBoom;
                break;
            case "BulletPlayerA":
                targetPool = bulletPlayerA;
                break;
            case "BulletPlayerB":
                targetPool = bulletPlayerB;
                break;
            case "BulleteEnemyA":
                targetPool = bulleteEnemyA;
                break;
            case "BulleteEnemyB":
                targetPool = bulleteEnemyB;
                break;
            case "BulletFollowerPrefab":
                targetPool = bulletFollowerPrefab;
                break;
            case "BulletBossAPrefab":
                targetPool = bulletBossAPrefab;
                break;
            case "BulletBossBPrefab":
                targetPool = bulletBossBPrefab;
                break;
            case "ExplosionPrefab":
                targetPool = explosionBPrefab;
                break;
        }

        for (int index = 0; index < targetPool.Length; index++)
        {
            if (!targetPool[index].activeSelf)
            {
                targetPool[index].SetActive(true);
                return targetPool[index];
            }            
        }
        return null;
    }

    public GameObject[] GetPool(string type)
    {
        switch (type)
        {
            case "EnemyB":
                targetPool = enemyB ;
                break;
            case "EnemyL":
                targetPool = enemyL;
                break;
            case "EnemyM":
                targetPool = enemyM;
                break;
            case "EnemyS":
                targetPool = enemyS;
                break;
            case "ItemCoin":
                targetPool = itemCoin;
                break;
            case "ItemPower":
                targetPool = itemPower;
                break;
            case "ItemBoom":
                targetPool = itemBoom;
                break;
            case "BulletPlayerA":
                targetPool = bulletPlayerA;
                break;
            case "BulletPlayerB":
                targetPool = bulletPlayerB;
                break;
            case "BulleteEnemyA":
                targetPool = bulleteEnemyA;
                break;
            case "BulleteEnemyB":
                targetPool = bulleteEnemyB;
                break;
            case "BulletFollowerPrefab":
                targetPool = bulletFollowerPrefab;
                break;
            case "BulletBossAPrefab":
                targetPool = bulletBossAPrefab;
                break;
            case "BulletBossBPrefab":
                targetPool = bulletBossBPrefab;
                break;
            case "ExplosionPrefab":
                targetPool = explosionBPrefab;
                break;
        }
        return targetPool;
    }
}
