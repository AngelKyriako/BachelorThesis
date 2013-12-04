using UnityEngine;
using System.Collections;

public class TargetedSkill: BaseSkill {

    private GameObject targetCursor;
    private TargetCursor currentCursor;
    private bool isSelected;

    public TargetedSkill(string _title, string _desc, Texture2D _icon, float _cd, string _castEff, string _projectile, string _triggerEff, GameObject _targetCursor)
        : base(_title, _desc, _icon, _cd, _castEff, _projectile, _triggerEff) {
        targetCursor = _targetCursor;
        currentCursor = null;
        isSelected = false;
    }

    public override void Pressed() {
        if (!isSelected && IsUsable)
            Select();
        else if (isSelected)
            Cast(currentCursor.Direction);        
    }

    public override void Unpressed() {
        if (isSelected)
            Unselect();
    }

    public override void Select() {
        if (IsUsable) {            
            GameObject obj;
            obj = (GameObject)GameObject.Instantiate(targetCursor);
            currentCursor = obj.GetComponent<TargetCursor>();
            currentCursor.SetUpTargetCursor(this);

            isSelected = true;
        }
    }

    public override void Unselect() {
        currentCursor.DestroyTargetCursor();
        currentCursor = null;

        isSelected = false;
    }

    public override bool IsSelected {
        get { return isSelected; }
        set { isSelected = value; }
    }
}
