using System.IO;

public class ResourcesPathManager {

    #region folders
    private const string charactersF = "Characters";
    private const string skillsF = "Skills",
                         targetF = "TargetCursors", castF = "CastEffects", triggerF = "TriggerEffects", projectilesF = "Projectiles";

#endregion

    private static ResourcesPathManager instance = new ResourcesPathManager();
    public static ResourcesPathManager Instance {
        get { return instance; }
    }

    private ResourcesPathManager() { }

    public string BabyDragonPath {
        get { return MergeToPath(charactersF, "BabyDragon"); }
    }

    public string TargetCursorPath(string _cursor) {
        return MergeToPath(skillsF, targetF, _cursor);
    }

    public string CastEffectPath(string _castEffect) {
        return MergeToPath(skillsF, castF, _castEffect);
    }

    public string TriggerEffectPath(string _triggerEffect) {
        return MergeToPath(skillsF, triggerF, _triggerEffect);
    }

    public string ProjectilePath(string _projectile) {
        return MergeToPath(skillsF, projectilesF, _projectile);
    }

    private string MergeToPath (params string[] nodes){
        string path = string.Empty;
        foreach (string node in nodes)
            path += node + "/";
        return path.Substring(0, path.Length - 1);
    }
}