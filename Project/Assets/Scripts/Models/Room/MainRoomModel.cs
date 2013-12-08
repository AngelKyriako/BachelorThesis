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
    Team10,
}

public enum PlayerColor {
    Red,
    Blue,
    Green,
    Purple,
    Yellow,
    Pink,
    Orange,
    Brown,
    Gray,
    Black
}

public class MainRoomModel {

    private Pair<PlayerColor, string>[] playerSlots;
    private PlayerTeam[] availableTeams;

    private static MainRoomModel instance = new MainRoomModel();
    public static MainRoomModel Instance {
        get { return MainRoomModel.instance; }
    }

    private MainRoomModel() {
        playerSlots = new Pair<PlayerColor, string>[PlayerSlotsCount];
        for (int i = 0; i < PlayerSlotsLength; ++i)
            playerSlots[i] = new Pair<PlayerColor, string>((PlayerColor)i, string.Empty);

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
    //slots
    public void SetPlayerNameInSlot(int _index, string playerName) {
        playerSlots[_index].Second = playerName;
    }
    public string GetPlayerNameInSlot(int _index) {
        return playerSlots[_index].Second;
    }
    public void EmptySlot(int _index) {
        playerSlots[_index].Second = string.Empty;
    }
    public bool IsSlotEmpty(int _index) {
        return playerSlots[_index].Equals(string.Empty);
    }
    public PlayerColor GetSlotColor(int _index) {
        return playerSlots[_index].First;
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
    
    public bool SlotOwnedByPlayer(int _slotNum, string _playerName) {
        return GetPlayerNameInSlot(_slotNum).Equals(_playerName);
    }
    #endregion
}