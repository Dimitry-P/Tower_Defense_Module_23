using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace SpaceShooter
{
    /// <summary>
    /// Контроллер панельки запуска эпизода. 
    /// </summary>
    public class EpisodeSelectionController : MonoBehaviour
    {
        /// <summary>
        /// Ссылка на эпизод.
        /// </summary>
        [SerializeField] private Episode m_Episode;

        /// <summary>
        /// Ссылка на текстовый контрол для выставления названия эпизода.
        /// </summary>
        [SerializeField] private Text m_EpisodeNickname;

        /// <summary>
        /// Ссылка на картинку для установки превьюхи.
        /// </summary>
        [SerializeField] private Image m_PreviewImage;

        private void Start()
        {
            if (m_EpisodeNickname != null)
                m_EpisodeNickname.text = m_Episode.EpisodeName;

            if (m_PreviewImage != null)
                m_PreviewImage.sprite = m_Episode.PreviewImage;
        }

        public void OnStartEpisodeButtonClicked()
        {
            LevelSequenceController.Instance.StartEpisode(m_Episode);
        }
    }
}