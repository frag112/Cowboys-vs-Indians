using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Targetable : MonoBehaviour
{
    [SerializeField] private UnitProgressBar _health;

    public void TakeDamage (float damage)
    {
        _health.Value -= damage;
        if (_health.Value > 0) return;
        Destroy(gameObject);
    }
}
