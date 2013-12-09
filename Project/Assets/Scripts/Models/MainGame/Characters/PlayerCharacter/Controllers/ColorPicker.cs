using UnityEngine;
using System.Collections.Generic;

public class ColorPicker: MonoBehaviour {

	void Awake () {
        enabled = false;
        Utilities.Instance.LogMessage(ResourcesPathManager.Instance.PlayerCharacterMaterialPath(((PlayerColor)GameManager.Instance.MyPlayer.customProperties["Color"]).ToString()));

	}

    void Start() {
        GameObject mesh = GameObject.Find(SceneHierarchyManager.Instance.PlayerCharacterMesh(name));
        string playerColorName = ((PlayerColor)GameManager.Instance.GetPlayer(name).customProperties["Color"]).ToString();

        mesh.renderer.material = (Material)Resources.Load(ResourcesPathManager.Instance.PlayerCharacterMaterialPath(playerColorName));

        Destroy(this);
    }
}
