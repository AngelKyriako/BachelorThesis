using UnityEngine;
using System.Collections;

public class GameManager: Photon.MonoBehaviour {

    void Start() {

    }

    void OnJoinedRoom() {
        Vector3 spawnPoint;
        if (!PhotonNetwork.isMasterClient)
            spawnPoint = GameObject.Find("SpawnPoint" + Random.Range(2, 10)).transform.position;
        else
            spawnPoint = GameObject.Find("SpawnPoint1").transform.position;
        GameObject babyDragon = PhotonNetwork.Instantiate("Characters/BabyDragon", spawnPoint, Quaternion.identity, 0);
    }

    void Update() {

    }
}