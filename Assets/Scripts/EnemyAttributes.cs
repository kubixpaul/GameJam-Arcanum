using UnityEngine;

public class EnemyAttributes : MonoBehaviour
{
    [SerializeField] private int enemyHitPoints = 2;
    [SerializeField] private int enemyArcana = 20;
    public int enemyDamage = 2;

    private bool _isKilled = false;
    public void TakeDamage(int damage)
    {
        enemyHitPoints -= damage;
        if (enemyHitPoints <= 0 && !_isKilled)
        {
            EnemySpawner.OnEnemyKill.Invoke();
            LevelManager.main.IncreaseArcana(enemyArcana);
            _isKilled = true;
            Destroy(gameObject);
        }
    }
    
    
}
