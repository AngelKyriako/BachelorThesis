public class ResourcesPathManager {

    private static ResourcesPathManager instance = new ResourcesPathManager();
    public static ResourcesPathManager Instance {
        get { return instance; }
    }

    private ResourcesPathManager() { }

    #region characters
    public string PlayerCharacterPrefabPath {
        get { return MergeToPath("Prefabs", "Characters", "BabyDragon"); }
    }
    public string Vision {
        get { return MergeToPath("Prefabs", "Characters", "Vision"); }
    }
    public string PlayerCharacterMaterialPath(string _color) {
        return MergeToPath("Materials", "PlayerCharacter", _color);
    }
    #endregion

    #region misc
    public string DeathPoint {
        get { return MergeToPath("Prefabs", "Misc", "DeathPoint"); }
    }
    public string LevelUp {
        get { return MergeToPath("Prefabs", "Misc", "LevelUp"); }
    }
    public string HitEffect {
        get { return MergeToPath("Prefabs", "Misc", "HitEffect"); }
    }
    #endregion

    #region skills
    public string TargetCursorPath(string _targetCursor) {
        return MergeToPath("Prefabs", "Skills", "TargetCursors", _targetCursor);
    }

    public string CastEffectPath(string _castEffect) {
        return MergeToPath("Prefabs", "Skills", "CastEffects", _castEffect);
    }

    public string TriggerEffectPath(string _triggerEffect) {
        return MergeToPath("Prefabs", "Skills", "TriggerEffects", _triggerEffect);
    }

    public string AoEObjectPath(string _aoeEffect) {
        return MergeToPath("Prefabs", "Skills", "AoEObjects", _aoeEffect);
    }

    public string SkillObjectPath(string skillObject) {
        return MergeToPath("Prefabs", "Skills", "MainObjects", skillObject);
    }
    public string ProjectilePath(string _projectile) {
        return MergeToPath("Prefabs", "Skills", "MainObjects", "Projectiles", _projectile);
    }

    private string MergeToPath (params string[] nodes){
        string path = string.Empty;
        foreach (string node in nodes)
            path += node + "/";
        return path.Substring(0, path.Length - 1);
    }
    #endregion
}