using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class PlayerController2 : MonoBehaviour {

	public float speed;
	public float jump;

	public Text countText2;
	public Text winText;
	public Text announceText;
	public Text timer;
	public Text scoreText2;

	private Rigidbody rb;
	public static int count;
	public static int score;
	private int wallscore;
	private int cubescore;
	private int collisionscore;

	// Use this for initialization
	void Start () {
		rb = GetComponent<Rigidbody> ();
		countText2.text = "Count: 0";
		count = 0;
		winText.text = "";
		score = 0;
		scoreText2.text = "Score: " + score.ToString ();
		wallscore = -1;
		cubescore = 2;
		collisionscore = -1;
	}

	void Update() {
		if (rb.position.y < 0.0f) {
			if (timer.enabled == true) {
				GameOver ();
			}
		}
	}

	void FixedUpdate() {
		float moveHorizontal = Input.GetAxis ("Horizontal2");
		float moveVertical = Input.GetAxis ("Vertical2");

		Vector3 movement = new Vector3 (moveHorizontal, 0.0f, moveVertical);
		if (Input.GetKeyDown (KeyCode.RightShift) && rb.position.y < 0.6f) {
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
				}
			}
			SetScoreText ();
		}
	}

	void SetCountScoreText() {
		countText2.text = "Count: " + count.ToString ();
		score += cubescore;
		SetScoreText ();
		if (count + PlayerController1.count >= 12) {
			GameOver ();
		}
	}

	void SetWinText() {
		if (rb.position.y < 0.0f) {
			winText.text = "A wins the game!";
		} else if (score > PlayerController1.score) {
			winText.text = "B wins the game!";
		} else if (score < PlayerController1.score) {
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

	void SetScoreText() {
		scoreText2.text = "Score: " + score.ToString ();
	}
}
