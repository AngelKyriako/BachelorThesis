using UnityEngine;
using System.Collections.Generic;

public struct ChatMessage {
    public string message;
    public Color color;

    public ChatMessage(string _msg, Color _color) {
        message = _msg;
        color = _color;
    }
}

public class ChatHolder{

    private const int MAX_MESSAGES_COUNT = 50;

    private List<ChatMessage> messages;

    private static ChatHolder instance = new ChatHolder();
    public static ChatHolder Instance {
        get { return ChatHolder.instance; }
    }

    private ChatHolder() {
        messages = new List<ChatMessage>();
    }

    public void AddChatMessage(ChatMessage message) {
        messages.Add(message);
        if (messages.Count > MAX_MESSAGES_COUNT)
            messages.RemoveAt(0);
    }
    public ChatMessage GetChatMessage(int _index) {
        return messages[_index];
    }
    public int MessageCount {
        get { return messages.Count; }
    }
}
