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
    private Dictionary<string, bool> allies;
    private Dictionary<string, PlayerCharacterPair> all;
    private int[] teamKills;

    private GameManager() { }

    void Awake() {
        gui = GameObject.Find("GUIScripts");
        all = new Dictionary<string, PlayerCharacterPair>();
        allies = new Dictionary<string, bool>();
        teamKills = new int[MainRoomModel.Instance.AvailableTeamsLength];
        for (int i = 0; i < MainRoomModel.Instance.AvailableTeamsLength; ++i)
            teamKills[i] = 0;

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
        Utilities.Instance.PreCondition(PhotonNetwork.isMasterClient, "GameManager", "AllPlayersReady", "This method is only available for the master client.");
        foreach (PhotonPlayer player in PhotonNetwork.playerList)           
            if (!(bool)player.customProperties["IsReady"] ||
                !MainRoomModel.Instance.SlotOwnedByPlayer((int)player.customProperties["Color"], player))
                return false;
        return true;
    }

    public void MasterClientLoadMainStage() {
        Utilities.Instance.PreCondition(PhotonNetwork.isMasterClient, "GameManager", "MasterClientLoadMainStage", "This method is only available for the master client.");
        LoadMainStage(GameVariables.Instance.Map.Key);
        photonView.RPC("LoadMainStage", PhotonTargets.Others, GameVariables.Instance.Map.Key);
    }

    public void InitMainStage() {
        CameraManager.Instance.SetUp();
        gameObject.AddComponent<CombatManager>();

        RegisterAllies();
        InitNetworkControllers();
        InitGUIScripts();
        SpawnPlayerCharacter();
        
    }

    private void RegisterAllies() {
        foreach (string _name in AllPlayerKeys) {
            if (CombatManager.Instance.AreAllies(_name, MyCharacter.name))
                allies.Add(_name, true);
        }
    }

    private void InitNetworkControllers() {
        foreach (string _name in AllPlayerKeys)
            GetPlayerNetController(_name).SetUp();
    }

    private void InitGUIScripts() {
        gui.AddComponent<DFPSCounter>().enabled = true;
    }

    private void SpawnPlayerCharacter() {
        MyCharacterModel.RespawnTimer = GameVariables.Instance.Timer.Value;
        MyDeathController.enabled = true;
        TeleportManager.Instance.StandardTeleportation(false);
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
        Destroy(gui);
        Destroy(gameObject);
    }

    #region Conquerors winning conditions
    public void CheckConquerorsWinningConditions(string _killerName) {
        Utilities.Instance.PreCondition(PhotonNetwork.isMasterClient, "GameManager", "CheckWinningConditionsOnKill", "This method is only available for the master client.");
        
        if (GameVariables.Instance.Mode.Value.Equals(GameMode.Conquerors) && KillersTeamReachedTargetKills(_killerName)){
            GameOver((int)GetPlayerTeam(_killerName));
            photonView.RPC("GameOver", PhotonTargets.Others, (int)GetPlayerTeam(_killerName));
        }
    }

    private bool KillersTeamReachedTargetKills(string _killer) {
        return PlayersTeamKills(_killer) >= GameVariables.Instance.TargetKills.Value;
    }
    #endregion

    #region BattleRoyal winning conditions
    public void CheckBattleRoyalWinningConditions() {
        Utilities.Instance.PreCondition(PhotonNetwork.isMasterClient, "GameManager", "CheckWinningConditionsOnKill", "This method is only available for the master client.");

        if ((GameVariables.Instance.Mode.Value.Equals(GameMode.BattleRoyal) && OneTeamStandingAlone())) {
            GameOver((int)GetFirstAlivePlayersTeam);
            photonView.RPC("GameOver", PhotonTargets.Others, (int)GetFirstAlivePlayersTeam);
        }
    }

    //returns false if players with different teams are alive
    private bool OneTeamStandingAlone() {
        PlayerTeam _team = GetFirstAlivePlayersTeam;
        foreach (string _name in AllPlayerKeys) {
            if (GetPlayerModel(_name).IsAlive && !_team.Equals(GetPlayerTeam(_name)))
                return false;
        }
        return true;
    }

    private PlayerTeam GetFirstAlivePlayersTeam {
        get {
            foreach (string _name in AllPlayerKeys)
                if (GetPlayerModel(_name).IsAlive)
                    return GetPlayerTeam(_name);
            return PlayerTeam.Team1;
        }
    }
    #endregion

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
            all.Add(_name, new PlayerCharacterPair(_player, GameObject.Find(SceneHierarchyManager.Instance.PlayerCharacterPath + "/" + _name)));
    }
    [RPC]
    private void SetMasterClient(string _name) {
        masterClient = all[_name];
    }
    [RPC]
    private void RequestForPlayerCharacters(PhotonMessageInfo info) {
        Utilities.Instance.PreCondition(PhotonNetwork.isMasterClient, "GameManager", "[RPC]RequestForPlayerCharacters", "This RPC is only available for the master client.");
        foreach (string _name in AllPlayerKeys)
            photonView.RPC("AddPlayerCharacter", info.sender, _name, all[_name].Player);
        photonView.RPC("SetMasterClient", info.sender, MyCharacter.name);
    }
    [RPC]
    private void GameOver(int winnerTeam) {
        PhotonNetwork.LoadLevel("GameOver");

        CameraManager.Instance.enabled = false;

        gameObject.AddComponent<GameOverManager>();
        GameOverManager.Instance.SetUp((PlayerTeam)winnerTeam);        
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
    public Color MyColor {
        get { return ColorHolder.Instance.GetPlayerColor(MyPlayerColor); }
    }
    public PlayerColor MyPlayerColor {
        get { return (PlayerColor)me.Player.customProperties["Color"]; }
    }
    public PlayerTeam MyTeam {
        get { return (PlayerTeam)me.Player.customProperties["Team"]; }
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
    public MovementController MyMovementController {
        get { return me.Character.GetComponent<MovementController>(); }
    }
    public PlayerCharacterDeathController MyDeathController {
        get { return me.Character.GetComponent<PlayerCharacterDeathController>(); }
    }
    public bool ItsMe(string _name) {
        return _name.Equals(MyCharacter.name);
    }
    public bool IsMyTeam(PlayerTeam _team) {
        return MyTeam.Equals(_team);
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
    public bool PlayerExists(string _name) {
        return all.ContainsKey(_name);
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
    public Color GetPlayerRGBColor(string _name) {
        return ColorHolder.Instance.GetPlayerColor(GetPlayerColor(_name));
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
    //player and character pair
    public PlayerCharacterPair GetPlayerCharacterPair(string _name) {
        return all[_name];
    }
    //Teams
    public bool IsAlly(string _name) {
        return allies.ContainsKey(_name);
    }
    public void RaiseKillsOfPlayersTeam(string _name) {
        ++teamKills[(int)GetPlayerTeam(_name)];
    }
    public int PlayersTeamKills(string _name) {
        return teamKills[(int)GetPlayerTeam(_name)];
    }
    public int TeamKills(int _teamIndex) {
        return teamKills[_teamIndex];
    }
    public int TeamsCount {
        get { return teamKills.Length; }
    }
#endregion
}
