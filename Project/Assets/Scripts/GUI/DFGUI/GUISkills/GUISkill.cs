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
    #endregion

    #region Event handlers

    public void OnSkillActivated(int _id) {
        if (!IsEmpty && _id == Id)
            StartCoroutine(ShowCooldown());
    }

    #endregion

    #region Drag and drop

    void OnDragStart(dfControl source, dfDragEventArgs args) {

        if (AllowDrag(args)) {

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
                DFCharacterModel.ClearActionSkill(Slot, Id);
                Id = 0;
            }
            Refresh();
        }
    }

    void OnDragDrop(dfControl source, dfDragEventArgs args) {

        if (AllowDrop(args)) {
            args.State = dfDragDropState.Dropped;

            var droppedSkill = args.Data as GUISkill;

            Id = droppedSkill.Id;
            if (droppedSkill.IsActionSlot) {
                DFCharacterModel.ClearActionSkill(droppedSkill.Slot, droppedSkill.Id);
                droppedSkill.Id = 0;
            }
            DFCharacterModel.SetActionSkill(Slot, Id);
            StartCoroutine(ShowCooldown());
        }
        else
            args.State = dfDragDropState.Denied;

        args.Use();
    }

    private bool AllowDrag(dfDragEventArgs args) {
        // Do not allow the user to drag and drop empty GUISkill instances
        return !IsEmpty && !isSpellActive && gameObject.GetComponent<dfControl>().IsVisible;
    }

    private bool AllowDrop(dfDragEventArgs args) {
        // Only allow drop if the source is another GUISkill and
        // this GUISkill is assignable
        var slot = args.Data as GUISkill;
        return slot != null && !slot.IsEmpty && IsActionSlot && !isSpellActive;

    }
    #endregion

    #region Private utility methods

    private IEnumerator ShowCooldown() {

        isSpellActive = true;

        var sprite = GetComponent<dfControl>().Find("CoolDown") as dfSprite;
        sprite.IsVisible = true;

        var endTime = Time.realtimeSinceStartup + DFSkillModel.Instance.Cooldown(Id, Slot);
        while (Time.realtimeSinceStartup < endTime) {
            sprite.FillAmount = 1f - DFSkillModel.Instance.CooldownTimer(Id, Slot) / DFSkillModel.Instance.Cooldown(Id, Slot);
            yield return null;
        }
        sprite.FillAmount = 1f;
        sprite.IsVisible = false;

        isSpellActive = false;
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
    #endregion
}
