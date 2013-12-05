using UnityEngine;
using System.Collections;

public class SerializableNetController: BaseNetController {

    #region attributes
    private float syncTime, syncDelay, lastSynchronizationTime;
    private Vector3 correctPlayerPosition;
    private Quaternion correctPlayerRotation;
    #endregion

    public override void Awake() {
        base.Awake();
        name = photonView.viewID.ToString();
    }

    public virtual void Start() {
        correctPlayerPosition = transform.position;
        correctPlayerRotation = Quaternion.identity;

        syncTime = syncDelay = 0.001f;
        lastSynchronizationTime = Time.time;
    }

    public virtual void Update() {
        if (!IsLocalClient)
            SyncWithRemote();
    }

    public void OnPhotonSerializeView(PhotonStream _stream, PhotonMessageInfo _info) {
        if (_stream.isWriting)
            SendData(_stream);
        else
            ReceiveData(_stream);
    }

    public virtual void SendData(PhotonStream _stream) {
        _stream.SendNext(transform.position);
        _stream.SendNext(transform.rotation);
    }

    public virtual void ReceiveData(PhotonStream _stream) {
        syncTime = 0.0001f;
        syncDelay = (Time.time - lastSynchronizationTime) + 0.0001f;
        lastSynchronizationTime = Time.time;

        correctPlayerPosition = (Vector3)_stream.ReceiveNext();
        correctPlayerRotation = (Quaternion)_stream.ReceiveNext();
    }

    public virtual void SyncWithRemote() {
        //Utilities.Instance.PreCondition((!float.IsNaN(correctPlayerPosition.x) && !float.IsNaN(correctPlayerPosition.y) && !float.IsNaN(correctPlayerPosition.z)),
        //                                "PlayerCharacterPlayerCharacterNetworkController", "SyncRemoteCharacter", "Fucking NaN Values: " + correctPlayerPosition);
        //Utilities.Instance.PreCondition((!float.IsNaN(correctPlayerRotation.x) && !float.IsNaN(correctPlayerRotation.y) && !float.IsNaN(correctPlayerRotation.z) && !float.IsNaN(correctPlayerRotation.w)),
        //                                "PlayerCharacterPlayerCharacterNetworkController", "SyncRemoteCharacter", "Fucking NaN Values: " + correctPlayerRotation);
        //Utilities.Instance.PreCondition(!float.IsNaN(syncTime / syncDelay), "PlayerCharacterPlayerCharacterNetworkController", "SyncRemoteCharacter", "Fucking NaN!!! syncTime: " + syncTime + ", syncDelay" + syncDelay);

        transform.position = Vector3.Lerp(transform.position, correctPlayerPosition, syncTime / syncDelay);
        transform.rotation = Quaternion.Lerp(transform.rotation, correctPlayerRotation, syncTime / syncDelay);

        syncTime += Time.deltaTime;
    }
}
