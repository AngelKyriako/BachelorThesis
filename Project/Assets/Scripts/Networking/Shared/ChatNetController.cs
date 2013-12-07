using UnityEngine;
using System.Collections;

public class ChatNetController: BaseNetController {

    public override void Awake() {
        base.Awake();
    }

    public void SendChatMessage(PhotonTargets _targets, string _input) {
        if (!_input.Equals(string.Empty)) {
            photonView.RPC("ChatMessageSent", _targets, _input);
        }
    }

    public void SendChatMessage(PhotonPlayer target, string _input) {
        if (!_input.Equals(string.Empty)) {
            _input = "[PM] " + _input;
            photonView.RPC("ChatMessageSent", target, _input);
        }
    }

    [RPC]
    private void ChatMessageSent(string text, PhotonMessageInfo info) {
        //info.sender.customProperties["color"];
        if (!info.sender.isMasterClient)
            ChatHolder.Instance.AddChatMessage(new ChatMessage("[" + info.sender.name + "] " + text, Color.blue));//@TODO add color to player properties
        else
            ChatHolder.Instance.AddChatMessage(new ChatMessage("[" + info.sender.name + "] " + text, Color.red));
    }
}
