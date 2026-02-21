using UnityEngine;
using UnityEngine.UI;

namespace TowerDefense
{
    public class TextUpdate : MonoBehaviour
    {
        public enum UpdateSource
        {
            Gold,
            Life,
            DPSKilled,
            TotalKills
        }
        public UpdateSource source = UpdateSource.Life;
        private Text m_text;

        private void Start()
        {
            m_text = GetComponent<Text>();

            switch (source)
            {
                case UpdateSource.Gold:
                    TDPlayer.GoldUpdateSubscribe(UpdateText);
                    break;
                case UpdateSource.Life:
                    TDPlayer.LifeUpdateSubscribe(UpdateText);
                    break;
            }
        }

        private void OnDestroy()
        {
            switch (source)
            {
                case UpdateSource.Gold:
                    TDPlayer.GoldUpdateUnsubscribe(UpdateText);
                    break;
                case UpdateSource.Life:
                    TDPlayer.LifeUpdateUnsubscribe(UpdateText);
                    break;
            }
        }


        private void UpdateText(int money)
        {
            m_text.text = money.ToString();
        }
    }
}
