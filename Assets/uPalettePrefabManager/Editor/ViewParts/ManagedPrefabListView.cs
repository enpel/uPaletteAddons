using NagaraStudio.uPalettePrefabManager.Editor;
using UnityEditor;
using UnityEngine;
using UnityEngine.UI;
using uPalette.Runtime.Core.Synchronizer.Color;

namespace NagaraStudio.Editor.ViewParts
{

    public class ManagedPrefabListView
    {
        private ManagedPrefabDataStore prefabDataStore;
        private Vector2 scrollPos;
    
        public ManagedPrefabListView(ManagedPrefabDataStore prefabDataStore)
        {
            this.prefabDataStore = prefabDataStore;
        }
    
        public void OnGUI()
        {
            if (!prefabDataStore)
            {
                EditorGUILayout.HelpBox("PrefabList not found or not created. Please create one.", MessageType.Warning);
                return;
            }

            if (prefabDataStore.ManagedPrefabDatas == null || prefabDataStore.ManagedPrefabDatas.Length == 0)
            {
                EditorGUILayout.HelpBox("No prefab data found.", MessageType.Info);
                return;
            }
        
            // ScriptableObjectに登録されているPrefabの一覧を表示
            scrollPos = EditorGUILayout.BeginScrollView(scrollPos);
            foreach (var prefabData in prefabDataStore.ManagedPrefabDatas)
            {
                EditorGUILayout.ObjectField(prefabData.Prefab, typeof(GameObject), false);
                DisplayGraphicsInPrefab(prefabData);
            }
            EditorGUILayout.EndScrollView();
        }
        
        private void DisplayGraphicsInPrefab(ManagedPrefabData prefab)
        {
            // Prefab内のImageを表示
            var components = prefab.Prefab.GetComponentsInChildren<Graphic>();
            
            GUILayout.BeginVertical();
            EditorGUI.indentLevel += 2; // +1だと見た目的にあんまり変わらないので+2にしている
            
            foreach (var component in components)
            {   
                DisplayGraphicComponentView(prefab.Prefab, component);
            }

            EditorGUI.indentLevel -= 2;
            GUILayout.EndVertical();
        }

        private void DisplayGraphicComponentView(GameObject prefab, Graphic graphic)
        {
            GUILayout.BeginHorizontal();
            EditorGUILayout.ObjectField(graphic.gameObject.name, graphic, typeof(Graphic), false);
            EditorGUILayout.ColorField(graphic.color);
            
            var synchronizer = graphic.gameObject.GetComponent<GraphicColorSynchronizer>();

            if (synchronizer != null)
            {
                // Synchronizerがある場合はInspectorを表示して、変更を監視する。
                EditorGUI.BeginChangeCheck();
                UnityEditor.Editor.CreateEditor(synchronizer).OnInspectorGUI();
                if(EditorGUI.EndChangeCheck()){
                    // 変更があったらPrefabを保存
                    PrefabUtility.SavePrefabAsset(prefab);
                }
                
                if (GUILayout.Button("Remove Synchronizer"))
                {
                    Object.DestroyImmediate(synchronizer, true);
                    PrefabUtility.SavePrefabAsset(prefab);
                }
            }
            else
            {
                if (GUILayout.Button("Add Synchronizer"))
                {
                    graphic.gameObject.AddComponent<GraphicColorSynchronizer>();
                    PrefabUtility.SavePrefabAsset(prefab);
                }
            }
            GUILayout.EndHorizontal();
        }
    }

}