using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Socket
{
    [System.Serializable]
    public struct Env
    {
        public string serverURL;
    }

    public class SocketManager : MonoBehaviour
    {
        #region Instance
        private static SocketManager instance;

        public static SocketManager Instance 
        { 
            get 
            { 
                if (instance == null)
                {
                    instance = GameObject.FindObjectOfType<SocketManager>();
                }
                return instance; 
            } 
        }
        #endregion

        /// <summary>
        /// 내부 할당용 소켓 인스턴스
        /// </summary>
        public Socket.P5_Websocket socket;

        public Env env;

        void Init()
        {
            Debug.Log("Socket Manager Initialized");

            if (socket == null)
            {
                GameObject obj = new GameObject("P5_Websocket");
                socket = obj.AddComponent<Socket.P5_Websocket>();
                socket.transform.parent = gameObject.transform;
            }

            socket.Init(env.serverURL);
        }
        
        // Start is called before the first frame update
        void Start()
        {
            SocketManager.Instance.Init();
        }

        public void Send_Message(string data)
        {
            socket.Send_Message(data);
        }
    }
}
