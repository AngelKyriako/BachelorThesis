using UnityEngine;
using System.Collections;

public class TargetCursor: MonoBehaviour {

    private Pair<BaseSkill, BaseCharacterModel> skillCasterPair;
    private CharacterSkillSlots slotSelected;

    void Awake() {
        enabled = false;
    }

    void Start() {
        PlayerInputManager.Instance.OnSkillSelectInput += OnSkillCast;
    }

	void Update () {
        Vector3 mousePos = PlayerInputManager.Instance.MousePosition;
        mousePos.z = Camera.main.transform.position.y;
        mousePos = Camera.main.ScreenToWorldPoint(mousePos);

        transform.position = new Vector3(mousePos.x, 2, mousePos.z + 2*mousePos.y/3);
	}

    private void OnSkillCast(CharacterSkillSlots _slot) {
        if (slotSelected.Equals(_slot)) {
            skillCasterPair.First.Cast(skillCasterPair.Second, transform.position);
            PlayerInputManager.Instance.OnSkillSelectInput -= OnSkillCast;
            Destroy(gameObject);
        }
    }

    public Pair<BaseSkill, BaseCharacterModel> SkillCasterPair {
        get { return skillCasterPair; }
        set { skillCasterPair = value; }
    }
    public CharacterSkillSlots SlotSelected {
        get { return slotSelected; }
        set { slotSelected = value; }
    }
}
