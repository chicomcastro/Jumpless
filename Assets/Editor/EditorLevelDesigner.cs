using UnityEngine;
using UnityEditor;

[CustomEditor(typeof(LevelGenerator))]
public class EditorLevelDesigner : Editor {

	public override void OnInspectorGUI()
	{
		DrawDefaultInspector();

		LevelGenerator myScript = (LevelGenerator) target;
		GUILayout.Space(10);

		GUILayout.BeginHorizontal();

		if (GUILayout.Button("Generate level"))
		{
			myScript.GenerateLevel();
		}
		if (GUILayout.Button("Delete level"))
		{
			myScript.DeleteLevel();
		}
		GUILayout.EndHorizontal();
	}

}
