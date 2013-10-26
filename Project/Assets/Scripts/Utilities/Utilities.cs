using UnityEngine;
using System.Collections;

public class Utilities {

    private static Utilities instance = new Utilities();

    private Utilities() { }

    public static Utilities Instance {
        get { return instance; }
    }

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

    public void LogMessage(string s) {
        Debug.Log(s);
    }
}
