using Symbioz.DofusProtocol.Messages;
using Symbioz.Enums;
using Symbioz.Helper;
using Symbioz.Network.Clients;
using Symbioz.World.Models.Fights;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Symbioz.Providers
{
    public class ArenaProvider : Singleton<ArenaProvider>
    {
        private static object m_locker = new object();

        public int[] ARENA_MAPS = new int[]
        {
            94634497,
			94634499,
			94634501,
			94634507,
			94634509,
			94634511,
			94634513,
			94634515,
			94634517,
			94634519,
			94634505,
			94634503
        };

        public const int ARENA_REQUEST_TIMOUT = 0;//30;

        public const int DEFAULT_RANK = 300;

        public const int FIGHTERS_PER_TEAM = 3; // 3 (3vs3)

        public const sbyte SEARCH_LEVEL_SHIFT = 20;

        public const short MINIMUM_LEVEL_TO_SEARCH_ARENA = 50;

        public List<ArenaGroup> m_arena_groups = new List<ArenaGroup>();

        public void SearchArena(WorldClient client)
        {
            lock (m_locker)
            {
                ArenaGroup arena = FindArena(client);

                if (arena == null)
                {
                    m_arena_groups.Add(new ArenaGroup(client));
                }
                else
                {
                    arena.AddClient(client, false);
                }
                client.Send(new GameRolePlayArenaRegistrationStatusMessage(true, (sbyte)PvpArenaStepEnum.ARENA_STEP_REGISTRED, 0));
            }
        }
        public ArenaGroup FindArena(WorldClient client)
        {
            return m_arena_groups.Find(x => !x.Full && x.CanJoin(client));
        }
        public void Unregister(WorldClient client)
        {
            lock (m_locker)
            {

                ArenaGroup group = m_arena_groups.Find(x => x.Contains(client));

                if (group == null)
                    Logger.Error("Unable to unregister " + client.Character.Record.Name + " no arena linked to this character");
                else
                {
                    group.Clients.FindAll(x => x.Ready).ForEach(x => x.UpdateRegistrationStatus(true, PvpArenaStepEnum.ARENA_STEP_REGISTRED));
                    group.Clients.ForEach(x => x.Ready = false);
                    group.Clients.FindAll(x => x.Requested).ForEach(x => x.AbortRequest(client.Character.Id));
                    group.GetClient(client).UpdateRegistrationStatus(false, PvpArenaStepEnum.ARENA_STEP_UNREGISTER);
                    group.Remove(client);
                    if (group.Empty)
                    {
                        m_arena_groups.Remove(group);
                    }
                }
            }

        }
        public void OnClientDisconnected(WorldClient client)
        {
            Unregister(client);
        }
        public void Answer(WorldClient client, bool accept)
        {
            lock (m_locker)
            {
                ArenaGroup group = m_arena_groups.Find(x => x.Contains(client));

                if (group == null)
                {
                    client.Character.ReplyError("Vous ne pouvez pas répondre a un match, désinscrit...");
                    return;
                }
                else
                {
                    group.Answer(client, accept);
                }

            }
        }
        public bool IsSearching(WorldClient client)
        {
            return m_arena_groups.Find(x => x.Contains(client)) != null;
        }
        public int RandomArenaMap()
        {
            return ARENA_MAPS.ToList().Random<int>();
        }
    }
}
