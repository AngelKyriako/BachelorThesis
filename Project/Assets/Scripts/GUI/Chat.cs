using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class Chat: Photon.MonoBehaviour {

    public static Chat SP;
    public List<string> messages = new List<string>();

    private int chatHeight = 140, chatWidth = 300;
    private Vector2 scrollPos = Vector2.zero;
    private string chatInput = "";
    private float lastUnfocusTime = 0;

    void Awake() { SP = this; }

    void OnGUI() {
        GUI.SetNextControlName("");

        GUILayout.BeginArea(new Rect(0, Screen.height - chatHeight, chatWidth, chatHeight));

        //Show scroll list of chat messages
        scrollPos = GUILayout.BeginScrollView(scrollPos);
        GUI.color = Color.blue;
        for (int i = messages.Count - 1; i >= 0; i--) {
            GUILayout.Label(messages[i]);
        }
        GUILayout.EndScrollView();
        GUI.color = Color.white;

        //Chat input
        GUILayout.BeginHorizontal();
        GUI.SetNextControlName("ChatField");
        chatInput = GUILayout.TextField(chatInput, GUILayout.MinWidth(200));

        if (Event.current.type == EventType.keyDown && Event.current.character == '\n') {
            if (GUI.GetNameOfFocusedControl() == "ChatField") {
                Send(PhotonTargets.All);
                lastUnfocusTime = Time.time;
                GUI.FocusControl("");
                GUI.UnfocusWindow();
            }
            else if (lastUnfocusTime < Time.time - 0.1f) {
                GUI.FocusControl("ChatField");
            }
        }

        GUILayout.FlexibleSpace();
        GUILayout.EndHorizontal();

        GUILayout.EndArea();
    }

    public static void AddMessage(string text) {
        SP.messages.Add(text);
        if (SP.messages.Count > 15)
            SP.messages.RemoveAt(0);
    }


    [RPC]
    void SendMessage(string text, PhotonMessageInfo info) { AddMessage("["+info.sender+"] " + text); }

    void Send(PhotonTargets target) {
        if (chatInput != "") {
            photonView.RPC("SendMessage", target, chatInput);
            chatInput = "";
        }
    }

    void Send(PhotonPlayer target) {
        if (chatInput != "") {
            chatInput = "[PM] " + chatInput;
            photonView.RPC("SendMessage", target, chatInput);
            chatInput = "";
        }
    }

    void OnJoinedRoom() { this.enabled = true; }
    void OnCreatedRoom() { this.enabled = true; }
    void OnLeftRoom() { this.enabled = false; }
}
