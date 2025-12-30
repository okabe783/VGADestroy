using Cysharp.Threading.Tasks;
using UnityEngine;
using System.Collections.Generic;

namespace Talk
{
    public class TalkSystem : SingletonMonoBehaviour<TalkSystem>
    {
        [SerializeField] private TalkView _talkView;
        [SerializeField] private string _url;

        private TalkManager _talkManager;
        private Dictionary<string, TalkData> _talkDataMap;

        public TalkManager TalkManager => _talkManager;

        private void Start()
        {
            Initialize().Forget();
        }

        private async UniTask Initialize()
        {
            TalkDataLoader loader = new();
            _talkDataMap = await loader.LoadFromUrl(_url);

            _talkManager = new TalkManager();
            _talkManager.SetView(_talkView);

            Debug.Log($"Talk Loaded: {_talkDataMap.Count} talks");
        }

        public bool TryGetTalkData(string talkID, out TalkData talkData)
        {
            return _talkDataMap.TryGetValue(talkID, out talkData);
        }
    }
}