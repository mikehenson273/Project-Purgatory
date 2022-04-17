using UnityEngine;
using UnityEngine.UI; // We need to edit text components
using UnityEngine.EventSystems; // And determine if our mouse is hovering
using UnityEngine.SceneManagement; // And have access to switching scenes.
using System.Collections;

//to replay.

public class DemoGameOver : MonoBehaviour
{
    public string MSG = "Would You Like\nTo Try Again?";
    public GUISkin skin;
    private float timer;
    private bool _replay;
    private string SceneToLoad;

    void Update()
    {
        if (!_replay)
        {
            //timer += Time.deltaTime;
            //if (timer > 1)
            //{
            //    _replay = true;
            //}
            _replay = true;
            Time.timeScale = 0.0000000001f;
        }
    }

    void OnGUI()
    {
        if (_replay)
        {
            Cursor.lockState = CursorLockMode.None;
            Cursor.visible = true;

            GUI.skin = skin;

            GUI.TextField(new Rect(Screen.width / 2 - 55, Screen.height / 2.5f, 100, 40), MSG, 200);

            if (GUI.Button(new Rect(Screen.width / 2 - 30, Screen.height / 2f, 50, 25), "YES"))
            {
                SceneManager.LoadScene("SampleScene");
            }

            else if (GUI.Button(new Rect(Screen.width / 2 - 30, Screen.height / 1.75f, 50, 25), "NO"))
            {
                Time.timeScale = 1f;
            }
        }
    }
}
