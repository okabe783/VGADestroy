namespace Talk
{
    public class TalkManager
    {
        private TalkData _currentData;
        private TalkNode _currentNode;
        private TalkView _talkView;

        public void StartTalk(TalkData data, int startNodeID = 0)
        {
            _currentData = data;
            _currentNode = data.GetNode(startNodeID);

            _talkView.Show(_currentNode);
        }

        public void SetView(TalkView talkView)
        {
            _talkView = talkView;
            _talkView.OnChoiceSelected += SelectChoice;
        }

        public TalkNode GetCurrentNode()
        {
            return _currentNode;
        }

        private void SelectChoice(int index)
        {
            TalkChoice choice = _currentNode.Choices[index];
            _currentNode = _currentData.GetNode(choice.JumpNodeID);
        }

        public bool IsEnd()
        {
            return _currentNode == null;
        }
    }
}