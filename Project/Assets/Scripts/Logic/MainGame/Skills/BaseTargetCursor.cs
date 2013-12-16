using UnityEngine;
using System.Collections;

public class BaseTargetCursor: MonoBehaviour {

    private BaseSkill skill;
    public LayerMask ignoredLayers;

    void Awake() {
        enabled = false;
    }

    public void SetUpTargetCursor(BaseSkill _skill) {
        skill = _skill;
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

        if (mousePos.y < Screen.height / 2)//@TODO: Modify this mpakalia !!!(Need to restrict the target cursor to a minimum distance from the player object)
            worldMousePos.z -= (Screen.height / 2 - mousePos.y)/100;
        transform.position = new Vector3(worldMousePos.x, skill.OwnerModel.ProjectileOriginPosition.y, worldMousePos.z);
	}

    public void DestroyTargetCursor() {
        Destroy(gameObject);
    }

    public virtual Vector3 Direction {
        get { return transform.position - skill.OwnerModel.transform.position; }
    }
}
