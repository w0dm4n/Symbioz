using Symbioz.Core.Startup;
using Symbioz.DofusProtocol.Messages;
using Symbioz.Helper;
using Symbioz.PathProvider;
using Symbioz.World.Models;
using Symbioz.World.PathProvider;
using Symbioz.World.Records;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Timers;

namespace Symbioz.World
{
    public class CyclicMobsMovementTask
    {
        public const short MoveInstanceInterval = 20000;
        public const short MoveCellsCount = 3;

        private static System.Timers.Timer CyclicMobsMovementTimer { get; set; }

        [StartupInvoke("CyclicMobsMovementTask", StartupInvokeType.Cyclics)]
        public static void Start()
        {
            CyclicMobsMovementTimer = new System.Timers.Timer(MoveInstanceInterval);
            CyclicMobsMovementTimer.Elapsed += (sender, e) => CyclicMobsMovementTimer_Tick();
            CyclicMobsMovementTimer.Start();
        }

        private static void CyclicMobsMovementTimer_Tick()
        {
            foreach (var map in MapRecord.Maps)
            {
                if (map.Instance != null)
                {
                    if (map.Instance.MonstersGroups.Count() > 0 && map.Instance.Clients.Count > 0)
                    {
                        GenerateMobMovements(map, map.Instance.MonstersGroups);
                    }
                }
            }
        }

        private static async void GenerateMobMovements(MapRecord map, List<MonsterGroup> groups)
        {
            await Task.Run(() =>
            {
                for (int i = 0; i < groups.Count; i++)
                {
                    MonsterGroup group = groups[i];
                    MoveGroup(map, group);
                }
            });
        }

        private static void MoveGroup(MapRecord map, MonsterGroup group)
        {
            var dispatchMobTimer = new System.Timers.Timer(new AsyncRandom().Next(0, 20000));
            dispatchMobTimer.Elapsed += (sender, e) => DispatchMobTimer_Elapsed(sender, e, map, group);
            dispatchMobTimer.Start();
        }

        private static void DispatchMobTimer_Elapsed(object sender, ElapsedEventArgs e, MapRecord map, MonsterGroup group)
        {
            if(sender is System.Timers.Timer && (System.Timers.Timer)sender != null)
            {
                ((System.Timers.Timer)sender).Stop();
                ((System.Timers.Timer)sender).Dispose();
            }

            var info = MonsterGroup.GetActorInformations(map, group);

            List<short> cells = Pathfinding.GetCircleCells(info.disposition.cellId, MoveCellsCount);
            cells.Remove(info.disposition.cellId);
            cells.RemoveAll(x => !map.WalkableCells.Contains(x));
            if (cells.Count == 0)
                return;
            var newCell = cells[new AsyncRandom().Next(0, cells.Count())];
            var path = new Pathfinder(map, info.disposition.cellId, newCell).FindPath();
            if (path != null)
            {
                path.Insert(0, info.disposition.cellId);
                map.Instance.Send(new GameMapMovementMessage(path, info.contextualId));
                group.CellId = (ushort)newCell;
            }
            else
                Logger.Error("Impossible de déplacer le groupe de monstres '" + group.MonsterGroupId + "' (carte : " + map.Id + ").");
        }
    }
}
