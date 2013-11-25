using System.IO;

public class ResourcesPathManager {

    private static ResourcesPathManager instance = new ResourcesPathManager();
    public static ResourcesPathManager Instance {
        get { return instance; }
    }

    private ResourcesPathManager() { }

    public string PlayerCharacterPath {
        get { return MergeToPath("Characters", "BabyDragon"); }
    }

    public string TargetCursorPath(string _targetCursor) {
        return MergeToPath("Skills", "TargetCursors", _targetCursor);
    }

    public string CastEffectPath(string _castEffect) {
        return MergeToPath("Skills", "CastEffects", _castEffect);
    }

    public string TriggerEffectPath(string _triggerEffect) {
        return MergeToPath("Skills", "TriggerEffects", _triggerEffect);
    }

    public string ProjectilePath(string _projectile) {
        return MergeToPath("Skills", "Projectiles", _projectile);
    }

    private string MergeToPath (params string[] nodes){
        string path = string.Empty;
        foreach (string node in nodes)
            path += node + "/";
        return path.Substring(0, path.Length - 1);
    }
}