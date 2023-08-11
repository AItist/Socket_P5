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
        // ������ ���� �ڵ�
        // ���ǻ���: �ؽ�ó ������ ���� �ɼ� None���� �ؾ� �Ѵ�.
        Texture2D fromTex = FromRenderer.material.GetTexture("_MainTex") as Texture2D;
        Texture2D nonCompressedTex = texToPng.CopyTexture(fromTex);
        byte[] fromPng = texToPng.GetPng(nonCompressedTex);

        // -----

        // �޴� ���� �ڵ�
        Texture2D toTex = texToPng.GetTexture2D(fromPng, texToPng.width, texToPng.height);

        ToRenderer.material.SetTexture("_MainTex", toTex);
    }
}
