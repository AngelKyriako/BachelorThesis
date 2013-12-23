using UnityEngine;
using System.Collections.Generic;

public class ColorPicker: MonoBehaviour {

	void Awake () {
        enabled = false;
	}

    void Start() {
        string playerColor = GameManager.Instance.GetPlayerColor(name).ToString();
        Material coloredMaterial = (Material)Resources.Load(ResourcesPathManager.Instance.PlayerCharacterMaterialPath(playerColor));

        GameObject mesh;
        mesh = GameObject.Find(SceneHierarchyManager.Instance.PlayerCharacterMesh(name));
        mesh.renderer.material = coloredMaterial;

        if (name.Equals(GameManager.Instance.MyCharacter.name)) {
            mesh = GameObject.Find(SceneHierarchyManager.Instance.GUIPlayerCharacterMesh);
            mesh.renderer.material = coloredMaterial;
        }

        Destroy(this);
    }
}
