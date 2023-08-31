using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Socket
{
    using Socket;
    using Newtonsoft.Json;
    using System;
    using WebSocketSharp;
    using ErrorEventArgs = WebSocketSharp.ErrorEventArgs;

    public class P5_Websocket : MonoBehaviour
    {
        private WebSocket _webSocket;
        public string _serverUrl = "ws://localhost:8082"; // replace with your WebSocket URL

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

        public void Init(string serverURL)
        {
            _serverUrl = serverURL;

            _webSocket = new WebSocket(_serverUrl);
            _webSocket.OnOpen += OnOpen;
            _webSocket.OnMessage += OnMessage;
            _webSocket.OnClose += OnClose;
            _webSocket.OnError += OnError;
            _webSocket.Connect();
        }

        private void OnOpen(object sender, EventArgs e)
        {
            Debug.Log("WebSocket opened.");
        }

        private void OnMessage(object sender, MessageEventArgs e)
        {
            //Debug.Log("WebSocket on message.");

            try
            {
                string str = System.Text.Encoding.UTF8.GetString(e.RawData);

                // string data = JsonConvert.DeserializeObject<string>(str);

                Enqueue(Convert.FromBase64String(str));
            }
            catch (Exception ex)
            {
                Debug.LogError(ex);
            }
        }

        private void OnClose(object sender, CloseEventArgs e)
        {
            Debug.Log("WebSocket closed.");
        }

        private void OnError(object sender, ErrorEventArgs e)
        {
            Debug.LogError("WebSocket error: " + e.Message);
        }

        public void Send_Message(string message)
        {
            _webSocket.Send(message);
        }
    }
}
