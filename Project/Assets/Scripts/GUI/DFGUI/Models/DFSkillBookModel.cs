using UnityEngine;
using System.Collections;

public class DFSkillBookModel : MonoBehaviour {

    void Awake() {
        
    }

    public string FireBallIcon {
        get { return SkillBook.Instance.GetIcon(0); }
    }

    public string WaterGunIcon {
        get { return SkillBook.Instance.GetIcon(0); }
    }

    public string MudShotIcon {
        get { return SkillBook.Instance.GetIcon(0); }
    }

    public string WIcon {
        get { return SkillBook.Instance.GetIcon(0); }
    }
}
