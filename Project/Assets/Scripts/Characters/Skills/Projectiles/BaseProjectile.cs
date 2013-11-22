using UnityEngine;
using System.Collections;

public class BaseProjectile: MonoBehaviour {

    private const int MAX_MOVEMENT_SPEED = 10, RANGE = 25;

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
	
	public virtual void Update () {
        transform.position = Vector3.MoveTowards(transform.position, destination, MAX_MOVEMENT_SPEED * Time.deltaTime);
        if (Vector3.Distance(origin, transform.position) > RANGE)
            Destroy(gameObject);

	}

    public virtual void OnCollisionEnter(Collision collision) {
        if (collision.gameObject.layer.Equals("VisibleEnemies") || collision.gameObject.layer.Equals("HiddenEnemies")) {
            skillCasterPair.First.Trigger(skillCasterPair.Second,
                                          collision.gameObject.GetComponent<PlayerCharacterModel>());
            foreach (ContactPoint contact in collision.contacts)
                Debug.DrawRay(contact.point, contact.normal, Color.red);

            Destroy(gameObject);
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
