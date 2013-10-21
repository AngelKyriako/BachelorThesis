using UnityEngine;
using System.Collections;

public class CharacterController: MonoBehaviour {

    private bool isLocal;
    private Vector3 destination;
    private float distance;

    public float moveSpeed = 10;

    void Start() {
        destination = transform.position;
    }

    void Update() {
            distance = Vector3.Distance(destination, transform.position);

            if (!Input.GetKey(KeyCode.LeftAlt) && !Input.GetKey(KeyCode.RightAlt) &&
                Input.GetMouseButtonDown(1) && GUIUtility.hotControl == 0) {

                Plane playerPlane = new Plane(Vector3.up, transform.position);
                Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
                float hitdist = 0.0f;

                if (playerPlane.Raycast(ray, out hitdist)) {
                    Vector3 targetPoint = ray.GetPoint(hitdist);
                    destination = ray.GetPoint(hitdist);
                    Quaternion targetRotation = Quaternion.LookRotation(targetPoint - transform.position);
                    transform.rotation = targetRotation;
                }
            }

            //// Moves the player if the mouse button is hold down
            //else if (Input.GetMouseButton(1) && GUIUtility.hotControl == 0) {

            //    Plane playerPlane = new Plane(Vector3.up, transform.position);
            //    Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            //    float hitdist = 0.0f;

            //    if (playerPlane.Raycast(ray, out hitdist)) {
            //        Vector3 targetPoint = ray.GetPoint(hitdist);
            //        destination = ray.GetPoint(hitdist);
            //        Quaternion targetRotation = Quaternion.LookRotation(targetPoint - transform.position);
            //        transform.rotation = targetRotation;
            //    }
            //    //	transform.position = Vector3.MoveTowards(transform.position, destinationPosition, moveSpeed * Time.deltaTime);
            //}

            // To prevent code from running if not needed
            if (distance > .5f) {
                transform.position = Vector3.MoveTowards(transform.position, destination, moveSpeed * Time.deltaTime);
            }
    }

    public void SetIsLocal(bool b) { isLocal = b; }
}