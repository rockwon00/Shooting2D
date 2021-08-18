﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using System.IO; //파일을 읽기 위한 라이브러리 추가

public class gamemanager : MonoBehaviour
{
    public string[] enemyObjs;
    public Transform[] spawnPoints;

    public float nextSpawnDelay;
    public float curSpawnDelay;

    public GameObject player;
    public Text scoreText;
    public Image[] lifeImage;
    public Image[] boomImage;
    public GameObject gameOverSet;
    public ObjectManager objectManager;

    public List<Spawn> spawnList;
    public int spawnIndex;
    public bool spawnEnd; //닫아주는 플래그변수

    void Awake()
    {
        spawnList = new List<Spawn>();
        enemyObjs = new string[] { "EnemyS", "EnemyM", "EnemyL","EnemyB" };
        ReadSpawnFile();
    }

    void ReadSpawnFile()
    {
        //변수초기화
        spawnList.Clear();
        spawnIndex = 0;
        spawnEnd = false;

        //리스폰 파일 읽기
        TextAsset textFile = Resources.Load("Spawn") as TextAsset;
        StringReader stringReader = new StringReader(textFile.text);

        while (stringReader != null)
        {
            string line = stringReader.ReadLine();
            Debug.Log(line);
            if (line == null)
                break;

            //리스폰 데이터 생성
            Spawn spawnData = new Spawn();
            spawnData.delay = float.Parse(line.Split(',')[0]);
            spawnData.type = line.Split(',')[1];
            spawnData.point = int.Parse(line.Split(',')[2]);
            spawnList.Add(spawnData); //구조체를 스폰리스트에 집어 넣는다.
        }

        //텍스트 파일 닫기
        stringReader.Close(); //파일을 읽은 후 반드시 닫는다.

        nextSpawnDelay = spawnList[0].delay;
    }

    void Update()
    {
        curSpawnDelay = curSpawnDelay + Time.deltaTime;
        if (curSpawnDelay > nextSpawnDelay && !spawnEnd)
        {
            SpawnEnemy();
            //nextSpawnDelay = Random.Range(0.5f, 3f);
            curSpawnDelay = 0;
        }

        //#.UI Score Update
        Player playerLogic = player.GetComponent<Player>();
        scoreText.text = string.Format("{0:n0}", playerLogic.score);
    } 

    void SpawnEnemy()
    {
        int enemyIndex = 0;
        switch (spawnList[spawnIndex].type)
        {
            
            case "S":
                enemyIndex = 0;
                break;
            case "M":
                enemyIndex = 1;
                break;
            case "L":
                enemyIndex = 2;
                break;
            case "B":
                enemyIndex = 3;
                break;
        }
        //int ranEnemy = Random.Range(0, 3);
        //int ranPoint = Random.Range(0, 9);
        int enemyPoint = spawnList[spawnIndex].point;
        GameObject enemy = objectManager.MakeObj(enemyObjs[enemyIndex]) ;
        enemy.transform.position = spawnPoints[enemyPoint].position;    

        Rigidbody2D rigid = enemy.GetComponent<Rigidbody2D>();
        Enemy enemyLogic = enemy.GetComponent<Enemy>();
        enemyLogic.player = player; //적 생성이 된 직후이기 때문에 플레이어 변수를 넘겨주는것이 가능함 
        enemyLogic.gamemanager = this;
        enemyLogic.objectManager = objectManager;

        if (enemyPoint == 5 || enemyPoint == 6)
        {
            rigid.velocity = new Vector2(enemyLogic.speed * (1), -1);
            enemy.transform.Rotate(Vector3.back * 90);
        }
        else if (enemyPoint == 7 || enemyPoint == 8)
        {
            rigid.velocity = new Vector2(enemyLogic.speed * (-1), -1);
            enemy.transform.Rotate(Vector3.back * 90);
        }
        else
        {
            rigid.velocity = new Vector2(0, enemyLogic.speed * (-1));
        }

        //리스폰 인덱스 증가
        spawnIndex++;
        if (spawnIndex == spawnList.Count)
        {
            spawnEnd = true;
            return;
        }

        //다음 리스폰 딜레이 갱신
        nextSpawnDelay = spawnList[spawnIndex].delay;


    }

    public void UpdateLifeIcon(int life)
    {
        //Image를 일단 모두 투명 상태로 두고
        for (int index = 0; index < 3; index++)
        {
            lifeImage[index].color = new Color(1, 1, 1, 0);
        }
        //목숨대로 반투명 설정
        for (int index = 0; index < life; index++)
        {
            lifeImage[index].color = new Color(1, 1, 1, 1);
        }
    }

    public void UpdateBoomIcon(int boom)
    {
        //#. UI Boom Init Disable
        for (int index = 0; index < 3; index++)
        {
            boomImage[index].color = new Color(1, 1, 1, 0);
        }
        //#. UI Boom Active
        for (int index = 0; index < boom; index++)
        {
            boomImage[index].color = new Color(1, 1, 1, 1);
        }
    }

    public void RespwanPlayer()
    {
        Invoke("RespwanplayerExe", 2f);
    }    

    void RespwanplayerExe()
    {
        player.transform.position = Vector3.down * 3.5f;
        player.SetActive(true);

        Player playerLogic = player.GetComponent<Player>();
        playerLogic.isHit = false;
    }

    public void CallExplosion(Vector3 pos, string type)
    {
        GameObject explosion = objectManager.MakeObj("Explosion");
        Explosion explosionLogic = explosion.GetComponent<Explosion>();

        explosion.transform.position = pos;
        explosionLogic.StartExplosion(type);
    }

    public void GameOver()
    {
        gameOverSet.SetActive(true);
    }

    public void GameRetry()
    {
        SceneManager.LoadScene(0);
    }
}

