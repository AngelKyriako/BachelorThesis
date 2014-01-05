using UnityEngine;
using System.Collections.Generic;

public class GUISkillBook: SingletonMono<MonoBehaviour> {

    private static GUISkillInventory[] allInventorySkills;

    private GUISkillBook() { }

	void Start () {        
        var control = gameObject.GetComponent<dfControl>();
        allInventorySkills = control.GetComponentsInChildren<GUISkillInventory>();
        RefreshAll();
	}

    public static void RefreshAll() {
        SkillBook.Instance.UpdateAllSkills();
        for (int i = 0; i < allInventorySkills.Length; ++i)
            allInventorySkills[i].Refresh();
    }
}
