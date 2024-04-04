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

public abstract class Singleton<T> where T : new()
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

// 优先级事件
public class PrioritySubscriber<T> where T : EventArgs
{
    public EventHandler<T> fireAction { get; }
    public int Priority { get; }

    public PrioritySubscriber(EventHandler<T> eventHandler, int priority)
    {
        fireAction = eventHandler;
        Priority = priority;
    }
}

// 优先级事件管理器
public class PriorityEventManager<T> where T : EventArgs
{
    private readonly List<PrioritySubscriber<T>> _subscribers = new List<PrioritySubscriber<T>>();

    // 添加订阅者
    public void AddListener(EventHandler<T> handler, int priority)
    {
        _subscribers.Add(new PrioritySubscriber<T>(handler, priority));
        // 按优先级排序，优先级较高的订阅者排在前面
        _subscribers.Sort((x, y) => x.Priority.CompareTo(y.Priority));
    }

    // 移除订阅者
    public void RemoveListener(EventHandler<T> handler)
    {
        _subscribers.RemoveAll(subscriber => subscriber.fireAction == handler);
    }

    // 触发事件
    public void Invoke(object sender, T args)
    {
        foreach (var subscriber in _subscribers)
        {
            subscriber.fireAction.Invoke(sender, args);
        }
    }
}
