using UnityEngine;
using System.Collections;

public class GizmosSphere: MonoBehaviour {

    private void OnDrawGizmos() {
        Gizmos.color = Color.red;
        Gizmos.DrawSphere(transform.position, 1f);
    }
}