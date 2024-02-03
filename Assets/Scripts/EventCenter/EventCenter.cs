using System;
using System.Collections.Generic;

public class EventCenter : Singleton<EventCenter>
{
    private Dictionary<string, EventHandler<object>> eventDict = new Dictionary<string, EventHandler<object>>();

    public void AddEventListener(string eventType, EventHandler<object> listener)
    {
        if (eventDict.ContainsKey(eventType))
        {
            eventDict[eventType] += listener;
        }
        else
        {
            eventDict.Add(eventType, listener);
        }
    }

    public void RemoveEventListener(string eventType, EventHandler<object> listener)
    {
        if (eventDict.ContainsKey(eventType))
        {
            eventDict[eventType] -= listener;
        }
    }

    public void TriggerEvent(string eventType, object sender, object eventParam = null)
    {
        if (eventDict.ContainsKey(eventType))
        {
            eventDict[eventType]?.Invoke(sender, eventParam);
        }
    }
}

public class Singleton<T> where T : new()
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
