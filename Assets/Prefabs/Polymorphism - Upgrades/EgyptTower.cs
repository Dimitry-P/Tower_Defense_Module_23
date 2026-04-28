using TowerDefense;
using UnityEngine;

public class EgyptTower : Tower
{
    [SerializeField] private float triggerRadius = 5f;

    private int charges;
    private int maxCharges;
    private bool triggered;

    public void SetUpgradeLevel(int level)
    {
        if (level <= 0) return;

        maxCharges = level;
        charges = maxCharges;
        enabled = charges > 0;
    }

    private void Update()
    {
        if (charges <= 0 || triggered) return;

        Collider2D[] hits = Physics2D.OverlapCircleAll(transform.position, triggerRadius);

        foreach (var hit in hits)
        {
            Enemy enemy = hit.GetComponentInParent<Enemy>();
            if (enemy != null)
            {
                triggered = true;
                DestroyAllEnemies();
                break;
            }
        }
    }

    private void DestroyAllEnemies()
    {
        Enemy[] allEnemies = FindObjectsOfType<Enemy>();

        Debug.Log("Destroying enemies count: " + allEnemies.Length);

        foreach (var enemy in allEnemies)
        {
            Destroy(enemy.gameObject);
            Debug.Log("Destroyed");
        }
        Debug.Log("Before " + charges);
        charges--;
        Debug.Log("After " + charges);
        triggered = false;

        if (charges <= 0)
            enabled = false;
    }
}