using UnityEngine;
using System.Collections;

public class AutoRotateModel: MonoBehaviour {

    void Start() {
        //gameObject.GetComponent<Animator>().enabled = true;
        //gameObject.GetComponent<Animator>().SetFloat("MovementSpeed", -1);
    }

    void Update() {
        transform.Rotate(Vector3.up * Time.deltaTime * 45);
    }

}
