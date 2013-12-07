public class BaseNetController: Photon.MonoBehaviour {

    public virtual void Awake() {
        Utilities.Instance.Assert(photonView != null, "BaseNetController", "Awake", "No photonView attached");
        if (PhotonNetwork.connectionState.Equals(ConnectionState.Disconnected))
            PhotonNetwork.offlineMode = true;	
	}

    void OnJoinedRoom() { enabled = true; }
    void OnCreatedRoom() { enabled = true; }
    void OnLeftRoom() { enabled = false; }

    public bool IsLocalClient {
        get { return photonView.isMine; }
    }
        public bool IsMasterClient {
        get { return PhotonNetwork.isMasterClient; }
    }
}
