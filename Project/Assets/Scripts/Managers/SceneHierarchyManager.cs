using UnityEngine;
using System.Collections;

public class SceneHierarchyManager {

    private static SceneHierarchyManager instance = new SceneHierarchyManager();
    public static SceneHierarchyManager Instance {
        get { return instance; }
    }

    private SceneHierarchyManager() { }

    public string PlayerCharacterPath {
        get { return MergeToPath("Characters", "PlayerCharacters"); }
    }

    public string PlayerCharacterProjectileSpawnerPath(string playerCharacterName) {
        return MergeToPath(PlayerCharacterPath, playerCharacterName, "ProjectileSpawner");
    }
    public string PlayerCharacterMesh(string playerCharacterName) {
        return MergeToPath(PlayerCharacterPath, playerCharacterName, "Sphere_002");
    }
    public string GUIPlayerCharacterMesh {
        get { return MergeToPath("DFGUI", "Graphics", "CharacterWindow", "GUIPlayerCharacter", "Model", "Sphere_002"); }
    }

    private string MergeToPath (params string[] nodes){
        string path = string.Empty;
        foreach (string node in nodes)
            path += node + "/";
        return path.Substring(0, path.Length - 1);
    }
}
