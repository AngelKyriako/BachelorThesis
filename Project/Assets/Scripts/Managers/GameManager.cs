using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class GameManager: SingletonPhotonMono<GameManager> {

    private const string pathToPlayerCharPrefab = "Characters/BabyDragon";

    private GameObject playerCharacter, gui;
    private Dictionary<string, GameObject> allies;
    private Dictionary<string, GameObject> enemies;

    private GameManager() { }

    void Awake() {
        gui = GameObject.Find("GUIScripts");
        allies = new Dictionary<string, GameObject>();
        enemies = new Dictionary<string, GameObject>();
    }

    void OnJoinedRoom() {
        Vector3 spawnPoint;
        if (!PhotonNetwork.isMasterClient)
            spawnPoint = GameObject.Find("SpawnPoint" + Random.Range(2, 10)).transform.position;
        else
            spawnPoint = GameObject.Find("SpawnPoint1").transform.position;
        PhotonNetwork.Instantiate(pathToPlayerCharPrefab, spawnPoint, Quaternion.identity, 0);
        GameManager.Instance.InitGUIScripts();
    }

    void OnLeaveRoom() {
        playerCharacter = null;
    }

    public void InitGUIScripts() {
        gui.GetComponent<ChatWindow>().enabled = true;
        gui.GetComponent<CharacterWindow>().enabled = true;
    }

    public void LogPlayerCharacters() {
        string alliesStr = string.Empty;
        string enemiesStr = string.Empty;
        foreach (KeyValuePair<string, GameObject> entry in allies)
            alliesStr += entry.Key + " ";
        foreach (KeyValuePair<string, GameObject> entry in enemies)
            enemiesStr += entry.Key + " ";
        Utilities.Instance.LogMessage("PlayerCharacter: "+playerCharacter.name);
        Utilities.Instance.LogMessage("Allies: " + alliesStr);
        Utilities.Instance.LogMessage("Enemies: " + enemiesStr);
    }

#region Accessors
    public GameObject PlayerCharacter {
        get { return playerCharacter; }
        set { playerCharacter = value; }
    }

    public GameObject Gui {
        get { return gui; }
    }

    public void AddAlly(string name, GameObject character) {
        if (!allies.ContainsKey(name))
            allies.Add(name, character);
    }
    public void RemoveAlly(string name) {
        if (allies.ContainsKey(name))
            allies.Remove(name);
    }
    public bool IsAlly(string name) {
        return allies.ContainsKey(name);
    }
    public GameObject GetAlly(string name) {
        return allies[name];
    }

    public void AddEnemy(string name, GameObject character) {
        if (!enemies.ContainsKey(name))
            enemies.Add(name, character);
    }
    public void RemoveEnemy(string name) {
        if (enemies.ContainsKey(name))
            enemies.Remove(name);
    }
    public bool IsEnemy(string name) {
        return enemies.ContainsKey(name);
    }
    public GameObject GetEnemy(string name) {
        return enemies[name];
    }
#endregion
}