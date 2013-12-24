using UnityEngine;
using System.Collections;

public class DeathPointController: MonoBehaviour {

    private const float TIME_TO_LIVE = 5f;

    private uint expWorth;
    private float timeStart;

    void Awake() {
        enabled = false;
    }

    public void SetUp(uint _exp) {
        expWorth = _exp;
        enabled = true;
    }

    void Start() {
        timeStart = Time.time;
        if(Vector3.Distance(transform.position, GameManager.Instance.MyCharacter.transform.position) <= CombatManager.Instance.ExpRadius)
            GameManager.Instance.MyCharacterModel.GainExp(expWorth);
    }

    void Update() {
        if ((Time.time - timeStart) >= TIME_TO_LIVE)
            Destroy(gameObject);
    }
}
