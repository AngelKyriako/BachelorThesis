using UnityEngine;
using System.Collections.Generic;

public class DestroyOnLoad : MonoBehaviour {
    
    public List<int> levelsToBeDestroyed;
    public bool usePhotonDestroy = false;

    void OnLevelWasLoaded(int level) {
        if (levelsToBeDestroyed.Contains(level)) {
            if (gameObject && !usePhotonDestroy)
                Destroy(gameObject);
            else if (gameObject && usePhotonDestroy)
                PhotonNetwork.Destroy(gameObject);
        }
    }
}
