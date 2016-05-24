using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Drawing;
using Symbioz.Enums;

namespace Symbioz.PathProvider
{
    public class PathHelper  
    {
        #region constants
        private static readonly Point VECTOR_RIGHT = new Point(1, 1);
        private static readonly Point VECTOR_DOWN_RIGHT = new Point(1, 0);
        private static readonly Point VECTOR_DOWN = new Point(1, -1);
        private static readonly Point VECTOR_DOWN_LEFT = new Point(0, -1);
        private static readonly Point VECTOR_LEFT = new Point(-1, -1);
        private static readonly Point VECTOR_UP_LEFT = new Point(-1, 0);
        private static readonly Point VECTOR_UP = new Point(-1, 1);
        private static readonly Point VECTOR_UP_RIGHT = new Point(0, 1);
        public static Dictionary<short, Point> DiagonalsCells = new Dictionary<short, Point>();
        public const int MAP_WIDTH = 14;
        public const int MAP_HEIGHT = 20;
        #endregion

        #region IsRightLine

        public static bool IsRightLine(short originCellId, short destinationCellId)
        {
            DiagonalsCells.Clear();
            LineOfSightUtil();
            List<short> line = GetLine(originCellId, destinationCellId);
            Point lastPoint = GetPoint(line[0]);
            Point currentPoint;
            bool onX = false;
            bool onY = false;
            if (lastPoint.X == GetPoint(line[1]).X)
                onX = true;
            else if (lastPoint.Y == GetPoint(line[1]).Y)
                onY = true;
            else
                return false;
            for (int i = 1; i < line.Count; i++)
            {

                currentPoint = GetPoint(line[i]);

                if (GetDistanceBetween(originCellId, GetCellId(currentPoint)) < 2)
                    continue;

                if (onX && currentPoint.X != lastPoint.X)
                    return false;
                else if (onY && currentPoint.Y != lastPoint.Y)
                    return false;
                else
                    lastPoint = currentPoint;
            }
            return true;
        }

        public static bool IsRightLine(List<short> line)
        {
            Point lastPoint = GetPoint(line[0]);
            Point currentPoint;
            bool onX = false;
            bool onY = false;

            if (lastPoint.X == GetPoint(line[1]).X)
                onX = true;
            else if (lastPoint.Y == GetPoint(line[1]).Y)
                onY = true;
            else
                return false;

            for (int i = 1; i < line.Count; i++)
            {
                currentPoint = GetPoint(line[i]);

                if (GetDistanceBetween(line.First(), GetCellId(currentPoint)) < 2)
                    continue;

                if (onX && currentPoint.X != lastPoint.X)
                    return false;
                else if (onY && currentPoint.Y != lastPoint.Y)
                    return false;
                else
                    lastPoint = currentPoint;
            }
            return true;
        }

        #endregion

        #region GetPushedCell

        /*public static short GetPushedCell(short Cell1, byte Radius)
        {
            for (int i = 0; i < Radius; i++)
            {

            }
        }*/

        #endregion

        #region Direction/nearcell

        public static short GetNearCell(short Cell1, DirectionsEnum Direction)
        {
            switch (Direction)
            {
                case DirectionsEnum.DIRECTION_EAST:
                    return (short)(Cell1 + 1);

                case DirectionsEnum.DIRECTION_WEST:
                    return (short)(Cell1 - 1);

                case DirectionsEnum.DIRECTION_SOUTH:
                    return (short)(Cell1 + 28);

                case DirectionsEnum.DIRECTION_NORTH:
                    return (short)(Cell1 - 28);

                case DirectionsEnum.DIRECTION_SOUTH_EAST:
                    return (short)(Cell1 + (DiagonalsCells.ContainsKey(Cell1) ? 14 : 15));

                case DirectionsEnum.DIRECTION_NORTH_WEST:
                    return (short)(Cell1 - (DiagonalsCells.ContainsKey(Cell1) ? 15 : 14));

                case DirectionsEnum.DIRECTION_SOUTH_WEST:
                    return (short)(Cell1 + (DiagonalsCells.ContainsKey(Cell1) ? 13 : 14));

                case DirectionsEnum.DIRECTION_NORTH_EAST:
                    return (short)(Cell1 - (DiagonalsCells.ContainsKey(Cell1) ? 14 : 13));

                default:
                    return 0;
            }
        }

