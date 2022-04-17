using UnityEngine;
using UnityEngine.UI; // We need to edit text components
using UnityEngine.EventSystems; // And determine if our mouse is hovering
using UnityEngine.SceneManagement; // And have access to switching scenes.
using System.Collections;

public class LoadCredits : MonoBehaviour
{
    #region variables

    private bool hasEntered = false;

    public string SceneToLoad;
    
    #endregion

    void Start()
    {

    }

    void Update()
    {
        if (hasEntered == false)
        {
            //do nothing
        }

        else if (hasEntered == true)
        {
            SceneManager.LoadScene(SceneToLoad);
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
    }

    void NotInSafety()
    {
        hasEntered = false;
    }
}