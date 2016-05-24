using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Timers;

namespace Symbioz.World.Models.Exchanges.Craft.Replay
{
    public abstract class ReplayEngine
    {
        public const double ReplayInterval = 1000;

        Timer ReplayTimer { get; set; }

        public int ActualReplayCountLeft { get; set; }

        public virtual void EndCraft()
        {

        }

        public ReplayEngine()
        {
            ReplayTimer = new Timer(ReplayInterval);
            ReplayTimer.Elapsed += ReplayTimer_Elapsed;
  
        }
        public virtual void Start()
        {
            ReplayTimer.Start();
        }
        public virtual void Stop()
        {
            ReplayTimer.Stop();
            ActualReplayCountLeft = 0;
            EndCraft();
        }
        public virtual void ReplayTimer_Elapsed(object sender, ElapsedEventArgs e)
        {
            throw new NotImplementedException();
        }
    }
}
