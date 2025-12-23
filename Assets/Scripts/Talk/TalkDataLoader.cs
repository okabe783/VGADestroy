using System.Collections.Generic;
using Talk.ExcelData;
using UnityEngine;
using UnityEngine.Networking;

namespace Talk
{
    public class TalkDataLoader
    {
        private Dictionary<string, TalkData> talkMap = new ();

        // ここでトークデータを一括読み込み
        public void LoadAll(string talkID,string url)
        {
            TalkCsvImporter importer = new();
            TalkBuilder builder = new();

            // csvの読み込み
            using UnityWebRequest req = UnityWebRequest.Get(url);
            req.SendWebRequest();
            
            while (!req.isDone){ }

            if (req.result != UnityWebRequest.Result.Success)
            {
                Debug.LogError(req.error);
                return;
            }
            
            string csvText = req.downloadHandler.text;
            // Build
            // IDを照合
            List<TalkRow> rows = importer.Import(csvText);
            TalkData talkData = builder.Build(talkID, rows);
            talkMap[talkID] = talkData;
        }

        public TalkData Get(string talkID)
        {
            return talkMap[talkID];
        }
    }
}