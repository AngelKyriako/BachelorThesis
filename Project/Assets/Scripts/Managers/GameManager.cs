using UnityEngine;
using ExitGames.Client.Photon;
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
        PhotonNetwork.Instantiate(ResourcesPathManager.Instance.PlayerCharacterPrefabPath, Vector3.zero, Quaternion.identity, 0);
        InitRoomScripts();
    }

    private void InitRoomScripts() {
        GameObject roomScripts = GameObject.Find("RoomScripts");
        roomScripts.GetComponent<RoomNetController>().enabled = true;
        roomScripts.GetComponent<MainRoomGUI>().enabled = true;        
    }

    public bool AllPlayersReady() {
        foreach (PhotonPlayer player in PhotonNetwork.playerList) {
            //Utilities.Instance.LogMessage("---------- " + player.ID + " ----------");
            //Utilities.Instance.LogMessage("Has Slot" + MainRoomModel.Instance.SlotOwnedByPlayer((int)player.customProperties["Color"], player));
            //Utilities.Instance.LogMessage("Player IsReady" + (bool)player.customProperties["IsReady"]);            
            //Utilities.Instance.LogMessage("Player Color: " + (PlayerColor)player.customProperties["Color"]);
            //Utilities.Instance.LogMessage("Player Team: " + (PlayerTeam)player.customProperties["Team"]);            
            if (!(bool)player.customProperties["IsReady"] ||
                !MainRoomModel.Instance.SlotOwnedByPlayer((int)player.customProperties["Color"], player))
                return false;
        }
        return true;
    }

    public void MasterClientLoadMainStage() {
        Utilities.Instance.PreCondition(PhotonNetwork.isMasterClient, "GameManager", "MasterClientLoadMainStage", "This function is only available for the master client.");
        LoadMainStage(GameVariables.Instance.Map.Key);
        photonView.RPC("LoadMainStage", PhotonTargets.Others, GameVariables.Instance.Map.Key);
    }

    public void InitMainStage() {
        gameObject.AddComponent<CombatManager>();        
        foreach (string _name in AllPlayerKeys)
            GetPlayerNetController(_name).SetUp();

        TeleportManager.Instance.TeleportMeToHeaven();
        InitGUIScripts();
    }

    private void InitGUIScripts() {
        //gui.AddComponent<MouseCursor>().enabled = true;     
        gui.AddComponent<GamePreferencesWindow>().enabled = true;
        gui.GetComponent<CharacterWindow>().enabled = true;
        gui.AddComponent<CharacterInfoPanel>().enabled = true;
        gui.AddComponent<TerrainMap>().enabled = true;
        gui.AddComponent<PlayersInfoWindow>().enabled = true;
        gui.AddComponent<FPSCounter>().enabled = true;
    }

    public void MasterClientRequestConnectedPlayers() {
        if(!PhotonNetwork.isMasterClient)
            photonView.RPC("RequestForPlayerCharacters", PhotonNetwork.masterClient);
    }
    public void RemovePlayerCharacter(string _name) {
        if (all.ContainsKey(_name))
            all.Remove(_name);
    }

    void OnLeaveRoom() {
    }

    #region Player properties updates
    public void UpdatePlayerTeamProperty() {
        MyPlayer.SetCustomProperties(
            new Hashtable() { { "Team", (PlayerTeam)GameManager.Instance.MyPlayer.customProperties["Team"] } });
    }

    public void UpdatePlayerColorProperty() {
        MyPlayer.SetCustomProperties(
            new Hashtable() { { "Color", (PlayerColor)GameManager.Instance.MyPlayer.customProperties["Color"] } });
    }

    public void UpdatePlayerIsReadyProperty() {
        MyPlayer.SetCustomProperties(
            new Hashtable() { { "IsReady", (bool)GameManager.Instance.MyPlayer.customProperties["IsReady"] } });
    }
    #endregion

    #region Room properties updates
    public void UpdateRoomProperties() {
        //PhotonNetwork.room.SetCustomProperties(new Hashtable() { 
        //    {"Mode", (GameMode)PhotonNetwork.room.customProperties["Mode"]},
        //    {"Map", (GameMap)PhotonNetwork.room.customProperties["Map"]},
        //    {"Difficulty", (GameDifficulty)PhotonNetwork.room.customProperties["Difficulty"]},
        //    {"TargetKills", (int)PhotonNetwork.room.customProperties["TargetKills"]},
        //    {"Timer", (double)PhotonNetwork.room.customProperties["Timer"]}
        //});
    }
    #endregion

    #region RPCs
    [RPC]
    public void LoadMainStage(string _stage) {
        PhotonNetwork.LoadLevel(_stage);
    }

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
        Utilities.Instance.PreCondition(PhotonNetwork.isMasterClient, "GameManager", "[RPC]RequestForPlayerCharacters", "This RPC is only available for the master client.");
        foreach (string _name in all.Keys)
            photonView.RPC("AddPlayerCharacter", info.sender, _name, all[_name].Player);
        photonView.RPC("SetMasterClient", info.sender, MyPhotonView.name);
    }
#endregion

    #region Accessors
    public GameObject Gui {
        get { return gui; }
    }
    // me
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
    public PlayerCharacterNetworkController MyNetworkController {
        get { return me.Character.GetComponent<PlayerCharacterNetworkController>(); }
    }
    public PhotonView MyPhotonView {
        get { return MyNetworkController.photonView; }
    }
    public PlayerCharacterModel MyCharacterModel {
        get { return me.Character.GetComponent<PlayerCharacterModel>(); }
    }
    public CameraController MyCameraController {
        get { return me.Character.GetComponent<CameraController>(); }
    }
    public MovementController MyMovementController {
        get { return me.Character.GetComponent<MovementController>(); }
    }
    public PlayerCharacterDeathController MyDeathController {
        get { return me.Character.GetComponent<PlayerCharacterDeathController>(); }
    }
    // master client
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
    // player
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
    // character game object
    public GameObject GetCharacter(string _name) {
        return all[_name].Character;
    }
    public PlayerCharacterModel GetPlayerModel(string _name) {
        return all[_name].Character.GetComponent<PlayerCharacterModel>();
    }
    public PlayerCharacterNetworkController GetPlayerNetController(string _name) {
        return all[_name].Character.GetComponent<PlayerCharacterNetworkController>();
    }
    public PhotonView GetPlayerPhotonView(string _name) {
        return all[_name].Character.GetComponent<PlayerCharacterNetworkController>().photonView;
    }
    public bool IsPlayerCharacterObject(string _name) {
        return all.ContainsKey(_name);
    }
    //Pair
    public PlayerCharacterPair GetPlayerCharacterPair(string _name) {
        return all[_name];
    }
#endregion
}