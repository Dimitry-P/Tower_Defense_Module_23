using TMPro;
using TowerDefense;
using UnityEngine;

namespace SpaceShooter
{
    public class TotalScoreDisplay : MonoBehaviour
    {
        [SerializeField] private TextMeshProUGUI totalScoreText;

        void Start()
        {
            if (totalScoreText == null)
            {
                Debug.LogWarning("Не привязан totalScoreText в инспекторе!");
                return;
            }

            int total = MapCompletion.Instance.GetTotalScore();
            totalScoreText.text = total.ToString();
        }
    }
}
