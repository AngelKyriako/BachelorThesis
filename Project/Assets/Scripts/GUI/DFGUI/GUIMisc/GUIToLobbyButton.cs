using UnityEngine;

public class GUIToLobbyButton: MonoBehaviour {

    void OnClick() {
        PhotonNetwork.LoadLevel("Lobby");
    }
}
