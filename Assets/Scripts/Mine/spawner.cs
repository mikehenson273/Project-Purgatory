using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class spawner : MonoBehaviour
{
    public Transform[] spawnLocations;
    public GameObject[] leverPrefab;
    public GameObject[] leverClone;

    private void Start()
    {
        spawnLever();
    }

    void spawnLever()
    {
        int randomNumber = Random.Range(0, 6);

        if (randomNumber == 0)
        {
            leverClone[0] = Instantiate(leverPrefab[0], spawnLocations[0].transform.position, Quaternion.Euler(0, 0, 0)) as GameObject; //1
        }
        else if (randomNumber == 1)
        {
            leverClone[0] = Instantiate(leverPrefab[0], spawnLocations[1].transform.position, Quaternion.Euler(0, 0, 0)) as GameObject; //2
        }
        else if (randomNumber == 2)
        {
            leverClone[0] = Instantiate(leverPrefab[0], spawnLocations[2].transform.position, Quaternion.Euler(0, 0, 0)) as GameObject; //3
        }
        else if (randomNumber == 3)
        {
            leverClone[0] = Instantiate(leverPrefab[0], spawnLocations[3].transform.position, Quaternion.Euler(0, 180, 0)) as GameObject; //4
        }
        else if (randomNumber == 4)
        {
            leverClone[0] = Instantiate(leverPrefab[0], spawnLocations[4].transform.position, Quaternion.Euler(0, -90, 0)) as GameObject; //5
        }
        else if (randomNumber == 5)
        {
            leverClone[0] = Instantiate(leverPrefab[0], spawnLocations[5].transform.position, Quaternion.Euler(0, 0, 0)) as GameObject; //6
        }
        else
        {
            //error catch
        }
    }

}
