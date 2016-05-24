using Symbioz.DofusProtocol.Messages;
using Symbioz.DofusProtocol.Types;
using Symbioz.Enums;
using Symbioz.Network.Clients;
using Symbioz.Network.Messages;
using Symbioz.Network.Servers;
using Symbioz.Providers;
using Symbioz.World.Models.Fights;
using Symbioz.World.Models.Fights.Fighters;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Symbioz.World.Handlers
{
    class FightPreparationHandler
    {
        [MessageHandler]
        public static void HandleChallengeAnswer(GameRolePlayPlayerFightFriendlyAnswerMessage message, WorldClient client)
        {
            if (client.Character.IsFighting)
                return;
            FightDual fight = FightProvider.Instance.GetFight(message.fightId) as FightDual;
            var sMessage = new GameRolePlayPlayerFightFriendlyAnsweredMessage(message.fightId, (uint)client.Character.Id, (uint)fight.InitiatorId, false);

            if (client.Character.Id == fight.InitiatorId)
            {
                var acceptor = WorldServer.Instance.GetOnlineClient(fight.AcceptorId);
                acceptor.Send(sMessage);
                return;
            }
            WorldClient target = WorldServer.Instance.GetOnlineClient((fight.InitiatorId));
            if (message.accept)
            {

                sMessage.accept = true;
                target.Send(sMessage);
                fight.BlueTeam.AddFighter(client.Character.CreateFighter(fight.BlueTeam));
                fight.RedTeam.AddFighter(target.Character.CreateFighter(fight.RedTeam));
                fight.StartPlacement();
            }
            else
            {
                target.Send(sMessage);
                FightProvider.Instance.RemoveFight(message.fightId);
            }
        }
        [MessageHandler]
        public static void HandleChallengeRequest(GameRolePlayPlayerFightRequestMessage message, WorldClient client)
        {

            if (client.Character.Map != null && client.Character.Map.HaveZaap)
            {
                client.Character.Reply("Action impossible sur cette carte.");
                return;
            }
            WorldClient target = WorldServer.Instance.GetOnlineClient((int)message.targetId);
            if (target.Character.Busy)
            {
                client.Character.Reply("Impossible car le joueur est occupé.");
                return;
            }
            if (message.friendly)
            {

                FightDual fight = FightProvider.Instance.CreateDualFight(client.Character.Map, client.Character.Record.CellId, message.targetCellId);
                fight.InitiatorId = client.Character.Id;
                fight.AcceptorId = target.Character.Id;
                Message sMessage = new GameRolePlayPlayerFightFriendlyRequestedMessage(fight.Id, (uint)client.Character.Id, message.targetId);
                client.Send(sMessage);
                target.Send(sMessage);
            }
        }
        [MessageHandler]
        public static void HandleAttackRequest(GameRolePlayAttackMonsterRequestMessage message, WorldClient client)
        {
            if (client.Character.Map == null)
                return;
            if (client.Character.CancelMonsterAgression)
            {
                client.Character.CancelMonsterAgression = false;
                return;
            }
            if (!client.Character.Map.IsValid())
            {
                client.Character.Reply("Unable to start placement,this map is not valid");
                return;
            }
            var group = client.Character.Map.Instance.GetMapMonsterGroup(message.monsterGroupId);
            if (group != null)
            {
                FightPvM fight = FightProvider.Instance.CreatePvMFight(group, client.Character.Map, (short)group.CellId);

                fight.BlueTeam.AddFighter(client.Character.CreateFighter(fight.BlueTeam)); // on ajoute le perso

                group.Monsters.ForEach(x => fight.RedTeam.AddFighter(x.CreateFighter(fight.RedTeam))); // on ajoute les monstres

                fight.StartPlacement();
            }
            else
            {
                client.Character.NotificationError("Unable to fight, MonsterGroup dosent exist...");
            }
        }
        [MessageHandler]
        public static void HandleSwapPositions(GameFightPlacementSwapPositionsRequestMessage message, WorldClient client)
        {
            var requested = client.Character.FighterInstance.Fight.GetFighter(message.requestedId);
            if (requested != null)
            {
                requested.SwapPosition(client.Character.FighterInstance);
            }
        }
        [MessageHandler]
        public static void HandlePositionChange(GameFightPlacementPositionRequestMessage message, WorldClient client)
        {
            if (!client.Character.FighterInstance.ReadyToFight)
            {
                client.Character.FighterInstance.CellId = (short)message.cellId;
                client.Character.FighterInstance.RefreshStats();
                client.Character.FighterInstance.Fight.Send(new GameEntityDispositionMessage(new IdentifiedEntityDispositionInformations((short)message.cellId, (sbyte)3, client.Character.FighterInstance.ContextualId)));
            }
        }
        [MessageHandler]
        public static void HandleFightJoin(GameFightJoinRequestMessage message, WorldClient client)
        {
            var fight = FightProvider.Instance.GetFight(message.fightId);
            if (!fight.ContainCharacterFighter(client.Character.Id))
                fight.TryJoin(client, message.fighterId);
            else
                client.Character.NotificationError("Vous avez déja rejoint ce combat");
        }
        [MessageHandler]
        public static void HandleFightReady(GameFightReadyMessage message, WorldClient client)
        {
            client.Character.FighterInstance.ToogleReady(message.isReady);
        }
        [MessageHandler]
        public static void HandleGameContextQuit(GameContextQuitMessage message, WorldClient client)
        {
            if (client.Character.FighterInstance.Fight.FightType == FightTypeEnum.FIGHT_TYPE_PVP_ARENA && !client.Character.FighterInstance.Fight.Started)
            {
                client.Character.ReplyError("Vous ne pouvez pas quitter le combat actuellement.");
                return;
            }
            try
            {
           
                client.Character.FighterInstance.Leave();
            }
            catch
            {
                client.Character.NotificationError("Error while leaving fight...");
            }
        }
        [MessageHandler]
        public static void HandleFightOptions(GameFightOptionToggleMessage message, WorldClient client)
        {
            client.Character.FighterInstance.Fight.SetOption((FightOptionsEnum)message.option, client.Character.FighterInstance.Team);
        }
    }
}