using SpaceShooter;
using System.Collections;
using System.Collections.Generic;
using TowerDefense;
using UnityEditor.VersionControl;
using UnityEngine;
using static System.Net.Mime.MediaTypeNames;
using static TowerDefense.Upgrades;

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
                    var newControl = Instantiate(m_TowerBuyPrefab, transform);
                    m_ActiveControl.Add(newControl);
                    newControl.transform.position += Vector3.left * 160 * i;

                    bool isMageTower = (i == 1);
                    Debug.Log("PARAM TYPE" + m_MageTowerUpgrade.GetType().Name);
                    Debug.Log("PARAM NAME" + m_MageTowerUpgrade.name);

                    int mageUnlocked = Upgrades.GetUpgradeLevel(m_MageTowerUpgrade);
                    //Данный метод одновременно вызывается в Awake из скрипта TDPlayer
                    //с передачей в аргументе UpgradeAsset healthUpgrade!!!!!!!!!!!!!!!!
                    
                    //При нажатии мышкой на BuildSite, Значение mageUnlocked вернётся сюда равным 0.
                    if (isMageTower && mageUnlocked == 0) continue;
                    else
                    {
                        newControl.SetTowerAsset(m_TowerAssets[i]);
                    }
                }
            }
            else
            {
               foreach(var control in m_ActiveControl)Destroy(control.gameObject);
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
