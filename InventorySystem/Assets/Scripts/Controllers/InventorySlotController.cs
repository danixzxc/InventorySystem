using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InventorySlotController 
{

    private InventorySlotView _view;
    public InventorySlotController(
        IReadOnlyInventorySlot slot, 
        InventorySlotView view)
    {
        _view = view;

        slot.ItemIdChanged += OnSlotItemIdChanged;
        slot.ItemAmountChanged += OnSlotItemAmountChanged;

        view.Title = slot.ItemId; // здесь можно через ID сделать локализацию
        view.Amount = slot.Amount;

    }

    private void OnSlotItemAmountChanged(int newAmount)
    {
        _view.Amount = newAmount;
    }

    private void OnSlotItemIdChanged(string newItemId)
    {
        _view.Title = newItemId;
    }
}
