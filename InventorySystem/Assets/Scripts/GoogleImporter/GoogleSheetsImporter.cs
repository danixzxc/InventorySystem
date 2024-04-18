
using Google.Apis.Auth.OAuth2;
using Google.Apis.Services;
using Google.Apis.Sheets.v4;
using Google.Apis.Sheets.v4.Data;
using System.Collections.Generic;
using System.Diagnostics;
using System.Threading.Tasks;
using UnityEngine;
public class GoogleSheetsImporter 
{
    private readonly List<string> _headers = new();
    private readonly SheetsService _service;
    private readonly string _sheetId;

    public GoogleSheetsImporter(string credentialsPath, string sheetId)
    {
        _sheetId = sheetId;
        
        GoogleCredential credential;
        using(var stream = new System.IO.FileStream(credentialsPath, System.IO.FileMode.Open, System.IO.FileAccess.Read))
        {
            credential = GoogleCredential.FromStream(stream).CreateScoped(SheetsService.Scope.SpreadsheetsReadonly);
        }

        _service = new SheetsService(new BaseClientService.Initializer()
        {
            HttpClientInitializer = credential
        });

    }

    public async Task DownloadAndParseSheet(string sheetName, IGoogleSheetParser parser)
    {
        UnityEngine.Debug.Log($"Starting downloading sheet (${sheetName})....");

        var range = $"{sheetName}!A1:Z";
        var request = _service.Spreadsheets.Values.Get(_sheetId, range);

        ValueRange response;
        try
        {
            response = await request.ExecuteAsync();
        }
        catch (System.Exception ex) 
        {
            UnityEngine.Debug.Log($"Error retrieving Google Sheets data: {ex.Message}");
            return;
        }

        //parse revieved data so response is not empty

        if (response != null && response.Values != null)
        {
            var tableArray = response.Values;
            UnityEngine.Debug.Log($"Sheet downloaded successfully: {sheetName}. Parsing...");

            var firstRow = tableArray[0];
            foreach (var cell in firstRow)
            {
                _headers.Add(cell.ToString());
            }

            var rowsCount = tableArray.Count;
            for ( var i = 1; i < rowsCount; i++ )
            {
                var row = tableArray[i];
                var rowLength = row.Count;

                for(var j = 0; j < rowLength; j++ )
                {
                    var cell = row[j];
                    var header = _headers[j];

                    parser.Parse(header, cell.ToString());

                    UnityEngine.Debug.Log($"Header: {header}, value: {cell}.");


                }
            }

            UnityEngine.Debug.Log($"Sheet parsed successfully.");
        }

        else
        {
            UnityEngine.Debug.LogWarning($"No data found in Google Sheets.");

        }

    }
}
