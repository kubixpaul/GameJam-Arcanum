using System;
using UnityEngine;

public class Spell : MonoBehaviour
{
    [SerializeField] private float spellSpeed = 5f;
    
    private Transform _target;
    private int _spellDamage = 1;
    
    [SerializeField] private Rigidbody2D rb;

    private void FixedUpdate()
    {
        if(!_target)  return;
        Vector2 direction = (_target.position - transform.position).normalized;

        rb.linearVelocity = direction * spellSpeed;
    }

    public void SetTarget(Transform target, int damage)
    {
        _target = target;
        _spellDamage = damage;
    }
    

    private void OnCollisionEnter2D(Collision2D other)
    {
        other.gameObject.GetComponent<EnemyAttributes>().TakeDamage(_spellDamage);
        Destroy(gameObject);
    }
}
