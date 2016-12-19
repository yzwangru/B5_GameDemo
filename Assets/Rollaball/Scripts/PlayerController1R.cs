using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class PlayerController1R : MonoBehaviour {

	public float speed;
	public float jump;

	public Text countText1;
	public Text winText;
	public Text timer;
	public Text scoreText1;
	public Text announceText;

	public Button restartButton;
	public Button exitButton;

	private Rigidbody rb;
	public static int count;
	private float timeLeft;
	public static int score;
	private int wallscore;
	private int cubescore;
	private int collisionscore;

	// Use this for initialization
	void Start () {
		rb = GetComponent<Rigidbody> ();
		count = 0;
		countText1.text = "Count: " + count.ToString ();
		winText.text = "";
		timeLeft = 120.0f;
		SetTimerText ();
		score = 0;
		scoreText1.text = "Score: " + score.ToString ();
		wallscore = -1;
		cubescore = 2;
		collisionscore = -1;
		announceText.text = "";
		restartButton = restartButton.GetComponent<Button> ();
		exitButton = exitButton.GetComponent<Button> ();
	}
	
	// Update is called once per frame
	void Update () {
		if (rb.position.y < 0.0f) {
			if (timer.enabled == true) {
				GameOver ();
			}
		}

		timeLeft -= Time.deltaTime;
		SetTimerText ();
	}

	void FixedUpdate() {
		float moveHorizontal = Input.GetAxis ("Horizontal1");
		float moveVertical = Input.GetAxis ("Vertical1");

		Vector3 movement = new Vector3 (moveHorizontal, 0.0f, moveVertical);
		if (Input.GetKeyDown (KeyCode.Space) && rb.position.y < 0.6f) {
			movement.y = jump;
		}

		rb.AddForce (movement * speed);
	}

	void OnTriggerEnter(Collider other) {
		if (timer.enabled == true) {
			if (other.gameObject.CompareTag ("Pick Up")) {
				other.gameObject.SetActive (false);
				count = count + 1;
				SetCountScoreText ();
			}
		}
	}

	void OnCollisionEnter(Collision collision) {
		if (timer.enabled == true) {
			if (collision.gameObject.CompareTag ("Wall")) {
				score += wallscore;
			} else if (collision.gameObject.CompareTag ("Player")) {
				if (rb.position.y < collision.rigidbody.position.y) {
					score += collisionscore;
					announceText.text = "B wins the collision!";
				} else if (rb.position.y > collision.rigidbody.position.y) {
					announceText.text = "A wins the collision!";
				}
			}
			SetScoreText ();
		}
	}

	void SetCountScoreText() {
		countText1.text = "Count: " + count.ToString ();
		score += cubescore;
		SetScoreText ();
		if (count + PlayerController2.count >= 12) {
			GameOver ();
		}
	}

	void SetTimerText() {
		int minutes = Mathf.FloorToInt (timeLeft / 60f);
		int seconds = Mathf.FloorToInt (timeLeft - minutes * 60);
		string nicetime = string.Format ("{0:0}:{1:00}", minutes, seconds);
		if (timeLeft < 0.0f) {
			GameOver ();
		} else if (timeLeft < 30.0f) {
			timer.text = "Time almost up! TimeLeft: " + nicetime;
			timer.color = Color.red;
		} else {
			timer.text = "TimeLeft: " + nicetime;
		}
	}

	void SetScoreText() {
		scoreText1.text = "Score: " + score.ToString ();
	}

	void SetWinText() {
		if (score < PlayerController2.score || rb.position.y < 0.0f) {
			winText.text = "B wins the game!";
		} else if (score > PlayerController2.score) {
			winText.text = "A wins the game!";
		} else {
			winText.text = "Nobody wins!";
		}
	}

	void GameOver() {
		timer.text = "";
		announceText.text = "";
		SetWinText ();
		timer.enabled = false;
		announceText.enabled = false;
	}

	public void InStartLevel() {
		Application.LoadLevel (1);
	}

	public void InPressExit() {
		Application.LoadLevel (0);
	}
}
