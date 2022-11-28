using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameStage : MonoBehaviour
{
    public float StageTime { get; private set; }
    private void Update()
    {
        StageTime += Time.deltaTime;
    }
}
