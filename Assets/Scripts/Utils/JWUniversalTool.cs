using System.Collections.Generic;
using UnityEngine;

namespace Johnwest
{
    /// <summary>
    /// 通用工具类
    /// </summary>
    public static class JWUniversalTool
    {
        public static string DictToString<TKey, TValue>(Dictionary<TKey, TValue> dict)
        {
            List<string> result = new List<string>();
            foreach (var item in dict)
            {
                result.Add(item.Key.ToString() + " : " + item.Value.ToString());
            }

            return string.Join("\n", result);
        }

        public static Transform FindChildByName(this Transform transform, string childName)
        {
            // 获取父物体的所有子物体
            if (transform != null)
            {
                foreach (Transform childTransform in transform)
                {
                    // 检查子物体的名称是否匹配
                    if (childTransform.name == childName)
                    {
                        // 找到了匹配的子物体
                        Debug.Log("找到子物体：" + childTransform.name);
                        return childTransform;
                    }
                }
            }

            // 如果找不到匹配的子物体
            Debug.Log("未找到名称为 " + childName + " 的子物体");
            return null;
        }

        public static Transform GetCanvase()
        {
            var canvas = GameObject.Find("Canvas");
            if (canvas != null)
            {
                return canvas.transform;
            }
            return null;
        }

        public static void LogWithClassMethodName(object message, System.Reflection.MethodBase method)
        {
            // 获取类名和方法名
            string className = method.ReflectedType.Name;
            string methodName = method.Name;

            // 输出带有类名和方法名的日志
            Debug.Log($"{className}.{methodName}: {message}");
        }

        public static void LogErrorWithClassMethodName(object message, System.Reflection.MethodBase method)
        {
            // 获取类名和方法名
            string className = method.ReflectedType.Name;
            string methodName = method.Name;

            // 输出带有类名和方法名的日志
            Debug.LogError($"{className}.{methodName}: {message}");
        }

        public static void LogWarningWithClassMethodName(object message, System.Reflection.MethodBase method)
        {
            // 获取类名和方法名
            string className = method.ReflectedType.Name;
            string methodName = method.Name;

            // 输出带有类名和方法名的日志
            Debug.LogWarning($"{className}.{methodName}: {message}");
        }
    }

}
