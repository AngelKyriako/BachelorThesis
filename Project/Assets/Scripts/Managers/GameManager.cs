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
    private PlayerCharacterPair masterClient;
    private Dictionary<string, PlayerCharacterPair> all;

    private GameManager() { }

    void Awake() {
        gui = GameObject.Find("GUIScripts");
        all = new Dictionary<string, PlayerCharacterPair>();
        if (PhotonNetwork.connectionState.Equals(ConnectionState.Disconnected))
            InitGUIScripts();
    }

    void OnJoinedRoom() {
        Vector3 spawnPoint;
        if (!PhotonNetwork.isMasterClient)
            spawnPoint = GameObject.Find("SpawnPoint" + Random.Range(2, 10)).transform.position;
        else
            spawnPoint = GameObject.Find("SpawnPoint1").transform.position;
        PhotonNetwork.Instantiate(ResourcesPathManager.Instance.PlayerCharacterPath, spawnPoint, Quaternion.identity, 0);
        InitGUIScripts();
    }

    void OnLeaveRoom() {
    }

    private void InitGUIScripts() {
        gui.GetComponent<ChatWindow>().enabled = true;
        gui.GetComponent<CharacterWindow>().enabled = true;
        gui.GetComponent<CharacterInfoPanel>().enabled = true;
        gui.GetComponent<TerrainMap>().enabled = true;
    }

    public void RequestConnectedPlayerCharacters() {
        Utilities.Instance.LogMessage("Requesting for Connected players from master client");
        photonView.RPC("SendPlayerCharacters", PhotonNetwork.masterClient);
    }
    public void RemovePlayerCharacter(string _name) {
        if (all.ContainsKey(_name))
            all.Remove(_name);
    }

    #region RPCs
    [RPC]
    public void AddPlayerCharacter(string _name, PhotonPlayer _player) {
        if (!all.ContainsKey(_name))
            all.Add(_name, new PlayerCharacterPair(_player, GameObject.Find(SceneHierarchyManager.Instance.PlayerCharacterPath +"/"+_name)));            
    }
    [RPC]
    private void SetMasterClient(string _name) {
        masterClient = all[_name];
    }
    [RPC]
    private void SendPlayerCharacters(PhotonMessageInfo info) {
        Utilities.Instance.PreCondition(PhotonNetwork.isMasterClient, "GameManager", "[RPC]SendPlayerCharacters", "This RPC is only available for the master client.");
        foreach (string _name in all.Keys) {
            photonView.RPC("AddPlayerCharacter", info.sender, _name, all[_name].Player);
        }
        photonView.RPC("SetMasterClient", info.sender, photonView.name);
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
    public PhotonPlayer MyPlayer {
        get { return me.Player; }
    }
    public GameObject MyCharacter {
        get { return me.Character; }
    }
    public PlayerCharacterModel MyCharacterModel {
        get { return me.Character.GetComponent<PlayerCharacterModel>(); }
    }
    public PhotonView MyPhotonView{
        get { return me.Character.GetComponent<PlayerCharacterNetworkController>().photonView; }
    }

    public PlayerCharacterPair MasterClient {
        get { return masterClient; }
        set { masterClient = value; }
    }
    public PlayerCharacterNetworkController MasterClientNetworkController {
        get { return masterClient.Character.GetComponent<PlayerCharacterNetworkController>(); }
    }
    public PhotonView MasterClientPhotonView {
        get { return masterClient.Character.GetComponent<PlayerCharacterNetworkController>().photonView; }
    }

    public ICollection<string> AllPlayerKeys {
        get { return all.Keys; }
    }
    public PhotonPlayer GetPlayer(string _name) {
        return all[_name].Player;
    }
    public GameObject GetCharacter(string _name) {
        return all[_name].Character;
    }
    public PlayerCharacterModel GetPlayerModel(string _name) {
        return all[_name].Character.GetComponent<PlayerCharacterModel>();
    }
    public PhotonView GetPlayerPhotonView(string _name) {
        return all[_name].Character.GetComponent<PlayerCharacterNetworkController>().photonView;
    }
#endregion
}