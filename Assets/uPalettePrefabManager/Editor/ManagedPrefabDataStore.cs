using System;
using UnityEngine;

namespace NagaraStudio.uPalettePrefabManager.Editor
{
    public class ManagedPrefabDataStore: ScriptableObject
    {
        public ManagedPrefabData[] ManagedPrefabDatas;
    }

    [Serializable]
    public class ManagedPrefabData
    {
        public GameObject Prefab;
    }
}