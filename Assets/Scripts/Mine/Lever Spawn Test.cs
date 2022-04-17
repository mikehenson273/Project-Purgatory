using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class LeverSpawnTest : MonoBehaviour
{
    public Transform[] spawnLocations;
    public GameObject[] leverPrefab;
    public GameObject[] leverClone;

    void spawnLever()
    {
        leverClone[0] = Instantiate(leverPrefab[0], spawnLocations[0].transform.position, Quaternion.Euler(0, 0, 0)) as GameObject;
    }

}
