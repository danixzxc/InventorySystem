using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

public class ConfigImportsMenu
{
    private const string SPREADSHEET_ID = "1-iom3nATfmnVG5_ARyPAUe3Apm4irSQ5zo_US_6bA5s";
    private const string ITEMS_SHEET_NAME = "InventoryItems";
    private const string CREDENTIALS_PATH = "astute-engine-420717-bd7b652dd90c.json";
    private const string SETTINGS_FILE_NAME = "GameSettings";

    [MenuItem("AmazingGames/Import Items Settings")]
    private static async void LoadItemsSettings()
    {
        var sheetsImporter = new GoogleSheetsImporter(CREDENTIALS_PATH, SPREADSHEET_ID);

        var gameSettings = LoadSettings();
        var itemsParser = new ItemsSettingsParser(gameSettings);
        await sheetsImporter.DownloadAndParseSheet(ITEMS_SHEET_NAME, itemsParser);

        var jsonForSaving = JsonUtility.ToJson(gameSettings);

        PlayerPrefs.SetString(SETTINGS_FILE_NAME, jsonForSaving);

        Debug.Log(jsonForSaving);

    }

    private static GameSettings LoadSettings()
    {
        var jsonLoaded = PlayerPrefs.GetString(SETTINGS_FILE_NAME);

        var gameSettings = !string.IsNullOrEmpty(jsonLoaded)
            ? JsonUtility.FromJson<GameSettings>(jsonLoaded) 
            : new GameSettings();

        return gameSettings;
    }
}
