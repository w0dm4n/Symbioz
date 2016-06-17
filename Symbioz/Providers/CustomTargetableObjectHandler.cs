using Shader.Helper;
using Symbioz.Core.Startup;
using Symbioz.Enums;
using Symbioz.Network.Clients;
using Symbioz.Network.Servers;
using Symbioz.World.Models;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Timers;

namespace Symbioz.Providers
{
    public static class CustomTargetableObjectHandler
    {
        private static Dictionary<ushort, Action<WorldClient, CharacterItemRecord, int, bool>> CustomItemIdsHandlers = new Dictionary<ushort, Action<WorldClient, CharacterItemRecord, int, bool>>();

        [StartupInvoke(StartupInvokeType.Others)]
        public static void Initialize()
        {
            //Handlers for ItemId [ItemGID, CatchMethod(WorldClient, CharacterItemRecord)]
            CustomItemIdsHandlers.Add(7401, HandleBlankParchment); //Parchemins vierges
        }

        #region CustomItemIdsHandlers

        public static bool CustomHandlerExistForItemId(ushort itemGID)
        {
            return CustomItemIdsHandlers.ContainsKey(itemGID);
        }

        public static void HandleByItemGID(WorldClient client, CharacterItemRecord item, int abstractTargetId, bool usedOnCell = false)
        {
            var handler = CustomItemIdsHandlers.FirstOrDefault(x => x.Key == item.GID);
            handler.Value(client, item, abstractTargetId, usedOnCell);
        }

        #region BlankParchment

        private static void HandleBlankParchment(WorldClient client, CharacterItemRecord item, int abstractTargetId, bool usedOnCell)
        {
            if (!client.Character.IsTracking)
            {
                if (!usedOnCell)
                {
                    int characterId = abstractTargetId;

                    WorldClient target = WorldServer.Instance.GetOnlineClient(characterId);
                    if (target != null && target != client)
                    {
                        target.Character.CurrentlyInTrackRequest = true;
                        client.Character.IsTracking = true;
                        client.Character.OnStartingUseDelayedObject(DelayedActionTypeEnum.DELAYED_ACTION_OBJECT_USE, DateTime.UtcNow.AddSeconds(5).ToEpochTimeInMillis(), item.GID);

                        var actionTimer = new Timer();
                        actionTimer.Interval = (5000);
                        actionTimer.Elapsed += (sender, e) => { client.Character.OnTrackingTimeElapsed(target, actionTimer, item); };
                        actionTimer.Start();
                    }
                }
                else
                {
                    int cellId = abstractTargetId;
                    client.Character.Reply("Vous devez cibler un personnage pour pouvoir le traquer !", Color.Red);
                }
            }
        }

        #endregion

        #endregion
    }
}
