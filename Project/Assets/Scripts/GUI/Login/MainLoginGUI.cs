﻿using UnityEngine;
using System.Collections;

public class MainLoginGUI: MonoBehaviour {

    private string playerName;
    private Rect fullscreen;

    public Texture2D background;
    public Rect layoutRect;
    public GUIStyle bigLabelStyle, smallLabelStyle, bigButtonStyle, errorMessageStyle;
    public int space;

    private void Awake() {
        playerName = "Guest" + Random.Range(1, 9999).ToString();
        PlayerPrefs.SetString("name", playerName);
        layoutRect = new Rect((Screen.width - layoutRect.width) / 2, (Screen.height - layoutRect.height) / 2,
                               layoutRect.width, layoutRect.height);
    }

    private void OnGUI() {
        GUI.DrawTexture(GUIUtilities.Instance.FullScreenRect, background, ScaleMode.StretchToFill);

        GUILayout.BeginArea(layoutRect);

        GUILayout.Space(3 * space);
        GUILayout.Label("Welcome to Dragonborn", bigLabelStyle);
        GUILayout.Space(2 * space);

        GUILayout.BeginHorizontal();
        GUILayout.Label("Enter name:", smallLabelStyle);
        GUILayout.Space(space);
        playerName = GUILayout.TextField(playerName, GUILayout.Width(170));
        if(GUI.changed) {
            playerName = playerName.TrimStart();
            PlayerPrefs.SetString("name", playerName);
        }
        GUILayout.EndHorizontal();

        if (!IsNameValid)
            GUILayout.Label("Please enter a valid name. (3-18 characters)", errorMessageStyle);
        GUILayout.Space(2 * space);

        GUILayout.BeginHorizontal();
        if (GUILayout.Button("Quit", bigButtonStyle))
            Application.Quit();
        if (GUILayout.Button("Login", bigButtonStyle) && IsNameValid)
            Application.LoadLevel("Lobby");
        GUILayout.Space(space);
        GUILayout.EndHorizontal();

        GUILayout.EndArea();
    }

    public bool IsNameValid{
        get { return playerName.Length > 3 && playerName.Length <= 18; }
    }
}