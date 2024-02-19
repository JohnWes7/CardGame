using System.Collections;
using System.Collections.Generic;
using UnityEngine;


namespace Johnwest
{
    /// <summary>
    /// 通用工具类
    /// </summary>
    public class JWUniversalTool
    {
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
