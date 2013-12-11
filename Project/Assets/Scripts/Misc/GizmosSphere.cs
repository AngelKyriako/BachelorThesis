using UnityEngine;
using System.Collections;

public class GizmosSphere: MonoBehaviour {

    public Color color;

    private void OnDrawGizmos() {
        Gizmos.color = color;
        Gizmos.DrawSphere(transform.position, 1f);
    }
}