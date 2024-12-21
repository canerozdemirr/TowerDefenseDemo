using Gameplay.Enemies;
using Gameplay.Projectiles;
using Utilities.TypeUtilities;

namespace Interfaces
{
    public interface IProjectileSpawner
    {
        BaseProjectile Spawn(Enums.ProjectileType projectileType);
        void DeSpawn(BaseProjectile enemy);
        void ClearPools();
    }
}
