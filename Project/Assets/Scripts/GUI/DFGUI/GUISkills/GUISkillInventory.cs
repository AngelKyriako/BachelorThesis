using System;
using System.Linq;
using System.Collections;

using UnityEngine;

public class GUISkillInventory: MonoBehaviour {

    #region Protected serialized fields

    [SerializeField]
    protected int skillId = 0;

    #endregion

    #region Private runtime variables

    private bool needRefresh = true;

    #endregion

    #region Public properties

    public int Id {
        get { return skillId; }
        set {
            skillId = value;
            Refresh();
        }
    }
    #endregion

    #region Unity Events

    void Start() {
        Refresh();

        var control = gameObject.GetComponent<dfControl>();
        control.SizeChanged += delegate(dfControl source, Vector2 value) {
            // Queue the refresh to be processed in LateUpdate after the
            // control and its child controls have recalculated their 
            // new render size
            needRefresh = true;
        };

    }

    void LateUpdate() {
        if (needRefresh) {
            needRefresh = false;
            var control = gameObject.GetComponent<dfControl>();
            var container = control.Parent as dfScrollPanel;

            if (container != null) {
                control.Width = container.Width - container.ScrollPadding.horizontal - 70;
            }
        }
    }

    #endregion

    //@TODO: WHAT TO ASSIGN HERE !!!!!!! (WhAT SHOULD I HAVE HERE DISPLAYED AT THE PLAYER
    //@TODO: Refresh when level up or when adding stats
    #region Private utility methods    
    public void Refresh() {
        var control = gameObject.GetComponent<dfControl>();

        var guiSkill = control.GetComponentInChildren<GUISkill>();
        var lblCosts = control.Find<dfLabel>("lblCosts");
        var lblName = control.Find<dfLabel>("lblName");
        var lblDescription = control.Find<dfLabel>("lblDescription");

        if (lblCosts == null)
            throw new Exception("Not found: lblCosts");
        if (lblName == null)
            throw new Exception("Not found: lblName");
        if (lblDescription == null)
            throw new Exception("Not found: lblDescription");

        guiSkill.Id = Id;
        lblName.Text = DFSkillModel.Instance.Title(Id);
        lblCosts.Text = string.Format("{0}/{1}/{2}", DFSkillModel.Instance.ManaCost(Id, CharacterSkillSlot.None),
                                                     DFSkillModel.Instance.Cooldown(Id, CharacterSkillSlot.None),
                                                     DFSkillModel.Instance.SkillAvailability(Id));
        lblDescription.Text = DFSkillModel.Instance.Description(Id);
        //@TODO: Add an effects Text section here (maybe an on hover)

        // Resize this control to match the size of the contents
        var descriptionHeight = lblDescription.RelativePosition.y + lblDescription.Size.y;
        var costsHeight = lblCosts.RelativePosition.y + lblCosts.Size.y;
        control.Height = Mathf.Max(descriptionHeight, costsHeight) + 5;
    }

    private bool SKillExists {
        get { return Id == 0; }
    }


    #endregion
}
