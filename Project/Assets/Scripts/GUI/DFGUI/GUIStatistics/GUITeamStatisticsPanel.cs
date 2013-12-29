using UnityEngine;

public class GUITeamStatisticsPanel: GUISlottedWindow {

    public override void SetUpWindow() {
        SetUpTeamSlot(slot, 0, true);
        for (int i = 1; i < GameManager.Instance.TeamsCount; ++i)
            SetUpTeamSlot((dfPanel)Instantiate(slot), (PlayerTeam)i, false);
    }

    private void SetUpTeamSlot(dfPanel _nextSlot, PlayerTeam _team, bool _isAlreadyAttached) {
        _nextSlot.gameObject.GetComponent<GUITeamStatisticsSlot>().SetUp(_team);
        if (!_isAlreadyAttached)
            gameObject.GetComponent<dfScrollPanel>().AddControl(_nextSlot);
    }
}
