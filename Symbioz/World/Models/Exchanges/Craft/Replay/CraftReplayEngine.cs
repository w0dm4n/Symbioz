using Symbioz.DofusProtocol.Messages;
using Symbioz.World.Records;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Timers;

namespace Symbioz.World.Models.Exchanges.Craft.Replay
{
    public class CraftReplayEngine : ReplayEngine, IDisposable
    {


        CraftExchange Instance { get; set; }

        RecipeRecord Recipe { get; set; }

        public CraftReplayEngine(CraftExchange craftinstance, RecipeRecord recipe)
        {
            Instance = craftinstance;
            Recipe = recipe;
            ActualReplayCountLeft = craftinstance.ReplayCount;
        }
        public override void EndCraft()
        {
            Instance.CraftedItems.ForEach(x => Instance.Client.Send(new ExchangeObjectRemovedMessage(false, x.UID)));
            Instance.CraftedItems.Clear();
            Instance.ReplayCount = 1;
            Instance.Client.Send(new ExchangeReplayCountModifiedMessage(Instance.ReplayCount));
        }
        public override void ReplayTimer_Elapsed(object sender, ElapsedEventArgs e)
        {
            if (Instance.IsNull())
            {
                return;
            }
            try
            {
                for (int i = 0; i < Instance.CraftedItems.Count(); i++)
                {
                    var item = Instance.CraftedItems[i];
                    Instance.Client.Character.Inventory.RemoveItem(item.UID, item.Quantity);
                }
                Instance.PerformCraft(Recipe);
                ActualReplayCountLeft--;
                if (ActualReplayCountLeft == 0)
                {
                    EndCraft();
                    Dispose();
                    return;
                }
            }
            catch
            {
                Logger.Info("Try to perform craft while exiting craft exchange, aborting...");
            }

        }
        public override void Stop()
        {
            base.Stop();
            Dispose();

        }

        public void Dispose()
        {
            Instance.ReplayEngine = null;
            Instance = null;
            Recipe = null;
        }
    }
}
