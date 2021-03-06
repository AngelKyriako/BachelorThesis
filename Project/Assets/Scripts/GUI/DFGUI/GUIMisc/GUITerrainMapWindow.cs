﻿using UnityEngine;
using System.Collections.Generic;

public class GUITerrainMapWindow: MonoBehaviour {

    public dfLabel playerIcon;
    private const int MAP_SIZE = 150;
    private float offsetX, offsetY;//435;//the more the value the more to the top it goes //Editor: 322, 800x600: 422

    private const KeyCode TOGGLE_BUTTON = KeyCode.M;
    private const int VISIBLE_ENEMY_LAYER = 9;

    private float terrainMapWidthScale, terrainMapHeightScale;
    private List<Pair<string, dfControl>> playerIcons;

	void Start () {
        playerIcons = new List<Pair<string, dfControl>>();
        dfLabel _nextIcon;
        foreach (string _name in GameManager.Instance.AllPlayerKeys) {
            if (!GameManager.Instance.ItsMe(_name))
                _nextIcon = (dfLabel)Instantiate(playerIcon);
            else
                _nextIcon = playerIcon;

            gameObject.GetComponent<dfPanel>().AddControl(_nextIcon);
            _nextIcon.Text = "*";
            _nextIcon.Color = GameManager.Instance.GetPlayerRGBColor(_name);
            playerIcons.Add(new Pair<string, dfControl>(_name, _nextIcon));
        }

        terrainMapWidthScale = Terrain.activeTerrain.terrainData.size.x / MAP_SIZE;
        terrainMapHeightScale = Terrain.activeTerrain.terrainData.size.z / MAP_SIZE;

        offsetX = playerIcon.Size.x / 2;
        offsetY = Screen.height - gameObject.GetComponent<dfControl>().Size.y + playerIcon.Size.y / 2;
	}
	
	void Update () {
        if (Input.GetKeyUp(TOGGLE_BUTTON))
            gameObject.GetComponent<dfPanel>().IsVisible = !gameObject.GetComponent<dfPanel>().IsVisible;

        if (gameObject.GetComponent<dfPanel>().IsVisible) {
            foreach (Pair<string, dfControl> _playerIconPair in playerIcons) {
                SetMapPosition(_playerIconPair.Second, TranslateCoordinatesToMap(_playerIconPair.First));
                _playerIconPair.Second.IsVisible = CombatManager.Instance.IsAlly(_playerIconPair.First) ||
                                                   GameManager.Instance.GetCharacter(_playerIconPair.First).layer.Equals(VISIBLE_ENEMY_LAYER);
            }
        }
	}

    private Vector2 TranslateCoordinatesToMap(string _playerId) {
        return new Vector2( GameManager.Instance.GetCharacter(_playerId).transform.position.x
                            / terrainMapWidthScale + offsetX,
                            GameManager.Instance.GetCharacter(_playerId).transform.position.z
                            / terrainMapHeightScale + offsetY
                          );
    }

    private Vector2 SetMapPosition(dfControl _control, Vector2 _screenPosition) {
        return _control.RelativePosition = _control.GetManager().ScreenToGui(_screenPosition);
    }
}