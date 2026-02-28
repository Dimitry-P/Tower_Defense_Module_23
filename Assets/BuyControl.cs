using System.Collections;
using System.Collections.Generic;
using TowerDefense;
using UnityEngine;
using SpaceShooter;

namespace TowerDefense
{
    public class BuyControl : MonoBehaviour
    {
        private RectTransform t;
        private Camera cam;
        private void Awake()
        {
            cam = Camera.main;
            t = GetComponent<RectTransform>();
            BuildSite.OnClickEvent += MoveToBuildSite;
            gameObject.SetActive(false);
        }
        private void MoveToBuildSite(Transform buildSite)
        {
            if (buildSite)
            {
                var position = cam.WorldToScreenPoint(buildSite.position);
                t.position = position;
                gameObject.SetActive(true);
            }
            else
            {
                gameObject.SetActive(false);
            }
            //foreach (var tbc in GetComponentsInChildren<TowerBuyControl>())
            //{
            //    tbc.SetBuildSite(buildSite);
            //}
        }
        private void OnDestroy()
        {
            BuildSite.OnClickEvent -= MoveToBuildSite;
        }
    }
}
