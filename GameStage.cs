using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameStage : MonoBehaviour
{
    [SerializeField] GameObject _gameOverText;
    private List<Entity> _actors = new List<Entity>();
    private void Start()
    {


    }
    private void CollectingNonMovingActors()
    {
        var allActors = FindObjectsOfType<Entity>();
        foreach (var actor in allActors)
        {
            if (actor.gameObject.GetComponent<Rigidbody>() == null)
            {
                _actors.Add(actor);
            }
        }
    }
    private void Update()
    {
        CollectingNonMovingActors();
        if (_actors.Count <=0)
        {
            _gameOverText.SetActive(true);
        }
        _actors.Clear();
    }
}
