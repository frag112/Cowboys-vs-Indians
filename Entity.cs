using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Entity : MonoBehaviour
{
    [SerializeField] private UnitProgressBar _healthBar;
    [SerializeField] private float _health;

    private void Start()
    {
        if (_healthBar)
        {
            _healthBar.MaxValue = _health;
            _healthBar.StartValue = _health;
        }
    }
    public void TakeDamage(float damage)
    {
        _health -= damage;
        if(_healthBar) _healthBar.Value -= damage;
        if (_health > 0) return;
       // if (_healthBar.Value > 0) return;
        Die();
    }

    protected virtual void Die()
    {
        Destroy(gameObject);
    }
}
