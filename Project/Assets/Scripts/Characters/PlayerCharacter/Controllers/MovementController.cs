using UnityEngine;
using System.Collections;

public class MovementController: MonoBehaviour {

    private const float MAX_MOVEMENT_SPEED = 10;

    public Animator animator;

    private NetworkController networkController;
    private float currentSpeed;
    private Vector3 destination;

    void Awake() {
        Utilities.Instance.Assert(animator, "CharacterController", "Awake", "animator not defined");
    }

    void Start() {
        networkController = gameObject.GetComponent<NetworkController>();
        animator.enabled = false;
        destination = transform.position;
        currentSpeed = 0f;
        if (networkController.PhotonView.isMine)
            PlayerInputManager.Instance.OnCharacterMovementInput += OnMovementInput;
    }

    void Update() {
        //position based on currentSpeed
        if (Vector3.Distance(destination, transform.position) > 0.5f && animator.enabled)
            transform.position = Vector3.MoveTowards(transform.position, destination, currentSpeed = MAX_MOVEMENT_SPEED * Time.deltaTime);
        else
            currentSpeed = 0f;
        animator.SetFloat("movementSpeed", currentSpeed);
    }

    void OnDestroy() {
        //PlayerInputManager.Instance.OnCharacterMovementInput -= OnMovementInput;
    }

    private void OnMovementInput(Ray ray) {
        ReceiveLocalInput(ray);
        networkController.PhotonView.RPC("SyncInputForCharacterMovement", PhotonTargets.Others, destination, transform.rotation);
    }

    private void ReceiveLocalInput(Ray ray) {
        animator.enabled = true;
        float hitdistance = 0.0f;

        if (new Plane(Vector3.up, transform.position).Raycast(ray, out hitdistance)) {
            Vector3 targetPoint = ray.GetPoint(hitdistance);
            destination = ray.GetPoint(hitdistance);
            //rotate on click
            transform.rotation = Quaternion.LookRotation(targetPoint - transform.position);
        }
    }

    public float CurrentSpeed {
        get { return currentSpeed; }
        set { currentSpeed = value; }
    }

    public Vector3 Destination {
        get { return destination; }
        set { destination = value; }
    }
}