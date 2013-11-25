using UnityEngine;
using System.Collections;

public class SceneHierarchyManager {

    private static SceneHierarchyManager instance = new SceneHierarchyManager();
    public static SceneHierarchyManager Instance {
        get { return instance; }
    }

    private SceneHierarchyManager() { }

    public string PlayerCharacterPath {
        get { return MergeToPath("Characters", "BabyDragons"); }
    }

    private string MergeToPath (params string[] nodes){
        string path = string.Empty;
        foreach (string node in nodes)
            path += node + "/";
        return path.Substring(0, path.Length - 1);
    }
}
