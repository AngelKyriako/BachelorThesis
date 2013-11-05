using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public struct ChatMessage {
    public string message;
    public Color color;

    public ChatMessage(string _msg, Color _color) {
        message = _msg;
        color = _color;
    }
}

public class ChatWindow: Photon.MonoBehaviour {

    private const int MAX_MESSAGES_COUNT = 30;
    private const int MAIN_HEIGHT = 140, MAIN_WIDTH = 300;

    private static ChatWindow instance;

    private List<ChatMessage> messages = new List<ChatMessage>();
    private Vector2 scrollPos = Vector2.zero;
    private string chatInput = "";
    private float lastUnfocusTime = 0;

    void Awake() {
        instance = this;
        enabled = false;
    }

    void OnGUI() {
        GUI.SetNextControlName("");

        GUILayout.BeginArea(new Rect(0, Screen.height - MAIN_HEIGHT, MAIN_WIDTH, MAIN_HEIGHT));

        //Show scroll list of chat messages
        scrollPos = GUILayout.BeginScrollView(scrollPos);
        for (int i = messages.Count - 1; i >= 0; i--) {
            GUI.color = messages[i].color;
            GUILayout.Label(messages[i].message);
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

    void OnJoinedRoom() { this.enabled = true; }
    void OnCreatedRoom() { this.enabled = true; }
    void OnLeftRoom() { this.enabled = false; }

    void Send(PhotonTargets targets) {
        if (chatInput != "") {
            photonView.RPC("MessageSent", targets, chatInput);
            chatInput = "";
        }
    }

    void Send(PhotonPlayer target) {
        if (chatInput != "") {
            chatInput = "[PM] " + chatInput;
            photonView.RPC("MessageSent", target, chatInput);
            chatInput = "";
        }
    }

    [RPC]
    void MessageSent(string text, PhotonMessageInfo info) {
        //info.sender.customProperties["color"];
        if (!info.sender.isMasterClient)
            AddMessage(new ChatMessage("[" + info.sender.name + "] " + text, Color.blue));//@TODO add color to player properties
        else
            AddMessage(new ChatMessage("[" + info.sender.name + "] " + text, Color.red));
    }

    public static void AddMessage(ChatMessage message) {
        instance.messages.Add(message);
        if (instance.messages.Count > MAX_MESSAGES_COUNT)
            instance.messages.RemoveAt(0);
    }
}
