using UnityEngine;
using BNG;
using System;

public class MoveableTargets : MonoBehaviour {
	public AccuracyTestManager ATM = null;
	public int orientation = 0; // 0 for floor, 1 for right wall, 2 for ceiling, 3 for left wall
	
	public float rotationSpeed = 100.0f;
	public float rotationOffsetX = 0f;
	public float rotationOffsetY = 0f;
	public float rotationOffsetZ = 0f;
	public bool useDefaultRotationOffsets = true;
	public bool rotationDEBUG = false;

	public float rightDistance = 22;
	// public bool movementDEBUG = false;
	// public bool sOL = true;
	// public bool sU = false;
	// public float dS = 0;
	// public float tT = 10;
	// public float mD1 = 0;
	// public float sT = 0;

	// Rotation variables
	private bool isFallen;
	private bool rotating = false;
	private int rotateDirection;
	private int riseDirection;
	private int fallDirection;

	private Quaternion fallenOrientation;
	private Quaternion uprightOrientation;
	private Vector3 baseOffset;
	private Vector3 rotationPoint;

	// Movement variables
	private bool isMoving;
	private bool movementTriggered;
	
	private Vector3 moveLeft;
	private Vector3 moveRight;
	private Vector3 leftPosition;
	private Vector3 rightPosition;
	
	// Get set in TriggerMove
	private Vector3 targetPosition;

	private Vector3 movementDirection;

	private float movementSpeed;
	private float movementSpeed1;
	private float movementSpeed2;
	private bool singleMove;
	private float startMoving1;
	private float stopMoving1;
	private float startMoving2;
	private float stopMoving2;

	private int movementStep;

	private bool hasStarted = false;
	// private bool testing = false;

	private void Start() {
		if(hasStarted) return; 
		hasStarted = true;
		rotating = rotationDEBUG;	
		movementTriggered = false;
		
		// Rotation mechanics
		isFallen = false;

		if(orientation < 0 || orientation > 3) 
			Debug.LogWarning("Unknown target orientation! Defaulting to ground.");

		if(useDefaultRotationOffsets) {
			switch(orientation) {
				case 0:
					baseOffset = new Vector3(0f, -1.1f, 0f);
					break;
				case 1:
					baseOffset = new Vector3(0f, 0f, 1.1f);
					break;
				case 2:
					baseOffset = new Vector3(0f, 1.1f, 0f);
					break;
				case 3:
					baseOffset = new Vector3(0f, 0f, -1.1f);
					break;
				default:
					baseOffset = new Vector3(0f, -1f, 0f);
					break;
			}
		} else {
			baseOffset = new Vector3(rotationOffsetX, rotationOffsetY, rotationOffsetZ);
		}

		rotationPoint = transform.position + baseOffset;

		foreach (Transform child in transform) {
			child.localPosition -= transform.InverseTransformVector(baseOffset);
		}

		transform.position = rotationPoint;

		uprightOrientation = Quaternion.Euler(transform.rotation.eulerAngles.x, transform.rotation.eulerAngles.y, transform.rotation.eulerAngles.z);
		switch(orientation) {
			case 0: 
				fallenOrientation = Quaternion.Euler(transform.rotation.eulerAngles.x + 90f, transform.rotation.eulerAngles.y, transform.rotation.eulerAngles.z);
				riseDirection = -1;
				fallDirection = 1;
				break;
			case 1:
				fallenOrientation = Quaternion.Euler(transform.rotation.eulerAngles.x, transform.rotation.eulerAngles.y + 90f, transform.rotation.eulerAngles.z);
				riseDirection = -1;
				fallDirection = 1;
				break;
			case 2: 
				fallenOrientation = Quaternion.Euler(transform.rotation.eulerAngles.x - 90f, transform.rotation.eulerAngles.y, transform.rotation.eulerAngles.z);
				riseDirection = 1;
				fallDirection = -1;
				break;
			case 3:
				fallenOrientation = Quaternion.Euler(transform.rotation.eulerAngles.x, transform.rotation.eulerAngles.y - 90f, transform.rotation.eulerAngles.z);
				riseDirection = 1;
				fallDirection = -1;
				break;
			default:
				Debug.LogWarning("Unknown target orientation! Defaulting to ground.");
				fallenOrientation = Quaternion.Euler(transform.rotation.eulerAngles.x + 90f, transform.rotation.eulerAngles.y, transform.rotation.eulerAngles.z);
				riseDirection = -1;
				fallDirection = 1;
				break;
		}

		fallDirection = riseDirection * -1;

		transform.rotation = uprightOrientation;
		
		rotateDirection = fallDirection;

		// Movement mechanics
		isMoving = false;

		switch(orientation) {
			case 0: 
				moveLeft = new Vector3(0, 0, -1);
				break;
			case 1:
				moveLeft = new Vector3(0, -1, 0);
				break;
			case 2:
				moveLeft = new Vector3(0, 0, 1);
				break;
			case 3:
				moveLeft = new Vector3(0, 1, 0);
				break;
			default:
				moveLeft = new Vector3(0, 0, -1);
				break;
		}
		moveRight = moveLeft * -1;

		leftPosition = transform.position;
		rightPosition = leftPosition + moveRight * rightDistance;

		// if(movementDEBUG) TriggerMove(sOL, sU, dS, tT, mD1, sT);
		// Debug.Log("Starting target!");
	}

