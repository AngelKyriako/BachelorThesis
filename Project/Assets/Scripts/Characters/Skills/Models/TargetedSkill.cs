using UnityEngine;
using System.Collections;

public class TargetedSkill: BaseSkill {

    private GameObject targetCursor;
    private bool isSelected;

    public TargetedSkill(string _title, string _desc, Texture2D _icon, float _cd, string _castEff, string _projectile, string _triggerEff, GameObject _targetCursor)
        : base(_title, _desc, _icon, _cd, _castEff, _projectile, _triggerEff) {
        targetCursor = _targetCursor;
        isSelected = false;
    }

    public override void Select(BaseCharacterModel _caster, CharacterSkillSlot _slot) {        
        if (IsUsable(_caster)) {
            isSelected = true;
            GameObject obj;
            if (targetCursor) {
                obj = (GameObject)GameObject.Instantiate(targetCursor);
                obj.GetComponent<TargetCursor>().SetUpTargetCursor(this, _caster, _slot);
            }
        }
    }

    public override void Cast(BaseCharacterModel _caster, Vector3 _direction) {
        if (isSelected == true) {
            base.Cast(_caster, _direction);
            isSelected = false;
        }
    }
	
    public override bool IsUsable(BaseCharacterModel _characterModel) {
        return (!isSelected && base.IsUsable(_characterModel));
    }
}
