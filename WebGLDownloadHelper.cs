using System.Collections;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using UnityEngine;

public class WebGLDownloadHelper
{
#if UNITY_WEBGL && !UNITY_EDITOR
    [DllImport("__Internal")]
    private static extern void downloadToFile(string content, string filename);
#endif


    /// <summary>
    /// 支持doc txt等纯文本格式
    /// </summary>
    /// <param name="content"></param>
    /// <param name="filename"></param>
    public static void DownloadToFile(string content, string filename)
    {
#if UNITY_WEBGL && !UNITY_EDITOR
        downloadToFile(content, filename);
#endif
    }

    [DllImport("__Internal")]
    private static extern void WebGLDownloadFile(byte[] array, int byteLength, string fileName);


    /// <summary>
    /// 图片下载
    /// </summary>
    /// <param name="texture2D"></param>
    public static void DownImage(Texture2D texture2D,string filePath,string fileName)
    {
        Texture2D texture = texture2D;
        byte[] textureBytes = DeCompress(texture2D).EncodeToJPG();
        string path = System.IO.Path.Combine(filePath, fileName);
        WebGLDownloadFile(textureBytes, textureBytes.Length, path);
    }


    /// <summary>
    /// 
    /// </summary>
    /// <param name="bytes">支持多种文件格式下载（doc docx xml xlsx txt等）</param>
    /// <param name="fileName">带文件格式的完整名称</param>
    public static void DownloadDocx(byte[] bytes, string fileName)
    {
        WebGLDownloadFile(bytes, bytes.Length, fileName);
    }

    static Texture2D DeCompress(Texture2D source)
    {
        RenderTexture renderTex = RenderTexture.GetTemporary(
                    source.width,
                    source.height,
                    0,
                    RenderTextureFormat.Default,
                    RenderTextureReadWrite.Linear);

        Graphics.Blit(source, renderTex);
        RenderTexture previous = RenderTexture.active;
        RenderTexture.active = renderTex;
        Texture2D readableText = new Texture2D(source.width, source.height);
        readableText.ReadPixels(new Rect(0, 0, renderTex.width, renderTex.height), 0, 0);
        readableText.Apply();
        RenderTexture.active = previous;
        RenderTexture.ReleaseTemporary(renderTex);
        return readableText;
    }
}
