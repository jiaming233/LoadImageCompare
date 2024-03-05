using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;

public class Sample : MonoBehaviour
{
    /// <summary>
    /// 路径
    /// </summary>
    public string path;
    /// <summary>
    /// 重复加载次数
    /// </summary>
    public int count = 10;

    #region UI
    public GameObject Canvas;
    public Transform raws;
    public Transform ratioRaws;
    public Transform sprites;
    #endregion

    private Dictionary<Transform, Texture2D> rawTex = new Dictionary<Transform, Texture2D>();
    private Dictionary<Transform, Sprite> rawSprites = new Dictionary<Transform, Sprite>();

    private void Start()
    {
        if (string.IsNullOrEmpty(path))
            path = System.IO.Path.Combine(Application.dataPath, "Resources/TestSprite.png");
    }

    #region RawImage
    /// <summary>
    /// 加载图片，通过RawImage显示
    /// </summary>
    public void LoadRawImage()
    {
        raws.gameObject.SetActive(true);
        sprites.gameObject.SetActive(false);
        ratioRaws.gameObject.SetActive(false);

        DateTime now = DateTime.Now;

        Transform item = raws.GetChild(0);
        Transform tra;
        for (int i = 0; i < count; i++)
        {
            tra = Instantiate(item, raws);
            tra.gameObject.SetActive(true);
            DateTime now1 = DateTime.Now;
            Texture2D texture2D = LoadLocalAsset.Instance.LoadTexture(path);
            Debug.LogWarning((DateTime.Now - now1).TotalMilliseconds);
            if (texture2D)
            {
                RawImage rawImage = tra.GetComponent<RawImage>();
                rawImage.texture = texture2D;
                rawImage.SetAlpha(1);

                rawTex.Add(tra, texture2D);
            }
        }

        Debug.LogWarning((DateTime.Now - now).TotalMilliseconds);
    }

    /// <summary>
    /// 加载图片并缓存，通过RawImage显示
    /// </summary>
    public void LoadRawImageCache()
    {
        raws.gameObject.SetActive(true);
        sprites.gameObject.SetActive(false);
        ratioRaws.gameObject.SetActive(false);

        DateTime now = DateTime.Now;

        Transform item = raws.GetChild(0);
        Transform tra;
        for (int i = 0; i < count; i++)
        {
            tra = Instantiate(item, raws);
            tra.gameObject.SetActive(true);
            DateTime now1 = DateTime.Now;
            Texture2D texture2D = LoadLocalAsset.Instance.LoadTextureCache(path);
            Debug.LogWarning((DateTime.Now - now1).TotalMilliseconds);
            if (texture2D)
            {
                RawImage rawImage = tra.GetComponent<RawImage>();
                rawImage.texture = texture2D;
                rawImage.SetAlpha(1);

                rawTex.Add(tra, texture2D);
            }
        }

        Debug.LogWarning((DateTime.Now - now).TotalMilliseconds);
    }

    /// <summary>
    /// 异步加载图片，通过RawImage显示
    /// </summary>
    public void LoadRawImageAsync()
    {
        raws.gameObject.SetActive(true);
        sprites.gameObject.SetActive(false);
        ratioRaws.gameObject.SetActive(false);

        DateTime now = DateTime.Now;

        int loaded = 0;

        Transform item = raws.GetChild(0);
        Transform tra;
        for (int i = 0; i < count; i++)
        {
            LoadLocalAsset.Instance.LoadTextureAsync(path, (progress, texture2D) =>
            {
                if (progress >= 1 && texture2D)
                {
                    tra = Instantiate(item, raws);
                    tra.gameObject.SetActive(true);

                    RawImage rawImage = tra.GetComponent<RawImage>();
                    rawImage.texture = texture2D;
                    rawImage.SetAlpha(1);

                    rawTex.Add(tra, texture2D);

                    loaded++;
                    if (loaded == count)
                        Debug.LogWarning((DateTime.Now - now).TotalMilliseconds);
                }
            });
        }
    }

    /// <summary>
    /// 异步加载图片，通过RawImage+AspectRatioFitter显示
    /// </summary>
    public void LoadRatioRawImageAsync()
    {
        raws.gameObject.SetActive(false);
        sprites.gameObject.SetActive(false);
        ratioRaws.gameObject.SetActive(true);

        DateTime now = DateTime.Now;

        int loaded = 0;

        Transform item = ratioRaws.GetChild(0);
        Transform tra;
        for (int i = 0; i < count; i++)
        {
            LoadLocalAsset.Instance.LoadTextureAsync(path, (progress, texture2D) =>
            {
                if (progress >= 1 && texture2D)
                {
                    tra = Instantiate(item, ratioRaws);
                    tra.gameObject.SetActive(true);

                    RawImage rawImage = tra.GetComponent<RawImage>();
                    rawImage.texture = texture2D;
                    rawImage.SetAlpha(1);
                    AspectRatioFitter aspectRatioFitter = tra.GetComponent<AspectRatioFitter>();
                    if (aspectRatioFitter)
                        aspectRatioFitter.aspectRatio = (float)texture2D.width / texture2D.height;

                    rawTex.Add(tra, texture2D);

                    loaded++;
                    if (loaded == count)
                        Debug.LogWarning((DateTime.Now - now).TotalMilliseconds);

                }
            });
        }
    }

