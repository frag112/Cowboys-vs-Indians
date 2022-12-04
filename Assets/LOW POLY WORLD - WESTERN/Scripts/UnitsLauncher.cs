using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UnitsLauncher : Entity
{
    [SerializeField] private UnitProgressBar _reloadBar;
    [SerializeField] private GameObject _spawnUnit;
    [SerializeField] private Transform _spawnLocation;
    
    public bool CanFire { get { return _reloadBar.Value >= _reloadBar.MaxValue; } }
    public void SpawnUnit()
    {
        if (!CanFire) return;
        
            Instantiate(_spawnUnit, _spawnLocation.position, Quaternion.identity);
        _reloadBar.Value = 0;

    }
    private void Update()
    {
        if (CanFire) return;
        _reloadBar.Value += Time.deltaTime * 4;
    }
}
