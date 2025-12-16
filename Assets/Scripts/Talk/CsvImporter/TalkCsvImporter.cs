using System.Collections.Generic;
using System.IO;

namespace Talk.ExcelData
{
    public class TalkRow
    {
        public string NodeID;
        public string Speaker;
        public string Text;
        public string JumpNodeID;
        public string RewardKind;
        public string RewardID;
        public string Amount;
    }

    // csvをインポートする
    public class TalkCsvImporter
    {
        public List<TalkRow> Import(string csvText)
        {
            List<TalkRow> row = new();
            StringReader dataReader = new(csvText);

            // csvの1行目は読み飛ばす
            string line = dataReader.ReadLine();

            if (string.IsNullOrEmpty(line)) return row;
            
            // 2行目以降のデータ
            while (true)
            {
                string data = dataReader.ReadLine();
                if(data == null) break;
                
                if(string.IsNullOrEmpty(data)) continue;
                
                string[] cols = data.Split(',');
                
                TalkRow talkRow = new()
                {
                    NodeID     = Get(cols, 0),
                    Speaker    = Get(cols, 1),
                    Text       = Get(cols, 2),
                    JumpNodeID = Get(cols, 3),
                    RewardKind = Get(cols, 4),
                    RewardID   = Get(cols, 5),
                    Amount     = Get(cols, 6),
                };
                
                row.Add(talkRow);
            }
            return row;
        }

        private string Get(string[] data, int index)
        {
            return index < data.Length ? data[index] : string.Empty;
        }
    }
}