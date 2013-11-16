using UnityEngine;
using System.Collections;

public class TargetCursor : MonoBehaviour {

    private Pair<BaseSkill, BaseCharacterModel> skillCasterPair;

    void Awake() {
        enabled = false;
    }

    void Start() {
        //add "OnSkillCast to input manager listener"
    }

	void Update () {
	//transform position where mouse is (hint down) 
	}

    //void OnMouseDown() {
    //    screenPoint = Camera.main.WorldToScreenPoint(scanPos);

    //    offset = scanPos - Camera.main.ScreenToWorldPoint(
    //        new Vector3(Input.mousePosition.x, Input.mousePosition.y, screenPoint.z));
    //}


    //void OnMouseDrag() {
    //    Vector3 curScreenPoint = new Vector3(Input.mousePosition.x, Input.mousePosition.y, screenPoint.z);

    //    Vector3 curPosition = Camera.main.ScreenToWorldPoint(curScreenPoint) + offset;
    //    transform.position = curPosition;
    //}

    private void OnSkillCast() {
        skillCasterPair.First.Cast(skillCasterPair.Second);
    }
    public Pair<BaseSkill, BaseCharacterModel> SkillCasterPair {
        get { return skillCasterPair; }
        set { skillCasterPair = value; }
    }
}
