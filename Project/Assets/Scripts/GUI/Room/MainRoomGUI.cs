using UnityEngine;
using System.Collections;

public class MainRoomGUI: MonoBehaviour {

    private const int SOUTH_HEIGHT = 200, SOUTH_BUTTONS_WIDTH = 500,
                      PREFERENCES_WIDTH = 400;

    private Rect westPreferencesRect,
                 eastSlotsRect, playerSlotRect,
                 southChatRect, southButtonRect;

    private RoomNetController networkController;

	void Awake () {
        networkController = GetComponent<RoomNetController>();
        networkController.MasterClientRequestForRoomState();
	}

    void Start() {
        #region Init Rects
        westPreferencesRect = new Rect(0, 0, PREFERENCES_WIDTH, Screen.height - SOUTH_HEIGHT);

        eastSlotsRect = new Rect(westPreferencesRect.width, 0, Screen.width - westPreferencesRect.width, Screen.height - SOUTH_HEIGHT);
        playerSlotRect = new Rect(0, 0, eastSlotsRect.width, eastSlotsRect.height / MainRoomModel.Instance.PlayerSlotsLength);

        southChatRect = new Rect(0, Screen.height - SOUTH_HEIGHT, Screen.width - SOUTH_BUTTONS_WIDTH, SOUTH_HEIGHT);
        southButtonRect = new Rect(southChatRect.width, Screen.height - SOUTH_HEIGHT, SOUTH_BUTTONS_WIDTH, SOUTH_HEIGHT);
        #endregion
    }
	
	void Update () {
	
	}

    void OnGUI() {
        if (!networkController.IsMasterClient)
            WestGamePreferencesViewed();
        else
            WestGamePreferencesEditable();
        EastPlayerSlots();
        SouthChat();
        SouthButtons();
    }

    private void WestGamePreferencesEditable() {
        GUILayout.BeginArea(westPreferencesRect);
        GUILayout.EndArea();
    }


    private void WestGamePreferencesViewed() {
        GUILayout.BeginArea(westPreferencesRect);
        GUILayout.EndArea();
    }

    private void EastPlayerSlots() {
        GUILayout.BeginArea(eastSlotsRect);
        GUILayout.EndArea();
    }

    private void PlayerSlot(int _slotNum) {
        playerSlotRect.y = _slotNum * playerSlotRect.height;
        GUILayout.BeginArea(playerSlotRect);
        //Clear - Player Name - Team
        GUILayout.EndArea();
    }

    private void SouthChat() {
        GUILayout.BeginArea(southChatRect);
        GUILayout.EndArea();
    }

    private void SouthButtons() {
        GUILayout.BeginArea(southButtonRect);
        GUILayout.EndArea();
    }
}