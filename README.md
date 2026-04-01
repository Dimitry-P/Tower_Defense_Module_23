# Tower_Defense_Module_23
SkillFactory_Project

Implement armor types, damage interactions, and projectile migration tools

Implemented a vulnerability system with different armor and damage types.

Two armor types were introduced:
- Normal armor
- Magic armor

Two damage types were introduced:
- Normal damage
- Magic damage

Magic damage completely ignores normal armor and interacts only with magic armor.
Magic armor reduces magic damage using the standard formula, but is only half as effective against normal (physical) damage.

Multiple parameters were updated across towers, enemies, and attacking projectiles to support the new damage system.

Created a new Projectile class with an overridden OnHit() method.

Added SetFromOtherProjectile() method to Projectile. This allows copying all relevant data
from an existing projectile instance to another one, which simplifies migration from old projectile types.

Implemented a custom inspector:

public class ProjectileInspector : Editor

This inspector adds a "Create TD Projectile" button to all Projectile components.
When pressed, it:
- Adds TDProjectile component
- Copies data from the old projectile via SetFromOtherProjectile()
- Removes the old Projectile component

This script is used purely as a migration utility to transfer data from legacy projectiles
to the new TDProjectile-based system.

Implemented armor-based damage calculation inside Enemy class:

public void TakeDamage(int damage, TDProjectile.DamageType damageType)
{
    m_destructible.ApplyDamage(
        ArmorDamageFunctions[m_ArmorType](damage, damageType, m_armor)
    );
}

Instead of applying raw damage directly, the system now calculates final damage
using an array of delegate functions:

ArmorDamageFunctions[m_ArmorType](damage, damageType, m_armor)

Key idea:
- Enemy receives raw damage (damage + damageType)
- Enemy has an armor type (m_ArmorType)
- Depending on armor type, damage is modified using a specific calculation function
- Each function returns final damage value

ArmorDamageFunctions is an array of delegates.
Each delegate calculates final damage based on:
(damage, damageType, armorValue)

m_ArmorType is an enum (internally int), which allows using it as an array index.
This approach avoids large switch statements and makes the system easily extensible.

Call chain:
1. Projectile calls enemy.TakeDamage(damage, damageType)
2. Enemy calculates final damage using ArmorDamageFunctions[m_ArmorType](...)
3. Destructible.ApplyDamage(finalDamage) reduces enemy health

Also updated gameplay mechanics:
Player can now click on BuildSite during gameplay and select one of two tower types.
Each tower shoots different projectile types.

Known issue:
Currently in TowerDefense enemies do not reach the final path point.
Instead, they start moving in a small circle near the last waypoint.
This bug is not yet resolved.
