using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class EntranceController : MonoBehaviour {

	public Canvas exitMenu;
	public Button playButton;
	public Button exitButton;

	// Use this for initialization
	void Start () {
		exitMenu = exitMenu.GetComponent<Canvas> ();
		playButton = playButton.GetComponent<Button> ();
		exitButton = exitButton.GetComponent<Button> ();
		exitMenu.enabled = false;
	}
	
	public void PressExit() {
		exitMenu.enabled = true;
		playButton.enabled = false;
		exitButton.enabled = false;
	}

	public void PressNo() {
		exitMenu.enabled = false;
		playButton.enabled = true;
		exitButton.enabled = true;
	}

	public void StartLevel() {
		Application.LoadLevel (1);
	}

	public void ExitGame() {
		Application.Quit ();
	}
}
