using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class QuadRenderer : Turtle {
	public Sentence sentence;

	public void Start() {
		Init();
		DrawTree();
	}
	protected override string getSentence() {
		return sentence.sentence;
	}
}
