using System.Collections;
using System.Collections.Generic;
#if UNITY_EDITOR
using UnityEditor;
#endif
using UnityEngine;

public class SerializeMaze : MonoBehaviour {
#if UNITY_EDITOR
    [CustomEditor(typeof(SerializeMaze))]
    class SerializeMazeEditor : Editor
    {
        SerializeMaze obj;
 
        void OnSceneGUI()
        {
            obj = (SerializeMaze)target;
        }
 
        public override void OnInspectorGUI()
        {
            base.OnInspectorGUI();
 
            if (GUILayout.Button("Build"))
            {
                if (obj)
                {
					Transform transform = obj.gameObject.transform;
					int childCount = transform.childCount;
					GameObject child;
					SerializeMesh childsm;
					for(int i = 0; i < childCount; ++i) {
						child = transform.GetChild(i).gameObject;
						childsm = child.AddComponent<SerializeMesh>();
						childsm.Serialize();
						childsm.Rebuild();
					}
                }
            }
        }
    }
#endif
}
