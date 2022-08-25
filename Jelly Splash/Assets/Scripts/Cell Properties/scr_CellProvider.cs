using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class scr_CellProvider : MonoBehaviour
{
    [SerializeField] private scr_CellLogic cellLogic;

    private void Awake()
    {
        scr_GameManager.instance.currentCellSetup = cellLogic;
    }
}
