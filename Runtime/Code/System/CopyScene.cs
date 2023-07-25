using Socket;
using System.Collections;
using System.Collections.Generic;
using UnityEditor;

namespace Socket
{
    public class CopyScene
    {
        [MenuItem("Custom/Import Demo Scene")]
        public static void ImportDemoScene()
        {
            string sourcePath = "Packages/com.prototype.socketp5/Runtime/Demo/Socket demo.unity";
            string targetPath = "Assets/Scenes/Demo.unity";

            AssetDatabase.CopyAsset(sourcePath, targetPath);
            AssetDatabase.Refresh();
        }
    }
}
