using Symbioz.Network.Messages;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Symbioz.DofusProtocol.Messages;
using Symbioz.Enums;
using Symbioz.Network.Clients;
using Symbioz.Providers;

namespace Symbioz.World.Handlers
{
    public class ArenaHandler
    {
        [MessageHandler]
        public static void HandleArenaRegister(GameRolePlayArenaRegisterMessage message, WorldClient client)
        {
            if (client.Character.Map == null)
            {
                client.Character.ReplyError("Impossible de s'inscrire au kolizeum actuellement.");
                return;
            }
            if (client.Character.Map.DugeonMap)
            {
                client.Character.ReplyError("Impossible de s'inscrire au kolizeum en donjon.");
                return;
            }
            if (client.Character.IsFighting)
            {
                client.Character.ReplyError("Impossible de s'inscrire au kolizeum en combat");
                return;
            }
            if (client.Character.Record.Level < ArenaProvider.MINIMUM_LEVEL_TO_SEARCH_ARENA)
            {
                client.Character.ReplyError("Vous devez être au moins niveau " + ArenaProvider.MINIMUM_LEVEL_TO_SEARCH_ARENA + " pour vous inscrire en kolizeum.");
                return;
            }
            ArenaProvider.Instance.SearchArena(client);
           
        }
        [MessageHandler]
        public static void HandleArenaUnregister(GameRolePlayArenaUnregisterMessage message, WorldClient client)
        {
            ArenaProvider.Instance.Unregister(client);
            client.Send(new GameRolePlayArenaRegistrationStatusMessage(false, (sbyte)PvpArenaStepEnum.ARENA_STEP_UNREGISTER, 0));
        }
        [MessageHandler]
        public static void HandleArenaAnswer(GameRolePlayArenaFightAnswerMessage message, WorldClient client)
        {
            if (client.Character.IsFighting)
            {
                client.Character.ReplyError("Impossible de répondre au kolizeum en combat, le combat a été annulé.");
                message.accept = false;
            }
            if (client.Character.Map != null && client.Character.Map.DugeonMap)
            {
                client.Character.ReplyError("Impossible de répondre au kolizeum en donjon.");
                message.accept = false;
            }
            ArenaProvider.Instance.Answer(client, message.accept);
        }
        public static void SendArenaUpdatePlayerInfos(WorldClient client, ushort rank, ushort bestDailyRank, ushort bestRank, ushort victoryCount, ushort arenaFightcount)
        {
            client.Send(new GameRolePlayArenaUpdatePlayerInfosMessage(rank, bestDailyRank, bestRank, victoryCount, arenaFightcount));
        }
    }
}
