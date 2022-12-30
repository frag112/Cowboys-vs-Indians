using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(GameStage))]
public class EnemyLauncher : MonoBehaviour
{
    [SerializeField] private float _startInSeconds, _nextLaunchMinSeconds, _nextLaunchMaxSeconds;
    private float _timeToNextLaunch;
    [SerializeField] private int _durationInSeconds;
    //private GameStage _stage;
    [SerializeField] GameObject _enemy; 
    [SerializeField] Transform _spawnPoint;


    private void Start()
    {
        //_stage = GetComponent<GameStage>();
        StartCoroutine(Spawn());
    }
    private IEnumerator Spawn()
    {
        for (int i = 0; i < _durationInSeconds; i++)
        {
            _timeToNextLaunch = Random.Range(_nextLaunchMinSeconds, _nextLaunchMaxSeconds);
            yield return new WaitForSeconds(_timeToNextLaunch);
            if (_spawnPoint)
            {
                Instantiate(_enemy, _spawnPoint.transform.position, Quaternion.identity);
            }
        }


    }

}
