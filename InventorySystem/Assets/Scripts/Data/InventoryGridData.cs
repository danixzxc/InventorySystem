using System.Collections.Generic;
using System;
using UnityEngine;

[Serializable]
public class InventoryGridData 
{
    public string OwnerId;
    public List<InventorySlotData> Slots;
    public Vector2Int Size;
}