        public static DirectionsEnum GetDirection(short Cell1, short cell2)
        {
            if (((short)(Cell1 + 1)) == cell2)
                return DirectionsEnum.DIRECTION_EAST;

            if (((short)(Cell1 - 1)) == cell2)
                return DirectionsEnum.DIRECTION_WEST;

            if (((short)(Cell1 + 28)) == cell2)
                return DirectionsEnum.DIRECTION_SOUTH;

            if (((short)(Cell1 - 28)) == cell2)
                return DirectionsEnum.DIRECTION_NORTH;

            if (((short)(Cell1 + (DiagonalsCells.ContainsKey(Cell1) ? 14 : 15))) == cell2)
                return DirectionsEnum.DIRECTION_SOUTH_EAST;

            if (((short)(Cell1 - (DiagonalsCells.ContainsKey(Cell1) ? 15 : 14))) == cell2)
                return DirectionsEnum.DIRECTION_NORTH_WEST;

            if (((short)(Cell1 + (DiagonalsCells.ContainsKey(Cell1) ? 13 : 14))) == cell2)
                return DirectionsEnum.DIRECTION_SOUTH_WEST;

            if (((short)(Cell1 - (DiagonalsCells.ContainsKey(Cell1) ? 14 : 13))) == cell2)
                return DirectionsEnum.DIRECTION_NORTH_EAST;

            else return DirectionsEnum.DIRECTION_NORTH;

        }

        public static short GetNearCellByDirection(short Cell1, DirectionsEnum Direction)
        {
            switch (Direction)
            {
                case DirectionsEnum.DIRECTION_EAST:
                    return (short)(Cell1 + 1);

                case DirectionsEnum.DIRECTION_WEST:
                    return (short)(Cell1 - 1);

                case DirectionsEnum.DIRECTION_SOUTH:
                    return (short)(Cell1 + 28);

                case DirectionsEnum.DIRECTION_NORTH:
                    return (short)(Cell1 - 28);

                case DirectionsEnum.DIRECTION_SOUTH_EAST:
                    return (short)(Cell1 + 29);

                case DirectionsEnum.DIRECTION_NORTH_WEST:
                    return (short)(Cell1 - 29);

                case DirectionsEnum.DIRECTION_SOUTH_WEST:
                    return (short)(Cell1 + 27);

                case DirectionsEnum.DIRECTION_NORTH_EAST:
                    return (short)(Cell1 - 27);

                default:
                    return 0;
            }
        }

        public static int GetDirectionNew(int BeginCell, int EndCell)
        {
            var ListChange = new int[] { 1, MAP_WIDTH, MAP_WIDTH * 2 - 1, MAP_WIDTH - 1, -1, -MAP_WIDTH, -MAP_WIDTH * 2 + 1, -(MAP_WIDTH - 1) };
            var Result = EndCell - BeginCell;

            for (int i = 7; i > -1; i--)
                if (Result == ListChange[i])
                    return i;

            var ResultX = GetCellXCoord(EndCell) - GetCellXCoord(BeginCell);
            var ResultY = GetCellYCoord(EndCell) - GetCellYCoord(BeginCell);

            if (ResultX == 0)
                if (ResultY > 0)
                    return 3;
                else
                    return 7;
            else if (ResultX > 0)
                return 1;
            else
                return 5;
        }

        #endregion

        #region GetLine

        public static List<short> GetLine(short originCellId, short destinationCellId)
        {
            LineOfSightUtil();
            Point[] linePoints = GetLine(_cellsPositions[originCellId], _cellsPositions[destinationCellId]);
            return linePoints.Select(n => GetCellId(n)).ToArray().ToList();
        }

