using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

[CustomEditor(typeof(MazeGenerator))]
public class MazeDriver : Editor {
	private MazeGenerator generator;
	public override void OnInspectorGUI() {
		DrawDefaultInspector();

		generator = (MazeGenerator) target;

		if(GUILayout.Button("Make Maze"))
			createSentence(generator.generate(), generator.sentenceName);
	}

    private void createSentence(string sentence, string name) {
        Sentence asset = Sentence.CreateInstance<Sentence>();
		asset.sentence = sentence;

        AssetDatabase.CreateAsset(asset, "Assets/Paul/Assets/Sentences/" + name + ".asset");
        AssetDatabase.SaveAssets();

        EditorUtility.FocusProjectWindow();

        Selection.activeObject = asset;
    }
}
