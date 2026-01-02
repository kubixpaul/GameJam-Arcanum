using System.Numerics;
using Unity.VisualScripting;
using UnityEngine;
using Vector2 = UnityEngine.Vector2;

public class EnemyMovement : MonoBehaviour
{

    [SerializeField] private Rigidbody2D rb;
    

    [SerializeField] private float moveSpeed = 2f;
    private Transform _target;
    private EnemyAttributes _enemy;
    private int _pathIndex = 0;

    private float _baseSpeed;
    private void Start()
    {
        _baseSpeed = moveSpeed;
        _target = LevelManager.main.path[_pathIndex];
    }

    private void Awake()
    {
        _enemy = GetComponent<EnemyAttributes>();
    }
    
    private void Update()
    {
        if (Vector2.Distance(_target.position, transform.position) < 0.1)
        {
            _pathIndex++;
            if (_pathIndex >= LevelManager.main.path.Length)
            {
                EnemySpawner.OnEnemyKill.Invoke();
                Destroy(gameObject);
                LevelManager.main.PlayerTakeDamage(_enemy.enemyDamage);
                return;
            }
            else
            {
                _target = LevelManager.main.path[_pathIndex];
            }
        }
    }

    private void FixedUpdate()
    {
        Vector2 direction = (_target.position - transform.position).normalized;
        rb.linearVelocity = direction * moveSpeed;
    }

    public void UpdateSpeed(float newSpeed)
    {
        moveSpeed = newSpeed;
    }

    public void ResetSpeed()
    {
        moveSpeed = _baseSpeed;
    }
}