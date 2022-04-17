using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;


public class TimedDoor : MonoBehaviour
{
    public int time1;
    public int timeCount;

    bool doorOpened = false;

    public Transform enemyDoor;

    void Start()
    {
        RandomGenerate();
        StartCoroutine("TimeUp");
        OpenDoor();
    }

    void Update()
    {

        if (time1 == timeCount)
        {
            StopCoroutine("TimeUp");
            OpenDoor();
        }
    }

    void OpenDoor()
    {
        AudioSource[] doorAudio = GameObject.Find("doorAudioFiles").GetComponents<AudioSource>();
        if (time1 == timeCount && doorOpened == false)
        {
            //place door rotation here
            enemyDoor.transform.rotation = Quaternion.Euler(0, 90, 0);

            //place audio of creature growl here
            doorAudio[0].Stop();
            doorAudio[1].Play();

            doorOpened = true;
        }

        else if (doorOpened == false)
        {
            //place door banging audio clip here
            doorAudio[0].Play();            
        }
    }

    public void RandomGenerate()
    {
        time1 = Random.Range(1, 11) + 5;
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