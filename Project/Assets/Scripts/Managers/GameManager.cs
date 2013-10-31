using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class GameManager: Photon.MonoBehaviour {

    private Dictionary<string, GameObject> playerCharacters;

    public string pathToPlayerCharPrefab = "Characters/BabyDragon";

    void Awake() {
        playerCharacters = new Dictionary<string, GameObject>();
    }

    void OnJoinedRoom() {
        Vector3 spawnPoint;
        if (!PhotonNetwork.isMasterClient)
            spawnPoint = GameObject.Find("SpawnPoint" + Random.Range(2, 10)).transform.position;
        else
            spawnPoint = GameObject.Find("SpawnPoint1").transform.position;
        PhotonNetwork.Instantiate(pathToPlayerCharPrefab, spawnPoint, Quaternion.identity, 0);
    }

    void OnLeaveRoom() {
    }

    void Update() {

    }

    public void AddPlayerCharacter(string name, GameObject character) {
        if (!playerCharacters.ContainsKey(name))
            playerCharacters.Add(name, character);
    }

    public void RemovePlayerCharacter(string name) {
        if (playerCharacters.ContainsKey(name))
            playerCharacters.Remove(name);
    }

    public GameObject GetPlayerCharacter(string name) {
        return playerCharacters[name];
    }
}