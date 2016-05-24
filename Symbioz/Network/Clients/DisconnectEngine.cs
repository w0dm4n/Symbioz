using Symbioz.Auth.Handlers;
using Symbioz.Helper;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Timers;

namespace Symbioz.Network.Clients
{
    public class DisconnectEngine
    {
        public static void New(DofusClient client, int timeout, string reason = null)
        {
            new DisconnectEngine(client, timeout, reason).Launch();
        }
        internal string m_reason { get; set; }
        internal DofusClient m_client { get; set; }
        internal Timer m_timer { get; set; }
        internal DisconnectEngine(DofusClient client, int timeout, string reason = null)
        {
            this.m_client = client;
            this.m_reason = reason;
            this.m_timer = new Timer(timeout);
            m_timer.Elapsed += m_timer_Elapsed;

        }
        public void Launch()
        {
            if (m_reason != null)
                ConnectionHandler.SendSystemMessage(m_client, m_reason);
            m_timer.Start();
        }
        void m_timer_Elapsed(object sender, ElapsedEventArgs e)
        {
            m_client.Disconnect();
        }

    }

}
