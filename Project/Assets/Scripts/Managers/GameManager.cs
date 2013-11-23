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
    private Dictionary<string, PlayerCharacterPair> others;

    private GameManager() { }

    void Awake() {
        gui = GameObject.Find("GUIScripts");
        others = new Dictionary<string, PlayerCharacterPair>();
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

    private void InitGUIScripts() {
        gui.GetComponent<ChatWindow>().enabled = true;
        gui.GetComponent<CharacterWindow>().enabled = true;
        gui.GetComponent<CharacterInfoPanel>().enabled = true;
    }
    
    private void LogPlayers() {
        string allStr = string.Empty;
        foreach (KeyValuePair<string, PlayerCharacterPair> entry in others)
            allStr += entry.Key + " ";
        Utilities.Instance.LogMessage("PlayerCharacter: " + me.Player.name);
        Utilities.Instance.LogMessage("Allies: " + allStr);
    }
    
#region Accessors
    public GameObject Gui {
        get { return gui; }
    }

    public PlayerCharacterPair Me {
        get { return me; }
        set { me = value; }
    }

    [RPC]
    public void AddPlayerCharacter(PlayerCharacterPair playerCharacterPair) {
        if (!others.ContainsKey(playerCharacterPair.Player.name))
            others.Add(name, playerCharacterPair);
        LogPlayers();
    }
    [RPC]
    public void RemovePlayerCharacter(string name) {
        if (others.ContainsKey(name))
            others.Remove(name);
    }
    public bool IsAlly(string name) {
        return others[name].Player.customProperties["team"].
                        Equals(me.Player.customProperties["team"]);
    }
    public PlayerCharacterPair GetPlayerCharacter(string name) {
        return others[name];
    }
#endregion
}