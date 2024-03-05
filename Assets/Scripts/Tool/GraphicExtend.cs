using UnityEngine;

public static class GraphicExtend
{
    /// <summary>
    /// 设置透明度
    /// </summary>
    /// <param name="component"></param>
    /// <param name="alpha"></param>
    public static void SetAlpha(this UnityEngine.UI.Graphic component, float alpha)
    {
        Color temp = component.color;
        component.color = new Color(temp.r, temp.g, temp.b, alpha);
    }
}