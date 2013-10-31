using UnityEngine;

public interface IAoESkill: IBaseSkill {
    int MaxTargers { get; set; }
    int AoERange { get; set; }
}
