using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TexToPng : MonoBehaviour
{
    public Texture2D sourceTexture2D;
    public int width;
    public int height;

    public Texture2D CopyTexture(Texture2D source)
    {
        Texture2D dest = new Texture2D(source.width, source.height, TextureFormat.RGB24, false);
        dest.SetPixels(source.GetPixels());
        dest.Apply();
        return dest;
    }

    public byte[] GetPng(Texture2D sourceTexture2D)
    {
        return sourceTexture2D.EncodeToPNG();
    }

    public Texture2D GetTexture2D(byte[] pngBytes, int width, int height)
    {
        if (pngBytes == null)
        {
            //Debug.LogWarning("png 데이터가 존재하지 않습니다.");
            return null;
        }

        Texture2D tex = new Texture2D(width, height, TextureFormat.RGB24, false);
        tex.LoadImage(pngBytes);
        tex.Apply();

        return tex;
    }
}
