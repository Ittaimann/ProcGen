using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LSystemDriver : MonoBehaviour {
	LSystem lSystem;
	Generator turtle;
	
	void Start() {
		lSystem = GameObject.FindWithTag("LSystem").GetComponent<LSystem>();
		turtle = GameObject.FindGameObjectWithTag("Generator").GetComponent<Generator>();
	}
	// Update is called once per frame
	void Update () {
		if(Input.GetMouseButtonDown(0)) {
			lSystem.Generate();	
			turtle.DrawTree();
		}
	}
}
