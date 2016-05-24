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
        [MessageHandler]
        public static void HandleFightTurnFinished(GameFightTurnFinishMessage message, WorldClient client)
        {
            if (client.Character.FighterInstance != null)
            client.Character.FighterInstance.EndTurn();
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
        [MessageHandler]
        public static void HandleSpellCast(GameActionFightCastRequestMessage message, WorldClient client)
        {
            client.Character.FighterInstance.CastSpellOnCell(message.spellId, message.cellId);
        }
        [MessageHandler]
        public static void HandleSpellCastOnTarget(GameActionFightCastOnTargetRequestMessage message, WorldClient client)
        {
            client.Character.FighterInstance.CastSpellOnTarget(message.spellId, message.targetId);
        }
        [MessageHandler]
        public static void HandleShowCell(ShowCellRequestMessage message,WorldClient client)
        {
            if (client.Character.FighterInstance != null)
            client.Character.FighterInstance.Fight.Send(new ShowCellMessage(client.Character.Id, message.cellId));
        }
    }
}
