using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class scr_CellOwner : MonoBehaviour
{
    [System.Serializable]
    public class cl_CellLogic
    {
        [Header("Individual Cell Property")]
        public Vector2Int TrueCoordinate;
        public scr_CellOwner NextCell;
        public int CurrentCellValue;
        public int PositionValue;
        public Animator Anim;
        public AnimationClip IdleClip;
        public AnimationClip WakeClip;
        public AnimationClip PopClip;
        public AnimationClip EmptyClip;

        public cl_CellLogic(Vector2Int Found, scr_CellOwner nextCell, int FoundValue, int Pos, Animator anim, AnimationClip idle, AnimationClip wake, AnimationClip pop, AnimationClip empty)
        {
            TrueCoordinate = Found;
            NextCell = nextCell;
            CurrentCellValue = FoundValue;
            PositionValue = Pos;
            Anim = anim;
            IdleClip = idle;
            WakeClip = wake;
            PopClip = pop;
            EmptyClip = empty;

            AnimatorOverrideController overrideController = new AnimatorOverrideController(Anim.runtimeAnimatorController);
            Anim.runtimeAnimatorController = overrideController;
            overrideController["BeeIdle"] = IdleClip;
            overrideController["BeePop"] = PopClip;
            overrideController["BeeWake"] = WakeClip;
            overrideController["Empty"] = EmptyClip;
        }
    }

    [HideInInspector] public scr_CellOwner Next;
    [HideInInspector] public int FoundValue;
    [HideInInspector] public Vector2Int TrueCoordinate;
    [HideInInspector] public int PositionValue;
    public Animator anim;
    public AnimationClip IdleClip;
    public AnimationClip WakeClip;
    public AnimationClip PopClip;
    public AnimationClip EmptyClip;
    public cl_CellLogic CellData;

    public void InitializeCellData()
    {
        CellData = new cl_CellLogic(TrueCoordinate, Next, FoundValue, PositionValue, anim, IdleClip, WakeClip, PopClip, EmptyClip);
    }
}