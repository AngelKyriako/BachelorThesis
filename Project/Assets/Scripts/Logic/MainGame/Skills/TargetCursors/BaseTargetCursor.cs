using UnityEngine;
using System.Collections;

public class BaseTargetCursor: MonoBehaviour {

    public LayerMask ignoredLayers;

    void Awake() {
        enabled = false;
    }

    //@TODO: Add color, range, intensity, angle for light !!!
    public void SetUpTargetCursor() {
        enabled = true;
    }

	void Update () {
        //Ray cameraToMouseRay = Camera.main.ScreenPointToRay(PlayerInputManager.Instance.MousePosition);
        //float ScreenDepth = 0;
        //RaycastHit hitInfo;
        //if (Physics.Raycast(cameraToMouseRay, out hitInfo)) {
        //    ScreenDepth = Vector3.Distance(Camera.main.transform.position, hitInfo.point);
        //    Debug.DrawLine(Camera.main.transform.position, hitInfo.point, Color.blue);
        //    Utilities.Instance.LogMessageToChat("Camera distance from point(ray): " + ScreenDepth);
        //}
        //if (Physics.Raycast(Camera.main.transform.position, Camera.main.transform.forward, out hitInfo)) {
        //    ScreenDepth = Vector3.Distance(Camera.main.transform.position, hitInfo.point);
        //    Debug.DrawLine(Camera.main.transform.position, hitInfo.point, Color.red);
        //    Utilities.Instance.LogMessageToChat("Camera distance from point(forward): " + ScreenDepth);
        //}
        Vector3 mousePos = PlayerInputManager.Instance.MousePosition;
        mousePos.z = Camera.main.transform.position.y;

        Vector3 worldMousePos = Camera.main.ScreenToWorldPoint(mousePos);

        if (mousePos.y < Screen.height / 2)//@TODO: Modify this mpakalia !!!
            worldMousePos.z -= (Screen.height / 2 - mousePos.y)/100;
        transform.position = new Vector3(worldMousePos.x, GameManager.Instance.MyCharacterModel.ProjectileOriginPosition.y, worldMousePos.z);
	}

    public void DestroyTargetCursor() {
        Destroy(gameObject);
    }

    public virtual Vector3 Destination {
        get { return transform.position; }
    }
}
