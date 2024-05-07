using System;
using System.Collections.Generic;

namespace Johnwest
{
    public static partial class JWUniversalTool
    {
        /// <summary>
        /// 将List转换为字符串
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="list"></param>
        /// <param name="printNum"></param>
        /// <returns></returns>
        public static string ListToString<T>(List<T> list, bool printNum = true)
        {
            if (list is null)
            {
                return "null";
            }

            List<string> result = new List<string>();
            foreach (var item in list)
            {
                result.Add(item.ToString());
            }

            string restultStr;
            if (printNum)
            {
                restultStr = $"Count: {list.Count}\n" + string.Join("\n", result);
            }
            else
            {
                restultStr = string.Join("\n", result);
            }
            return restultStr;
        }

        public static string DictToString<TKey, TValue>(Dictionary<TKey, TValue> dict)
        {
            if (dict is null)
            {
                return "null";
            }

            List<string> result = new List<string>();
            foreach (var item in dict)
            {
                result.Add(item.Key.ToString() + " : " + item.Value.ToString());
            }

            return string.Join("\n", result);
        }

        public static Dictionary<string, int> DictKeyToStringDict<T>(Dictionary<T, int> dict)
        {
            var result = new Dictionary<string, int>();
            foreach (var item in dict)
            {
                result[item.Key.ToString()] = item.Value;
            }
            return result;
        }
    }

    /// <summary>
    /// 简单单例父类
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public abstract class Singleton<T> where T : Singleton<T>, new()
    {
        private static T instance;

        public static T Instance
        {
            get
            {
                if (instance == null)
                {
                    instance = new T();
                }
                return instance;
            }
        }
    }
}