        public static Point[] GetLine(Point origin, Point destination)
        {
            return GetLine((double)origin.X, (double)origin.Y, (double)destination.X, (double)destination.Y);
        }

        #region GetLine

        public static Point[] GetLine(double x1, double y1, double x2, double y2)
        {
            int _loc_10 = 0;
            uint _loc_22 = 0;
            uint _loc_23 = 0;
            List<Point3d> _loc_7 = new List<Point3d>();
            Point3d _loc_21;
            Point3d _loc_8 = new Point3d(x1, y1);
            Point3d _loc_9 = new Point3d(x2, y2);
            Point3d _loc_11 = new Point3d(_loc_8.X + 0.5, _loc_8.Y + 0.5, _loc_8.Z);
            Point3d _loc_12 = new Point3d(_loc_9.X + 0.5, _loc_9.Y + 0.5, _loc_9.Z);
            double _loc_13, _loc_14, _loc_15, _loc_16;
            _loc_13 = _loc_14 = _loc_15 = _loc_16 = 0;
            bool _loc_17 = _loc_11.Z > _loc_12.Z;
            List<double> _loc_18 = new List<double>();
            List<double> _loc_19 = new List<double>();
            uint _loc_20 = 0;
            if (Math.Abs(_loc_11.X - _loc_12.X) == Math.Abs(_loc_11.Y - _loc_12.Y))
            {
                _loc_16 = Math.Abs(_loc_11.X - _loc_12.X);
                _loc_13 = _loc_12.X > _loc_11.X ? (1) : (-1);
                _loc_14 = _loc_12.Y > _loc_11.Y ? (1) : (-1);
                _loc_15 = _loc_16 == 0 ? (0) : (_loc_17 ? ((_loc_8.Z - _loc_9.Z) / _loc_16) : ((_loc_9.Z - _loc_8.Z) / _loc_16));
                _loc_20 = 1;
            }
            else if (Math.Abs(_loc_11.X - _loc_12.X) > Math.Abs(_loc_11.Y - _loc_12.Y))
            {
                _loc_16 = Math.Abs(_loc_11.X - _loc_12.X);
                _loc_13 = _loc_12.X > _loc_11.X ? (1) : (-1);
                _loc_14 = _loc_12.Y > _loc_11.Y ? (Math.Abs(_loc_11.Y - _loc_12.Y) == 0 ? (0) : (Math.Abs(_loc_11.Y - _loc_12.Y) / _loc_16)) : ((-Math.Abs(_loc_11.Y - _loc_12.Y)) / _loc_16);
                _loc_15 = _loc_16 == 0 ? (0) : (_loc_17 ? ((_loc_8.Z - _loc_9.Z) / _loc_16) : ((_loc_9.Z - _loc_8.Z) / _loc_16));
                _loc_20 = 2;
            }
            else
            {
                _loc_16 = Math.Abs(_loc_11.Y - _loc_12.Y);
                _loc_13 = _loc_12.X > _loc_11.X ? (Math.Abs(_loc_11.X - _loc_12.X) == 0 ? (0) : (Math.Abs(_loc_11.X - _loc_12.X) / _loc_16)) : ((-Math.Abs(_loc_11.X - _loc_12.X)) / _loc_16);
                _loc_14 = _loc_12.Y > _loc_11.Y ? (1) : (-1);
                _loc_15 = _loc_16 == 0 ? (0) : (_loc_17 ? ((_loc_8.Z - _loc_9.Z) / _loc_16) : ((_loc_9.Z - _loc_8.Z) / _loc_16));
                _loc_20 = 3;
            }
            _loc_10 = 0;
            while (_loc_10 < _loc_16)
            {
                if (_loc_20 == 2)
                {
                    if (Math.Floor(_loc_11.Y + _loc_14 / 2) == Math.Floor(_loc_11.Y + _loc_14 * 3 / 2))
                    {
                        _loc_19.Clear();
                        _loc_19.Add(Math.Floor(_loc_11.Y + _loc_14));
                        if (_loc_11.Y + _loc_14 / 2 == _loc_19[0] && _loc_11.Y + _loc_14 * 3 / 2 < _loc_19[0])
                        {
                            _loc_19.Clear();
                            _loc_19.Add(Math.Ceiling(_loc_11.Y + _loc_14 / 3));
                        }
                        else if (_loc_11.Y + _loc_14 / 2 == _loc_19[0] && _loc_11.Y + _loc_14 * 3 / 2 > _loc_19[0])
                        {
                            _loc_19.Clear();
                            _loc_19.Add(Math.Floor(_loc_11.Y + _loc_14));
                        }
                        else if (_loc_11.Y + _loc_14 * 3 / 2 == _loc_19[0] && _loc_11.Y + _loc_14 / 2 < _loc_19[0])
                        {
                            _loc_19.Clear();
                            _loc_19.Add(Math.Ceiling(_loc_11.Y + _loc_14));
                        }
                        else if (_loc_11.Y + _loc_14 * 3 / 2 == _loc_19[0] && _loc_11.Y + _loc_14 / 2 > _loc_19[0])
                        {
                            _loc_19.Clear();
                            _loc_19.Add(Math.Floor(_loc_11.Y + _loc_14));
                        }
                    }
                    else if (Math.Ceiling(_loc_11.Y + _loc_14 / 2) == Math.Ceiling(_loc_11.Y + _loc_14 * 3 / 2))
                    {
                        _loc_19.Clear();
                        _loc_19.Add(Math.Ceiling(_loc_11.Y + _loc_14));
                        if (_loc_11.Y + _loc_14 / 2 == _loc_19[0] && _loc_11.Y + _loc_14 * 3 / 2 < _loc_19[0])
                        {
                            _loc_19.Clear();
                            _loc_19.Add(Math.Floor(_loc_11.Y + _loc_14));
                        }
                        else if (_loc_11.Y + _loc_14 / 2 == _loc_19[0] && _loc_11.Y + _loc_14 * 3 / 2 > _loc_19[0])
                        {
                            _loc_19.Clear();
                            _loc_19.Add(Math.Ceiling(_loc_11.Y + _loc_14));
                        }
                        else if (_loc_11.Y + _loc_14 * 3 / 2 == _loc_19[0] && _loc_11.Y + _loc_14 / 2 < _loc_19[0])
                        {
                            _loc_19.Clear();
                            _loc_19.Add(Math.Floor(_loc_11.Y + _loc_14));
                        }
                        else if (_loc_11.Y + _loc_14 * 3 / 2 == _loc_19[0] && _loc_11.Y + _loc_14 / 2 > _loc_19[0])
                        {
                            _loc_19.Clear();
                            _loc_19.Add(Math.Ceiling(_loc_11.Y + _loc_14));
                        }
                    }
                    else
                    {
                        _loc_19.Clear();
                        _loc_19.Add(Math.Floor(_loc_11.Y + _loc_14 / 2));
                        _loc_19.Add(Math.Floor(_loc_11.Y + _loc_14 * 3 / 2));
                    }
                }
                else if (_loc_20 == 3)
                {
                    if (Math.Floor(_loc_11.X + _loc_13 / 2) == Math.Floor(_loc_11.X + _loc_13 * 3 / 2))
                    {
                        _loc_18.Clear();
                        _loc_18.Add(Math.Floor(_loc_11.X + _loc_13));
                        if (_loc_11.X + _loc_13 / 2 == _loc_18[0] && _loc_11.X + _loc_13 * 3 / 2 < _loc_18[0])
                        {
                            _loc_18.Clear();
                            _loc_18.Add(Math.Ceiling(_loc_11.X + _loc_13 / 3));
                        }
                        else if (_loc_11.X + _loc_13 / 2 == _loc_18[0] && _loc_11.X + _loc_13 * 3 / 2 > _loc_18[0])
                        {
                            _loc_18.Clear();
                            _loc_18.Add(Math.Floor(_loc_11.X + _loc_13));
                        }
                        else if (_loc_11.X + _loc_13 * 3 / 2 == _loc_18[0] && _loc_11.X + _loc_13 / 2 < _loc_18[0])
                        {
                            _loc_18.Clear();
                            _loc_18.Add(Math.Ceiling(_loc_11.X + _loc_13));
                        }
                        else if (_loc_11.X + _loc_13 * 3 / 2 == _loc_18[0] && _loc_11.X + _loc_13 / 2 > _loc_18[0])
                        {
                            _loc_18.Clear();
                            _loc_18.Add(Math.Floor(_loc_11.X + _loc_13));
                        }
                    }
                    else if (Math.Ceiling(_loc_11.X + _loc_13 / 2) == Math.Ceiling(_loc_11.X + _loc_13 * 3 / 2))
                    {
                        _loc_18.Clear();
                        _loc_18.Add(Math.Ceiling(_loc_11.X + _loc_13));
                        if (_loc_11.X + _loc_13 / 2 == _loc_18[0] && _loc_11.X + _loc_13 * 3 / 2 < _loc_18[0])
                        {
                            _loc_18.Clear();
                            _loc_18.Add(Math.Floor(_loc_11.X + _loc_13));
                        }
                        else if (_loc_11.X + _loc_13 / 2 == _loc_18[0] && _loc_11.X + _loc_13 * 3 / 2 > _loc_18[0])
                        {
                            _loc_18.Clear();
                            _loc_18.Add(Math.Ceiling(_loc_11.X + _loc_13));
                        }
                        else if (_loc_11.X + _loc_13 * 3 / 2 == _loc_18[0] && _loc_11.X + _loc_13 / 2 < _loc_18[0])
                        {
                            _loc_18.Clear();
                            _loc_18.Add(Math.Floor(_loc_11.X + _loc_13));
                        }
                        else if (_loc_11.X + _loc_13 * 3 / 2 == _loc_18[0] && _loc_11.X + _loc_13 / 2 > _loc_18[0])
                        {
                            _loc_18.Clear();
                            _loc_18.Add(Math.Ceiling(_loc_11.X + _loc_13));
                        }
                    }
                    else
                    {
                        _loc_18.Clear();
                        _loc_18.Add(Math.Floor(_loc_11.X + _loc_13 / 2));
                        _loc_18.Add(Math.Floor(_loc_11.X + _loc_13 * 3 / 2));
                    }
                }

                if (_loc_19.Count > 0)
                {
                    _loc_22 = 0;
                    while (_loc_22 < _loc_19.Count)
                    {

                        _loc_21 = new Point3d(Math.Floor(_loc_11.X + _loc_13), (double)_loc_19[(int)_loc_22]);
                        _loc_7.Add(_loc_21);
                        _loc_22 = _loc_22 + 1;
                    }
                }
                else if (_loc_18.Count > 0)
                {
                    _loc_23 = 0;
                    while (_loc_23 < _loc_18.Count)
                    {

                        _loc_21 = new Point3d((double)_loc_18[(int)_loc_23], Math.Floor(_loc_11.Y + _loc_14));
                        _loc_7.Add(_loc_21);
                        _loc_23 = _loc_23 + 1;
                    }
                }
                else
                {
                    _loc_21 = new Point3d(Math.Floor(_loc_11.X + _loc_13), Math.Floor(_loc_11.Y + _loc_14));
                    _loc_7.Add(_loc_21);
                }
                _loc_11.X = _loc_11.X + _loc_13;
                _loc_11.Y = _loc_11.Y + _loc_14;
                _loc_10++;
            }
            return _loc_7.Select(n => n.GetPoint()).ToArray();
        }

