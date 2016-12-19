﻿using UnityEngine;
using System;
using System.Collections;

public class _IKController : MonoBehaviour {

	protected Animator animator;

	public bool ikActive = false;
	public GameObject ball;
	//public Transform rightHandObj = null;
	//public Transform lookObj = null;

	void Start () 
	{
		animator = GetComponent<Animator>();
	}

	//a callback for calculating IK
	void OnAnimatorIK()
	{
		if(animator) {

			//if the IK is active, set the position and rotation directly to the goal. 
			if(ikActive) {

				// Set the look target position, if one has been assigned
				//if(lookObj != null) {
				//	animator.SetLookAtWeight(1);
				//	animator.SetLookAtPosition(lookObj.position);
				//}    

				// Set the right hand target position and rotation, if one has been assigned
				//if(ball != null) {
					animator.SetIKPositionWeight(AvatarIKGoal.RightHand,1);
					animator.SetIKRotationWeight(AvatarIKGoal.RightHand,1);  
					animator.SetIKPosition(AvatarIKGoal.RightHand,ball.transform.Find("GrabPoint").transform.position);
					animator.SetIKRotation(AvatarIKGoal.RightHand,ball.transform.Find("GrabPoint").transform.rotation);
				//}        

			}

			//if the IK is not active, set the position and rotation of the hand and head back to the original position
			else {          
				animator.SetIKPositionWeight(AvatarIKGoal.RightHand,0);
				animator.SetIKRotationWeight(AvatarIKGoal.RightHand,0); 
				animator.SetLookAtWeight(0);
			}
		}
	}    
}
