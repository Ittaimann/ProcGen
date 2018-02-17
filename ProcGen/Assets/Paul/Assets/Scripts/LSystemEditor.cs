using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

[CustomEditor(typeof(LSystem))]
public class LSystemEditor : Editor {
	LSystem lSystem;
	public override void OnInspectorGUI() {
		DrawDefaultInspector();

		lSystem = (LSystem) target;

		if(GUILayout.Button("Save Sentence")) {
			createSentence();
		}
	}
    public void createSentence() {
        Sentence asset = Sentence.CreateInstance<Sentence>();
		asset.sentence = lSystem.sentence;

        AssetDatabase.CreateAsset(asset, "Assets/" + lSystem.sentenceName + ".asset");
        AssetDatabase.SaveAssets();

        EditorUtility.FocusProjectWindow();

        Selection.activeObject = asset;
    }
}
