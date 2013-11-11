using UnityEngine;
using System.Collections;

public class CombatManager: SingletonPhotonMono<CombatManager> {

    public delegate void SkillCast();
    public event SkillCast OnSkillCast;

    private CombatManager() { }

	void Awake () {
	
	}
	
	void Update () {

	}
}
