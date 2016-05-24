using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;

namespace Symbioz.SSync
{
    public class SSyncServer
    {
        public delegate void OnSocketAcceptedDel(Socket socket);
        public delegate void OnServerFailedToStartDel(Exception ex);
        public event OnSocketAcceptedDel OnSocketAccepted;
        public event OnServerFailedToStartDel OnServerFailedToStart;
        public event Action OnServerStarted;
        private Socket SServer { get; set; }
        public IPEndPoint EndPoint { get; set; }
        public SSyncServer(string ip,int port)
        {
            EndPoint = new IPEndPoint(IPAddress.Parse(ip), port);
            SServer = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
        }
        void OnListenSucces()
        {
            if (OnServerStarted != null)
                OnServerStarted();
        }
        public void Start()
        {
            try
            {
                SServer.Bind(EndPoint);
            }
            catch (Exception ex)
            {
                OnListenFailed(ex);
                return;
            }
            SServer.Listen(100); 
            StartAccept(null);
            OnListenSucces();
        }
        void OnListenFailed(Exception ex)
        {
            if (OnServerFailedToStart != null)
                OnServerFailedToStart(ex);
        }
        protected void StartAccept(SocketAsyncEventArgs args)
        {
            if (args == null)
            {
                args = new SocketAsyncEventArgs();
                args.Completed += AcceptEventCompleted;
            }
            else
            {
                args.AcceptSocket = null;
            }

            bool willRaiseEvent = SServer.AcceptAsync(args);
            if (!willRaiseEvent)
            {
                ProcessAccept(args);
            }
        }
        private void AcceptEventCompleted(object sender, SocketAsyncEventArgs e)
        {
            ProcessAccept(e);
        }
        public void Stop()
        {
            SServer.Shutdown(SocketShutdown.Both);
        }
        void ProcessAccept(SocketAsyncEventArgs args)
        {
            if (OnSocketAccepted != null)
                OnSocketAccepted(args.AcceptSocket);
            StartAccept(args); 
        }
    }
}
