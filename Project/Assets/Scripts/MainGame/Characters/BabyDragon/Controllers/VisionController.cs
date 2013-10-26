using UnityEngine;
using System.Collections;

public class VisionController: MonoBehaviour {

    private GameObject vision;
    private RaycastHit blockedVisionBy;

    public GameObject visionPrefab;
    public Vector3 visionPosition = Vector3.zero;
    public int radius = 1;
    public string visibleLayer="",
                  hiddenLayer="";
    public LayerMask ignoredLayers;

    void Awake() {
        Utilities.Instance.Assert(visionPrefab, "VisionController", "Awake", "Invalid vision prefab");
        Utilities.Instance.Assert(visibleLayer.Length != 0 && hiddenLayer.Length != 0, "VisionController", "Awake", "Invalid layers");
    }

	void Start () {
        vision = (GameObject)Instantiate(visionPrefab);
        vision.transform.parent = transform;
        if (vision.renderer)
            vision.renderer.enabled = false;

        vision.transform.localScale = new Vector3(radius, radius, radius);
        vision.transform.localPosition = visionPosition;

        blockedVisionBy = new RaycastHit();
	}

    void OnTriggerEnter(Collider other) {
        if (other.gameObject.layer == LayerMask.NameToLayer(hiddenLayer)){
            SetGameObjectLayer(other.gameObject, LayerMask.NameToLayer(visibleLayer));
        }
        //if (Physics.Raycast(transform.position, transform.position - other.transform.position, out blockedVisionBy, ignoredLayers)) {
        //}
    }

    void OnTriggerStay(Collider other) {
        if (other.gameObject.layer == LayerMask.NameToLayer(hiddenLayer)){
            SetGameObjectLayer(other.gameObject, LayerMask.NameToLayer(visibleLayer));
        }
    }

    void OnTriggerExit(Collider other) {
        if (other.gameObject.layer == LayerMask.NameToLayer(visibleLayer)) {
            SetGameObjectLayer(other.gameObject, LayerMask.NameToLayer(hiddenLayer));
        }
    }

    private static void SetGameObjectLayer(GameObject obj, int layer) {
        if (obj == null)
            return;
        obj.layer = layer;
        foreach (Transform child in obj.transform)
            if (child)
                SetGameObjectLayer(child.gameObject, layer);
    }
}