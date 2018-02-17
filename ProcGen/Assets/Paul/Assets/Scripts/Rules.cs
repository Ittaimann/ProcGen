using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

public class Rules : ScriptableObject {
    public List<Rule> rules;
    [MenuItem("Assets/Create/LRules")]
    public static void create() {
        Rules asset = Rules.CreateInstance<Rules>();

        AssetDatabase.CreateAsset(asset, "Assets/NewRules.asset");
        AssetDatabase.SaveAssets();

        EditorUtility.FocusProjectWindow();

        Selection.activeObject = asset;
    }
}