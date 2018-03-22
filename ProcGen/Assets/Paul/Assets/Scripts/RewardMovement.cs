using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RewardMovement : MonoBehaviour {
	public int increment;
	public float floatFreq;
	private float floatPos;
	private float totalTime;
	void Awake() {
		floatPos = 0f;
		totalTime = 0f;
	}
	void Update() {
		totalTime += Time.deltaTime;
		transform.Rotate(0f, 2 * Mathf.PI / increment, 0f);
		float oldPos = floatPos;
		floatPos = .25f * Mathf.Cos(Mathf.PI * floatFreq * totalTime);
		transform.Translate(0, floatPos - oldPos, 0);
	}
}
