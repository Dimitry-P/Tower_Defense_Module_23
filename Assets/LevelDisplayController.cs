using UnityEngine;
using SpaceShooter;

namespace TowerDefense
{
    public class LevelDisplayController : MonoBehaviour
    {
        [SerializeField] private MapLevel[] levels;
        [SerializeField] private BranchLevel[] branchLevels;


        void Start()
        {
            var drawLevel = 0;

            while (drawLevel < levels.Length)
            {
                levels[drawLevel].Initialise();
                drawLevel++;
            }

            RefreshLevels();
        }

        //void Start()
        //{
        //    var drawLevel = 0;
        //    var score = 1;
        //    while(score != 0 && drawLevel < levels.Length)
        //    {
        //        levels[drawLevel].Initialise();

        //        drawLevel++; 
        //    }

        //for (int i = MapCompletion.Instance.UnlockedLevels + 1; i < levels.Length; i++)
        //{
        //    levels[i].gameObject.SetActive(false);
        //}

        //for (int i = 0; i < branchLevels.Length; i++)
        //{
        //    branchLevels[i].TryActivate();
        //}
        //}

        //public void RefreshBranchLevels()
        //{
        //    for (int i = 0; i < branchLevels.Length; i++)
        //    {
        //        branchLevels[i].TryActivate();
        //    }
        //}
        public void RefreshLevels()
        {
            for (int i = levels.Length - 1; i > MapCompletion.Instance.UnlockedLevels; i--)
            {
                levels[i].gameObject.SetActive(false);
            }

            for (int i = 0; i < branchLevels.Length; i++)
            {
                branchLevels[i].TryActivate();
            }
        }
    }
}
