public class ResourcesPathManager {

    private static ResourcesPathManager instance = new ResourcesPathManager();
    public static ResourcesPathManager Instance {
        get { return instance; }
    }

    private ResourcesPathManager() { }

    #region characters
    public string PlayerCharacterPrefabPath {
        get { return MergeToPath("Characters", "Prefabs", "BabyDragon"); }
    }
    public string PlayerCharacterMaterialPath(string _color) {
        return MergeToPath("Characters", "Materials", _color);
    }
    #endregion

    #region misc
    public string Vision {
        get { return MergeToPath("Characters", "Prefabs", "Vision"); }
    }
    public string ExpRadiusSphere {
        get { return MergeToPath("Characters", "Prefabs", "ExpSphere"); }
    }
    #endregion

    #region skills
    //casting
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
    //icons
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
    #endregion
}