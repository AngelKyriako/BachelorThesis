using UnityEngine;
using System.Collections.Generic;

public class VisionController: MonoBehaviour {

    private const float DISTANCE_FROM_GROUND = 3.0f, UPDATE_FREQUENCY = 1.5f;
    private const int EASY_RADIUS = 30, MEDIUM_RADIUS = 15, HARD_RADIUS = 10;
    private readonly Dictionary<GameDifficulty, int> DIFFICULTY_TO_RADIUS = new Dictionary<GameDifficulty, int> (){ { GameDifficulty.Easy, EASY_RADIUS },
                                                                                                                    { GameDifficulty.Medium, MEDIUM_RADIUS },
                                                                                                                    { GameDifficulty.Hard, HARD_RADIUS }
                                                                                                                  };

    public Vector3 visionLocalPosition = Vector3.zero,
                   visionLocalRotation = Vector3.zero;
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
        enabled = false;
    }

    public void SetUp(bool isAlly, GameDifficulty _difficulty) {
        Utilities.Instance.SetGameObjectLayer(gameObject, isAlly ? LayerMask.NameToLayer("Allies") : LayerMask.NameToLayer("HiddenEnemies"));
        baseRadius = DIFFICULTY_TO_RADIUS[_difficulty];
        enabled = isAlly;
    }

    void Start() {
        vision = (GameObject)GameObject.Instantiate(Resources.Load(ResourcesPathManager.Instance.Vision));
        vision.transform.parent = transform;
        visionCollider = vision.GetComponent<SphereCollider>();
        if (vision.renderer)
            vision.renderer.enabled = false;
        vision.transform.localPosition = visionLocalPosition;
        vision.transform.localEulerAngles = visionLocalRotation;

        blockedVisionBy = new RaycastHit();
        lastUpdateTime = Time.time;
    }

    void Update() {
        if (Time.time - lastUpdateTime > UPDATE_FREQUENCY)
            visionCollider.radius = baseRadius * model.GetAttribute((int)AttributeType.VisionRadius).FinalValue;
    }

    void OnTriggerEnter(Collider other) {
        if (other.gameObject.layer.Equals(LayerMask.NameToLayer(hiddenLayer))) {
            Utilities.Instance.SetGameObjectLayer(other.gameObject, LayerMask.NameToLayer(visibleLayer));
        }
        //if (Physics.Raycast(transform.position, transform.position - other.transform.position, out blockedVisionBy, ignoredLayers)) {
        //}
    }

    void OnTriggerStay(Collider other) {
        if (other.gameObject.layer.Equals(LayerMask.NameToLayer(hiddenLayer))) {
            Utilities.Instance.SetGameObjectLayer(other.gameObject, LayerMask.NameToLayer(visibleLayer));
        }
    }

    void OnTriggerExit(Collider other) {
        if (other.gameObject.layer.Equals(LayerMask.NameToLayer(visibleLayer))) {
            Utilities.Instance.SetGameObjectLayer(other.gameObject, LayerMask.NameToLayer(hiddenLayer));
        }
    }
}