
using System.Collections.Generic;
using UnityEngine;

public class GameStatePlayerPrefsProvider : IGameStateProvider, IGameStateSaver
{
    private const string KEY = "GAME STATE"; //название файла, или ключ сервера

    public GameStateData GameState { get; private set; }

    public void LoadGameState()
    {
        if (PlayerPrefs.HasKey(KEY))
        {
            var json = PlayerPrefs.GetString(KEY);
            GameState = JsonUtility.FromJson<GameStateData>(json);
        }
        else
        {
            GameState = InitFromSettings();
            SaveGameState();
        }
    }

    private GameStateData InitFromSettings()
    {
        var gameState = new GameStateData
        {
            Inventories = new List<InventoryGridData>
            {
                CreateTestInventory("Ololosha"),
                CreateTestInventory("Shrek")
            }
        };
        return gameState;
    }

    private InventoryGridData CreateTestInventory(string ownerId)
    {
        var size = new Vector2Int(3, 4);
        var createdInventorySlots = new List<InventorySlotData>();
        var length = size.x * size.y;
        for (var i = 0; i < length; i++)
        {
            createdInventorySlots.Add(new InventorySlotData());
        }

        var createdInventoryData = new InventoryGridData
        {
            OwnerId = ownerId,
            Size = size,
            Slots = createdInventorySlots
        };

        return createdInventoryData;
    }

    public void SaveGameState()
    {
         var json = JsonUtility.ToJson(GameState);
        PlayerPrefs.SetString(KEY, json);
    }

}
