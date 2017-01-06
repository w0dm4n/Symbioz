using Symbioz.DofusProtocol.Messages;
using Symbioz.DofusProtocol.Types;
using Symbioz.Enums;
using Symbioz.Network.Clients;
using Symbioz.Network.Messages;
using Symbioz.Network.Servers;
using Symbioz.Providers;
using Symbioz.World.Models.Fights;
using Symbioz.World.Models.Fights.Fighters;
using Symbioz.World.Models.Fights.FightsTypes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Timers;

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
                if (client.Character.CurrentlyInTrackRequest)
                {
                    client.Character.OnEndingUseDelayedObject(DelayedActionTypeEnum.DELAYED_ACTION_OBJECT_USE);
                    client.Character.CurrentlyInTrackRequest = false;
                }
                if (target.Character.CurrentlyInTrackRequest)
                {
                    target.Character.OnEndingUseDelayedObject(DelayedActionTypeEnum.DELAYED_ACTION_OBJECT_USE);
                    target.Character.CurrentlyInTrackRequest = false;
                }
                sMessage.accept = true;
                target.Send(sMessage);
                fight.BlueTeam.AddFighter(client.Character.CreateFighter(fight.BlueTeam, fight));
                fight.RedTeam.AddFighter(target.Character.CreateFighter(fight.RedTeam, fight));
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
            if (target.Character.IsIgnoring(client.Character.Record.AccountId))
            {
                client.Send(new TextInformationMessage(1, 370, new string[1] { target.Character.Record.Name }));
                return;
            }
            if (client.Character.Restrictions.isDead)
            {
                client.Character.Reply("Impossible car vous êtes mort.");
                return;
            }
            if (target.Character.Restrictions.isDead)
            {
                client.Character.Reply("Impossible car ce joueur est mort.");
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
            else
            {
                if (client.Character.Record.Level < 50)
                {
                    client.Character.Reply("Vous devez etre au minimum niveau 50 pour agresser un joueur");
                    return;
                }
                if (target.Character.Record.Level > 50)
                {
                    if (target.Character.Map.SubAreaId == 95 || target.Character.Map.SubAreaId == 96 || target.Character.Map.SubAreaId == 97 || target.Character.Map.SubAreaId == 98 
                        || target.Character.Map.SubAreaId == 99 || target.Character.Map.SubAreaId == 100 || target.Character.Map.SubAreaId == 92 || target.Character.Map.SubAreaId == 173
                        || target.Character.Map.SubAreaId == 335 || target.Character.Map.Id == 115609089)
                    {
                        client.Character.Reply("Impossible d'agresser sur cette zone !");
                        return;
                    }
                    if (target.Account.Role > Auth.Models.ServerRoleEnum.PLAYER)
                    {
                        client.Character.Reply("Impossible sur un membre de l'équipe d'Hestia");
                        return;
                    }
                    FightAgression fight = (FightAgression)FightProvider.Instance.CreateAgressionFight(client.Character.Map, client.Character.Record.CellId, target.Character.Record.CellId);
                    fight.BlueTeam.AddFighter(client.Character.CreateFighter(fight.BlueTeam, fight));
                    fight.RedTeam.AddFighter(target.Character.CreateFighter(fight.RedTeam, fight));
                    fight.PlayersIP.Add(client.SSyncClient.OnlyIp);
                    fight.PlayersIP.Add(target.SSyncClient.OnlyIp);
                    fight.StartPlacement();
                }
                else
                {
                    client.Character.Reply("Impossible d'agresser un personnage qui n'est pas au moin niveau 50 !");
                }
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
                if (client.Character.isDebugging)
                    client.Character.Reply("Impossible car cette map est invliade !");
                return;
            }
            var group = client.Character.Map.Instance.GetMapMonsterGroup(message.monsterGroupId);
            if (group != null)
            {
                client.Character.Map.Instance.MonstersGroups.Remove(group);
                client.Character.Map.Instance.RefreshActorsOnMap();
                FightPvM fight = FightProvider.Instance.CreatePvMFight(group, client.Character.Map, (short)group.CellId);

                fight.BlueTeam.AddFighter(client.Character.CreateFighter(fight.BlueTeam, fight)); // on ajoute le perso

                group.Monsters.ForEach(x => fight.RedTeam.AddFighter(x.CreateFighter(fight.RedTeam))); // on ajoute les monstres

                fight.StartPlacement();
            }
            else
            {
                if (client.Character.isDebugging)
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
            if (client.Character.Restrictions.cantAttackMonster == true)
            {
                client.Character.Reply("Impossible de rejoindre un combat en étant mort !");
                return;
            }
            if (client.Character.CurrentlyInTrackRequest)
            {
                client.Character.OnEndingUseDelayedObject(DelayedActionTypeEnum.DELAYED_ACTION_OBJECT_USE);
                client.Character.CurrentlyInTrackRequest = false;
            }
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


        static void LeaveFight(System.Timers.Timer Timer, WorldClient client)
        {
            client.Character.FighterInstance.Leave();
            Timer.Enabled = false;
            Timer.Stop();
            Timer.Dispose();
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
                Random r = new Random();
                var Timer = new System.Timers.Timer();
                Timer.Interval = 1000;
                Timer.Elapsed += (sender, e) => { LeaveFight(Timer, client); };
                Timer.Enabled = true;
            }
            catch
            {
                client.Character.NotificationError("Une erreur est survenue en essayant de quitter le combat !");
            }
        }

        [MessageHandler]
        public static void HandleFightOptions(GameFightOptionToggleMessage message, WorldClient client)
        {
            client.Character.FighterInstance.Fight.SetOption((FightOptionsEnum)message.option, client.Character.FighterInstance.Team);
        }
    }
}