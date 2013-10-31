using UnityEngine;

public class BaseSkill: IBaseSkill {

    private string title, description;
    private Texture2D icon;

    public BaseSkill() {
    }

    public string Title {
        get {
            throw new System.NotImplementedException();
        }
        set {
            throw new System.NotImplementedException();
        }
    }

    public string Description {
        get {
            throw new System.NotImplementedException();
        }
        set {
            throw new System.NotImplementedException();
        }
    }

    public Texture2D Icon {
        get {
            throw new System.NotImplementedException();
        }
        set {
            throw new System.NotImplementedException();
        }
    }

    public bool LineOfSight {
        get {
            throw new System.NotImplementedException();
        }
        set {
            throw new System.NotImplementedException();
        }
    }

    public GameObject TargetEffect {
        get {
            throw new System.NotImplementedException();
        }
        set {
            throw new System.NotImplementedException();
        }
    }

    public GameObject CastEffect {
        get {
            throw new System.NotImplementedException();
        }
        set {
            throw new System.NotImplementedException();
        }
    }

    public float CoolDownTime {
        get {
            throw new System.NotImplementedException();
        }
        set {
            throw new System.NotImplementedException();
        }
    }

    public float CoolDownTimer {
        get { throw new System.NotImplementedException(); }
    }

    public bool IsReady {
        get { throw new System.NotImplementedException(); }
    }

    public void Cast() {
        throw new System.NotImplementedException();
    }

    public void Update() {
        throw new System.NotImplementedException();
    }
}
