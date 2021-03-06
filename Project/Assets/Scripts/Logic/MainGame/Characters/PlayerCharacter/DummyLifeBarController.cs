﻿using UnityEngine;

public class DummyLifeBarController: MonoBehaviour {

    private BaseCharacterModel model;
    private Renderer dummyLifeBarRenderer;

    void Awake() {        
        enabled = false;
    }

    public void SetUp(bool allyAndNotMe) {
        dummyLifeBarRenderer = GameObject.Find("Characters/PlayerCharacters/" + name + "/DummyLifeBar").renderer;
        dummyLifeBarRenderer.enabled = false;
        if (allyAndNotMe || GameDifficulty.Easy.Equals(GameVariables.Instance.Difficulty.Value)) {
            model = GameManager.Instance.GetPlayerModel(name);
            enabled = true;
        }
        else
            Destroy(this);
    }
    
    void Update() {
        if (model.GetVital(0).CurrentValue > model.GetVital(0).FinalValue / 2)
            dummyLifeBarRenderer.enabled = false;
        else if (model.GetVital(0).CurrentValue > model.GetVital(0).FinalValue / 4)
            SetColor(Color.yellow);
        else
            SetColor(Color.red);
    }


    private void SetColor(Color _color) {
        if (!IsColored(_color)) {
            dummyLifeBarRenderer.material.color = _color;
            dummyLifeBarRenderer.enabled = true;
        }
    }

    private bool IsColored(Color _color) {
        return dummyLifeBarRenderer.material.color.Equals(_color);
    }
}
