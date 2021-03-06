using System;
using System.Collections;
using System.Collections.Generic;

using UnityEngine;

public class GUIMessageDisplay: SingletonMono<GUIMessageDisplay> {

    private const float TIME_BEFORE_FADE = 3f;
    private const float FADE_TIME = 2f;

    private List<MessageInfo> messages = new List<MessageInfo>();
    private dfLabel lblTemplate;

    private GUIMessageDisplay() { }

    #region Public methods

    public void AddMessage(string text) {

        if (lblTemplate == null)
            return;

        // Raise existing messages 
        for (int i = 0; i < messages.Count; i++) {
            var label = messages[i].label;
            label.RelativePosition += new Vector3(0, -label.Height);
        }

        var go = GameObject.Instantiate(lblTemplate.gameObject) as GameObject;
        go.transform.parent = transform;
        go.transform.position = lblTemplate.transform.position;
        go.name = "Message" + messages.Count;

        var newLabel = go.GetComponent<dfLabel>();
        newLabel.Text = text;
        newLabel.IsVisible = true;

        messages.Add(new MessageInfo { label = newLabel, startTime = Time.realtimeSinceStartup });

    }

    #endregion

    #region Unity events
    void Start() {
        lblTemplate = GetComponentInChildren<dfLabel>();
        lblTemplate.IsVisible = false;
    }

    void Update() {

        for (int i = messages.Count - 1; i >= 0; i--) {

            var message = messages[i];
            var elapsed = Time.realtimeSinceStartup - message.startTime;

            if (elapsed < TIME_BEFORE_FADE)
                continue;

            if (elapsed >= TIME_BEFORE_FADE + FADE_TIME) {
                messages.RemoveAt(i);
                Destroy(message.label.gameObject);
                continue;
            }

            var opacity = 1f - (elapsed - TIME_BEFORE_FADE) / FADE_TIME;
            message.label.Opacity = opacity;

        }

    }

    #endregion

    #region Support classes

    private class MessageInfo {
        public dfLabel label;
        public float startTime;
    }

    #endregion
}