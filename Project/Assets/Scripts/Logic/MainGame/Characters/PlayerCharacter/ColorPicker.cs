using UnityEngine;
using System.Collections.Generic;

public class ColorPicker: MonoBehaviour {

	void Awake () {
        enabled = false;
	}

    void Start() {
        string playerColorName = ((PlayerColor)GameManager.Instance.GetPlayer(GameManager.Instance.MyCharacter.name).customProperties["Color"]).ToString();
        Material coloredMaterial = (Material)Resources.Load(ResourcesPathManager.Instance.PlayerCharacterMaterialPath(playerColorName));

        GameObject mesh;
        mesh = GameObject.Find(SceneHierarchyManager.Instance.PlayerCharacterMesh(name));
        mesh.renderer.material = coloredMaterial;

        mesh = GameObject.Find(SceneHierarchyManager.Instance.GUIPlayerCharacterMesh);
        mesh.renderer.material = coloredMaterial;

        Destroy(this);
    }
}
