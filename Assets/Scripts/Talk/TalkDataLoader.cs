using System.Collections.Generic;
using Talk.ExcelData;

namespace Talk
{
    public class TalkDataLoader
    {
        private Dictionary<string, TalkData> talkMap;

        // ここでトークデータを一括読み込み
        public void LoadAll(string csvText,string talkID)
        {
            TalkCsvImporter importer = new();
            TalkBuilder builder = new();
            // csvの読み込み
            List<TalkRow> rows = importer.Import(csvText);
            // Build
            // IDを照合
            TalkData talkData = builder.Build(talkID, rows);
            talkMap.Add(talkData.TalkID,talkData);
        }

        public TalkData Get(string talkID)
        {
            return talkMap[talkID];
        }
    }
}