        #endregion

        #endregion

        #region Utils

        public static short ReadCell(short cell)
        {
            return (short)(cell & 4095);
        }

        public static short GetCellId(Point p)
        {
            LineOfSightUtil();
            short CellId = (short)((p.X - p.Y) * MAP_WIDTH + p.Y + (p.X - p.Y) / 2);
            return CellId;
        }

        public static Point GetPoint(short CellId)
        {//retourne un point correspondant a la CellId
            LineOfSightUtil();
            return _cellsPositions[CellId];
        }

        private static Point[] _cellsPositions = new Point[561];

        public static int GetCellXCoord(int CellId)
        {//horizontal
            int w = 15;
            return ((CellId - (w - 1) * GetCellYCoord(CellId)) / w);
        }

        public static int GetCellYCoord(int CellId)
        {//verticale
            int w = 15;
            int loc5 = (int)(CellId / ((w * 2) - 1));
            int loc6 = CellId - loc5 * ((w * 2) - 1);
            int loc7 = loc6 % w;
            return (loc5 - loc7);
        }

        #endregion

        #region GetDistance
        public static int GetDistanceBetween(short originCellId, short destinationCellId)
        {//distance (en pm) (manhatann)
            LineOfSightUtil();
            int diffX = Math.Abs(GetPoint(originCellId).X - GetPoint(destinationCellId).X);
            int diffY = Math.Abs(GetPoint(originCellId).Y - GetPoint(destinationCellId).Y);
            return (diffX + diffY);
        }

