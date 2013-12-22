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
    private readonly Dictionary<PlayerColor, Color> ChatColorMap = new Dictionary<PlayerColor, Color>(){
                                                                        {PlayerColor.None, Color.black },
                                                                        {PlayerColor.Red, Color.red },
                                                                        {PlayerColor.Blue, Color.blue },
                                                                        {PlayerColor.Gray, Color.gray },
                                                                        {PlayerColor.Orange, new Color32(255,70,0,255) },
                                                                        {PlayerColor.Green, Color.green },
                                                                        {PlayerColor.Pink, new Color32(240,105,180,255) },
                                                                        {PlayerColor.Yellow, new Color32(255,180,0,255) },
                                                                        {PlayerColor.Teal, new Color32(30,244,155,255) },
                                                                        {PlayerColor.White, Color.white },
                                                                        {PlayerColor.Purple, new Color32(128,0,128,255) }
                                                                   };

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

    public Color GetChatColor(PlayerColor _color) {
        return ChatColorMap[_color];
    }
}
