using Symbioz.Core.Startup;
using Symbioz.DofusProtocol.Messages;
using Symbioz.Helper;
using Symbioz.PathProvider;
using Symbioz.World.PathProvider;
using Symbioz.World.Records;
using Symbioz.World.Records.Alliances.Prisms;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Symbioz.World
{
    public class CyclicPrismsMovementTask
    {
        public const short MoveInstanceInterval = 30000;
        public const short MoveCellsCount = 3;

        private static System.Timers.Timer CyclicPrismsMovementTimer { get; set; }

        [StartupInvoke("CyclicPrismsMovementTask", StartupInvokeType.Cyclics)]
        public static void Start()
        {
            CyclicPrismsMovementTimer = new System.Timers.Timer(MoveInstanceInterval);
            CyclicPrismsMovementTimer.Elapsed += (sender, e) => CyclicPrismsMovementTimer_Tick();
            CyclicPrismsMovementTimer.Start();
        }

        private static void CyclicPrismsMovementTimer_Tick()
        {
            foreach (var prism in PrismRecord.Prisms)
            {
                if (prism.Map != null)
                {
                    if (prism.Map.Clients.Count > 0)
                    {
                        MovePrism(prism);
                    }
                }
            }
        }

        private static void MovePrism(PrismRecord prism)
        {
            var random = new AsyncRandom();
            var map = prism.Map.Record;

            List<short> cells = Pathfinding.GetCircleCells((short)prism.CellId, MoveCellsCount);
            cells.Remove((short)prism.CellId);
            cells.RemoveAll(x => !map.WalkableCells.Contains(x));
            if (cells.Count == 0)
                return;
            var newCell = cells[random.Next(0, cells.Count())];
            var path = new Pathfinder(map, (short)prism.CellId, newCell).FindPath();
            if (path != null)
            {
                path.Insert(0, (short)prism.CellId);
                map.Instance.Send(new GameMapMovementMessage(path, PrismRecord.ConstantContextualId));
                prism.CellId = (ushort)newCell;
            }
            else
                Logger.Error("Impossible de déplacer le prisme '" + prism.Id + "' (carte : " + map.Id + ").");
        }
    }
}
