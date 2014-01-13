using UnityEngine;

public class GUIToLobbyButton: MonoBehaviour {

    void OnClick() {
        SkillBook.Instance.RefreshAllSkills();
        PhotonNetwork.LoadLevel("Lobby");
    }
}
