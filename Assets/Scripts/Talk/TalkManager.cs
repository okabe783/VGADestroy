using System.Collections.Generic;
using UnityEngine;

namespace Talk
{
    public class TalkManager
    {
        private Dictionary<string, TalkData> _talkMap;
        private TalkData _currentData;
        private TalkNode _currentNode;
        private TalkView _talkView;
        
        public void Register(Dictionary<string, TalkData> talkMap)
        {
            _talkMap = talkMap;
        }

        public void SetView(TalkView talkView)
        {
            _talkView = talkView;
            _talkView.OnChoiceSelected += SelectChoice;
        }

        public void StartTalk(TalkData data, int startNodeID = 0)
        {
            _currentData = data;
            _currentNode = data.GetNode(startNodeID);
            _talkView.Show(_currentNode);
        }


        private void SelectChoice(int index)
        {
            TalkChoice choice = _currentNode.Choices[index];
            _currentNode = _currentData.GetNode(choice.JumpNodeID);

            if (_currentNode != null)
            {
                _talkView.Show(_currentNode);
            }
        }

        public bool IsEnd()
        {
            return _currentNode == null;
        }
    }
}