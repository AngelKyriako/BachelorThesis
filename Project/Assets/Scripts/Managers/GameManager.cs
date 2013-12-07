﻿using UnityEngine;
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

    void OnJoinedRoom() {//@TODO: Create all instances here with only PhotonPlayer objects
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

    private void StartGame() {//@TODO: photon Instantiate local Playercharacter objects and set all game objects to the maps
                              //       Maybe use a local Player object instead of the PhotonPlayer instance.
    }

    private void InitGUIScripts() {
        //gui.GetComponent<MouseCursor>().enabled = true;        
        gui.GetComponent<GameVariablesWindow>().enabled = true;
        gui.GetComponent<ChatWindow>().enabled = true;
        gui.GetComponent<CharacterWindow>().enabled = true;
        gui.GetComponent<CharacterInfoPanel>().enabled = true;
        gui.GetComponent<TerrainMap>().enabled = true;
        gui.GetComponent<PlayersInfoWindow>().enabled = true;
    }

    public void MasterClientRequestConnectedPlayers() {
        if(!PhotonNetwork.isMasterClient)
            photonView.RPC("RequestForPlayerCharacters", PhotonNetwork.masterClient);
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
    private void RequestForPlayerCharacters(PhotonMessageInfo info) {
        Utilities.Instance.PreCondition(PhotonNetwork.isMasterClient, "GameManager", "[RPC]MasterClientRequestForPlayerCharacters", "This RPC is only available for the master client.");
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
    public string GetPlayerName(string _name) {
        return all[_name].Player.name;
    }
    public PlayerTeam GetPlayerTeam(string _name) {
        return (PlayerTeam)all[_name].Player.customProperties["Team"];
    }
    public PlayerColor GetPlayerColor(string _name) {
        return (PlayerColor)all[_name].Player.customProperties["Color"];
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
    public bool IsPlayerCharacterObject(string _name) {
        return all.ContainsKey(_name);
    }
#endregion
}