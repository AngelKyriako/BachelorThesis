using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class ChatWindow: SingletonMono<ChatWindow> {
    
    private const int MAIN_X = 0, MAIN_HEIGHT = 150, OFFSET_Y = 120;
    private const int INPUT_BAR_HEIGHT = 20, INPUT_BAR_WIDTH = 200;
    private string chatInput = "";
    private Vector2 scrollPos = Vector2.zero;        
    private float lastUnfocusTime = 0;
    private Rect messagesLayoutRect, inputLayoutRect;

    private ChatNetController networkController;

    private ChatWindow() { }

    void Start() {
        messagesLayoutRect = new Rect(MAIN_X, Screen.height - MAIN_HEIGHT - OFFSET_Y, Screen.width, MAIN_HEIGHT);
        inputLayoutRect = new Rect(MAIN_X, Screen.height - INPUT_BAR_HEIGHT, INPUT_BAR_WIDTH, INPUT_BAR_HEIGHT);
        networkController = GetComponent<ChatNetController>();
    }

    void OnGUI() {
        GUI.SetNextControlName("");

        GUILayout.BeginArea(messagesLayoutRect);

        //Show scroll list of chat messages
        scrollPos = GUILayout.BeginScrollView(scrollPos);
        for (int i = ChatHolder.Instance.MessageCount - 1; i >= 0; i--) {
            GUI.color = ChatHolder.Instance.GetChatMessage(i).color;
            GUILayout.Label(ChatHolder.Instance.GetChatMessage(i).message);
        }
        GUILayout.EndScrollView();
        GUI.color = Color.white;
        GUILayout.EndArea();


        GUILayout.BeginArea(inputLayoutRect);
        GUILayout.BeginHorizontal();
        GUI.SetNextControlName("ChatField");
        chatInput = GUILayout.TextField(chatInput, GUILayout.MinWidth(200));

        if (Event.current.type == EventType.keyDown && Event.current.character == '\n') {
            if (GUI.GetNameOfFocusedControl() == "ChatField") {
                networkController.SendChatMessage(PhotonTargets.All, chatInput);
                chatInput = string.Empty;          
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
}
