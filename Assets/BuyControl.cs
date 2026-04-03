using System.Collections;
using System.Collections.Generic;
using TowerDefense;
using UnityEngine;
using SpaceShooter;

namespace TowerDefense
{
    public class BuyControl : MonoBehaviour
    {
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
            }
            else
            {
                gameObject.SetActive(false);
            }
            foreach (var tbc in GetComponentsInChildren<TowerBuyControl>())
            {
                tbc.SetBuildSite(buildSite);
            }
        }
    }
}
