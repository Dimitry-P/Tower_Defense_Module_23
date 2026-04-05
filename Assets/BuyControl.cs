using System.Collections;
using System.Collections.Generic;
using TowerDefense;
using UnityEngine;
using SpaceShooter;

namespace TowerDefense
{
    public class BuyControl : MonoBehaviour
    {
        [SerializeField] private TowerBuyControl m_TowerBuyPrefab;
        [SerializeField] private TowerAsset[] m_TowerAssets;
        [SerializeField] private UpgradeAsset m_MageTowerUpgrade;
        private List<TowerBuyControl> m_ActiveControl;
        private RectTransform m_RectTransform;
        private Camera cam;

        #region События Unity
        private void Awake()
        {
            cam = Camera.main;
            m_RectTransform = GetComponent<RectTransform>();
            BuildSite.OnClickEvent += MoveToBuildSite;
            gameObject.SetActive(false);
        }
        private void OnDestroy()
        {
            BuildSite.OnClickEvent -= MoveToBuildSite;
        }
        #endregion

        private void MoveToBuildSite(Transform buildSite)
        {
            if (buildSite)
            {
                var position = cam.WorldToScreenPoint(buildSite.position);
                m_RectTransform.position = position;
                gameObject.SetActive(true);
                m_ActiveControl = new List<TowerBuyControl>();
               for(int i = 0; i < m_TowerAssets.Length; i++)
                {
                    if (i != 1 || Upgrades.GetUpgradeLevel(m_MageTowerUpgrade) > 0)
                    {
                        var newControl = Instantiate(m_TowerBuyPrefab, transform);
                        m_ActiveControl.Add(newControl);
                        newControl.transform.position += Vector3.left * 160 * i;
                        newControl.SetTowerAsset(m_TowerAssets[i]);
                    }
                }
            }
            else
            {
                if (m_ActiveControl != null)
                {
                    foreach (var control in m_ActiveControl)
                        Destroy(control.gameObject);
                }

                gameObject.SetActive(false);
            }
            if (buildSite != null)
            {
                foreach (var tbc in GetComponentsInChildren<TowerBuyControl>())
                {
                    tbc.SetBuildSite(buildSite);
                }
            }
        }
    }
}
