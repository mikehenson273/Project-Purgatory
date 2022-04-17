using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class CollisionDeactivate : MonoBehaviour
{
    #region variables

    private bool hasEntered = false;

    public GameObject FakeWall;

    public int timeCount;

    public int time = 2;


    #endregion

    void Start()
    {

    }

    void Update()
    {
        if (hasEntered == false)
        {
            FakeWall.GetComponent<MeshRenderer>().enabled = true;
        }

        else if (hasEntered == true)
        {
            FakeWall.GetComponent<MeshRenderer>().enabled = false;

            StartCoroutine("TimeUp");

            if (time == timeCount)
            {
                StopCoroutine("TimeUp");
                FakeWall.GetComponent<BoxCollider>().enabled = true;

            }

            else if (time > timeCount)
            {
                FakeWall.GetComponent<BoxCollider>().enabled = false;
            }

            else if (time < timeCount)
            {
                StopCoroutine("TimeUp");
                timeCount = 0;
                FakeWall.GetComponent<MeshRenderer>().enabled = true;
                hasEntered = false;
            }
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.tag == "Player" && hasEntered == false || other.tag == "Enemy" && hasEntered == false && other.tag == "Player" 
            || other.tag == "Enemy" && hasEntered == false && other.tag == "Player" && other.tag == "Breadcrumb" 
            || other.tag == "Breadcrumb" && hasEntered == false && other.tag == "Player"
            || other.tag == "Breadcrumb" && hasEntered == false)
        {
            InSafety();
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.tag == "Player" && hasEntered == true || other.tag == "Enemy" && hasEntered == true && other.tag == "Player"
            || other.tag == "Enemy" && hasEntered == true && other.tag == "Player" && other.tag == "Breadcrumb"
            || other.tag == "Breadcrumb" && hasEntered == true && other.tag == "Player"
            || other.tag == "Breadcrumb" && hasEntered == true)
        {
            NotInSafety();
            timeCount = 0;
        }
    }

    void InSafety()
    {
        hasEntered = true;
        StartCoroutine("TimeUp");

        if (time == timeCount)
        {
            FakeWall.GetComponent<BoxCollider>().enabled = true;
        }

        else if (time > timeCount)
        {
            FakeWall.GetComponent<BoxCollider>().enabled = false;
        }
    }

    void NotInSafety()
    {
        hasEntered = false;
    }

    IEnumerator TimeUp()
    {
        while (true)
        {
            yield return new WaitForSeconds(1);
            timeCount++;
        }
    }
}
