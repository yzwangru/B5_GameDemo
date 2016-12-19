using UnityEngine;
using System.Collections;

public class CameraController : MonoBehaviour {

	public GameObject player1, player2;

	private Vector3 offset;

	void Start() {
		offset = transform.position - (player1.transform.position + player2.transform.position) / 2;
	}

	// LateUpdate is called once per frame
	void LateUpdate() {
		transform.position = (player1.transform.position + player2.transform.position) / 2 + offset;
	}
}
