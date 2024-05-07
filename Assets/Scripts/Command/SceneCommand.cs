using System;
using System.Collections.Generic;
using UnityEngine;
using QFramework;
using System.Collections;
using UnityEngine.SceneManagement;

/// <summary>
/// 用于异步加载场景的命令
/// </summary>
public class AsyncLoadSceneCommand : AbstractCommand
{
    public string sceneName;
    public MonoBehaviour mono;

    public AsyncLoadSceneCommand(string sceneName, MonoBehaviour mono)
    {
        this.sceneName = sceneName;
        this.mono = mono;
    }

    protected override void OnExecute()
    {
        mono.StartCoroutine(AsyncLoadScene(sceneName));
    }

    IEnumerator AsyncLoadScene(string sceneName)
    {
        // 异步加载新场景
        AsyncOperation asyncLoad = SceneManager.LoadSceneAsync(sceneName);

        // 等待加载完毕
        while (!asyncLoad.isDone)
        {
            // 这里可以添加加载进度的显示代码
            // 例如: progress = asyncLoad.progress;
            yield return null;
        }
    }
}

/// <summary>
/// 用于保存数据的命令
/// </summary>
public class SaveDataCommand : AbstractCommand
{
    protected override void OnExecute()
    {
        PlayerModel.Instance.SaveToLocal();
    }
}

/// <summary>
/// 在两个gamemanager中 检查playermodel有没有被初始化 根据SceneModel中的isModelInit
/// 如果没有被初始化 则加载本地存档
/// (在游戏点击开始的时候设置init为ture 游戏关闭的时候设置为false)
/// </summary>
public class CheckPlayerModelLoadLoacalSaveCommand : AbstractCommand
{
    protected override void OnExecute()
    {
        var sceneModel = this.GetModel<SceneModel>();
        if (sceneModel.isModelInit == false)
        {
            PlayerModel.Instance.LoadLocalSave();
            sceneModel.isModelInit = true;
        }
        else
        {
            Debug.Log("already init player model");
        }

    }
}

/// <summary>
/// 触发事件要ship 保存memento 到playermodel 中
/// </summary>
public class TriggerSaveShipMementoCommand : AbstractCommand
{
    public object sender = null;
    public object args = null;

    protected override void OnExecute()
    {
        EventCenter.Instance.TriggerEvent("ShipMementoSave", sender, args);
    }
}