	// Rotation mechanics

	public void HitTarget(int damageAmount) {
		if(!isFallen && !rotating) {
			TriggerFall();
			if(ATM != null) ATM.AddPoints(damageAmount);
		}
	}

	public void TriggerFall() {
		// If we have already fallen or are currently rotating, we don't want to rotate again
		// Debug.Log("Triggering fall");
		if(isFallen || rotating) return;
		rotating = true;
		rotateDirection = fallDirection;
	}

	public void TriggerStandUp() {
		// If we have already risen or are currently rotating, we don't want to rotate again
		if(!isFallen || rotating) return;
		rotating = true;
		rotateDirection = riseDirection;
	}

	public void ResetUpright() {
		rotating = false; // stop any rotation if it's occurring
		isFallen = false;
		transform.rotation = uprightOrientation;
	}

	public void ResetFallen() {
		rotating = false;
		isFallen = true;
		transform.rotation = fallenOrientation;
	}

	// Movement Mechanics
	public void TriggerMove(bool startOnLeft, bool startUpright, float delayStart, float totalTime, float movementDist1 = 0, float stillTime = 0) {
		if(!hasStarted) Start();
		
		if(startOnLeft) {
			transform.position = leftPosition;
			movementDirection = moveRight;
		} else {
			transform.position = rightPosition;
			movementDirection = moveLeft;
		}

		movementStep = 0;
		float curTime = (float) Time.timeAsDouble;
		startMoving1 = curTime + delayStart;

		if(startUpright) {
			ResetUpright();
			singleMove = true;

			movementSpeed1 = rightDistance / (totalTime - delayStart);
			
			stopMoving1 = startMoving1 + totalTime;
			stopMoving2 = stopMoving1;
			targetPosition = startOnLeft ? rightPosition : leftPosition;
		} else {
			ResetFallen();
			singleMove = false;
			
			float travelTime = (totalTime - delayStart - stillTime) / 2;
			movementSpeed1 = movementDist1 / travelTime;
			movementSpeed2 = (rightDistance - movementDist1) / travelTime;

			stopMoving1 = startMoving1 + travelTime;
			startMoving2 = stopMoving1 + stillTime;
			stopMoving2 = curTime + totalTime;
			targetPosition = startOnLeft ? leftPosition + movementDist1 * moveRight : rightPosition + movementDist1 * moveLeft;
		}

		movementSpeed = movementSpeed1;

		movementTriggered = true;
		// testing = true;
		// if(movementTriggered) Debug.Log("movement Triggered!!!");
	}

	private void Update() {
		// Reset localRotation to prevent error buildup (breaks for floor targets for some reason)
		// This happens naturally over time for some reason, don't know why
		if(orientation != 0) {
			foreach (Transform child in transform) {
				child.localRotation = Quaternion.identity;
			}
		}

		float curTime = (float) Time.timeAsDouble;

		// if(testing) Debug.Log($"{singleMove} {movementTriggered} {movementStep} {curTime} {startMoving1}");
		if(!singleMove) {
			if(movementTriggered && movementStep < 4) {
				if(curTime >= stopMoving2) {
					isMoving = false;
					movementStep = 4;
					transform.position = targetPosition;
				} else if(curTime >= startMoving2 && movementStep < 3) {
					isMoving = true;
					movementStep = 3;
					TriggerFall();
				} else if(curTime >= stopMoving1 && movementStep < 2) {
					isMoving = false;
					transform.position = targetPosition;

					targetPosition = (movementDirection == moveLeft ? leftPosition : rightPosition);
					movementSpeed = movementSpeed2;
					TriggerStandUp();
					movementStep = 2;
				} else if(curTime >= startMoving1 && movementStep < 1) {
					isMoving = true;
					movementStep = 1;
				}
			}
		} else {
			if(movementTriggered && movementStep < 2) {				
				if(curTime >= stopMoving1 && movementStep < 2) {
					isMoving = false;
					transform.position = targetPosition;
					movementStep = 2;
				} else if(curTime >= startMoving1 && movementStep < 1) {
					isMoving = true;
					movementStep = 1;
				}
			}
		}

		if(isMoving) {
			transform.Translate(movementDirection * movementSpeed * Time.deltaTime, Space.World);
		}

		if(rotating) {
			Quaternion targetRotation = rotateDirection == fallDirection ? fallenOrientation : uprightOrientation;
			transform.rotation = Quaternion.RotateTowards(transform.rotation, targetRotation, rotationSpeed * Time.deltaTime);

			// Once it's close enough, lock it in the correct position. 
			if(Quaternion.Angle(transform.rotation, targetRotation) < 0.1f) {

				transform.rotation = targetRotation;
				isFallen = rotateDirection == fallDirection;

				if(rotationDEBUG) {
					rotating = true;
					rotateDirection *= -1;
					return;
				}

				rotating = false;
			}
		}
	}
}
