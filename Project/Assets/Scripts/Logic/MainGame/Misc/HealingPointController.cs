using UnityEngine;
using System.Collections.Generic;

public class HealingPointController : MonoBehaviour {

    private int HEALTH_VITAL_INDEX = 0, MANA_VITAL_INDEX = 1;

    public float healthRegeneration = 2,
                 manaRegeneration = 4,
                 regenerationFrequency = 1,
                 healingRadius = 5;
    public List<int> teamsList;

    private float lastHealingTime;
    private bool IsValidForHealing;

    void Awake() {
        Utilities.Instance.Assert(gameObject.GetComponent<SphereCollider>() != null, "HealingPointController", "Awake", "No sphereCollider attached on object");
        IsValidForHealing = teamsList.Contains((int)GameManager.Instance.MyTeam);
    }

    void Start() {
        gameObject.GetComponent<SphereCollider>().radius = healingRadius;
        gameObject.GetComponent<SphereCollider>().isTrigger = true;
        lastHealingTime = Time.time;        
	}
	
	void OnTriggerStay (Collider other) {
        if (IsValidForHealing && (Time.time - lastHealingTime > regenerationFrequency) && !other.gameObject.layer.Equals(LayerMask.NameToLayer("Void"))) {
            BaseCharacterModel _characterToHeal = Utilities.Instance.GetPlayerCharacterModel(other.transform);
            if (GameManager.Instance.ItsMe(_characterToHeal.name)) {
                lastHealingTime = Time.time;
                GameManager.Instance.MyCharacterModel.GetVital(HEALTH_VITAL_INDEX).CurrentValue += healthRegeneration;
                GameManager.Instance.MyCharacterModel.GetVital(MANA_VITAL_INDEX).CurrentValue += manaRegeneration;
            }
        }
	}
}
