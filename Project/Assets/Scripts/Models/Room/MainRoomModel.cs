using System;
using System.Collections.Generic;

public enum PlayerTeam {
    Team1,
    Team2,
    Team3,
    Team4,
    Team5,
    Team6,
    Team7,
    Team8,
    Team9,
    Team10
}

public enum PlayerColor {
    Blue,
    Purple,
    Red,
    Orange,
    Pink,
    Green,
    Yellow,
    Gray,
    White,
    Black,
    None
}

public struct RoomSlot {
    public PlayerColor Color;
    public PhotonPlayer Player;

    public RoomSlot(PlayerColor _color, PhotonPlayer _player) {
        Color = _color;
        Player = _player;
    }
}

public class MainRoomModel {

    private PlayerColor mySlot;
    private PlayerTeam myTeam;
    private RoomSlot[] playerSlots;
    private PlayerTeam[] availableTeams;

    private static MainRoomModel instance = new MainRoomModel();
    public static MainRoomModel Instance {
        get { return MainRoomModel.instance; }
    }

    private MainRoomModel() {
        mySlot = PlayerColor.None;
        myTeam = default(PlayerTeam);
        playerSlots = new RoomSlot[PlayerSlotsCount - 1];
        for (int i = 0; i < PlayerSlotsLength; ++i)
            playerSlots[i] = new RoomSlot((PlayerColor)i, null);

        availableTeams = new PlayerTeam[TeamsCount];
        for (int i = 0; i < availableTeams.Length; ++i)
            availableTeams[i] = (PlayerTeam)i;
    }


    private int PlayerSlotsCount {
        get { return Enum.GetValues(typeof(PlayerColor)).Length; }
    }
    private int TeamsCount {
        get { return Enum.GetValues(typeof(PlayerTeam)).Length; }
    }

    #region Accessors
    //Mine
    public PlayerColor MySlot {
        get { return mySlot; }
        set { mySlot = value; }
    }
    public PlayerTeam MyTeam {
        get { return myTeam; }
        set { myTeam = value; }
    }
    //slots
    public void SetPlayerInSlot(int _index, PhotonPlayer _player) {
        playerSlots[_index].Player = _player;
    }
    public PhotonPlayer GetPlayerInSlot(int _index) {
        return playerSlots[_index].Player;
    }
    public string GetPlayerNameInSlot(int _index) {
        return (playerSlots[_index].Player != null) ? playerSlots[_index].Player.name : string.Empty;
    }
    public int GetPlayerPhotonViewIdInSlot(int _index) {
        return playerSlots[_index].Player.ID;
    }
    public void EmptySlot(int _index) {
        playerSlots[_index].Player = null;
    }
    public bool IsSlotEmpty(int _index) {
        return playerSlots[_index].Player == null;
    }
    public PlayerColor GetSlotColor(int _index) {
        return playerSlots[_index].Color;
    }
    public int PlayerSlotsLength {
        get { return playerSlots.Length; }
    }
    //teams
    public PlayerTeam[] AvailableTeams {
        get { return availableTeams; }
    }
    public PlayerTeam GetAvailableTeam(int _index) {
        return availableTeams[_index];
    }
    public int AvailableTeamsLength {
        get { return availableTeams.Length; }
    }

    public bool SlotOwnedByPlayer(int _slotNum, PhotonPlayer _player) {
        return _slotNum != (int)PlayerColor.None && _player.Equals(GetPlayerInSlot(_slotNum));
    }
    public bool LocalClientOwnsSlot {
        get { return !mySlot.Equals(PlayerColor.None); }
    }
    #endregion
}