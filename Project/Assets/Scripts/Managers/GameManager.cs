using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public struct PlayerCharacterPair {
    public PhotonPlayer Player;
    public GameObject Character;

    public PlayerCharacterPair(PhotonPlayer _player, GameObject _character) {
        Player = _player;
        Character = _character;
    }
}

public class GameManager: SingletonPhotonMono<GameManager> {

    private GameObject gui;
    private PlayerCharacterPair me;
    private Dictionary<string, Pair<PhotonPlayer, PlayerCharacterModel>> all;

    private GameManager() { }

    void Awake() {
        gui = GameObject.Find("GUIScripts");
        all = new Dictionary<string, Pair<PhotonPlayer, PlayerCharacterModel>>();
        if (PhotonNetwork.connectionState.Equals(ConnectionState.Disconnected))
            InitGUIScripts();
    }

    void OnJoinedRoom() {
        Vector3 spawnPoint;
        if (!PhotonNetwork.isMasterClient)
            spawnPoint = GameObject.Find("SpawnPoint" + Random.Range(2, 10)).transform.position;
        else
            spawnPoint = GameObject.Find("SpawnPoint1").transform.position;
        PhotonNetwork.Instantiate(ResourcesPathManager.Instance.BabyDragonPath, spawnPoint, Quaternion.identity, 0);
        InitGUIScripts();
    }

    void OnLeaveRoom() {
    }

    public void HostAddToAll(string _name) {
        if (me.Player.isMasterClient)
            AddToAll(_name, me.Player);
        else
            photonView.RPC("AddToAll", PhotonNetwork.masterClient, _name, me.Player);
    }

    private void InitGUIScripts() {
        gui.GetComponent<ChatWindow>().enabled = true;
        gui.GetComponent<CharacterWindow>().enabled = true;
        gui.GetComponent<CharacterInfoPanel>().enabled = true;
    }

    private PlayerCharacterModel NewPlayerCharacterModel() {
        PlayerCharacterModel model;
        GameObject tempObj = new GameObject("tempObj");

        tempObj.AddComponent<PlayerCharacterModel>();
        model = tempObj.GetComponent<PlayerCharacterModel>();

        //MonoBehaviour.Destroy(tempObj);
        return model;
    }

    #region RPCs (To be used only by the master client)
    [RPC]
    private void AddToAll(string _name, PhotonPlayer _player) {
        if (!all.ContainsKey(_name))
            all.Add(_name, new Pair<PhotonPlayer, PlayerCharacterModel>(_player, NewPlayerCharacterModel()));
    }
    [RPC]
    private void RemoveFromAll(string _name) {
        if (all.ContainsKey(_name))
            all.Remove(_name);
    }

    [RPC]
    public bool AreAllies(string _name1, string _name2) {
        return GetPlayer(_name1).customProperties["team"].Equals(GetPlayer(_name2).customProperties["team"]);
    }
#endregion

    #region Accessors
    public GameObject Gui {
        get { return gui; }
    }

    public PlayerCharacterPair Me {
        get { return me; }
        set { me = value; }
    }
    public PhotonView MyPhotonView{
        get { return me.Character.GetComponent<NetworkController>().photonView; }
    }
    public PlayerCharacterModel MyCharacterModel {
        get { return me.Character.GetComponent<PlayerCharacterModel>(); }
    }

    public PhotonPlayer GetPlayer(string _name) {
        return all[_name].First;
    }
    public PlayerCharacterModel GetPlayerModel(string _name) {
        return all[_name].Second;
    }
#endregion
}