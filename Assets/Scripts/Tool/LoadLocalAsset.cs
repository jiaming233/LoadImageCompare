using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.Networking;

/// <summary>
/// 资源加载
/// </summary>
public class LoadLocalAsset : Singleton<LoadLocalAsset>
{
    /// <summary>
    /// 已加载图片缓存
    /// </summary>
    private Dictionary<string, Texture2D> textures = new Dictionary<string, Texture2D>();

    /// <summary>
    /// 加载图片
    /// </summary>
    public Texture2D LoadTexture(string path)
    {
        if (string.IsNullOrEmpty(path))
        {
            Debug.LogError("图片加载路径为null");
            return null;
        }

        var bytes = FileTool.FileRead(path);
        if (bytes != null)
        {
            Texture2D texture = new Texture2D(Screen.width, Screen.height);
            texture.LoadImage(bytes);
            texture.Compress(true);

            return texture;
        }
        else
        {
            Debug.LogError("图片加载失败:" + path);
            return null;
        }
    }

    /// <summary>
    /// 异步加载图片
    /// </summary>
    /// <param name="path"></param>
    /// <param name="call"></param>
    public void LoadTextureAsync(string path, UnityAction<float, Texture2D> call)
    {
        if (string.IsNullOrEmpty(path))
        {
            call?.Invoke(-1, null);
        }
        else
            StartCoroutine(ILoadTexture(path, call));
    }
    /// <summary>
    /// 加载图片协程
    /// </summary>
    private IEnumerator ILoadTexture(string path, UnityAction<float, Texture2D> call)
    {
        switch (Application.platform)
        {
            case RuntimePlatform.Android:
            case RuntimePlatform.IPhonePlayer:
                path = "file://" + path;
                break;
        }

        using (UnityWebRequest request = UnityWebRequestTexture.GetTexture(path))
        {
            request.SendWebRequest();
            if (request.result == UnityWebRequest.Result.ConnectionError)
            {
                Debug.LogError("图片加载出错：" + request.error);
                call?.Invoke(-1, null);
                yield break;
            }
            while (!request.isDone)
            {
                yield return new WaitForSeconds(0.5f);
                call?.Invoke(request.downloadProgress, null);
            }
            if (request.isDone)
            {
                Texture2D texture = DownloadHandlerTexture.GetContent(request);
                if (texture)
                    call?.Invoke(1, texture);
                else
                {
                    Debug.LogError(string.Format("路径为{0}的图片加载失败！", path));
                    call?.Invoke(-1, null);
                }
            }
        }
    }

    /// <summary>
    /// 加载图片并缓存
    /// </summary>
    public Texture2D LoadTextureCache(string path)
    {
        if (string.IsNullOrEmpty(path))
        {
            Debug.LogError("图片加载路径为null");
            return null;
        }

        if (textures.ContainsKey(path) && textures[path] != null)
        {
            Debug.LogWarning("图片已加载:" + path);
            return textures[path];
        }

        var bytes = FileTool.FileRead(path);
        if (bytes != null)
        {
            Texture2D texture = new Texture2D(Screen.width, Screen.height);
            texture.LoadImage(bytes);
            texture.Compress(true);

            if (textures.ContainsKey(path))
                textures[path] = texture;
            else
                textures.Add(path, texture);

            return texture;
        }
        else
        {
            Debug.LogError("图片加载失败:" + path);
            if (textures.ContainsKey(path))
                textures.Remove(path);

            return null;
        }
    }

    /// <summary>
    /// 异步加载图片并缓存
    /// </summary>
    /// <param name="path"></param>
    /// <param name="call"></param>
    public void LoadTextureAsyncCache(string path, UnityAction<float, Texture2D> call)
    {
        if (string.IsNullOrEmpty(path))
        {
            Debug.LogError("图片加载路径为null");
            call?.Invoke(-1, null);
        }
        else
        {
            if (textures.ContainsKey(path) && textures[path] != null)
            {
                Debug.LogWarning("图片已加载:" + path);
                call?.Invoke(1, textures[path]);
            }
            else
                StartCoroutine(ILoadTextureCache(path, call));
        }
    }
    /// <summary>
    /// 加载图片协程
    /// </summary>
    private IEnumerator ILoadTextureCache(string path, UnityAction<float, Texture2D> call)
    {
        switch (Application.platform)
        {
            case RuntimePlatform.Android:
            case RuntimePlatform.IPhonePlayer:
                path = "file://" + path;
                break;
        }

        using (UnityWebRequest request = UnityWebRequestTexture.GetTexture(path))
        {
            request.SendWebRequest();
            if (request.result == UnityWebRequest.Result.ConnectionError)
            {
                Debug.LogError("图片加载出错：" + request.error);
                if (textures.ContainsKey(path))
                    textures.Remove(path);

                call?.Invoke(-1, null);
                yield break;
            }
            while (!request.isDone)
            {
                yield return new WaitForSeconds(0.5f);
                call?.Invoke(request.downloadProgress, null);
            }
            if (request.isDone)
            {
                Texture2D texture = DownloadHandlerTexture.GetContent(request);
                if (texture)
                {
                    if (textures.ContainsKey(path))
                        textures[path] = texture;
                    else
                        textures.Add(path, texture);

                    call?.Invoke(1, texture);
                }
                else
                {
                    Debug.LogError(string.Format("路径为{0}的图片加载失败！", path));
                    if (textures.ContainsKey(path))
                        textures.Remove(path);

                    call?.Invoke(-1, null);
                }
            }
        }
    }

    /// <summary>
    /// 卸载图片,移除缓存
    /// </summary>
    /// <param name="path">图片本地路径</param>
    public void UnloadTexture(string path)
    {
        textures.Remove(path);
        System.DateTime now = System.DateTime.Now;
        Resources.UnloadUnusedAssets();
        System.GC.Collect();
        Debug.LogWarning((System.DateTime.Now - now).TotalMilliseconds);
    }

    /// <summary>
    /// 卸载所有已加载图片
    /// </summary>
    public void UnloadTextureAll()
    {
        textures.Clear();
        System.DateTime now = System.DateTime.Now;
        Resources.UnloadUnusedAssets();
        System.GC.Collect();
        Debug.LogWarning((System.DateTime.Now - now).TotalMilliseconds);
    }
  
    protected override void InstanceDestroy()
    {
        textures.Clear();
        Resources.UnloadUnusedAssets();
        System.GC.Collect();
    }
}