    /// <summary>
    /// 销毁RawImages
    /// </summary>
    public void ClearRaws()
    {
        foreach (Texture2D texture2D in rawTex.Values)
            Destroy(texture2D);

        List<Transform> trans = rawTex.Keys.ToList();
        for (int i = 0; i < trans.Count; i++)
        {
            Destroy(trans[i].gameObject);
        }
        trans.Clear();
        rawTex.Clear();
    }

    /// <summary>
    /// 销毁RawImages,并清空缓存
    /// </summary>
    public void ClearRawsCache()
    {
        LoadLocalAsset.Instance.UnloadTextureAll();

        List<Transform> trans = rawTex.Keys.ToList();
        for (int i = 0; i < trans.Count; i++)
        {
            Destroy(trans[i].gameObject);
        }
        trans.Clear();
        rawTex.Clear();
    }
    #endregion

    #region Sprite
    /// <summary>
    /// 加载图片，通过Sprite显示
    /// </summary>
    public void LoadSprite()
    {
        raws.gameObject.SetActive(false);
        ratioRaws.gameObject.SetActive(false);
        sprites.gameObject.SetActive(true);

        DateTime now = DateTime.Now;
        Transform item = sprites.GetChild(0);
        Transform tra;

        for (int i = 0; i < count; i++)
        {
            tra = Instantiate(item, sprites);
            tra.gameObject.SetActive(true);
            Texture2D texture2D = LoadLocalAsset.Instance.LoadTexture(path);
            if (texture2D)
            {
                Image image = tra.GetComponent<Image>();
                Sprite s = Sprite.Create(texture2D, new Rect(0.0f, 0.0f, texture2D.width, texture2D.height), new Vector2(0.5f, 0.5f));
                image.sprite = s;
                image.preserveAspect = true;

                rawSprites.Add(tra, s);
                rawTex.Add(tra, texture2D);
            }
        }

        Debug.LogWarning((DateTime.Now - now).TotalMilliseconds);
    }

    /// <summary>
    /// 加载图片并缓存，通过Sprite显示
    /// </summary>
    public void LoadSpriteCache()
    {
        raws.gameObject.SetActive(false);
        ratioRaws.gameObject.SetActive(false);
        sprites.gameObject.SetActive(true);

        DateTime now = DateTime.Now;
        Transform item = sprites.GetChild(0);
        Transform tra;

        for (int i = 0; i < count; i++)
        {
            tra = Instantiate(item, sprites);
            tra.gameObject.SetActive(true);
            Texture2D texture2D = LoadLocalAsset.Instance.LoadTextureCache(path);
            if (texture2D)
            {
                Image image = tra.GetComponent<Image>();
                Sprite s = Sprite.Create(texture2D, new Rect(0.0f, 0.0f, texture2D.width, texture2D.height), new Vector2(0.5f, 0.5f));
                image.sprite = s;
                image.preserveAspect = true;

                rawSprites.Add(tra, s);
                rawTex.Add(tra, texture2D);
            }
        }

        Debug.LogWarning((DateTime.Now - now).TotalMilliseconds);
    }

    /// <summary>
    /// 异步加载图片，通过Sprite显示
    /// </summary>
    public void LoadSpriteAsync()
    {
        raws.gameObject.SetActive(false);
        sprites.gameObject.SetActive(true);
        ratioRaws.gameObject.SetActive(false);

        DateTime now = DateTime.Now;

        int loaded = 0;

        Transform item = sprites.GetChild(0);
        Transform tra;
        for (int i = 0; i < count; i++)
        {
            LoadLocalAsset.Instance.LoadTextureAsync(path, (progress, texture2D) =>
            {
                if (progress >= 1 && texture2D)
                {
                    tra = Instantiate(item, sprites);
                    tra.gameObject.SetActive(true);

                    Image image = tra.GetComponent<Image>();
                    Sprite s = Sprite.Create(texture2D, new Rect(0.0f, 0.0f, texture2D.width, texture2D.height), new Vector2(0.5f, 0.5f));
                    image.sprite = s;
                    image.preserveAspect = true;

                    rawSprites.Add(tra, s);
                    rawTex.Add(tra, texture2D);

                    loaded++;
                    if (loaded == count)
                        Debug.LogWarning((DateTime.Now - now).TotalMilliseconds);
                }
            });
        }
    }
  
    /// <summary>
    /// 销毁Sprites
    /// </summary>
    public void ClearSprites()
    {
        Destroy(Canvas);
        Resources.UnloadUnusedAssets();
    }

    /// <summary>
    /// 销毁Sprites,并清空缓存
    /// </summary>
    public void ClearSpritesCache()
    {
        foreach (Texture2D tex in rawTex.Values)
            Destroy(tex);
        //创建的Sprite在Scene Memory中，需要Destroy才会释放内存， Resouces.UnloadUnusedAsset无用
        foreach (Sprite sprite in rawSprites.Values)
            Destroy(sprite);
        List<Transform> trans = rawSprites.Keys.ToList();
        for (int i = 0; i < trans.Count; i++)
        {
            Destroy(trans[i].gameObject);
        }
        trans.Clear();
        rawTex.Clear();
        rawSprites.Clear();
    }
    #endregion
}