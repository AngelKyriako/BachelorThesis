using UnityEngine;
using System.Collections;

public class BaseProjectile: Photon.MonoBehaviour {

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

    public virtual void OnCollisionEnter(Collision collision) {
        //@TODO Check out collision with remote characters
        if (!collision.gameObject.GetComponent<NetworkController>().photonView.Equals(GameManager.Instance.MyPhotonView))
            Utilities.Instance.LogMessage("OnCollision with other object");    
        if (collision.gameObject.layer.Equals("VisibleEnemies") || collision.gameObject.layer.Equals("HiddenEnemies")) {
            Utilities.Instance.LogMessage("It is an enemy");
            skillCasterPair.First.Trigger(skillCasterPair.Second,
                                          collision.gameObject.GetComponent<PlayerCharacterModel>());
            foreach (ContactPoint contact in collision.contacts)
                Debug.DrawRay(contact.point, contact.normal, Color.red);

            CombatManager.Instance.HostDestroySceneObject(gameObject);
            Utilities.Instance.LogMessage("Just triggered the effect");
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
