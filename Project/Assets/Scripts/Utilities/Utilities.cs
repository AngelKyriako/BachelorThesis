using UnityEngine;

public class Utilities {

    private static Utilities instance = new Utilities();

    private Utilities() { }

    public static Utilities Instance {
        get { return instance; }
    }

    #region debugging
    public void Assert(bool b, string s) {
        if (!b) {
            Debug.LogError(s);
            Application.Quit();
        }
    }
    public void Assert(bool b, string scriptName, string methodName, string s) {
        if (!b) {
            Debug.LogError(s + " @ script: " + scriptName + ", method: " + methodName);
            Application.Quit();
        }
    }
    public void PreCondition(bool b, string scriptName, string methodName, string s) {
        if (!b) {
            Debug.LogError("Invalid pre condition: '" + s + "' @ script: " + scriptName + ",method:" + methodName);
            Application.Quit();
        }
    }
    public void PostCondition(bool b, string scriptName, string methodName, string s) {
        if (!b) {
            Debug.LogError("Invalid post condition: '" + s + "' @ script: " + scriptName + ",method:" + methodName);
            Application.Quit();
        }
    }
    public void LogMessageToChat(string _str) {
        ChatHolder.Instance.AddChatMessage(new ChatMessage(_str, Color.blue));
    }

    public void LogColoredMessageToChat(string _str, Color _color) {
        ChatHolder.Instance.AddChatMessage(new ChatMessage(_str, _color));
    }
    #endregion

    #region GUI Display
    public string VitalDisplay(float _value) {
        int intVal = (int)_value;
        return (intVal != _value) ? (intVal + 1).ToString() : intVal.ToString();
    }

    //public string StatPercentageDisplay(float _statValue) {
    //    string val = System.Math.Round(_statValue, 2).ToString();
    //    return val.Contains(".") ? val.Split('.')[1] + "%" : val + "%";
    //}
    public string StatPercentageDisplay(float _statValue) {
        return  System.Math.Round((_statValue - (int)_statValue) * 100, 2).ToString() + "%";
    }

    public string TimeCounterDisplay(float _time) {
        return (_time > 5) ? System.Math.Round(_time, 0).ToString() : System.Math.Round(_time, 1).ToString();
    }
    #endregion

    public bool GotLucky(float _chance/*o.xx*/) {
        double range = 1d;
        double scaled = (new System.Random().NextDouble() * range);

        return (float)scaled <= _chance;
    }

    public void SetGameObjectLayer(GameObject obj, int l) {
        if (obj == null)
            return;
        obj.layer = l;
        foreach (Transform child in obj.transform)
            if (child)
                SetGameObjectLayer(child.gameObject, l);
    }

    public PlayerCharacterModel GetPlayerCharacterModel(Transform obj) {
        if (obj == null)
            return null;
        else if (obj.GetComponent<PlayerCharacterModel>() != null)
            return obj.GetComponent<PlayerCharacterModel>();

        return GetPlayerCharacterModel(obj.transform.parent);
    }
}