        public static int GetDistance(short originCellId, short destinationCellId)
        {//distance entre une celle et une autre (euclidienne)
            LineOfSightUtil();
            Point p1 = GetPoint(originCellId);
            Point p2 = GetPoint(destinationCellId);
            return Math.Abs(p1.X - p2.X) + Math.Abs(p1.Y - p2.Y);
        }

        public static int GetDistanceForPath(short originCellId, short destinationCellId)
        {
            return GetDistanceBetween(originCellId, destinationCellId) + GetDistance(originCellId, destinationCellId);
        }
        #endregion

        #region Load

        public static void LineOfSightUtil()
        {//charge les points
            DiagonalsCells.Clear();
            for (int i = 0; i <= (MAP_HEIGHT * 28); i += 28)
            {
                for (int k = i; k <= i + MAP_WIDTH; k++)
                {
                    DiagonalsCells.Add((short)k, new Point((k - i) * 2, i / 14));
                }
            }

            int _loc_4, _loc_1, _loc_2, _loc_3, _loc_5;
            _loc_1 = _loc_2 = _loc_3 = _loc_4 = _loc_5 = 0;

            while (_loc_5 < MAP_HEIGHT)
            {
                _loc_4 = 0;
                while (_loc_4 < MAP_WIDTH)
                {
                    _cellsPositions[_loc_3] = new Point(_loc_1 + _loc_4, _loc_2 + _loc_4);
                    _loc_3++;
                    _loc_4++;
                }

                _loc_1++;
                _loc_4 = 0;
                while (_loc_4 < MAP_WIDTH)
                {
                    _cellsPositions[_loc_3] = new Point(_loc_1 + _loc_4, _loc_2 + _loc_4);
                    _loc_3++;
                    _loc_4++;
                }
                _loc_2--;
                _loc_5++;
            }
        }

        #endregion
    }

    #region Point3d
    class Point3d
    {
        public double X { get; set; }
        public double Y { get; set; }
        public double Z { get; set; }

        public Point3d(double x, double y)
            : this(x, y, 0)
        {

        }

        public Point3d(double x, double y, double z)
        {
            this.X = x;
            this.Y = y;
            this.Z = z;
        }

        public override string ToString()
        {
            return X + "," + Y;
        }

        public System.Drawing.Point GetPoint()
        {
            return new System.Drawing.Point((int)this.X, (int)this.Y);
        }
    }
    #endregion
}
