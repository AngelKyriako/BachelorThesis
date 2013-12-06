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

public class ChatWindow: SingletonMono<ChatWindow> {

    private const int MAX_MESSAGES_COUNT = 30;
    private const int MAIN_X = 0, MAIN_HEIGHT = 140, MAIN_WIDTH = 300;

    private string chatInput = "";
    private List<ChatMessage> messages;
    private Vector2 scrollPos = Vector2.zero;        
    private float lastUnfocusTime = 0;
    private Rect layoutRect;

    private ChatNetController networkController;

    private ChatWindow() { }

    void Awake() {
        enabled = false;
    }

    void Start() {
        messages = new List<ChatMessage>();        
        layoutRect = new Rect(MAIN_X, Screen.height - MAIN_HEIGHT, MAIN_WIDTH, MAIN_HEIGHT);
        networkController = GetComponent<ChatNetController>();
    }

    void OnGUI() {
        GUI.SetNextControlName("");

        GUILayout.BeginArea(layoutRect);

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
                networkController.SendChatMessage(PhotonTargets.All);
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

    public void AddMessage(ChatMessage message) {
        messages.Add(message);
        if (messages.Count > MAX_MESSAGES_COUNT)
            messages.RemoveAt(0);
    }

    public string Input {
        get { return chatInput; }
        set { chatInput = value; }
    }
}
