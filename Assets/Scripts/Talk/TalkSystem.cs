using UnityEngine;

namespace Talk
{
    public class TalkSystem : SingletonMonoBehaviour<TalkSystem>
    {
        [SerializeField] 
        private TalkView _talkView;
        private TalkManager _talkManager;
        private TalkDataLoader _talkDataLoader;

        private void Start()
        {
            _talkDataLoader =  new TalkDataLoader();
            _talkManager = new TalkManager();
            _talkManager.SetView(_talkView);
        }
        
        public TalkManager TalkManager => _talkManager;
    }
}