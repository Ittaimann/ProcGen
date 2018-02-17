using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Generator : Turtle {
	public LSystem lSystem;
	protected override string getSentence() {
		return lSystem.sentence;
	}
}