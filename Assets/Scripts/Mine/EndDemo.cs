using UnityEngine;
using UnityEngine.UI; // We need to edit text components
using UnityEngine.EventSystems; // And determine if our mouse is hovering
using UnityEngine.SceneManagement; // And have access to switching scenes.
using System.Collections;

public class EndDemo : MonoBehaviour
{

    #region variables

    private bool hasEntered = false;

    private float timer;

    public GameObject gameOver;
    #endregion

    void Start()
    {
        AudioListener.pause = false;
    }

    void Update()
    {
        timer += Time.deltaTime;

        if (hasEntered == false)
        {
            //do nothing
        }

        else if (hasEntered == true)
        {
            //SceneManager.LoadScene(SceneToLoad);
        }

        if (timer > .1f)
        {
            Cursor.lockState = CursorLockMode.Locked;
            Cursor.visible = false;
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.tag == "Player")
        {
            InSafety();
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.tag == "Player")
        {
            NotInSafety();
        }
    }

    void InSafety()
    {
        hasEntered = true;
        Destroy(Instantiate(gameOver), .1f);
    }

    void NotInSafety()
    {
        AudioSource[] FireAudio = GameObject.Find("Fire Audio").GetComponents<AudioSource>();
        hasEntered = false;
        //place play audio here
        FireAudio[0].Play();
    }
}
