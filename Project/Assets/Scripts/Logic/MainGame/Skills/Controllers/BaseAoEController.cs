using UnityEngine;
using System.Collections;

public class BaseAoEController: MonoBehaviour {

    public float timeToLive;

	void Start () {
	
	}
	
	void Update () {
	
	}

    public virtual void OnTriggerWithAlly() {
    }

    public virtual void OnTriggerWithEnemy() {
    }
}
