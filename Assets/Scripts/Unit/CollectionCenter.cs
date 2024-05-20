using QFramework;
using System.Collections.Generic;
using UnityEngine;

public class CollectionCenter : MonoBehaviour, IController
{
    /// <summary>
    /// 搜索半径
    /// </summary>
    [SerializeField] private float collectionRadius = 5f;
    // 动态变量
    [SerializeField] private List<DropItem> collectingItem = new();
    [SerializeField] public float speed = 0.1f;

    private void Update()
    {
        RaycastHit2D[] hitinfo = Physics2D.CircleCastAll(transform.position, collectionRadius, Vector2.zero, 0f, LayerMask.GetMask("DropItem"));
        
        //if (hitinfo.Length > 0)
        //{
        //    Debug.Log($"搜索到 {hitinfo.Length} 数量的物品");
        //}
        
        foreach (var hitCollider in hitinfo)
        {
            DropItem dropItem = hitCollider.transform.GetComponent<DropItem>();
            if (dropItem != null && dropItem.Pickuper == null)
            {
                dropItem.Pickuper = this;
                collectingItem.Add(dropItem);
            }
        }

        // 移动正在被收集的物品
        List<DropItem> deleteList = new();
        foreach (DropItem item in collectingItem)
        {
            // 计算移动方向
            Vector3 dir = (transform.position - item.transform.position);
            dir.z = 0;

            // 如果距离小于0.2f则算做已经到达 可以销毁物品并计算金钱加入仓库
            float distance = dir.magnitude;
            if (distance < 0.2f)
            {
                deleteList.Add(item);
                Destroy(item.gameObject);
                Debug.Log($"收集到物品: SO: {item.ItemSO} / {item.Num}");
                this.SendCommand(new ReceiveDropItemCommand(item));
                continue;
            }

            // 计算移动和相距距离
            Vector3 deltaMove = speed * Time.deltaTime * dir.normalized;
            item.transform.position = item.transform.position + deltaMove;
        }
        // 移除已经被收集的物品
        collectingItem.RemoveAll(item => deleteList.Contains(item));
    }

    public IArchitecture GetArchitecture()
    {
        return GameArchitecture.Interface;
    }

    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.yellow; // 设置 Gizmos 颜色
        Gizmos.DrawWireSphere(transform.position, collectionRadius); // 绘制表示射程范围的圆形
    }

}
