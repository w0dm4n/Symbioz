﻿
using Symbioz.Enums;
using Symbioz.PathProvider;
using Symbioz.World.Models.Fights.Fighters;
using Symbioz.World.Records;
using System.Collections.Generic;
using System.Linq;

namespace Symbioz.World.PathProvider
{
    internal class Pathfinder
    {
        public const byte LINE_COST = 10;

        private List<Node> m_cells;
        private Node m_start;
        private Node m_end;

        private SortedNodeList<Node> m_openList;
        private NodeList<Node> m_closeList;

        public Pathfinder(MapRecord map, short start, short end)
        {
            this.m_cells = GetNodes(map);

            this.m_start = this.m_cells.FirstOrDefault(x => x.CellID == start);
            this.m_end = this.m_cells.FirstOrDefault(x => x.CellID == end);

            this.m_openList = new SortedNodeList<Node>();
            this.m_closeList = new NodeList<Node>();
        }
        public Pathfinder(MapRecord map, short start)
        {
            this.m_cells = GetNodes(map);

            this.m_start = this.m_cells.FirstOrDefault(x => x.CellID == start);

            this.m_openList = new SortedNodeList<Node>();
            this.m_closeList = new NodeList<Node>();
        }

        public List<short> FindPath()
        {
            this.m_end.Walkable = true;
            this.m_openList.Add(this.m_start);
            var infinite = 0;
            while (this.m_openList.Count > 0)
            {
                var bestCell = this.m_openList.RemoveFirst();
                if (bestCell.CellID == this.m_end.CellID)
                {
                    var sol = new List<short>();
                    while (bestCell.Parent != null && bestCell != this.m_start)
                    {
                        sol.Add(bestCell.CellID);
                        bestCell = bestCell.Parent;
                    }
                    sol.Reverse();

                    this.m_end.Walkable = false;
                    return sol;
                }
                this.m_closeList.Add(bestCell);
                this.AddToOpen(bestCell, this.GetNeighbors(bestCell));
                infinite++;
                if (infinite > 100000)
                    break;
            }
            this.m_end.Walkable = false;
            return null;
        }
        public List<short> FindPath(short target)
        {
            this.m_end = this.m_cells.FirstOrDefault(x => x.CellID == target);
            this.m_end.Walkable = true;

            this.m_openList.Add(this.m_start);
            var infinite = 0;
            while (this.m_openList.Count > 0)
            {
                var bestCell = this.m_openList.RemoveFirst();
                if (bestCell.CellID == this.m_end.CellID)
                {
                    var sol = new List<short>();
                    while (bestCell.Parent != null && bestCell != this.m_start)
                    {
                        sol.Add(bestCell.CellID);
                        bestCell = bestCell.Parent;
                    }
                    sol.Reverse();

                    this.m_end.Walkable = false;
                    return sol;
                }
                this.m_closeList.Add(bestCell);
                this.AddToOpen(bestCell, this.GetNeighbors(bestCell));
                infinite++;
                if (infinite > 100000)
                    break;
            }
            this.m_end.Walkable = false;
            return null;
        }

        public List<short> FindPathProche(short target, int pm, short start)
        {
            List <short> lst = FindPath(target);

            if (lst == null)
            {
                List<DirectionsEnum> dirs = new List<DirectionsEnum>();
                dirs.Add(DirectionsEnum.DIRECTION_NORTH_EAST);
                dirs.Add(DirectionsEnum.DIRECTION_NORTH_WEST);
                dirs.Add(DirectionsEnum.DIRECTION_SOUTH_EAST);
                dirs.Add(DirectionsEnum.DIRECTION_SOUTH_WEST);
                foreach (DirectionsEnum dir in dirs)
                {
                    short cell = PathHelper.GetNearCellByDirection(target, dir);
                    int nbr = PathHelper.GetDistanceBetween(start, cell);

                    if (nbr > pm)
                        continue;
                    lst = FindPath(cell);
                    if (lst != null)
                        return (lst);
                }
            }
            else
                return (lst);
            return null;
        }

        private List<Node> GetNeighbors(Node node)
        {
            var nodes = new List<Node>();

            node.Neighbors.ForEach(x =>
            {
                var cell = this.m_cells.FirstOrDefault(y => y.CellID == x.ID);
                if (cell != null && cell.Walkable)
                {
                    if (cell.Parent == null)
                        cell.Parent = node;

                    nodes.Add(cell);
                }
            });

            return nodes;
        }

        private Node GetBestNode()
        {
            this.m_openList.OrderBy(x => x.H);
            return this.m_openList.First();
        }
        public void PutEntities(List<Fighter> fighters)
        {
            foreach (var fighter in fighters)
            {
                var node = this.m_cells.FirstOrDefault(x => x.CellID == fighter.CellId);
                if (node != this.m_start)
                    node.Walkable = false;
            }
        }

        public static List<Node> GetNodes(MapRecord map)
        {
            var nodes = new List<Node>();
            for (short cell = 0; cell < 560;cell++)
            {
                var node = new Node(CoordCells.GetCell(cell));
                if (map.WalkableCells.Contains(cell))
                    node.Walkable = true;
                else
                    node.Walkable = false;

                nodes.Add(node);
            }
            return nodes;
        }

        private void AddToOpen(Node current, IEnumerable<Node> nodes)
        {
            foreach (var node in nodes)
            {
                if (!this.m_openList.Contains(node))
                {
                    if (!this.m_closeList.Contains(node))
                        this.m_openList.AddDichotomic(node);
                }
                else
                {
                    if (node.CostWillBe() < this.m_openList[node].G)
                        node.Parent = current;
                }
            }
        }
    }
}
