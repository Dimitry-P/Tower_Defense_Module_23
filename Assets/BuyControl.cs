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

        private void MoveToBuildSite(BuildSite buildSite)
        {
            if (buildSite)
            {
                var position = cam.WorldToScreenPoint(buildSite.transform.root.position);
                m_RectTransform.position = position;
                
                m_ActiveControl = new List<TowerBuyControl>();
                foreach (var asset in buildSite.buildableTowers)
                {
                    var newControl = Instantiate(m_TowerBuyPrefab, transform);
                    m_ActiveControl.Add(newControl);
                    //newControl.transform.position += Vector3.left * 160 * i;

                    //Данный метод одновременно вызывается в Awake из скрипта TDPlayer
                    //с передачей в аргументе UpgradeAsset healthUpgrade!!!!!!!!!!!!!!!!
                    
                    //При нажатии мышкой на BuildSite, Значение mageUnlocked вернётся сюда равным 0.
                    if (!asset.IsAvailable()) continue;
                    else
                    {
                        newControl.SetTowerAsset(asset);
                    }
                }
                if(m_ActiveControl.Count > 0)
                {
                    gameObject.SetActive(true);
                    var angle = 360 / m_ActiveControl.Count;
                    for (int i = 0; i < m_ActiveControl.Count; i++)
                    {
                        var offset = Quaternion.AngleAxis(angle * i, Vector3.forward) * (Vector3.left * 160);
                        m_ActiveControl[i].transform.position += offset;
                    }
                    if (buildSite != null)
                    {
                        foreach (var tbc in GetComponentsInChildren<TowerBuyControl>())
                        {
                            tbc.SetBuildSite(buildSite.transform.root);
                        }
                    }
                }
            }
            else
            {
                foreach (var control in m_ActiveControl) Destroy(control.gameObject);
                m_ActiveControl.Clear();
                gameObject.SetActive(false);
            }
        }
    }
}
