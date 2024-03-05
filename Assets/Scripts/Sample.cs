using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;

public class Sample : MonoBehaviour
{
    /// <summary>
    /// ·��
    /// </summary>
    public string path;
    /// <summary>
    /// �ظ����ش���
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
    /// ����ͼƬ��ͨ��RawImage��ʾ
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
    /// ����ͼƬ�����棬ͨ��RawImage��ʾ
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
    /// �첽����ͼƬ��ͨ��RawImage��ʾ
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
    /// �첽����ͼƬ��ͨ��RawImage+AspectRatioFitter��ʾ
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
    /// ����RawImages
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
    /// ����RawImages,����ջ���
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
    /// ����ͼƬ��ͨ��Sprite��ʾ
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
    /// ����ͼƬ�����棬ͨ��Sprite��ʾ
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
    /// �첽����ͼƬ��ͨ��Sprite��ʾ
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
    /// ����Sprites
    /// </summary>
    public void ClearSprites()
    {
        Destroy(Canvas);
        Resources.UnloadUnusedAssets();
    }

    /// <summary>
    /// ����Sprites,����ջ���
    /// </summary>
    public void ClearSpritesCache()
    {
        foreach (Texture2D tex in rawTex.Values)
            Destroy(tex);
        //������Sprite��Scene Memory�У���ҪDestroy�Ż��ͷ��ڴ棬 Resouces.UnloadUnusedAsset����
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