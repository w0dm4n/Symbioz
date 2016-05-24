using Symbioz.DofusProtocol.Messages;
using Symbioz.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Timers;

namespace Symbioz.World.Models.Exchanges.Craft.Replay
{
    public class SmithMagicReplayEngine : ReplayEngine
    {
        SmithMagicExchange Instance { get; set; }

        public SmithMagicReplayEngine(SmithMagicExchange instance)
        {
            this.Instance = instance;

            ActualReplayCountLeft = instance.ReplayCount;
        }
        public override void EndCraft()
        {

            Instance.ReplayCount = 1;
            Instance.Client.Send(new ExchangeReplayCountModifiedMessage(Instance.ReplayCount));


        }
        public override void ReplayTimer_Elapsed(object sender, ElapsedEventArgs e)
        {
            if (Instance.IsNull())
            {
                return;
            }
            return; // TODO
            Instance.UsedItem.RemoveAllEffect(EffectsEnum.Eff_AddSummonLimit);
            Instance.UsedItem.AddEffects(Instance.UsedRune.GetEffects());

            Instance.Client.Send(new ExchangeCraftResultMagicWithObjectDescMessage((sbyte)CraftResultEnum.CRAFT_SUCCESS,
               new DofusProtocol.Types.ObjectItemNotInContainer(700, Instance.UsedItem.GetEffects(), Instance.UsedItem.UID, 1), 2));
       

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
        }
    }
}
