using Symbioz.Network.Messages;
using Symbioz.DofusProtocol.Messages;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Symbioz.Network.Clients;
using Symbioz.Enums;
using Symbioz.World.Records;
using Symbioz.World.Models.Maps;
using System.Diagnostics;
using Symbioz.DofusProtocol.Types;
using Symbioz.World.Models;
using Symbioz.Providers.SpellEffectsProvider.Buffs;
using Symbioz.Network.Servers;
using Symbioz.World.Models.Parties.Dungeon;

namespace Symbioz.World.Handlers
{
    class ContextRolePlayHandler
    {
        static UInt16ReflectedStat GetReflectedStat(StatsRecord stats, StatsBoostTypeEnum type)
        {
            var fieldInfo = StatsRecord.GetFieldInfo("Base"+type.ToString());
            return new UInt16ReflectedStat(fieldInfo, stats);
        }
        [MessageHandler]
        public static void HandleStatsUpgrade(StatsUpgradeRequestMessage message, WorldClient client)
        {
            if (client.Character.IsFighting)
            {
                client.Send(new StatsUpgradeResultMessage((sbyte)StatsUpgradeResultEnum.IN_FIGHT, 0));
                return;
            }
            StatsBoostTypeEnum statId = (StatsBoostTypeEnum)message.statId;
            if (statId < StatsBoostTypeEnum.Strength || statId > StatsBoostTypeEnum.Intelligence)
            {
                Logger.Error("Wrong statsid");
            }
            if (message.boostPoint > 0)
            {
                UInt16ReflectedStat linkedStat = GetReflectedStat(client.Character.StatsRecord,statId);

                BreedRecord breed = BreedRecord.GetBreed(client.Character.Record.Breed);
                int num = linkedStat.GetValue();
                ushort num2 = message.boostPoint;
                if (num2 >= 1 && message.boostPoint <= (short)client.Character.Record.StatsPoints)
                {
                    uint[][] thresholds = breed.GetThresholds(statId);
                    int thresholdIndex = breed.GetThresholdIndex(num, thresholds);
                    while ((long)num2 >= (long)((ulong)thresholds[thresholdIndex][1]))
                    {
                        short num3;
                        short num4;
                        if (thresholdIndex < thresholds.Length - 1 && (double)num2 / thresholds[thresholdIndex][1] > (double)((ulong)thresholds[thresholdIndex + 1][0] - (ulong)((long)num)))
                        {
                            num3 = (short)((ulong)thresholds[thresholdIndex + 1][0] - (ulong)((long)num));
                            num4 = (short)((long)num3 * (long)((ulong)thresholds[thresholdIndex][1]));
                            if (thresholds[thresholdIndex].Length > 2)
                            {
                                num3 = (short)((long)num3 * (long)((ulong)thresholds[thresholdIndex][2]));
                            }
                        }
                        else
                        {
                            num3 = (short)System.Math.Floor((double)num2 / thresholds[thresholdIndex][1]);
                            num4 = (short)((long)num3 * (long)((ulong)thresholds[thresholdIndex][1]));
                            if (thresholds[thresholdIndex].Length > 2)
                            {
                                num3 = (short)((long)num3 * (long)((ulong)thresholds[thresholdIndex][2]));
                            }
                        }
                        num += (int)num3;
                        num2 -= (ushort)num4;
                        thresholdIndex = breed.GetThresholdIndex(num, thresholds);
                    }
          
                   
                    if (statId == StatsBoostTypeEnum.Vitality)
                    {
                        var previousVitality = linkedStat.GetValue();
                        linkedStat.SetValue((short)num);
                        client.Character.StatsRecord.LifePoints += (short)(client.Character.StatsRecord.BaseVitality - previousVitality);
                        client.Character.CurrentStats.LifePoints += (uint)(client.Character.StatsRecord.BaseVitality - previousVitality);
                    }
                    else
                    {
                        linkedStat.SetValue((short)num);
                    }
                    client.Character.Record.StatsPoints -= (ushort)(message.boostPoint - num2);
                    client.Send(new StatsUpgradeResultMessage((sbyte)StatsUpgradeResultEnum.SUCCESS, message.boostPoint));
                    client.Character.RefreshStats();

                }
            }
        }
        [MessageHandler]
        public static void HandleSpellUpgrade(SpellUpgradeRequestMessage message, WorldClient client)
        {
            if (client.Character.IsFighting)
            {
                client.Character.Reply("Vous ne pouvez pas effectuer cette action en combat.");
                return;
            }
            else
            {
                client.Character.BoostSpell(message.spellId, message.spellLevel);
            }
        }
        [MessageHandler]
        public static void HandleBasicPing(BasicPingMessage message, DofusClient client)
        {
            client.Send(new BasicPongMessage(message.quiet));
        }
        [MessageHandler]
        public static void HandleLeaveDialog(LeaveDialogRequestMessage message, WorldClient client)
        {

            if (client.Character.CurrentDialogType == DialogTypeEnum.DIALOG_EXCHANGE)
            {
                client.Character.LeaveExchange();
            }
            if (client.Character.CurrentDialogType != null)
            {
                client.Send(new LeaveDialogMessage((sbyte)client.Character.CurrentDialogType));
                client.Character.CurrentDialogType = null;
            }
        }
        [MessageHandler]
        public static void HandleShortcutBarSwap(ShortcutBarSwapRequestMessage message, WorldClient client)
        {
            switch ((ShortcutBarEnum)message.barType)
            {
                case ShortcutBarEnum.SPELL_SHORTCUT_BAR:
                    SpellShortcutRecord.SwapSortcut(client.Character.Id, message.firstSlot, message.secondSlot);
                    break;
                case ShortcutBarEnum.GENERAL_SHORTCUT_BAR:
                    GeneralShortcutRecord.SwapSortcut(client.Character.Id, message.firstSlot, message.secondSlot);
                    break;
            }
            client.Character.RefreshShortcuts();
        }
        [MessageHandler]
        public static void HandleShortcutBarRemove(ShortcutBarRemoveRequestMessage message, WorldClient client)
        {
            switch ((ShortcutBarEnum)message.barType)
            {
                case ShortcutBarEnum.GENERAL_SHORTCUT_BAR:
                    GeneralShortcutRecord.RemoveShortcut(client.Character.Id, message.slot);
                    break;
                case ShortcutBarEnum.SPELL_SHORTCUT_BAR:
                    SpellShortcutRecord.RemoveShortcut(client.Character.Id, message.slot);
                    break;
            }
            client.Send(new ShortcutBarRemovedMessage(message.barType, message.slot));
        }
        [MessageHandler]
        public static void HandleFriendJoin(FriendJoinRequestMessage message,WorldClient client)
        {
            WorldClient target = WorldServer.Instance.GetOnlineClient(message.name);
            if (target != null)
            {
                client.Character.Teleport(target.Character.Record.MapId, target.Character.Record.CellId);
                client.Character.Reply("Vous avez été téléporté a " + message.name);
            }
            else
            {
                client.Character.Reply("Le personnage n'éxiste pas ou n'est pas connécté.");
            }
        }
        [MessageHandler]
        public static void HandleShortcutBarAdd(ShortcutBarAddRequestMessage message, WorldClient client)
        {
            switch ((ShortcutBarEnum)message.barType)
            {
                case ShortcutBarEnum.GENERAL_SHORTCUT_BAR:
                    if (message.shortcut is ShortcutObjectItem)
                    {
                        ShortcutObjectItem shortcutObj = (ShortcutObjectItem)message.shortcut;
                        GeneralShortcutRecord.AddShortcut(client.Character.Id, shortcutObj.slot, ShortcutObjectItem.Id, shortcutObj.itemUID, shortcutObj.itemGID);
                    }
                    if (message.shortcut is ShortcutSmiley)
                    {
                        ShortcutSmiley shortcutSmiley = (ShortcutSmiley)message.shortcut;
                        GeneralShortcutRecord.AddShortcut(client.Character.Id, shortcutSmiley.slot, ShortcutSmiley.Id, shortcutSmiley.smileyId, 0);
                    }
                    if (message.shortcut is ShortcutEmote)
                    {
                        ShortcutEmote shortcutEmote = (ShortcutEmote)message.shortcut;
                        GeneralShortcutRecord.AddShortcut(client.Character.Id, shortcutEmote.slot, ShortcutEmote.Id, shortcutEmote.emoteId, 0);
                    }
                    break;

                case ShortcutBarEnum.SPELL_SHORTCUT_BAR:
                    ShortcutSpell shortcut = (ShortcutSpell)message.shortcut;
                    SpellShortcutRecord.AddShortcut(client.Character.Id, shortcut.slot, shortcut.spellId);
                    break;
            }
            client.Character.RefreshShortcuts();
        }
        [MessageHandler]
        public static void HandePlayerStatusChangeRequest(PlayerStatusUpdateRequestMessage message, WorldClient client)
        {
            client.Character.PlayerStatus = message.status;
            if((PlayerStatusEnum)message.status.statusId == PlayerStatusEnum.PLAYER_STATUS_AFK || (PlayerStatusEnum)message.status.statusId == PlayerStatusEnum.PLAYER_STATUS_PRIVATE || (PlayerStatusEnum)message.status.statusId == PlayerStatusEnum.PLAYER_STATUS_SOLO)
            {
                if(DungeonPartyProvider.Instance.GetDPCByCharacterId(client.Character.Id) != null)
                {
                    DungeonPartyProvider.Instance.RemoveCharacter(client.Character);
                }
            }
            client.Send(new PlayerStatusUpdateMessage(client.Character.Record.AccountId, (uint)client.Character.Id, message.status));
        }
    }
}