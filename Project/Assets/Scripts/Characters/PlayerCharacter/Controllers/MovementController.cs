using UnityEngine;
using System.Collections;

public class MovementController: MonoBehaviour {

    private Vector3 destination;
    private float distance,
                  currentSpeed;

    public Animator animator;
    private float movementSpeed = 10;

    void Awake() {
        Utilities.Instance.Assert(animator, "CharacterController", "Awake", "animator not defined");
        Utilities.Instance.Assert(movementSpeed > 0, "CharacterController", "Awake", "movementSpeed not valid"); 
    }

    void Start() {
        animator.enabled = false;
        distance = 0;
        currentSpeed = 0;
    }

    void Update() {

        if (!Input.GetKey(KeyCode.LeftAlt) && !Input.GetKey(KeyCode.RightAlt) &&
            Input.GetMouseButtonDown(1) && GUIUtility.hotControl == 0) {
            animator.enabled = true;
            Plane playerPlane = new Plane(Vector3.up, transform.position);
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            float hitdist = 0.0f;

            if (playerPlane.Raycast(ray, out hitdist)) {
                Vector3 targetPoint = ray.GetPoint(hitdist);
                destination = ray.GetPoint(hitdist);
                Quaternion targetRotation = Quaternion.LookRotation(targetPoint - transform.position);
                transform.rotation = targetRotation;
            }
            currentSpeed = movementSpeed;
            distance = Vector3.Distance(destination, transform.position);
        }
        else
            currentSpeed = 0;

        animator.SetFloat("movementSpeed", currentSpeed);
        if (distance > .2f)
            transform.position = Vector3.MoveTowards(transform.position, destination, movementSpeed * Time.deltaTime);
    }

    public float MovementSpeed {
        get { return movementSpeed; }
        set { movementSpeed = value; }
    }
}