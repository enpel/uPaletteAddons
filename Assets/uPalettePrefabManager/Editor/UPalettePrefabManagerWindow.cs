using NagaraStudio.Editor;
using NagaraStudio.Editor.ViewParts;
using UnityEditor;
using UnityEngine;

namespace NagaraStudio.uPalettePrefabManager.Editor
{
    public class UPalettePrefabManagerWindow : EditorWindow
    {
    
        private ManagedPrefabDataStore prefabDataStore;
        private ManagedPrefabListView prefabListView;
    
        [MenuItem("Window/uPalette/Prefab Manager")]
        public static void ShowWindow()
        {
            GetWindow<UPalettePrefabManagerWindow>("Prefab Manager");
        }
    
        private void OnEnable()
        {
            // 初回起動時にPrefabListを探して読み込む
            string[] guids = AssetDatabase.FindAssets("t:ManagedPrefabDataStore");
            if (guids.Length > 0)
            {
                string path = AssetDatabase.GUIDToAssetPath(guids[0]);
                prefabDataStore = AssetDatabase.LoadAssetAtPath<ManagedPrefabDataStore>(path);
            }
            else
            {
                // 見つからなかった場合、新規作成
                prefabDataStore = CreateInstance<ManagedPrefabDataStore>();
                AssetDatabase.CreateAsset(prefabDataStore, "Assets/ManagedPrefabDataStore.asset");
                AssetDatabase.SaveAssets();
            }
        
            prefabListView = new ManagedPrefabListView(prefabDataStore);
        }

        public void OnGUI()
        {
            GUILayout.Label("Prefab Manager", EditorStyles.boldLabel);
            prefabListView.OnGUI();
            
            
            if (GUILayout.Button("OpenUPaletteWindow"))
            {
                UPaletteUtility.OpenPaletteWindow();
            }
        }
    }
}