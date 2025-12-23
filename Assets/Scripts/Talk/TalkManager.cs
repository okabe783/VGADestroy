namespace Talk
{
    public class TalkManager
    {
        private TalkData _currentData;
        private TalkNode _currentNode;

        public void StartTalk(TalkData data, int startNodeID = 0)
        {
            _currentData = data;
            _currentNode = data.GetNode(startNodeID);
        }

        public TalkNode GetCurrentNode()
        {
            return _currentNode;
        }

        public void SelectChoice(int index)
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