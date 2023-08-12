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
        Socket.P5_Websocket socket;

        public Env env;

        #region byteQueue

        // 웹소켓에서 입력받은 데이터를 저장하는 큐
        public Queue<byte[]> byteQueue = new Queue<byte[]>();
        private object lockObject = new object();

        /// <summary>
        /// byteQueue의 데이터 수가 0인가?
        /// </summary>
        /// <returns>bool</returns>
        public bool IsQueueIsEmpty()
        {
            return byteQueue.Count == 0;
        }

        /// <summary>
        /// byteQueue에 byte[] 배열 투입
        /// </summary>
        /// <param name="item"></param>
        public void Enqueue(byte[] item)
        {
            lock (lockObject)
            {
                byteQueue.Enqueue(item);
            }
        }

        /// <summary>
        /// byteQueue에 마지막 데이터를 가져오기
        /// </summary>
        /// <returns>byte[] 배열</returns>
        public byte[] Dequeue_LastOne()
        {
            lock (lockObject)
            {
                byte[] item = null;
                while (byteQueue.Count > 0)
                {
                    item = byteQueue.Dequeue();
                }
                return item;
            }
        }

        #endregion byteQueue

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
