using System;
using System.Linq;
using System.Collections;

using UnityEngine;

public class GUISkill: MonoBehaviour {

    #region Protected serialized fields

    [SerializeField]
    protected int skillId = 0;

    [SerializeField]
    protected CharacterSkillSlot slot = CharacterSkillSlot.None;

    [SerializeField]
    protected bool isActionSlot = false;

    #endregion

    #region Private non-serialized fields

    private Vector2 dragCursorOffset;
    private bool isSpellActive = false;

    #endregion

    #region Public properties

    public bool IsActionSlot {
        get { return isActionSlot; }
        set {
            isActionSlot = value;
            Refresh();
        }
    }

    public int Id {
        get { return skillId; }
        set {
            skillId = value;
            Refresh();
        }
    }

    public CharacterSkillSlot Slot {
        get { return slot; }
        set {
            slot = value;
            Refresh();
        }
    }

    #endregion

    #region Unity events

    void Start() {
        Refresh();
    }

    void Update() {

        if (IsActionSlot && Id != 0) {
            if (Input.GetKeyDown((KeyCode)((int)slot + 48))) {
                CastSpell();
            }
        }

    }
    #endregion

    #region Event handlers

    public void OnSkillActivated(BaseSkill skill) {//@TODO: check how this works
        if (!IsEmpty) {
            Debug.Log("Skill activated: " + DFSkillModel.Instance.Title(Id));
            StartCoroutine(ShowCooldown());
        }
    }

    void OnDoubleClick() {
        if (!isSpellActive && !IsEmpty && IsActionSkill) {
            CastSpell();
        }
    }

    #endregion

    #region Drag and drop @TODO Modify actual game models here !!!

    void OnDragStart(dfControl source, dfDragEventArgs args) {

        if (allowDrag(args)) {

            if (IsEmpty) {
                // Indicates that the drag-and-drop operation cannot be performed
                args.State = dfDragDropState.Denied;
            }
            else {

                // Get the offset that will be used for the drag cursor
                var sprite = GetComponent<dfControl>().Find("Icon") as dfSprite;
                var ray = sprite.GetCamera().ScreenPointToRay(Input.mousePosition);
                if (!sprite.GetHitPosition(ray, out dragCursorOffset))
                    return;

                // Set the variables that will be used to render the drag cursor. 
                // The UI library provides all of the drag and drop events necessary 
                // but does not provide a default drag visualization and requires 
                // that the application provide the visualization. We'll do that by
                // supplying a Texture2D that will be placed at the mouse location 
                // in the OnGUI() method. 
                GUIDragCursor.Show(sprite, Input.mousePosition, dragCursorOffset);

                if (IsActionSlot) {
                    // Visually indicate that they are *moving* the spell rather than
                    // just dragging it into a slot
                    sprite.SpriteName = "";
                }

                // Indicate that the drag and drop operation can continue and set
                // the user-defined data that will be sent to potential drop targets
                args.State = dfDragDropState.Dragging;
                args.Data = this;

            }

            // Do not let the OnDragStart event "bubble up" to higher-level controls
            args.Use();

        }

    }

    void OnDragEnd(dfControl source, dfDragEventArgs args) {

        GUIDragCursor.Hide();

        if (isActionSlot) {
            if (args.State == dfDragDropState.CancelledNoTarget) {
                Id = 0;
                slot = CharacterSkillSlot.None;
            }
            Refresh();
        }
    }

    void OnDragDrop(dfControl source, dfDragEventArgs args) {

        if (allowDrop(args)) {

            args.State = dfDragDropState.Dropped;

            var droppedSkill = args.Data as GUISkill;

            Id = droppedSkill.Id;
            if (droppedSkill.IsActionSlot) {
                droppedSkill.Id = 0;
            }

        }
        else {
            args.State = dfDragDropState.Denied;
        }

        args.Use();

    }

    private bool allowDrag(dfDragEventArgs args) {
        // Do not allow the user to drag and drop empty GUISkill instances
        return !isSpellActive && !IsEmpty;
    }

    private bool allowDrop(dfDragEventArgs args) {

        if (isSpellActive)
            return false;

        // Only allow drop if the source is another GUISkill and
        // this GUISkill is assignable
        var slot = args.Data as GUISkill;
        return slot != null && IsActionSlot;

    }

    #endregion

    #region Private utility methods

    private IEnumerator ShowCooldown() {

        isSpellActive = true;

        var assignedSkill = SkillBook.Instance.GetSkill(Id);

        var sprite = GetComponent<dfControl>().Find("CoolDown") as dfSprite;
        sprite.IsVisible = true;

        var startTime = Time.realtimeSinceStartup;
        var endTime = startTime + assignedSkill.CoolDown;

        while (Time.realtimeSinceStartup < endTime) {

            var elapsed = Time.realtimeSinceStartup - startTime;
            var lerp = 1f - elapsed / assignedSkill.CoolDownTimer;

            sprite.FillAmount = lerp;

            yield return null;

        }

        sprite.FillAmount = 1f;
        sprite.IsVisible = false;

        isSpellActive = false;

    }

    private void CastSpell() {
        //@TODO: check if possible to Cast Skill here
    }

    private void Refresh() {
        var sprite = GetComponent<dfControl>().Find<dfSprite>("Icon");
        sprite.SpriteName = DFSkillModel.Instance.Icon(Id);

        var label = GetComponentInChildren<dfButton>();
        label.IsVisible = IsActionSlot;
        label.Text = slot.ToString();
    }

    private bool IsEmpty {
        get { return Id == 0; }
    }

    private bool IsActionSkill {
        get { return slot.Equals(CharacterSkillSlot.None); }
    }

    #endregion
}
