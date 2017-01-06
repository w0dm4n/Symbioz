using Symbioz.DofusProtocol.Messages;
using Symbioz.Network.Clients;
using Symbioz.Network.Messages;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Symbioz.World.Handlers
{
    class FightHandler
    {

        public static void EndTurn(System.Timers.Timer Timer, WorldClient client)
        {
            if (client.Character.FighterInstance != null)
                client.Character.FighterInstance.EndTurn();
            Timer.Enabled = false;
            Timer.Stop();
            Timer.Dispose();
        }

        [MessageHandler]
        public static void HandleFightTurnFinished(GameFightTurnFinishMessage message, WorldClient client)
        {
            var Timer = new System.Timers.Timer();
            Timer.Interval = 350;
            Timer.Elapsed += (sender, e) => { EndTurn(Timer, client); };
            Timer.Enabled = true;
        }

        [MessageHandler]
        public static void HandleTurnReady(GameFightTurnReadyMessage message, WorldClient client)
        {
            if (client.Character.FighterInstance != null)
            {
                client.Character.FighterInstance.Fight.Synchronizer.ToggleReady(client.Character.FighterInstance);
            }
           
        }
        [MessageHandler]
        public static void HandleAcknowledgement(GameActionAcknowledgementMessage message, WorldClient client)
        {
            if (message.valid && client.Character.IsFighting && client.Character.FighterInstance.IsPlaying) 
            {
                client.Character.FighterInstance.Fight.Acknowledge();
            }          
        }

        public static void CastSpellCell(System.Timers.Timer Timer, WorldClient client, GameActionFightCastRequestMessage message)
        {
            if (client.Character.FighterInstance != null)
                client.Character.FighterInstance.CastSpellOnCell(message.spellId, message.cellId, 0, false);
            Timer.Enabled = false;
            Timer.Stop();
            Timer.Dispose();
        }

        public static void CastSpellTarget(System.Timers.Timer Timer, WorldClient client, GameActionFightCastOnTargetRequestMessage message)
        {
            if (client.Character.FighterInstance != null)
                client.Character.FighterInstance.CastSpellOnTarget(message.spellId, message.targetId, true);
            Timer.Enabled = false;
            Timer.Stop();
            Timer.Dispose();
        }

        [MessageHandler]
        public static void HandleSpellCast(GameActionFightCastRequestMessage message, WorldClient client)
        {
            var Timer = new System.Timers.Timer();
            Timer.Interval = 250;
            Timer.Elapsed += (sender, e) => { CastSpellCell(Timer, client, message); };
            Timer.Enabled = true;        
        }
        [MessageHandler]
        public static void HandleSpellCastOnTarget(GameActionFightCastOnTargetRequestMessage message, WorldClient client)
        {
            var Timer = new System.Timers.Timer();
            Timer.Interval = 250;
            Timer.Elapsed += (sender, e) => { CastSpellTarget(Timer, client, message); };
            Timer.Enabled = true;
        }
        [MessageHandler]
        public static void HandleShowCell(ShowCellRequestMessage message,WorldClient client)
        {
            if (client.Character.FighterInstance != null)
            client.Character.FighterInstance.Fight.Send(new ShowCellMessage(client.Character.Id, message.cellId));
        }
    }
}
