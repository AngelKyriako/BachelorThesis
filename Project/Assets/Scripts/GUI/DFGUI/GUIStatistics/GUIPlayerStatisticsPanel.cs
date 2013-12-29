using UnityEngine;

public class GUIPlayerStatisticsPanel: GUISlottedWindow {

    public override void SetUpWindow() {
        SetUpSlot(slot, GameManager.Instance.MyCharacter.name, true);
        foreach (string _name in GameManager.Instance.AllPlayerKeys)
            if (!GameManager.Instance.ItsMe(_name))
                SetUpSlot((dfPanel)Instantiate(slot), _name, false);
    }

    private void SetUpSlot(dfPanel _nextSlot, string _name, bool _isAlreadyAttached) {
        _nextSlot.gameObject.GetComponent<GUIPlayerStatisticsSlot>().SetUp(_name);
        if (!_isAlreadyAttached)
            gameObject.GetComponent<dfScrollPanel>().AddControl(_nextSlot);
    }
}