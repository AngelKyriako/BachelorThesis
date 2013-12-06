using UnityEngine;
using System.Collections;

public class ChatNetController: BaseNetController {

    private ChatWindow chat;

    public override void Awake() {
        base.Awake();
        chat = GetComponent<ChatWindow>();
    }

    public void SendChatMessage(PhotonTargets _targets) {
        if (!chat.Input.Equals(string.Empty)) {
            photonView.RPC("ChatMessageSent", _targets, chat.Input);
            chat.Input = string.Empty;
        }
    }

    public void SendChatMessage(PhotonPlayer target) {
        if (!chat.Input.Equals(string.Empty)) {
            chat.Input = "[PM] " + chat.Input;
            photonView.RPC("ChatMessageSent", target, chat.Input);
            chat.Input = string.Empty;
        }
    }

    [RPC]
    private void ChatMessageSent(string text, PhotonMessageInfo info) {
        //info.sender.customProperties["color"];
        if (!info.sender.isMasterClient)
            chat.AddMessage(new ChatMessage("[" + info.sender.name + "] " + text, Color.blue));//@TODO add color to player properties
        else
            chat.AddMessage(new ChatMessage("[" + info.sender.name + "] " + text, Color.red));
    }
}
