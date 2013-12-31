using UnityEngine;
using System.Collections;

public class TargetedSkill: BaseSkill {

    private GameObject targetCursor;
    private BaseTargetCursor currentCursor;

    public TargetedSkill(int _id, string _title, string _desc, float _cd, string _castEff, string _mainObject, string _triggerEff, GameObject _targetCursor)
        : base(_id, _title, _desc, _cd, _castEff, _mainObject, _triggerEff) {
        targetCursor = _targetCursor;
        currentCursor = null;
    }

    public override void Pressed() {
        if (!IsSelected && IsUsable)
            Select();
        else if (IsSelected && IsUsable) {
            LookAtTargetLocation();
            Cast(currentCursor.Destination);
            DFSkillModel.Instance.CastSkill(Slot);
        }
        else if (!SufficientMana)
            GUIMessageDisplay.Instance.AddMessage("No juice bro");
        else if (CoolDownTimer != 0f)
            GUIMessageDisplay.Instance.AddMessage("Chill for a sec there");
        else if(!RequirementsFulfilled())
            GUIMessageDisplay.Instance.AddMessage("This skill is not for that ugly face of yours");
    }

    public override void Unpressed() {
        if (IsSelected)
            Unselect();
    }

    public override void Select() {
        if (IsUsable) {            
            GameObject obj;
            obj = (GameObject)GameObject.Instantiate(targetCursor);
            currentCursor = obj.GetComponent<BaseTargetCursor>();
            currentCursor.SetUpTargetCursor();
        }
    }

    public override void Unselect() {
        currentCursor.DestroyTargetCursor();
        currentCursor = null;
    }

    public void LookAtTargetLocation() {
        OwnerModel.transform.LookAt(currentCursor.Destination);
    }

    public override bool IsSelected {
        get { return currentCursor != null; }
        set {  }
    }
}
