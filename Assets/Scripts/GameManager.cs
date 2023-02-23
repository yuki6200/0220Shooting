using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GameManager : MonoBehaviour
{
    public static GameManager gameManager = null;   //½Ì±ÛÅæÀ¸·Î ¸¸µé±â

    public Transform[] spawnPoints;
    public GameObject[] enemyPrefabs;

    public float curEnemySpawnDelay;
    public float nextEnemySpawnDelay;

    public GameObject player;

    public Image[] lifeicon;
    public GameObject overSet;

    int i = 0;

    public Text txtScore;

    private void Awake()    //½Ì±ÛÅæÀ¸·Î ¸¸µé±â
    {
        if (gameManager == null)
        {
            gameManager = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
    }

    // Start is called before the first frame update
    void Start()
    {
        txtScore.text = "0";
    }
   
    // Update is called once per frame
    void Update()
    {
        curEnemySpawnDelay += Time.deltaTime;
        if (curEnemySpawnDelay > nextEnemySpawnDelay)
        {
            SpawnEnemy();

            nextEnemySpawnDelay = Random.Range(0.5f, 3.0f);
            curEnemySpawnDelay = 0;
        }      
        
    }

    public void GetScore()
    {
        txtScore.text = Enemy.getScore.ToString();
    }

    void SpawnEnemy()
    {
        int ranType = Random.Range(0, 3);
        int ranPoint = Random.Range(0, 7);
        GameObject goEnemy = Instantiate(enemyPrefabs[ranType],
            spawnPoints[ranPoint].position, Quaternion.identity);
        Enemy enemyLogic = goEnemy.GetComponent<Enemy>();
        enemyLogic.playerObject = player;
        enemyLogic.Move(ranPoint);
    }

    public void GameOver()
    {
        lifeicon[lifeicon.Length-lifeicon.Length].enabled = false;
        Invoke("OverSet", 0.5f);        
    }
    void OverSet()
    {
        overSet.gameObject.SetActive(true);
        Time.timeScale = 0;
    }
    public void RespawnPlayer()
    {
        i++;
        lifeicon[lifeicon.Length - i].enabled = false;
        Invoke("AlivePlayer", 1.0f);
    }

    void AlivePlayer()
    {
        GameObject[] bullets = GameObject.FindGameObjectsWithTag("EnemyBullet");
        for (int i = 0; i < bullets.Length; i++)
        {
            Destroy(bullets[i]);
        }
    }    
}