using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(GameStage))]
public class EnemyLauncher : MonoBehaviour
{
    [SerializeField] private float _startInSeconds, _durationInSeconds, _nextLaunchMinSeconds, _nextLaunchMaxSeconds;
    private float _timeToNextLaunch;
    private GameStage _stage;
    [SerializeField] GameObject _enemy; 
    [SerializeField] Transform _spawnPoint;


    private void Start()
    {
        _stage = GetComponent<GameStage>();
        CalculateNextLaunch();
    }
    private void Update() // move checks to courotine
    {
        if (_startInSeconds > _stage.StageTime) return;
        if (_startInSeconds + _durationInSeconds < _stage.StageTime) return;
        if (_timeToNextLaunch > 0)
        {
            _timeToNextLaunch -= Time.deltaTime;
            return;
        }
        if (_spawnPoint)
        {
            Instantiate(_enemy, _spawnPoint.transform.position, Quaternion.identity);
        }

        CalculateNextLaunch();
    }
    void CalculateNextLaunch()
    {
        _timeToNextLaunch = Random.Range(_nextLaunchMinSeconds, _nextLaunchMaxSeconds);
    }

}
