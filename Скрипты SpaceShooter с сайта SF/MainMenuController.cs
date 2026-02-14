using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace SpaceShooter
{
    /// <summary>
    /// Контроллер главного меню. Стартовая сцена должна идти под номером 0 в BuildSettings как первая сцена.
    /// По сути сцена с главным меню.
    /// </summary>
    public class MainMenuController : MonoBehaviour
    {
        [SerializeField] private GameObject m_EpisodeSelection;

        public void OnButtonStartNew()
        {
            // показываем окно выбора эпизодов.
            m_EpisodeSelection.gameObject.SetActive(true);

            // главное меню выключаем.
            gameObject.SetActive(false);
        }

        public void OnButtonExit()
        {
            Application.Quit();
        }
    }
}