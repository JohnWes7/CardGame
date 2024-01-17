using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DroneEnemy : Enemy
{
    [SerializeField] private GameObject turret;
    [SerializeField] private Rigidbody2D rigi2D;
    [SerializeField] private GameObject Target;

    // 如果没有设定目标 有一个警戒范围如果有物体进入了警戒返回 就朝这个物体移动
    // TODO

    // 状态: 待机 如果有任何的目标出现 解除待机开始
    // >
    // 向核心移动 中途如果有unit在自爆范围内 直接自爆造成伤害

    // stretagy pattern 用在炮塔开火
    // 创造一个SO 的父类
    // 子类只重写开火方式 这样开火方式就到了static的SO里面去了

    private void Start()
    {
        rigi2D = GetComponent<Rigidbody2D>();
        
    }

}
