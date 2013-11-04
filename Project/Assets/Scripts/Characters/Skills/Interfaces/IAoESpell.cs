using UnityEngine;

public interface IAoESpell: IBaseSpell {
    int MaxTargets { get; set; }
    int AoERange { get; set; }
}
