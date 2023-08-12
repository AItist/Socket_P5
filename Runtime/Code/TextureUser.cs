using Socket;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;

namespace Socket
{
    public class TextureUser : MonoBehaviour
    {
        public bool debug = true;
        public string fileName = "MyTexture"; // 원하는 파일 이름
        private string savePath;

        public int tWidth = 512;
        public int tHeight = 512;
        public int tDepth = 3;
        public string setTexKey = "_MainTex";

        private object lockObject = new object();

        [Header("데모 씬에서 실행인가?")]
        public bool isDemoScene;
        public MeshRenderer targetRenderer;

        private Texture2D _tex;
        public Texture2D TEX
        {
            get
            {
                lock (lockObject)
                {
                    return _tex;
                }
            }
            set
            {
                lock (lockObject)
                {
                    _tex = value;
                }
            }
        }
        

        // Update is called once per frame
        void Update()
        {
            if (SocketManager.Instance.IsQueueIsEmpty()) { return; }

            byte[] bytes = SocketManager.Instance.Dequeue_LastOne();


            TEX = Update_CreateTexture2D(tWidth, tHeight, tDepth, bytes);

            if (debug)
            {
                savePath = Application.dataPath + "/" + fileName + ".png"; // 저장 경로 설정
                SaveTextureToPNG(TEX, savePath);
            }

            if (!isDemoScene) { return; }
            if (TEX == null) { return; }

            targetRenderer.material.SetTexture(setTexKey, TEX);
        }

        private Texture2D Update_CreateTexture2D(int tWidth, int tHeight, int tDepth, byte[] decompressedData)
        {
            if (decompressedData == null) { return null; }

            Texture2D recovered = new Texture2D(tWidth, tHeight);
            recovered.LoadImage(decompressedData);
            recovered.Apply();

            return recovered;
        }

        public void SaveTextureToPNG(Texture2D texture, string path)
        {
            byte[] textureBytes = texture.EncodeToPNG();
            File.WriteAllBytes(path, textureBytes);
            Debug.Log("Texture saved to: " + path);
        }
    }
}
