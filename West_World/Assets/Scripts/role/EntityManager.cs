using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EntityManager : MonoBehaviour
{
    public static Dictionary<int, BaseGameEntity> entityMap = new Dictionary<int, BaseGameEntity>();
    /// <summary>
    /// 注册数据
    /// </summary>
    /// <param name="newEntity"></param>
    public static void RegisterEntity(BaseGameEntity newEntity)
    {
        entityMap.Add(newEntity.m_ID, newEntity);
    }
    /// <summary>
    /// 通过ID获得实例
    /// </summary>
    /// <param name="id"></param>
    /// <returns></returns>
    public static BaseGameEntity GetEntityFromID(int id)
    {
        BaseGameEntity baseGameEntity;
        entityMap.TryGetValue(id, out baseGameEntity);
        return baseGameEntity;
    }
    /// <summary>
    /// 删除数据
    /// </summary>
    /// <param name="entity"></param>
    public static void RemoveEntitty(BaseGameEntity entity)
    {
        entityMap.Remove(entity.m_ID);
    }
}
