using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TTP_Demo : MonoBehaviour
{
    public MeshRenderer FromRenderer;
    public MeshRenderer ToRenderer;
    public TexToPng texToPng;

    // Update is called once per frame
    void Update()
    {
        // 보내는 쪽의 코드
        // 주의사항: 텍스처 보낼때 압축 옵션 None으로 해야 한다.
        Texture2D fromTex = FromRenderer.material.GetTexture("_MainTex") as Texture2D;
        Texture2D nonCompressedTex = texToPng.CopyTexture(fromTex);
        byte[] fromPng = texToPng.GetPng(nonCompressedTex);

        // -----

        // 받는 쪽의 코드
        Texture2D toTex = texToPng.GetTexture2D(fromPng, texToPng.width, texToPng.height);

        ToRenderer.material.SetTexture("_MainTex", toTex);
    }
}
