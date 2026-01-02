using System;
using System.Collections;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Events;
using Random = UnityEngine.Random;
using TMPro;

public class EnemySpawner : MonoBehaviour
{
    [SerializeField] private int baseEnemies = 8;
    [SerializeField] private float enemiesPerSec = 0.5f;
    [SerializeField] private GameObject[] enemyPrefabs;
    [SerializeField] private float timeBetweenRounds = 5f;
    [SerializeField] private float difficulty = 0.75f;
    [SerializeField] private float epsCap = 15f;

    [SerializeField] private TextMeshProUGUI roundUI;
    [SerializeField] private TextMeshProUGUI enemyUI;
    [SerializeField] private GameObject winScreen;

    private int _currentRound = 1;
    private float _timeSinceLastSpawn;
    private int _enemiesAlive;
    private int _enemiesLeftToSpawn;
    private float _eps;
    private bool _isSpawning = false;
    
    public static UnityEvent OnEnemyKill = new UnityEvent();

    private void Awake()
    {
        OnEnemyKill.AddListener(EnemyKilled);
    }
    
    private void Start()
    {
        winScreen.SetActive(false);
        StartCoroutine(StartRound());
    }
    private IEnumerator StartRound()
     {
         roundUI.text = _currentRound.ToString();
         if (_currentRound == 1)
         {
             yield return new WaitForSeconds(timeBetweenRounds * 2);
         }
         else
         {
             yield return new WaitForSeconds(timeBetweenRounds);
         }
         _isSpawning = true;
         _enemiesLeftToSpawn = EnemiesPerRound();
         enemyUI.text = _enemiesLeftToSpawn.ToString();
         _eps = EnemiesPerSecond();
     }

    private void EndRound()
    {
        _isSpawning = false;
        _timeSinceLastSpawn = 0f;
        if (_currentRound + 1 > 20)
        {
            winScreen.SetActive(true);
            Time.timeScale = 0f;
            MenuUIManager.IsDead = true;
        }
        _currentRound++;
        StartCoroutine(StartRound());
    }

     private int EnemiesPerRound()
     {
         return Mathf.RoundToInt(baseEnemies * Mathf.Pow(_currentRound, difficulty));
     }
     
     private float EnemiesPerSecond()
     {
         return Mathf.Clamp(enemiesPerSec * Mathf.Pow(_currentRound, difficulty), 0f, epsCap);
     }

    void Update()
    {
        if (!_isSpawning)
        {
            return;
        }
        _timeSinceLastSpawn += Time.deltaTime;
        if (_timeSinceLastSpawn >= (1f / _eps) && _enemiesLeftToSpawn > 0)
        {
            SpawnEnemy();
            _enemiesLeftToSpawn--;
            _enemiesAlive++;
            _timeSinceLastSpawn = 0f;
        }

        if (_enemiesAlive == 0 && _enemiesLeftToSpawn == 0)
        {
            EndRound();
        }
    }

    private void SpawnEnemy()
    {
        GameObject prefabToSpawn;
        int index;
        if (_currentRound < 2)
        {
            index = 0; // Only Basic Enemy
        }
        else if (_currentRound <= 4)
        {
            index = Random.Range(0, 4); // 1:4 chance to spawn Tank
        }
        else if (_currentRound <= 7) 
        {
            index = Random.Range(0, 6); // 1:6 Speed, 1:3 Tank, 1:2 Basic
        }
        else if (_currentRound <= 10)
        {
            index = Random.Range(0, 7); // 2:7 Speed, 2:7 Tank, 3:7 Basic 
        }
        else
        {
            index = Random.Range(0, enemyPrefabs.Length); // 1:3 Chance for all types
        }
        prefabToSpawn = enemyPrefabs[index];
        Instantiate(prefabToSpawn, LevelManager.main.startPoint.position,  Quaternion.identity);
    }

    private void EnemyKilled()
    {
        _enemiesAlive--;
    }
}
