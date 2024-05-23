using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using UnityEditorInternal;
using System.Reflection;

namespace Shaper2D
{
	[CustomEditor(typeof(Shaper2D))]
	public class Shaper2DEditor : Editor
	{

		private Shaper2D script;

		[MenuItem("GameObject/2D Object/Shaper 2D")]
		static void Create()
		{
			GameObject go = new GameObject();
			go.AddComponent<Shaper2D>();
			go.name = "Shaper 2D";
			go.transform.position = new Vector3(SceneView.lastActiveSceneView.pivot.x,
				SceneView.lastActiveSceneView.pivot.y, 0f);
			if (Selection.activeGameObject != null)
			{
				go.transform.parent = Selection.activeGameObject.transform;
			}

			Selection.activeGameObject = go;
		}

		void Awake()
		{
			script = (Shaper2D)target;
		}

		public override void OnInspectorGUI()
		{
			DrawDefaultInspector();

			EditorGUILayout.BeginHorizontal();
			GUILayout.Box(new GUIContent("Triangle count: " + script.TriangleCount.ToString()), EditorStyles.helpBox);
			EditorGUILayout.EndHorizontal();

			Shaper2D.ColType colliderType = (Shaper2D.ColType)EditorGUILayout.EnumPopup(
				new GUIContent("Auto collider 2D",
					"Automatically create a collider. Set to \"None\" if you want to create your collider by hand"),
				script.colliderType);
			if (colliderType != script.colliderType)
			{
				Undo.RecordObject(script, "Change collider type");
				script.colliderType = colliderType;
				AddCollider(colliderType);
				script.UpdateCollider();
				EditorUtility.SetDirty(script);
				EditorGUIUtility.ExitGUI();
			}

			EditorGUILayout.BeginHorizontal();
			EditorGUILayout.PrefixLabel(new GUIContent("Export mesh",
				"Save mesh as a separate prefab in project root"));
			if (GUILayout.Button("Export mesh asset"))
			{
				ExportMesh();
			}

			EditorGUILayout.EndHorizontal();

			GUILayout.Space(10);

			int[] layerIDs = GetSortingLayerUniqueIDs();
			string[] layerNames = GetSortingLayerNames();
			int selected = -1;
			for (int i = 0; i < layerIDs.Length; i++)
			{
				if (layerIDs[i] == script.sortingLayer)
				{
					selected = i;
				}
			}

			if (selected == -1)
			{
				for (int i = 0; i < layerIDs.Length; i++)
				{
					if (layerIDs[i] == 0)
					{
						selected = i;
					}
				}
			}

			EditorGUI.BeginChangeCheck();
			GUIContent[] dropdown = new GUIContent[layerNames.Length + 2];
			for (int i = 0; i < layerNames.Length; i++)
			{
				dropdown[i] = new GUIContent(layerNames[i]);
			}

			dropdown[layerNames.Length] = new GUIContent();
			dropdown[layerNames.Length + 1] = new GUIContent("Add Sorting Layer...");
			selected = EditorGUILayout.Popup(new GUIContent("Sorting Layer", "Name of the Renderer's sorting layer"),
				selected, dropdown);
			if (EditorGUI.EndChangeCheck())
			{
				Undo.RecordObject(script, "Change sorting layer");
				if (selected == layerNames.Length + 1)
				{
					EditorApplication.ExecuteMenuItem("Edit/Project Settings/Tags and Layers");
				}
				else
				{
					script.sortingLayer = layerIDs[selected];
				}

				EditorUtility.SetDirty(script);
			}

			EditorGUI.BeginChangeCheck();
			var order = EditorGUILayout.IntField("Order in Layer", script.orderInLayer);
			if (EditorGUI.EndChangeCheck())
			{
				Undo.RecordObject(script, "Change order in layer");
				script.orderInLayer = order;
				EditorUtility.SetDirty(script);
			}
		}

		void AddCollider(Shaper2D.ColType type)
		{
			var ok = true;
			if (script.GetComponent<Collider2D>() != null)
			{
				ok = EditorUtility.DisplayDialog("Warning",
					"This will remove existing Collider2D with all its settings.", "Remove", "Cancel");
				if (ok)
				{
					while (script.GetComponent<Collider2D>() != null)
					{
						DestroyImmediate(script.GetComponent<Collider2D>());
					}

					while (script.GetComponent<PlatformEffector2D>() != null)
					{
						DestroyImmediate(script.GetComponent<PlatformEffector2D>());
					}
				}
			}

			if (ok && type != Shaper2D.ColType.None)
			{
				if (type == Shaper2D.ColType.Polygon)
				{
					script.gameObject.AddComponent<PolygonCollider2D>();
				}
				else if (type == Shaper2D.ColType.Edge)
				{
					script.gameObject.AddComponent<EdgeCollider2D>();
				}

				InternalEditorUtility.SetIsInspectorExpanded(script.GetComponent<PolygonCollider2D>(), false);
				InternalEditorUtility.SetIsInspectorExpanded(script.GetComponent<EdgeCollider2D>(), false);
				script.UpdateCollider();
				GUIUtility.ExitGUI();
			}
		}

		private void ExportMesh()
		{
			Mesh mesh = script.GetMesh();
			if (System.IO.File.Exists("Assets/" + mesh.name.ToString() + ".asset") && !EditorUtility.DisplayDialog(
					"Warning", "Asset with this name already exists in root of your project.", "Overwrite", "Cancel"))
			{
				return;
			}

			AssetDatabase.CreateAsset(Instantiate(mesh), $"Assets/{mesh.name}.asset");
			AssetDatabase.SaveAssets();
		}

		private int[] GetSortingLayerUniqueIDs()
		{
			var internalEditorUtilityType = typeof(InternalEditorUtility);
			var sortingLayerUniqueIDsProperty = internalEditorUtilityType.GetProperty("sortingLayerUniqueIDs",
				BindingFlags.Static | BindingFlags.NonPublic);
			return (int[])sortingLayerUniqueIDsProperty.GetValue(null, new object[0]);
		}

		private string[] GetSortingLayerNames()
		{
			var internalEditorUtilityType = typeof(InternalEditorUtility);
			var sortingLayersProperty =
				internalEditorUtilityType.GetProperty("sortingLayerNames",
					BindingFlags.Static | BindingFlags.NonPublic);
			return (string[])sortingLayersProperty.GetValue(null, new object[0]);
		}
	}
}
