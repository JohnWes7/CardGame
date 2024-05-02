using System;
using System.Collections.Generic;
using UnityEngine;
using QFramework;
using System.Collections;
using UnityEngine.SceneManagement;

// 用于异步加载场景的命令
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

// 用于保存数据的命令
public class SaveDataCommand : AbstractCommand
{
    protected override void OnExecute()
    {
        PlayerModel.Instance.SaveToLocal();
    }
}
