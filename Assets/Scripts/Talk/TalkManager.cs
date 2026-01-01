using System;
using UnityEngine;

namespace Talk
{
    public class TalkManager
    {
        private enum TalkState
        {
            None,
            Talking,
            EndWait
        }

        private TalkState _state = TalkState.None;

        private TalkData _currentData;
        private TalkNode _currentNode;
        private TalkView _talkView;

        public bool IsTalking => _state != TalkState.None;

        public void SetView(TalkView talkView)
        {
            _talkView = talkView;
            _talkView.OnChoiceSelected += SelectChoice;
        }

        public async void StartTalk(TalkData data, int startNodeID = 0)
        {
            try
            {
                if (_state != TalkState.None) return;

                _currentData = data;
                _currentNode = data.GetNode(startNodeID);

                if (_currentNode == null)
                {
                    Debug.LogError("[TalkManager] Start node not found");
                    return;
                }

                _state = TalkState.Talking;

                TalkSystem.I.BeginTalk();

                await _talkView.OpenAsync(_currentNode);

                // 選択肢がない＝最後の文章
                if (_currentNode.Choices.Count == 0)
                    EnterEndWait();
            }
            catch (Exception e)
            {
                Debug.LogError(e);
            }
        }

        public void RequestNextOrEndByInput()
        {
            switch (_state)
            {
                case TalkState.EndWait:
                    EndTalk();
                    break;

                case TalkState.Talking:
                    if (_currentNode == null)
                        return;
                    
                    if (_currentNode.Choices.Count == 0)
                        EnterEndWait();
                    break;
            }
        }

        private void SelectChoice(int index)
        {
            if (_state != TalkState.Talking)
                return;

            if (_currentNode == null || index < 0 || index >= _currentNode.Choices.Count)
                return;

            TalkChoice choice = _currentNode.Choices[index];

            // 明示的終了
            if (choice.JumpNodeID < 0)
            {
                EnterEndWait();
                return;
            }

            _currentNode = _currentData.GetNode(choice.JumpNodeID);

            if (_currentNode == null)
            {
                EnterEndWait();
                return;
            }

            _talkView.Draw(_currentNode);

            if (_currentNode.Choices.Count == 0)
                EnterEndWait();
        }

        private void EnterEndWait()
        {
            _state = TalkState.EndWait;
        }

        private async void EndTalk()
        {
            if (_state == TalkState.None)
                return;

            await _talkView.CloseAsync();

            _currentData = null;
            _currentNode = null;

            TalkSystem.I.EndTalk();
            _state = TalkState.None;
        }
    }
}
