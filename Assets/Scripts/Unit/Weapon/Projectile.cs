using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Projectile : MonoBehaviour
{
    [SerializeField] protected UnitObject creater;
    [SerializeField] protected ProjectileSO projectileSO;
    [SerializeField] protected Transform target;
    [SerializeField] protected const float PROJECTILE_DURATION = 5f;

    public ProjectileSO ProjectileSO { get => projectileSO; set => projectileSO = value; }
    public Transform Target { get => target; set => target = value; }

    public static Projectile ProjectileCreateFactory(ProjectileSO projectileSO, Transform target)
    {
        return ProjectileCreateFactory(projectileSO, target, Vector3.zero);
    }

    public static Projectile ProjectileCreateFactory(ProjectileSO projectileSO, Transform target, Vector3 originPos, UnitObject creater = null)
    {
        GameObject go = Instantiate<GameObject>(projectileSO.prefab);

        Projectile projectile = go.GetComponent<Projectile>();
        projectile = projectile != null ? projectile : go.AddComponent<Projectile>();
        projectile.target = target;
        projectile.projectileSO = projectileSO;

        projectile.transform.position = originPos;

        return projectile;
    }
}
