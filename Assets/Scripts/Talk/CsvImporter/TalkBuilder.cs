using System.Collections.Generic;
using UnityEngine;

namespace Talk.ExcelData
{
    public class TalkBuilder
    {
        public Dictionary<string, TalkData> BuildAll(List<TalkRow> rows)
        {
            Dictionary<string, TalkData> map = new();

            foreach (TalkRow row in rows)
            {
                if (!int.TryParse(row.NodeID, out int nodeID))
                {
                    Debug.LogWarning($"Invalid NodeID: {row.NodeID}");
                    continue;
                }

                if (!map.TryGetValue(row.TalkID, out TalkData talkData))
                {
                    talkData = new TalkData
                    {
                        TalkID = row.TalkID,
                        NodeMap = new Dictionary<int, TalkNode>()
                    };
                    map.Add(row.TalkID, talkData);
                }

                if (!talkData.NodeMap.TryGetValue(nodeID, out TalkNode node))
                {
                    node = new TalkNode
                    {
                        NodeID = nodeID,
                        Speaker = row.Speaker,
                        Text = row.BodyText,
                        Choices = new List<TalkChoice>()
                    };
                    talkData.NodeMap.Add(nodeID, node);
                }

                if (string.IsNullOrEmpty(row.ChoiceText)) continue;

                if (!int.TryParse(row.JumpNodeID, out int jumpNodeID))
                {
                    Debug.LogWarning($"Invalid JumpNodeID: {row.JumpNodeID}");
                    continue;
                }

                node.Choices.Add(new TalkChoice
                {
                    Text = row.ChoiceText,
                    JumpNodeID = jumpNodeID
                });
            }

            return map;
        }

        public TalkData Build(string talkID, List<TalkRow> rows)
        {
            TalkData talkData = new()
            {
                TalkID = talkID,
                NodeMap = new Dictionary<int, TalkNode>()
            };

            foreach (TalkRow row in rows)
            {
                if (!int.TryParse(row.NodeID, out int nodeID))
                {
                    Debug.LogWarning($"[TalkBuilder] Invalid NodeID: {row.NodeID}");
                    continue;
                }

                // Node がなければ生成
                if (!talkData.NodeMap.TryGetValue(nodeID, out TalkNode node))
                {
                    node = new TalkNode
                    {
                        NodeID = nodeID,
                        Speaker = row.Speaker,
                        Text = row.BodyText,
                        Choices = new List<TalkChoice>()
                    };

                    talkData.NodeMap.Add(nodeID, node);
                }

                // Choice行の場合
                if (string.IsNullOrEmpty(row.ChoiceText)) continue;
                if (!int.TryParse(row.JumpNodeID, out int jumpNodeID))
                {
                    Debug.LogWarning($"[TalkBuilder] Invalid JumpNodeID: {row.JumpNodeID}");
                    continue;
                }

                node.Choices.Add(new TalkChoice
                {
                    Text = row.ChoiceText,
                    JumpNodeID = jumpNodeID
                });
            }

            Debug.Log(
                $"[TalkBuilder] Build Complete\n" +
                $" TalkID    : {talkData.TalkID}\n" +
                $" NodeCount: {talkData.NodeMap.Count}"
            );

            return talkData;
        }
    }
}