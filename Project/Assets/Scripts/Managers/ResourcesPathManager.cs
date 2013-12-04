using System.IO;

public class ResourcesPathManager {

    private static ResourcesPathManager instance = new ResourcesPathManager();
    public static ResourcesPathManager Instance {
        get { return instance; }
    }

    private ResourcesPathManager() { }
    //characters
    public string PlayerCharacterPath {
        get { return MergeToPath("Characters", "BabyDragon"); }
    }
    //skill casting
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
    //skill icons
    public string SkillIcon48x48(string _iconName) {
        return MergeToPath("Skills", "Icons", "48x48", _iconName);
    }

    public string SkillIcon512x512(string _iconName) {
        return MergeToPath("Skills", "Icons", "512x512", _iconName);
    }

    private string MergeToPath (params string[] nodes){
        string path = string.Empty;
        foreach (string node in nodes)
            path += node + "/";
        return path.Substring(0, path.Length - 1);
    }
}