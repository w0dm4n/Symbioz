using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Symbioz.SSync
{
    public class SSyncClient : IDisposable
    {
        public event OnFailToConnectDel OnFailedToConnect;
        public event Action OnClosed;
        public event Action OnConnected;
        public event OnDataArrivalDel OnDataArrival;
        public delegate void OnFailToConnectDel(Exception ex);
        public delegate void OnDataArrivalDel(byte[] datas);
        public IPEndPoint EndPoint { get { return Sock.RemoteEndPoint as IPEndPoint; } }
        public string Ip
        {
            get
            {
                return EndPoint.Address + ":" + EndPoint.Port;

            }
        }
        public bool DataArrivals(BufferSegment data)
        {
            if (OnDataArrival != null)
                OnDataArrival(data.SegmentData);
            return true;
        }
        public int sizeBuffer = 8192;
        private object m_lock = new object();

        public Socket Sock
        {
            get;
            private set;
        }


        private uint _bytesReceived;

        private static readonly BufferManager Buffers = BufferManager.Default;

        private uint _bytesSent;

        private static long _totalBytesReceived;

        private static long _totalBytesSent;

        public static long TotalBytesSent
        {
            get { return _totalBytesSent; }
        }

        public static long TotalBytesReceived
        {
            get { return _totalBytesReceived; }
        }

        protected BufferSegment _bufferSegment;

        protected int _offset, _remainingLength;

        public SSyncClient()
        {
            Sock = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
            _bufferSegment = Buffers.CheckOut();
        }
        public SSyncClient(Socket _sock)
        {
            this.Sock = _sock;
            _bufferSegment = Buffers.CheckOut();
            Receive();

        }

        public void Receive()
        {

            if (Sock != null || Sock.Connected)
            {

                var args = SocketHelpers.AcquireSocketArg();
                var offset = 0; //_offset + _remainingLength;

                args.SetBuffer(_bufferSegment.Buffer.Array, _bufferSegment.Offset + offset, sizeBuffer - offset);

                args.UserToken = this;
                args.Completed += ReceiveAsyncComplete;

                var willRaiseEvent = Sock.ReceiveAsync(args);

                if (!willRaiseEvent)
                {
                    ProcessRecieve(args);
                }
            }
        }

        private void ProcessRecieve(SocketAsyncEventArgs args)
        {

            try
            {
                var bytesReceived = args.BytesTransferred;

                if (args.BytesTransferred == 0)
                {
                     
                    OnSocketClosed();
                }
                else
                {
                    unchecked
                    {
                        _bytesReceived += (uint)bytesReceived;
                    }

                    Interlocked.Add(ref _totalBytesReceived, bytesReceived);

                    if (this.DataArrivals(_bufferSegment))
                    {
                        _offset = 0;
                        _bufferSegment.DecrementUsage();
                        _bufferSegment = Buffers.CheckOut();
                    }
                    else
                    {
                        EnsureBuffer();
                    }

                    this.Receive();
                }
            }
            catch (ObjectDisposedException)
            {

                OnSocketClosed();
            }
            catch (Exception)
            {

                OnSocketClosed();
            }
            finally
            {
                args.Completed -= ReceiveAsyncComplete;
                SocketHelpers.ReleaseSocketArg(args);
            }
        }

        void OnSocketClosed()
        {
            if (OnClosed != null)
                OnClosed();
        }
        private void ReceiveAsyncComplete(object sender, SocketAsyncEventArgs args)
        {
            ProcessRecieve(args);
        }

        protected void EnsureBuffer() //(int size)
        {
            //if (size > BufferSize - _offset)
            {
                // not enough space left in buffer: Copy to new buffer
                var newSegment = Buffers.CheckOut();
                Array.Copy(_bufferSegment.Buffer.Array,
                    _bufferSegment.Offset + _offset,
                    newSegment.Buffer.Array,
                    newSegment.Offset,
                    _remainingLength);
                _bufferSegment.DecrementUsage();
                _bufferSegment = newSegment;
                _offset = 0;
            }
        }
        public void Send(byte[] datas)
        {

            if (Sock != null && Sock.Connected)
            {
                var args = SocketHelpers.AcquireSocketArg();
                if (args != null)
                {
                    args.Completed += SendAsyncComplete;
                    args.UserToken = this;
                    args.SetBuffer(datas, 0, datas
                        .Length);
                    Sock.SendAsync(args);

                    //  Logger.WriteMsg(string.Format("[Send] {0}", message.ToString()), ConsoleColor.DarkGray);
                    unchecked
                    {
                        _bytesSent += (uint)datas.Length;
                    }

                    Interlocked.Add(ref _totalBytesSent, datas.Length);
                }
                else
                {
                }
            }

        }
        

        private void SendAsyncComplete(object sender, SocketAsyncEventArgs args)
        {
            args.Completed -= SendAsyncComplete;
            SocketHelpers.ReleaseSocketArg(args);
        }

        public void Connect(string host, int port)
        {
            Connect(IPAddress.Parse(host), port);
        }
        void OnSocketConnected()
        {
            Receive();
            if (OnConnected != null)
                OnConnected();

        }
        void OnSocketFailedToConnect(Exception ex)
        {
            if (OnFailedToConnect != null)
                OnFailedToConnect(ex);
        }
        public void Connect(IPAddress addr, int port)
        {
            if (Sock != null)
            {
                if (Sock.Connected)
                {
                    Sock.Disconnect(true);
                }
                try
                {
                    Sock.Connect(addr, port);
                 
                }
                catch (Exception ex)
                {
                    OnSocketFailedToConnect(ex);
                }

                Receive();
                OnSocketConnected();
            }
        }

        ~SSyncClient()
        {
            Dispose(false);
        }

        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        public void Dispose(bool disposing)
        {
            if (Sock != null && Sock.Connected)
            {
                try
                {
                    Sock.Shutdown(SocketShutdown.Both);
                    Sock.Close();
                    Sock = null;
                }
                catch
                {

                }
            }
        }
    }
}

