using UnityEngine;
using UnityEngine.UI; // We need to edit text components
using UnityEngine.EventSystems; // And determine if our mouse is hovering
using UnityEngine.SceneManagement; // And have access to switching scenes.
using System.Collections;

public class SkipCredits : MonoBehaviour
{
    public GameObject SkipHint;
    public GameObject HiddenHint;

    public int timeCount;
    public int timeCount1;

    private float time = 3;

    public bool Hint = false;

    public string SceneToLoad;

    // Use this for initialization
    void Start()
    {
        Time.timeScale = 1f;
        StartCoroutine("TimeUp1");
    }

    void FixedUpdate()
    {
        if (Input.anyKeyDown && time > timeCount && !Input.GetKeyDown("e") && !Hint)
        {
            SkipHint.SetActive(true);
            StartCoroutine("TimeUp");
            Hint = true;
        }

        else if (Input.anyKeyDown && !Input.GetKeyDown("e") && Hint && timeCount > .0001f)
        {
            HiddenHint.SetActive(true);
        }

        else if (time <= timeCount)
        {
            SkipHint.SetActive(false);
            HiddenHint.SetActive(false);
            Hint = false;
            StopCoroutine("TimeUp");
            timeCount = 0;
        }
        
        else if (Input.GetKeyDown("e"))
        {
            SceneManager.LoadScene(SceneToLoad);
        }

        else if (timeCount1 == 47)
        {
            SceneManager.LoadScene(SceneToLoad);
        }
    }

    IEnumerator TimeUp()
    {
        while (true)
        {
            yield return new WaitForSeconds(1);
            timeCount++;
        }
    }

    IEnumerator TimeUp1()
    {
        while (true)
        {
            yield return new WaitForSeconds(1);
            timeCount1++;
        }
    }
}
