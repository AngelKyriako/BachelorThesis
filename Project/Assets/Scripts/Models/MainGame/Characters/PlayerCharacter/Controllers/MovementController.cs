using UnityEngine;
using System.Collections;

public class MovementController: MonoBehaviour {

    private const float groundedY = 0;

    public Animator animator;
    public float baseMovementSpeed = 10;

    private PlayerCharacterModel model;
    private PlayerCharacterNetworkController networkController;
    private float currentSpeed;
    private Vector3 destination;

    void Awake() {
        Utilities.Instance.Assert(animator, "CharacterController", "Awake", "animator not defined");
        animator.enabled = true;
    }

    void Start() {
        model = gameObject.GetComponent<PlayerCharacterModel>();
        networkController = gameObject.GetComponent<PlayerCharacterNetworkController>();
        //animator.enabled = false;
        destination = transform.position;
        currentSpeed = 0f;
        if (networkController.IsLocalClient)
            PlayerInputManager.Instance.OnCharacterMovementInput += OnMovementInput;
    }

    void Update() {
        if (networkController.IsLocalClient && !model.IsStunned) {
            AnimatorMovementSpeed = currentSpeed;

            //move to destination
            if (Vector3.Distance(destination, transform.position) > 0.5f && animator.enabled) {
                UpdateCurrentSpeed();
                transform.position = Vector3.MoveTowards(transform.position, destination, currentSpeed * Time.deltaTime);
            }
            else
                currentSpeed = 0f;
            //GroundCharacter();
        }
    }

    void OnDestroy() {
        //if (PlayerCharacterNetworkController.IsLocalClient)
        //    PlayerInputManager.Instance.OnCharacterMovementInput -= OnMovementInput;
    }

    private void GroundCharacter() {
        // Raycast down from the center of the character.. 
        Ray ray = new Ray(transform.position + Vector3.up, -Vector3.up);
        RaycastHit hitInfo = new RaycastHit();

        if (Physics.Raycast(ray, out hitInfo)) {
            if (hitInfo.distance > 0f) {

                // MatchTarget allows us to take over animation and smoothly transition our character towards a location - the hit point from the ray.
                // Here we're telling the Root of the character to only be influenced on the Y axis (MatchTargetWeightMask) and only occur between 0.35 and 0.5
                // of the timeline of our animation clip
                animator.MatchTarget(hitInfo.point, Quaternion.identity, AvatarTarget.Root, new MatchTargetWeightMask(new Vector3(0, 1, 0), 0), 0.35f, 0.5f);
            }
        }
    }

    private void OnMovementInput(Ray ray) {
        //animator.enabled = true;
        float hitdistance = 0.0f;

        if (new Plane(Vector3.up, transform.position).Raycast(ray, out hitdistance) && !model.IsStunned) {
            Vector3 targetPoint = ray.GetPoint(hitdistance);
            destination = ray.GetPoint(hitdistance);
            destination.Set(destination.x, groundedY, destination.z);
            //rotate on click
            transform.rotation = Quaternion.LookRotation(targetPoint - transform.position);
        }
    }

    private void UpdateCurrentSpeed() {
        currentSpeed = baseMovementSpeed * model.GetAttribute((int)AttributeType.MovementSpeed).FinalValue;
    }

    public float AnimatorMovementSpeed {
        get { return animator.GetFloat("MovementSpeed"); }
        set { animator.SetFloat("MovementSpeed", value); }     
    }
    public float CurrentSpeed {        
        get { return currentSpeed; }
    }
}