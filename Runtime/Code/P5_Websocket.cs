using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Socket
{
    using Socket;
    using Newtonsoft.Json;
    using System;
    using System.Threading.Tasks;
    using WebSocketSharp;
    using ErrorEventArgs = WebSocketSharp.ErrorEventArgs;

    public class P5_Websocket : MonoBehaviour
    {
        private WebSocket _webSocket;
        public string _serverUrl = "ws://localhost:8074"; // replace with your WebSocket URL

        public bool _isScene1;
        private int attemptCount = 0;


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

        public async void Init(string serverURL, bool isScene1)
        {
            _isScene1 = isScene1;
            _serverUrl = serverURL;

            attemptCount++;
            await Task.Run(() => ConnectToWebSocket());
        }

        public async void Init()
        {
            attemptCount++;
            await Task.Run(() => ConnectToWebSocket());
        }

        private void ConnectToWebSocket()
        {
            Debug.Log($"Attempt to Connect {attemptCount}");

            if (_webSocket != null)
            {
                _webSocket.OnOpen -= OnOpen;
                _webSocket.OnMessage -= OnMessage;
                _webSocket.OnClose -= OnClose;
                _webSocket.OnError -= OnError;
                _webSocket.Close();
            }

            _webSocket = new WebSocket(_serverUrl);
            _webSocket.OnOpen += OnOpen;
            _webSocket.OnMessage += OnMessage;
            _webSocket.OnClose += OnClose;
            _webSocket.OnError += OnError;
            _webSocket.Connect();
        }

        private void OnOpen(object sender, EventArgs e)
        {
            attemptCount = 0;
            Debug.Log("WebSocket opened.");
        }

        private void OnMessage(object sender, MessageEventArgs e)
        {
            //Debug.Log("WebSocket on message.");

            try
            {
                if (_isScene1)
                {
                    string str = System.Text.Encoding.UTF8.GetString(e.RawData);

                    string data = JsonConvert.DeserializeObject<string>(str);

                    Enqueue(Convert.FromBase64String(data));
                }
                else
                {
                    string str = System.Text.Encoding.UTF8.GetString(e.RawData);

                    // string data = JsonConvert.DeserializeObject<string>(str);

                    Enqueue(Convert.FromBase64String(str));
                }
            }
            catch (Exception ex)
            {
                Debug.LogError(ex);
            }
        }

        private void OnClose(object sender, CloseEventArgs e)
        {
            Debug.Log("WebSocket closed.");

            Init();
        }

        private void OnError(object sender, ErrorEventArgs e)
        {
            Debug.LogError("WebSocket error: " + e.Message);
            _webSocket.Close();

            Init();
        }

        public void Send_Message(string message)
        {
            _webSocket.Send(message);
        }
    }
}
