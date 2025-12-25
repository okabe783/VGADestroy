using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace Talk
{
    // TalkManager経由で描画をする
    public class TalkView : MonoBehaviour
    {
        [Header("セリフ")] [SerializeField] 
        private Text _nameText;
        [SerializeField] 
        private Text _contentText;

        [Header("選択肢")] [SerializeField] 
        private Button _choiceButton;
        [SerializeField] 
        private Transform _choiceRoot;

        private readonly List<Button> _choiceButtons = new();

        // 通知を受け取るイベント
        public Action<int> OnChoiceSelected;

        // Manager側のイベントに登録する
        public void Show(TalkNode talkNode)
        {
            _nameText.text = talkNode.Speaker;
            _contentText.text = talkNode.Text;
            // 既存選択肢をクリア
            ClearChoices();

            // 選択肢再生成
            for (int i = 0; i < talkNode.Choices.Count; i++)
            {
                int index = i;

                Button button = Instantiate(_choiceButton, _choiceRoot);
                button.GetComponentInChildren<Text>().text = talkNode.Choices[i].Text;

                button.onClick.AddListener(() => OnChoiceSelected?.Invoke(index));
                _choiceButtons.Add(button);
            }
        }

        private void ClearChoices()
        {
            foreach (Button button in _choiceButtons)
            {
                Destroy(button.gameObject);
            }

            _choiceButtons.Clear();
        }
    }
}