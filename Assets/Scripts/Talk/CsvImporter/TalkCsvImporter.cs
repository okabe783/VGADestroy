using System.Collections.Generic;
using System.IO;

namespace Talk.ExcelData
{
    public class TalkRow
    {
        public string TalkID; // 会話ID（test_npc_001）
        public string NodeID; // ノードID（0, 10, 20）
        public string Speaker; // NPC / Player / Choice
        public string BodyText; // ノード本文（NPCセリフ）
        public string ChoiceText; // 選択肢テキスト
        public string JumpNodeID; // 遷移先ノード
        public string RewardKind;
        public string RewardID;
        public string Amount;
    }

    // csvをインポートする
    public class TalkCsvImporter
    {
        public List<TalkRow> Import(string csvText)
        {
            List<TalkRow> rows = new();
            StringReader reader = new(csvText);

            // 1行目：#TalkID,test_npc_001
            reader.ReadLine();

            while (true)
            {
                string line = reader.ReadLine();
                if (line == null) break;
                if (string.IsNullOrEmpty(line)) continue;

                string[] cols = line.Split(',');

                TalkRow row = new()
                {
                    TalkID = Get(cols, 0),
                    NodeID = Get(cols, 1),
                    Speaker = Get(cols, 2),
                    BodyText = Get(cols, 3),
                    ChoiceText = Get(cols, 4),
                    JumpNodeID = Get(cols, 5),
                    RewardKind = Get(cols, 6),
                    RewardID = Get(cols, 7),
                    Amount = Get(cols, 8),
                };

                rows.Add(row);
            }

            return rows;
        }


        private string Get(string[] data, int index)
        {
            return index < data.Length ? data[index] : string.Empty;
        }
    }
}