using System;
using System.Reflection;
using UnityEngine;

namespace NagaraStudio.Editor
{
    public class UPaletteUtility
    {

        public static void OpenPaletteWindow()
        {
            
            // クラス名とアセンブリ名を使って型を取得
            // "Namespace.InternalClass, AssemblyName" の形式でアセンブリ名を記入
            var internalClassType = Type.GetType("uPalette.Editor.Core.PaletteEditor.PaletteEditorWindow, uPalette.Editor");

            if (internalClassType != null)
            {
                // 静的メソッドの情報を取得
                var staticMethod = internalClassType.GetMethod("Open", BindingFlags.Static | BindingFlags.NonPublic | BindingFlags.Public);

                if (staticMethod != null)
                {
                    // 静的メソッドを呼び出し
                    staticMethod.Invoke(null, null);
                }
                else
                {
                    Debug.LogError("Static method 'StaticMethod' not found in class 'InternalClass'.");
                }
            }
            else
            {
                Debug.LogError("Class 'InternalClass' not found. Please check the class name and namespace.");
            }
        }
    }
}