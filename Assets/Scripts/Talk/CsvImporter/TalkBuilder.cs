using System.Collections.Generic;

namespace Talk.ExcelData
{
    public class TalkBuilder
    {
        public TalkData Build(string talkID, List<TalkRow> rows)
        {
            // 最初にインスタンスを生成する
            TalkData talkData = new()
            {
                TalkID = talkID,
                TalkNodes = new List<TalkNode>()
            };
            
            // NodeIDからTalkNodeの対応表
            Dictionary<int,TalkNode> nodeMap = new();
            
            foreach (TalkRow row in rows)
            {
                int nodeID = int.Parse(row.NodeID);
                
                // Nodeがなければ生成する
                if (!nodeMap.TryGetValue(nodeID, out TalkNode node))
                {
                    node = new TalkNode
                    {
                        NodeID = nodeID,
                        Speaker = row.Speaker,
                        Text = row.Text,
                        Choices = new List<TalkChoice>(),
                        Rewards = new List<TalkReward>()
                    };
                    
                    nodeMap.Add(nodeID, node);
                    talkData.TalkNodes.Add(node);
                }
                
                // Choiceがあれば追加
                if (!string.IsNullOrEmpty(row.JumpNodeID))
                {
                    node.Choices.Add(new TalkChoice
                    {
                        Text = row.Text,
                        JumpNodeID = int.Parse(row.JumpNodeID)
                    });
                } 
            }

            return talkData;
        }
    }
}