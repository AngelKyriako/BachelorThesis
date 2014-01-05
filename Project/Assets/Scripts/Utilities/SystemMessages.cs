using UnityEngine;
using System.Collections.Generic;

public class SystemMessages {

    public enum MessageType {
        Kill,
        Suicide,
        CriticalHit,
        Evasion,
        ManyKills1,
        ManyKills2,
        ManyDeaths1,
        ManyDeaths2
    }

    public enum SystemMode {
        Normal,
        Kinky
    }

    private const SystemMode DEFAULT_MODE = SystemMode.Normal;

    private Dictionary<int, Dictionary<MessageType, string>> messagesHolder;

    private static SystemMessages instance;
    public static SystemMessages Instance {
        get {
            if (instance != null)
                return instance;
            else {
                instance = new SystemMessages();
                return instance;
            }
        }
    }

    private SystemMessages() {
        messagesHolder = new Dictionary<int, Dictionary<MessageType, string>>();

        messagesHolder.Add(0, new Dictionary<MessageType, string>(){
                                    {MessageType.Kill, " killed "},
                                    {MessageType.Suicide, " killed himself"},
                                    {MessageType.CriticalHit, " achieved a critical hit on "},
                                    {MessageType.Evasion, " evaded the attack of "},
                                    {MessageType.ManyKills1, ""},
                                    {MessageType.ManyKills2, ""},
                                    {MessageType.ManyDeaths1, ""},
                                    {MessageType.ManyDeaths2, ""}
                                  });
        messagesHolder.Add(1, new Dictionary<MessageType, string>(){
                                    {MessageType.Kill, " received the balls from "},
                                    {MessageType.Suicide, " likes to play with himself "},
                                    {MessageType.CriticalHit, " had it rough on "},
                                    {MessageType.Evasion, " is playing hard to get on "},
                                    {MessageType.ManyKills1, " is masturbating over your pillows"},
                                    {MessageType.ManyKills2, " is enjoying your mothers, do something about it "},
                                    {MessageType.ManyDeaths1, " you should stop drinking"},
                                    {MessageType.ManyDeaths2, " just go for a smoke buddy and come back later"}
                                  });
    }

    public string Kill(string _killer, string _dead) {
        return _killer + GetMessageTemplate(MessageType.Kill) + _dead;
    }
    public string Suicide(string _dead) {
        return _dead+GetMessageTemplate(MessageType.Suicide);
    }
    public string CriticalHit(string _atk, string _def) {
        return _atk + GetMessageTemplate(MessageType.CriticalHit) + _def;
    }
    public string Evasion(string _atk, string _def) {
        return _def + GetMessageTemplate(MessageType.Evasion) + _atk ;
    }

    private string GetMessageTemplate(MessageType _type){
        return messagesHolder[(int)DEFAULT_MODE][_type];
    }
}
