using System.Collections.Generic;
using Cysharp.Threading.Tasks;
using Talk.ExcelData;
using UnityEngine;
using UnityEngine.Networking;

namespace Talk
{
    public class TalkDataLoader
    {
        // ここでトークデータを一括読み込み
        public async UniTask<Dictionary<string, TalkData>> LoadFromUrl(string url)
        {
            using UnityWebRequest req = UnityWebRequest.Get(url);
            await req.SendWebRequest();

            if (req.result != UnityWebRequest.Result.Success)
            {
                Debug.LogError(req.error);
                return null;
            }

            TalkCsvImporter importer = new();
            List<TalkRow> rows = importer.Import(req.downloadHandler.text);

            TalkBuilder builder = new();
            return builder.BuildAll(rows);
        }

    }
}