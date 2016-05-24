using Symbioz.Network.Clients;
using System;
using System.Collections.Generic;
using System.Linq;
using Symbioz.DofusProtocol.Messages;
using System.Text;
using System.Threading.Tasks;
using Symbioz.Providers;
using Symbioz.Enums;
using Symbioz.World.Handlers;
using Symbioz.World.Records;

namespace Symbioz.World.Models.Fights
{
    public class ArenaGroup
    {
        public ArenaGroup(WorldClient master)
        {
            AddClient(master, true);
        }

        public bool Full { get { return RedTeam.Count == ArenaProvider.FIGHTERS_PER_TEAM && BlueTeam.Count == ArenaProvider.FIGHTERS_PER_TEAM; } }

        public bool Empty { get { return RedTeam.Count + BlueTeam.Count == 0; } }

        public List<TemporaryArenaClient> RedTeam { get { return Clients.FindAll(x => x.TeamColor == TeamColorEnum.RED_TEAM); } }

        public List<TemporaryArenaClient> BlueTeam { get { return Clients.FindAll(x => x.TeamColor == TeamColorEnum.BLUE_TEAM); } }
        /// <summary>
        /// Clients + Ready?
        /// </summary>
        public List<TemporaryArenaClient> Clients = new List<TemporaryArenaClient>();

        public List<TemporaryArenaClient> GetClients(PvpArenaStepEnum step)
        {
            return Clients.FindAll(x => x.ActualStep == step);
        }

        public void AddClient(WorldClient client, bool master)
        {
            TemporaryArenaClient newClient = null;

            if (RedTeam.Count < ArenaProvider.FIGHTERS_PER_TEAM)
            {
                newClient = new TemporaryArenaClient(client, TeamColorEnum.RED_TEAM, master);
            }
            else if (BlueTeam.Count < ArenaProvider.FIGHTERS_PER_TEAM)
            {
                newClient = new TemporaryArenaClient(client, TeamColorEnum.BLUE_TEAM, master);
            }

            Clients.Add(newClient);

            newClient.UpdateRegistrationStatus(true, PvpArenaStepEnum.ARENA_STEP_REGISTRED);

            if (Full)
                RequestFight();
        }
        public void Answer(WorldClient client, bool accept)
        {
            if (accept)
                AcceptFight(client);
            else
                DenyFight(client);
        }
        void DenyFight(WorldClient client)
        {
            TemporaryArenaClient arenaClient = GetClient(client);
            ArenaProvider.Instance.Unregister(client);
            arenaClient.UpdateFighterStatus(Clients);
        }
        void AcceptFight(WorldClient client)
        {
            
            TemporaryArenaClient arenaClient = GetClient(client);
            arenaClient.Ready = true;
            arenaClient.UpdateRegistrationStatus(false, PvpArenaStepEnum.ARENA_STEP_WAITING_FIGHT);
            arenaClient.UpdateFighterStatus(Clients);

            foreach (var aClient in Clients)
            {
                if (aClient.Ready && aClient != arenaClient)
                {
                    aClient.UpdateFighterStatus(new List<TemporaryArenaClient>() { aClient });
                }
            }
            if (RedTeam.All(x => x.Ready) && BlueTeam.All(x => x.Ready))
                StartFighting();
         



        }
        public bool CanJoin(WorldClient client)
        {
            uint masterLevel = GetMaster().WorldClient.Character.Record.Level;
            uint clientLevel = client.Character.Record.Level;
            if (clientLevel >= masterLevel - ArenaProvider.SEARCH_LEVEL_SHIFT && clientLevel <= masterLevel + ArenaProvider.SEARCH_LEVEL_SHIFT)
                return true;
            else
                return false;
        }
        public TemporaryArenaClient GetMaster()
        {
            return Clients.Find(x => x.Master);
        }
        public void StartFighting()
        {
           

            Clients.ForEach(x => x.UpdateRegistrationStatus(false, PvpArenaStepEnum.ARENA_STEP_STARTING_FIGHT));
           
            int arenaMapId = ArenaProvider.Instance.RandomArenaMap();

            BlueTeam.ForEach(x => MapsHandler.SendCurrentMapMessage(x.WorldClient, arenaMapId));
            RedTeam.ForEach(x => MapsHandler.SendCurrentMapMessage(x.WorldClient, arenaMapId));

            var fight = FightProvider.Instance.CreateArenaFight(MapRecord.GetMap(arenaMapId), this);
            BlueTeam.ForEach(x => fight.BlueTeam.AddFighter(x.WorldClient.Character.CreateFighter(fight.BlueTeam)));
            RedTeam.ForEach(x => fight.RedTeam.AddFighter(x.WorldClient.Character.CreateFighter(fight.RedTeam)));
            fight.StartPlacement();
            Clients.Clear();
            ArenaProvider.Instance.m_arena_groups.Remove(this);
        }

        public void Remove(WorldClient client)
        {
            var arenaClient = GetClient(client);
            if (arenaClient.Master)
            {
                if (Clients.Count > 1)
                {
                    Clients.Find(x => x != arenaClient).Master = true;
                }
            }
            Clients.Remove(arenaClient);
        }

        public TemporaryArenaClient GetClient(WorldClient client)
        {
            return Clients.Find(x => x.WorldClient == client);
        }
        public bool Contains(WorldClient client)
        {
            return Clients.Find(x => x.WorldClient == client) != null;
        }
        public void RequestFight()
        {
            RedTeam.ForEach(x => x.RequestFight(RedTeam.ConvertAll<int>(w => w.WorldClient.Character.Id)));
            BlueTeam.ForEach(x => x.RequestFight(BlueTeam.ConvertAll<int>(w => w.WorldClient.Character.Id)));
        }
    }
}
