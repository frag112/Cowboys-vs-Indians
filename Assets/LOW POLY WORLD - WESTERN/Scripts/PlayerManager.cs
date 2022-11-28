using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerManager : MonoBehaviour
{
    [SerializeField] private UnitsLauncher _spawnBase;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (!Input.GetMouseButtonDown(0)) return;

        // var point = Camera.main.ScreenPointToRay(Input.mousePosition);
        //  var targets = Physics.SphereCastAll(point, 3f);

        //  if (targets.Length == 0) return;

        //  var target = targets[0];
        //  _spawnBase.SpawnUnit(target.collider.gameObject);
        _spawnBase.SpawnUnit();
    }
}
