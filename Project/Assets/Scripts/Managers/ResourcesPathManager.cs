using System.IO;

public class ResourcesPathManager {

    #region folders
    private const string charactersF = "Characters";
    private const string skillsF = "Skills",
                         targetF = "TargetCursors", castsF = "CastEffects", triggerF = "TriggerEffects", projectilesF = "Projectiles";

#endregion

    private static ResourcesPathManager instance = new ResourcesPathManager();
    public static ResourcesPathManager Instance {
        get { return instance; }
    }

    private ResourcesPathManager() { }

    public string BabyDragonPath {
        get { return MergeToPath(charactersF, "BabyDragon"); }
    }

    public string ProjectilePath {
        get { return MergeToPath(skillsF, projectilesF, "TestProjectile"); }
    }

    private string MergeToPath (params string[] nodes){
        string path = string.Empty;
        foreach (string node in nodes)
            path += node + "/";
        return path.Substring(0, path.Length - 1);
    }
}