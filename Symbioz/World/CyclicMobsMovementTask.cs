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
    /// <summary>
    /// A refaire, baclé :3
    /// </summary>
    class CyclicMobsMovementTask
    {
        public const short MoveInstanceInterval = 20000; // pourquoi les noms de constantes ne sont-il pas en majuscules ? u_u
        public const short MoveCellsCount = 3;

        static System.Timers.Timer m_timer { get; set; }
  //      [StartupInvoke(StartupInvokeType.Cyclics)]
        public static void Start()
        {
            m_timer = new System.Timers.Timer(MoveInstanceInterval); // pourquoi j'ai eut la flemme d'ajouter la réference system.timers? u_u
            m_timer.Elapsed += m_timer_Elapsed;
            m_timer.Start();
            
        }
        static void m_timer_Elapsed(object sender, ElapsedEventArgs e)
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
        static void MoveGroup(MapRecord map,MonsterGroup group)
        {

            var random = new AsyncRandom();
            var info = MonsterGroup.GetActorInformations(map, group);
            List<short> cells = Pathfinding.GetCircleCells(info.disposition.cellId, MoveCellsCount);
            cells.Remove(info.disposition.cellId);
            cells.RemoveAll(x => !map.WalkableCells.Contains(x));
            cells.RemoveAll(x => PathHelper.GetDistanceBetween(info.disposition.cellId, x) <= MoveCellsCount);
            if (cells.Count == 0)
                return;
            var newCell = cells[random.Next(0, cells.Count())];
            var path = new Pathfinder(map, info.disposition.cellId, newCell).FindPath();
            if (path != null)
            {
                path.Insert(0, info.disposition.cellId);
                map.Instance.Send(new GameMapMovementMessage(path, info.contextualId));
                group.CellId = (ushort)newCell;
            }
            else
                Logger.Error("Unable to move group" + group.MonsterGroupId + " on map " + map.Id + ", wrong path");

        }
        /// <summary>
        /// Thread de sleep a virer, plutot un cooldownAction qui renvoit vers MoveGroup()
        /// </summary>
        /// <param name="map"></param>
        /// <param name="groups"></param>
        static async void GenerateMobMovements(MapRecord map, List<MonsterGroup> groups)
        {
            await Task.Run(() =>
            {
                for (int i = 0; i < groups.Count; i++)
                {
                    MonsterGroup group = groups[i];
                    MoveGroup(map, group);
                  ///  Thread.Sleep(2000);
                }
                
            });
        }
    }
}
