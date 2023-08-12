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
        /// ���� �Ҵ�� ���� �ν��Ͻ�
        /// </summary>
        Socket.P5_Websocket socket;

        public Env env;

        #region byteQueue

        // �����Ͽ��� �Է¹��� �����͸� �����ϴ� ť
        public Queue<byte[]> byteQueue = new Queue<byte[]>();
        private object lockObject = new object();

        /// <summary>
        /// byteQueue�� ������ ���� 0�ΰ�?
        /// </summary>
        /// <returns>bool</returns>
        public bool IsQueueIsEmpty()
        {
            return byteQueue.Count == 0;
        }

        /// <summary>
        /// byteQueue�� byte[] �迭 ����
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
        /// byteQueue�� ������ �����͸� ��������
        /// </summary>
        /// <returns>byte[] �迭</returns>
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
