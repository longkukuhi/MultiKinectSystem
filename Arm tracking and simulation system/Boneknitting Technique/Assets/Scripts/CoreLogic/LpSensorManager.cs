using System;
using System.Text;
using System.Collections.Generic;
using System.Net;
using System.Net.Sockets;
using System.Diagnostics;
using UnityEngine;
#if UNITY_EDITOR
using UnityEditor;
#endif

class LpSensorManager
{
    private int port = 23333;
    private string IpStr = "127.0.0.1";
    private Socket serverSocket;
    private IPAddress ip;
    private IPEndPoint ip_end_point;
    private string blue_addr = "00:04:3E:9E:00:C5";
    private byte[] byteArray_Receive = new byte[40];
    private BodyCtrlData ref_body_data;
    private WristCtrlData ref_wrist_data;
    private Process client;
    private bool bIsKilled;

    public LpSensorManager(ref BodyCtrlData body_data, ref WristCtrlData wrist_data, int p, string b_a, int deta_time)
    {
        port = p;
        blue_addr = b_a;
        IpStr = "127.0.0.1";
        ip = IPAddress.Parse(IpStr);
        ip_end_point = new IPEndPoint(ip, port);
        ref_body_data = body_data;
        ref_wrist_data = wrist_data;
        client = new Process();
        client.StartInfo.FileName = string.Format(Application.streamingAssetsPath+@"\LpSensor\LpSensorTest.exe");
        client.StartInfo.UseShellExecute = false;
        client.StartInfo.CreateNoWindow = true;
        client.StartInfo.WindowStyle = ProcessWindowStyle.Hidden;
        client.StartInfo.Arguments = string.Format("{0} {1} {2}", port, blue_addr, deta_time);
        client.EnableRaisingEvents = true;

        bIsKilled = false;
    }
    public bool Init()
    {
        client.Start();
        IPAddress ip = IPAddress.Parse(IpStr);
        IPEndPoint ip_end_point = new IPEndPoint(ip, port);
        //创建服务器Socket对象，并设置相关属性  
        serverSocket = new Socket(AddressFamily.InterNetwork, SocketType.Dgram, ProtocolType.Udp);
        //绑定ip和端口  
        serverSocket.Bind(ip_end_point);
        //设置最长的连接请求队列长度  
        UnityEngine.Debug.Log("启动监听成功");

#if UNITY_EDITOR
        EditorApplication.playModeStateChanged += KillWhenExit;
#endif
        return true;
    }

#if UNITY_EDITOR
    private void KillWhenExit(PlayModeStateChange obj)
    {
        switch (obj)
        {
            case PlayModeStateChange.ExitingPlayMode:
                DisconnectDevice();
                break;
        }
    }
#endif

    public void receiveData()
    {
        //接受数据
        EndPoint ep = (EndPoint)ip_end_point;
        while (true)
        {
            //接收到数据  
            int intReceiveLength = serverSocket.ReceiveFrom(byteArray_Receive, ref ep);
            //转换数据为字符串  
            ref_body_data.jointRotation[12].w = BitConverter.ToSingle(byteArray_Receive, 0);
            ref_body_data.jointRotation[12].x = BitConverter.ToSingle(byteArray_Receive, 4);
            ref_body_data.jointRotation[12].y = BitConverter.ToSingle(byteArray_Receive, 8);
            ref_body_data.jointRotation[12].z = BitConverter.ToSingle(byteArray_Receive, 12);
            //UnityEngine.Debug.Log(BitConverter.ToSingle(byteArray_Receive, 16) + ":" + BitConverter.ToSingle(byteArray_Receive, 20) + ":" + BitConverter.ToSingle(byteArray_Receive, 24));
            //ref_body_data.jointRotation[12] = Quaternion.Euler(-BitConverter.ToSingle(byteArray_Receive, 16), -BitConverter.ToSingle(byteArray_Receive, 20), BitConverter.ToSingle(byteArray_Receive, 24));
        }
    }
    public void DisconnectDevice()
    {
        if (!bIsKilled)
        {
            serverSocket.Close();
            client.Kill();
            bIsKilled = true;
        }
    }
}

