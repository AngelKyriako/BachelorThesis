using UnityEngine;
using System.Collections;

public class BaseProjectile: MonoBehaviour {

    public int movementSpeed=10, range=25;

    private Pair<BaseSkill, BaseCharacterModel> skillCasterPair;
    private Vector3 origin, destination;

    void Awake() {
        enabled = false;
    }

	void Start () {
        origin = transform.position;
        transform.LookAt(destination);
        transform.Rotate(0, 180, 0);

        skillCasterPair.Second.gameObject.transform.LookAt(destination);
	}

    public virtual void SetUpProjectile(Pair<BaseSkill, BaseCharacterModel> _pair, Vector3 _dest) {
        skillCasterPair = _pair;
        destination = _dest;
        enabled = true;
    }

	public virtual void Update () {
        transform.position = Vector3.MoveTowards(transform.position, destination, movementSpeed * Time.deltaTime);
        if (Vector3.Distance(origin, transform.position) > range)
            CombatManager.Instance.HostDestroySceneObject(gameObject);

	}

    //if (!collision.gameObject.name.Equals(GameManager.Instance.Me.Character.name)) {
    //     Utilities.Instance.LogMessage("On collision with enemy: " + collision.gameObject.GetComponent<PlayerCharacterModel>().Name);
    //     if (PhotonNetwork.isMasterClient) {
    //         Utilities.Instance.LogMessage("Master client is triggering effect of caster: " + skillCasterPair.Second.Name);
    //         skillCasterPair.First.Trigger(skillCasterPair.Second,
    //                       collision.gameObject.GetComponent<PlayerCharacterModel>());
    //         CombatManager.Instance.HostDestroySceneObject(gameObject);
    //     }
    // }

    public virtual void OnCollisionEnter(Collision collision) {
        if (!collision.gameObject.GetComponent<PlayerCharacterNetworkController>().photonView.Equals(GameManager.Instance.MyPhotonView)) {
            Utilities.Instance.LogMessage("On collision with enemy: " + collision.gameObject.GetComponent<PlayerCharacterModel>().Name);
            if (PhotonNetwork.isMasterClient) {
                skillCasterPair.First.Trigger(skillCasterPair.Second,
                                          collision.gameObject.GetComponent<PlayerCharacterModel>());
                CombatManager.Instance.HostDestroySceneObject(gameObject);
            }
            foreach (ContactPoint contact in collision.contacts)
                Debug.DrawRay(contact.point, contact.normal, Color.red);
            if (PhotonNetwork.isMasterClient)
            Utilities.Instance.LogMessage("Just triggered the effect on: " + collision.gameObject.GetComponent<PlayerCharacterModel>());
        }
    }

    public Pair<BaseSkill, BaseCharacterModel> SkillCasterPair {
        get { return skillCasterPair; }
        set { skillCasterPair = value; }
    }
    public Vector3 Destination {
        get { return destination; }
        set { destination = value; }
    }
}
