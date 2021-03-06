using UnityEngine;
using System.Collections;

using ManageHealth;

public class MenuController_Paused : MonoBehaviour 
{
    public GameObject HealthManagement;

    public bool isPaused; // Bool which determines wether or not our game has been paused.
	public string buttonToTogglePause; // A string which we declare that will act as the button that will pause the game. Make sure this button is setup in your input settings.
	public string canvasIndex; // A string that declares the currently rendered canvas.
	public string startingIndex; // The string that states which canvas will be rendered first when our game is paused.
	public bool useCursorLock;

	public enum MenuTypes
	{
		unity3D, unity2D
	}

	public MenuTypes menuType;

	void Start() 
	{
		if (menuType == MenuTypes.unity2D)
        {

		}

		canvasIndex = startingIndex; // At start, we set the currently rendered canvas to equal the starting canvas.
	}

	void Update()
	{
		if (Input.GetButtonDown (buttonToTogglePause) && isPaused == false)
        {
			isPaused = true;
			CheckPause ();
		}

		///
		// This update method is basically saying that if we press the button that we defined earlier on our keyboard, then we set the isPaused variable equal to the opposite
		// of whatever it just was. Then, we set currently rendered canvas equal to the starting canvas, so that if we have multiple canvases, and we arent on the starting
		// canvas when we un-pause, then we will be when we re-pause the game. We then check the current status of our pase by calling the CheckPause function below.
		///
	}

	public void CheckPause()
	{
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;

        if (isPaused)
        {
            Time.timeScale = 0.0000000001f;
            HealthManagement.GetComponent<HealthManagementSystem>().healTime = 0f;
            HealthManagement.GetComponent<HealthManagementSystem>().healRate = 1f;

            AudioListener.pause = true;

            if (useCursorLock)
            {
                Cursor.lockState = CursorLockMode.None;
				Cursor.visible = true;
			}
		}

		if (!isPaused)
        {
            HealthManagement.GetComponent<HealthManagementSystem>().healRate = .025f;
            Time.timeScale = 1;
			canvasIndex = startingIndex;

            AudioListener.pause = false;
        }

		///
		// This function determines how the game acts when it is and is not paused. Here, we have slowed time down by setting Time.timeScale = .0001, which basically means that
		// 100000 (1.15 days) have to pass in real life before one second passes in the game. It is worth noting that setting timeScale = 0 can create issues in your game,
		// so it is best to set it to an extremely low number instead of just 0.
		///
	}
}
