using UnityEngine;

public class VisionController: MonoBehaviour {

    private const float UPDATE_FREQUENCY = 1.5f;

    public Vector3 visionPosition = Vector3.zero;
    public string visibleLayer = "",
                  hiddenLayer = "";
    public LayerMask ignoredLayers;

    private GameObject vision;
    private SphereCollider visionCollider;
    private float baseRadius;
    private RaycastHit blockedVisionBy;
    private float lastUpdateTime;
    private BaseCharacterModel model;

    void Awake() {
        Utilities.Instance.Assert(visibleLayer.Length != 0 && hiddenLayer.Length != 0, "VisionController", "Awake", "Invalid layers");
        model = gameObject.GetComponent<BaseCharacterModel>();
    }

    void Start() {
        vision = (GameObject)GameObject.Instantiate(Resources.Load(
                                                        ResourcesPathManager.Instance.Vision(
                                                            GameVariables.Instance.Difficulty.Key)));
        vision.transform.parent = transform;
        if (vision.renderer)
            vision.renderer.enabled = false;

        visionCollider = vision.GetComponent<SphereCollider>();
        baseRadius = visionCollider.radius;
        vision.transform.localPosition = visionPosition;

        blockedVisionBy = new RaycastHit();
        lastUpdateTime = 0f;
    }

    void Update() {
        if (Time.time - lastUpdateTime > UPDATE_FREQUENCY)
            visionCollider.radius = baseRadius * model.GetAttribute((int)AttributeType.VisionRadius).FinalValue;
    }

    void OnTriggerEnter(Collider other) {
        if (other.gameObject.layer == LayerMask.NameToLayer(hiddenLayer)) {
            Utilities.Instance.SetGameObjectLayer(other.gameObject, LayerMask.NameToLayer(visibleLayer));
        }
        //if (Physics.Raycast(transform.position, transform.position - other.transform.position, out blockedVisionBy, ignoredLayers)) {
        //}
    }

    void OnTriggerStay(Collider other) {
        if (other.gameObject.layer == LayerMask.NameToLayer(hiddenLayer)) {
            Utilities.Instance.SetGameObjectLayer(other.gameObject, LayerMask.NameToLayer(visibleLayer));
        }
    }

    void OnTriggerExit(Collider other) {
        if (other.gameObject.layer == LayerMask.NameToLayer(visibleLayer)) {
            Utilities.Instance.SetGameObjectLayer(other.gameObject, LayerMask.NameToLayer(hiddenLayer));
        }
    }
}