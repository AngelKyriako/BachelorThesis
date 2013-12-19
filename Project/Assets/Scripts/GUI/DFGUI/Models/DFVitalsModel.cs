using UnityEngine;
using System.Collections;

public class DFVitalsModel: MonoBehaviour {

    private const int DEFAULT_VITAL = 100;
    
    private PlayerCharacterModel myModel;
	
    void Start () {
        //myModel = GameManager.Instance.MyCharacterModel;
	}

    public float CurrentHealth {
        get { return myModel ? myModel.GetVital(0).CurrentValue : MaxHealth; }
    }
    public float MaxHealth {
        get { return myModel ? myModel.GetVital(0).FinalValue : DEFAULT_VITAL; }
    }

    public float CurrentMana {
        get { return myModel ? myModel.GetVital(1).CurrentValue : MaxMana; }
    }
    public float MaxMana {
        get { return myModel ? myModel.GetVital(1).FinalValue : DEFAULT_VITAL; }
    }

    public uint CurrentExp {
        get { return myModel ? myModel.CurrentExp : MaxExp; }
    }
    public uint MaxExp {
        get { return myModel ? myModel.ExpToLevel : DEFAULT_VITAL; }
    }
}