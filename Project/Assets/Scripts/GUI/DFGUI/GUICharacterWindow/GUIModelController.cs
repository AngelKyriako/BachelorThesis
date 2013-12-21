using UnityEngine;
using System.Collections;

public class GUIModelController: SingletonMono<GUIModelController> {
    
    void Start() {
        gameObject.GetComponent<Animator>().enabled = true;
        gameObject.GetComponent<Animator>().SetFloat("MovementSpeed", 0);
    }

    void Update() {
        transform.Rotate(Vector3.up * Time.deltaTime * 45);
    }

    public void Hide() {
        Utilities.Instance.SetGameObjectLayer(gameObject, LayerMask.NameToLayer("Void"));
    }

    public void Show() {
        Utilities.Instance.SetGameObjectLayer(gameObject, LayerMask.NameToLayer("GUI"));
    }

}
