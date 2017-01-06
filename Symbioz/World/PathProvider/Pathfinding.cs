using Symbioz.Enums;
using Symbioz.World.Records;
using Symbioz.World.Records.Items;
using System;
using System.Collections.Generic;
using System.Linq;
using _cell = Symbioz.World.PathProvider.CoordCells.CellData;

namespace Symbioz.World.PathProvider
{
    internal class Pathfinding
    {
        public static List<_cell> FIXEDCELLS = new List<_cell>();

        static Pathfinding()
        {
            FIXEDCELLS = CoordCells.Cells;
        }
        public static List<short> GetCircleCells(short start, short radius)
        {
            var cells = new List<CoordCells.CellData>();

            var center = FIXEDCELLS.FirstOrDefault(x => x.ID == start);
            center.Line.ForEach(x => cells.Add(x));

            for (var i = 0; i < radius - 1; i++)
            {
                var newCells = new List<CoordCells.CellData>();
                foreach (var cell in cells)
                {
                    cell.Line.ForEach(x =>
                    {
                        if (!cells.Contains(x) && !newCells.Contains(x))
                            newCells.Add(x);
                    });
                }
                cells.AddRange(newCells);
            }
            return cells.ConvertAll<short>(x => x.ID);
        }
        /// <summary>
        /// retourne chaque CellID du chemin
        /// </summary>
        /// <param name="cells">chemin concassé</param>
        public static Dictionary<short, DirectionsEnum> RolePlayMove(Dictionary<short, DirectionsEnum> cells)
        {
            var indexedCells = cells.Keys.ToList();
            var Cells = new Dictionary<short, DirectionsEnum>();

            for (var i = 0; i < cells.Count - 1; i++)
            {
                if (indexedCells[i] <= 0 || indexedCells[i] >= 559 || indexedCells[i + 1] <= 0 || indexedCells[i + 1] >= 559)
                    continue;

                var loc1 = FIXEDCELLS.FirstOrDefault(x => x.ID == indexedCells[i]);
                var loc2 = FIXEDCELLS.FirstOrDefault(x => x.ID == indexedCells[i + 1]);

                if (loc1 == null || loc2 == null)
                    continue;

                if (loc1.X - loc2.X == loc1.Y - loc2.Y && loc1.Y - loc2.Y < 0)
                {
                    for (var j = (short)(loc1.ID + 1); j <= loc2.ID; j++)
                        Cells.Add(j, DirectionsEnum.DIRECTION_EAST);
                }
                else if (loc1.X - loc2.X == loc1.Y - loc2.Y && loc1.Y - loc2.Y > 0)
                {
                    for (var j = (short)(loc1.ID - 1); j >= loc2.ID; j--)
                        Cells.Add(j, DirectionsEnum.DIRECTION_WEST);
                }
                else if (-(loc1.X - loc2.X) == loc1.Y - loc2.Y && loc1.X - loc2.X > 0)
                {
                    for (var j = (short)(loc1.ID - 28); j >= loc2.ID; j -= 28)
                        Cells.Add(j, DirectionsEnum.DIRECTION_NORTH);
                }
                else if (-(loc1.X - loc2.X) == loc1.Y - loc2.Y && loc1.X - loc2.X < 0)
                {
                    for (var j = (short)(loc1.ID + 28); j <= loc2.ID; j += 28)
                        Cells.Add(j, DirectionsEnum.DIRECTION_SOUTH);
                }
                else if (loc1.Y - loc2.Y == 0 && loc1.X - loc2.X < 0)
                {
                    for (var j = (short)(loc1.X + 1); j <= loc2.X; j++)
                    {
                        var celldata = FIXEDCELLS.FindAll(x => x.X == j).FirstOrDefault(y => y.Y == loc1.Y);
                        if (celldata != null)
                            Cells.Add(celldata.ID, DirectionsEnum.DIRECTION_SOUTH_EAST);
                    }
                }
                else if (loc1.X - loc2.X == 0 && loc1.Y - loc2.Y > 0)
                {
                    for (short j = (short)(loc1.Y - 1); j >= loc2.Y; j--)
                    {
                        var celldata = FIXEDCELLS.FindAll(x => x.Y == j).FirstOrDefault(y => y.X == loc1.X);
                        if (celldata != null)
                            Cells.Add(celldata.ID, DirectionsEnum.DIRECTION_SOUTH_WEST);
                    }
                }
                else if (loc1.X - loc2.X == 0 && loc1.Y - loc2.Y < 0)
                {
                    for (var j = (short)(loc1.Y + 1); j <= loc2.Y; j++)
                    {
                        var celldata = FIXEDCELLS.FindAll(x => x.Y == j).FirstOrDefault(y => y.X == loc1.X);
                        if (celldata != null)
                            Cells.Add(celldata.ID, DirectionsEnum.DIRECTION_NORTH_EAST);
                    }
                }
                else if (loc1.Y - loc2.Y == 0 && loc1.X - loc2.X > 0)
                {
                    for (var j = (short)(loc1.X - 1); j >= loc2.X; j--)
                    {
                        var celldata = FIXEDCELLS.FindAll(x => x.X == j).FirstOrDefault(y => y.Y == loc1.Y);
                        if (celldata != null)
                            Cells.Add(celldata.ID, DirectionsEnum.DIRECTION_NORTH_WEST);
                    }
                }
                else
                { Logger.Error("Error while dispatching a roleplay move..."); }
            }

            return Cells;
        }

        public static bool isOnLeftLine(short cell)
        {
            bool gauche = false;
            //0 a 14  -> gauche
            //15 a 27  -> droite
            //28 a 41 -> gauche
            for (int i = 546; i > -1; i -= 14)
            {
                if (i <= cell)
                    break;
                if (gauche)
                    gauche = false;
                else
                    gauche = true;
            }
            return gauche;
        }

        public static int getSeparatedNumberOfLines(short start, short end)
        {
            bool ok = false;
            //0 a 14  -> gauche
            //15 a 27  -> droite
            //28 a 41 -> gauche
            short tmp = 0;
            if (start > end)
            {
                tmp = start;
                start = end;
                end = tmp;
            }
            int line = 0;
            for (int i = 546; i > -1; i -= 14)
            {
                if (ok)
                    line++;
                if (i > end && (i - 14) < end)//si on et sur la ligne end
                    ok = true;
                if (i > start && (i - 14) < start)//si on et sur la ligne start
                {
                    ok = false;
                    break;
                }
            }
            return line;
        }

        public static short[] getCiblesByZoneByWeapon(MapRecord CurrentMap, WeaponRecord template, int cell, int castCellID)
        {
            short[] allCell = new short[4];

            bool gauche = false;
            //0 a 14  -> gauche
            //15 a 27  -> droite
            //28 a 41 -> gauche
            for (int i = 546; i > -1; i -= 14)
            {
                if (i <= castCellID)
                    break;
                if (gauche)
                    gauche = false;
                else
                    gauche = true;
            }

            char c = getDirBetweenTwoCase(castCellID, cell, 15, gauche);

            if (c == 0)
            {
                c = getDirBetweenTwoCase(castCellID, cell, 14, gauche);
            }
            if (c == 0)
            {
                //On cible quand meme le fighter sur la case
                allCell[0] = (short)cell;
                return allCell;
            }
            switch (template.TypeId)
            {
                //Cases devant celle ou l'on vise
                case 7://marteau
                    allCell[0] = (short)cell;//de base premier cell
                    switch (c)
                    {
                        case 'f'://hautgauche
                            if (gauche)
                            {
                                allCell[1] = (short)(cell - 14);
                                allCell[2] = (short)(cell - 13);
                                allCell[3] = (short)(castCellID - 1);
                            }
                            else
                            {
                                allCell[1] = (short)(cell - 15);
                                allCell[2] = (short)(cell - 14);
                                allCell[3] = (short)(castCellID - 1);
                            }
                            break;
                        case 'd'://basgauche
                            if (gauche)
                            {
                                allCell[1] = (short)(cell + 14);
                                allCell[2] = (short)(cell + 15);
                                allCell[3] = (short)(castCellID - 1);
                            }
                            else
                            {
                                allCell[1] = (short)(cell + 13);
                                allCell[2] = (short)(cell + 14);
                                allCell[3] = (short)(castCellID - 1);
                            }
                            break;
                        case 'h'://hautdroite
                            if (gauche)
                            {
                                allCell[1] = (short)(cell - 14);
                                allCell[2] = (short)(cell - 13);
                                allCell[3] = (short)(castCellID + 1);
                            }
                            else
                            {
                                allCell[1] = (short)(cell - 15);
                                allCell[2] = (short)(cell - 14);
                                allCell[3] = (short)(castCellID + 1);
                            }
                            break;
                        case 'b'://basdroite
                            if (gauche)
                            {
                                allCell[1] = (short)(cell + 14);
                                allCell[2] = (short)(cell + 15);
                                allCell[3] = (short)(castCellID + 1);
                            }
                            else
                            {
                                allCell[1] = (short)(cell + 14);
                                allCell[2] = (short)(cell + 13);
                                allCell[3] = (short)(castCellID + 1);
                            }
                            break;
                    }
                    break;
                case 4://baton
                    allCell[0] = (short)cell;//de base premier cell
                    switch (c)
                    {
                        case 'f'://hautgauche
                            if (gauche)
                            {
                                allCell[1] = (short)(cell - 13);
                                allCell[2] = (short)(castCellID - 1);
                            }
                            else
                            {
                                allCell[1] = (short)(cell - 14);
                                allCell[2] = (short)(castCellID - 1);
                            }
                            break;
                        case 'd'://basgauche
                            if (gauche)
                            {
                                allCell[1] = (short)(cell + 15);
                                allCell[2] = (short)(castCellID - 1);
                            }
                            else
                            {
                                allCell[1] = (short)(cell + 14);
                                allCell[2] = (short)(castCellID - 1);
                            }
                            break;
                        case 'h'://hautdroite
                            if (gauche)
                            {
                                allCell[1] = (short)(cell - 14);
                                allCell[2] = (short)(castCellID + 1);
                            }
                            else
                            {
                                allCell[1] = (short)(cell - 15);
                                allCell[2] = (short)(castCellID + 1);
                            }
                            break;
                        case 'b'://basdroite
                            if (gauche)
                            {
                                allCell[1] = (short)(cell + 14);
                                allCell[2] = (short)(castCellID + 1);
                            }
                            else
                            {
                                allCell[1] = (short)(cell + 13);
                                allCell[2] = (short)(castCellID + 1);
                            }
                            break;
                    }
                    break;
                case 8://pelle
                    allCell[0] = (short)cell;//de base premier cell
                    switch (c)
                    {
                        case 'f'://hautgauche
                            if (gauche)
                                allCell[1] = (short)(cell - 14);
                            else
                                allCell[1] = (short)(cell - 15);
                            break;
                        case 'd'://basgauche
                            if (gauche)
                                allCell[1] = (short)(cell + 14);
                            else
                                allCell[1] = (short)(cell + 13);
                            break;
                        case 'h'://hautdroite
                            if (gauche)
                                allCell[1] = (short)(cell - 13);
                            else
                                allCell[1] = (short)(cell - 14);
                            break;
                        case 'b'://basdroite
                            if (gauche)
                                allCell[1] = (short)(cell + 15);
                            else
                                allCell[1] = (short)(cell + 14);
                            break;
                    }
                    break;
                case 21://pioche
                case 6://epee
                case 22://faux
                case 5://dagues
                case 3://baguette
                case 2://arc
                case 19://hache
                case 20://outils
                    allCell[0] = (short)cell;
                    break;
            }
            return allCell;
        }

        public static char getDirBetweenTwoCase(int cell1ID, int cell2ID, int line, bool gauche)
        {
            List<char> dirs = new List<char>();
            dirs.Add('b');
            dirs.Add('d');
            dirs.Add('f');
            dirs.Add('h');

            foreach (char c in dirs)
            {
                int cell = cell1ID;
                for (int i = 0; i <= 64; i++)
                {
                    if (GetCaseIDFromDirrection(cell, c, line) == cell2ID)
                    {
                        switch (c)
                        {
                            case 'h':
                                switch (line)
                                {
                                    case 15:
                                        if (!gauche)
                                            return ('f');
                                        break;
                                }
                                break;
                            case 'd':
                                switch (line)
                                {
                                    case 15:
                                        if (gauche)
                                            return ('b');
                                        break;
                                }
                                break;
                        }
                        return c;
                    }
                    cell = GetCaseIDFromDirrection(cell, c, line);
                }
            }
            return '\0';
        }

        public static int GetCaseIDFromDirrection(int CaseID, char Direction, int line)
        {
            switch (Direction)
            {
                case 'b':
                    return CaseID + line;
                case 'd':
                    return CaseID + (line - 1);
                case 'f':
                    return CaseID - line;
                case 'h':
                    return CaseID - line + 1;
            }
            return -1;
        }

        public static Dictionary<short, DirectionsEnum> FightMove(Dictionary<short, DirectionsEnum> cells)
        {
            var indexedCells = cells.Keys.ToList();
            var Cells = new Dictionary<short, DirectionsEnum>();

            for (var i = 0; i < cells.Count - 1; i++)
            {
                if (indexedCells[i] <= 0 || indexedCells[i] >= 559 || indexedCells[i + 1] <= 0 || indexedCells[i + 1] >= 559)
                    continue;

                var loc1 = FIXEDCELLS.FirstOrDefault(x => x.ID == indexedCells[i]);
                var loc2 = FIXEDCELLS.FirstOrDefault(x => x.ID == indexedCells[i + 1]);

                if (loc1 == null || loc2 == null)
                    continue;

                if (loc1.Y - loc2.Y == 0 && loc1.X - loc2.X < 0)
                {
                    for (var j = (short)(loc1.X + 1); j <= loc2.X; j++)
                    {
                        var celldata = FIXEDCELLS.FindAll(x => x.X == j).FirstOrDefault(y => y.Y == loc1.Y);
                        if (celldata != null)
                            Cells.Add(celldata.ID, DirectionsEnum.DIRECTION_SOUTH_EAST);
                    }
                }
                else if (loc1.X - loc2.X == 0 && loc1.Y - loc2.Y > 0)
                {
                    for (short j = (short)(loc1.Y - 1); j >= loc2.Y; j--)
                    {
                        var celldata = FIXEDCELLS.FindAll(x => x.Y == j).FirstOrDefault(y => y.X == loc1.X);
                        if (celldata != null)
                            Cells.Add(celldata.ID, DirectionsEnum.DIRECTION_SOUTH_WEST);
                    }
                }
                else if (loc1.X - loc2.X == 0 && loc1.Y - loc2.Y < 0)
                {
                    for (var j = (short)(loc1.Y + 1); j <= loc2.Y; j++)
                    {
                        var celldata = FIXEDCELLS.FindAll(x => x.Y == j).FirstOrDefault(y => y.X == loc1.X);
                        if (celldata != null)
                            Cells.Add(celldata.ID, DirectionsEnum.DIRECTION_NORTH_EAST);
                    }
                }
                else if (loc1.Y - loc2.Y == 0 && loc1.X - loc2.X > 0)
                {
                    for (var j = (short)(loc1.X - 1); j >= loc2.X; j--)
                    {
                        var celldata = FIXEDCELLS.FindAll(x => x.X == j).FirstOrDefault(y => y.Y == loc1.Y);
                        if (celldata != null)
                            Cells.Add(celldata.ID, DirectionsEnum.DIRECTION_NORTH_WEST);
                    }
                }
                else
                { Console.WriteLine("ERROR !"); }
            }

            return Cells;
        }

  

      
        public static _cell GetCell(short cell)
        {
            return FIXEDCELLS.FirstOrDefault(x => x.ID == cell);
        }

     
        public static List<_cell> GetRectangleCells(_cell start, _cell end)
        {
            int minX = Math.Min(start.X, end.X),
                minY = Math.Min(start.Y, end.Y),
                maxX = Math.Max(start.X, end.X),
                maxY = Math.Max(start.Y, end.Y);

            var cells = new List<_cell>();
            for (var x = minX; x <= maxX; x++)
            {
                for (var y = minY; y <= maxY; y++)
                {
                    var cell = CoordCells.GetCell(x, y);
                    if (cell != null)
                        cells.Add(cell);
                }
            }
            return cells;
        }


        public static List<Point> GetLine(int param1, int param2, int param3, int param4, int param5, int param6)
        {
            List<Point> loc7 = new List<Point>();
            var loc8 = new Point3D(param1, param2, param3);
            var loc9 = new Point3D(param4, param5, param6);

            var loc11 = new Point3D(loc8.x + 0.5, loc8.y + 0.5, loc8.z);
            var loc12 = new Point3D(loc9.x + 0.5, loc9.y + 0.5, loc9.z);
            var loc13 = 0.0;
            var loc14 = 0.0;
            var loc15 = 0.0;
            var loc16 = 0.0;
            var loc17 = loc11.z < loc12.z;
            List<double> loc18;
            List<double> loc19;
            var loc20 = 0;
            Point loc21 = null;
            var loc22 = 0;
            var loc23 = 0;
            var loc24 = double.NaN;
            var loc25 = double.NaN;
            var loc26 = double.NaN;
            var loc27 = double.NaN;
            var loc28 = double.NaN;
            var loc29 = double.NaN;
            var loc30 = double.NaN;
            var loc31 = double.NaN;
            if (Math.Abs(loc11.x - loc12.x) == Math.Abs(loc11.y - loc12.y)) // horizontal && vertical
            {
                loc16 = Math.Abs(loc11.x - loc12.x);
                loc13 = loc12.x > loc11.x ? 1 : -1;
                loc14 = loc12.y > loc11.y ? 1 : -1;
                loc15 = loc16 == 0 ? 0 : (loc17 ? ((loc8.z - loc9.z) / loc16) : ((loc9.z - loc8.z) / loc16));
                loc20 = 1;
            }
            else if (Math.Abs(loc11.x - loc12.x) > Math.Abs(loc11.y - loc12.y)) // bas de l'écran
            {
                loc16 = Math.Abs(loc11.x - loc12.x);
                loc13 = loc12.x > loc11.x ? 1 : -1;
                loc14 = loc12.y > loc11.y ? (Math.Abs(loc11.y - loc12.y) == 0 ? 0 : (Math.Abs(loc11.y - loc12.y) / loc16)) : ((-Math.Abs(loc11.y - loc12.y)) / loc16);
                loc14 *= 100;
                loc14 = Math.Ceiling(loc14) / 100;
                loc15 = loc16 == 0 ? 0 : (loc17 ? ((loc8.z - loc9.z) / loc16) : ((loc9.z - loc8.z) / loc16));
                loc20 = 2;
            }
            else // haut de l'écran
            {
                loc16 = Math.Abs(loc11.y - loc12.y);
                loc13 = loc12.x > loc11.x ? (Math.Abs(loc11.x - loc12.x) == 0 ? 0 : (Math.Abs(loc11.x - loc12.x) / loc16)) : ((-Math.Abs(loc11.x - loc12.x)) / loc16);
                loc13 *= 100;
                loc13 = Math.Ceiling(loc13) / 100;
                loc14 = loc12.y > loc11.y ? 1 : -1;
                loc15 = loc16 == 0 ? 0 : (loc17 ? ((loc8.z - loc9.z) / loc16) : ((loc9.z - loc8.z) / loc16));
                loc20 = 3;
            }

            for (var i = 0; i < loc16; i++)
            {
                loc18 = new List<double>();
                loc19 = new List<double>();

                loc22 = (int)(3 + loc16 / 2);
                loc23 = (int)(97 - loc16 / 2);

                if (loc20 == 2)
                {
                    loc24 = Math.Ceiling(loc11.y * 100 + loc14 * 50) / 100;
                    loc25 = Math.Floor(loc11.y * 100 + loc14 * 150) / 100;
                    loc26 = Math.Floor(Math.Abs(Math.Floor(loc24) * 100 - loc24 * 100)) / 100;
                    loc27 = Math.Ceiling(Math.Abs(Math.Ceiling(loc25) * 100 - loc25 * 100)) / 100;

                    if (Math.Floor(loc24) == Math.Floor(loc25))
                    {
                        loc19 = new List<double> { Math.Floor(loc11.y + loc14) };

                        if (loc24 == loc19[0] && loc25 > loc19[0])
                            loc19 = new List<double> { Math.Ceiling(loc11.y + loc14) };
                        else if (loc24 == loc19[0] && loc25 < loc19[0])
                            loc19 = new List<double> { Math.Floor(loc11.y + loc14) };
                        else if (loc25 == loc19[0] && loc24 > loc19[0])
                            loc19 = new List<double> { Math.Ceiling(loc11.y + loc14) };
                        else if (loc25 == loc19[0] && loc24 < loc19[0])
                            loc19 = new List<double> { Math.Floor(loc11.y + loc14) };
                    }
                    else if (Math.Ceiling(loc24) == Math.Ceiling(loc25))
                    {
                        loc19 = new List<double> { Math.Ceiling(loc11.y + loc14) };

                        if (loc24 == loc19[0] && loc25 > loc19[0])
                            loc19 = new List<double> { Math.Floor(loc11.y + loc14) };
                        else if (loc24 == loc19[0] && loc25 < loc19[0])
                            loc19 = new List<double> { Math.Ceiling(loc11.y + loc14) };
                        else if (loc25 == loc19[0] && loc24 > loc19[0])
                            loc19 = new List<double> { Math.Floor(loc11.y + loc14) };
                        else if (loc25 == loc19[0] && loc24 < loc19[0])
                            loc19 = new List<double> { Math.Ceiling(loc11.y + loc14) };
                    }
                    else if (((int)loc26 * 100) <= loc22)
                        loc19 = new List<double> { Math.Floor(loc25) };
                    else if (((int)loc27 * 100) >= loc23)
                        loc19 = new List<double> { Math.Floor(loc24) };
                    else
                        loc19 = new List<double> { Math.Floor(loc24), Math.Floor(loc25) };
                }
                else if (loc20 == 3)
                {
                    loc28 = Math.Ceiling(loc11.x * 100 + loc13 * 50) / 100;
                    loc29 = Math.Floor(loc11.x * 100 + loc13 * 150) / 100;
                    loc30 = Math.Floor(Math.Abs(Math.Floor(loc28) * 100 - loc28 * 100)) / 100;
                    loc31 = Math.Ceiling(Math.Abs(Math.Ceiling(loc29) * 100 - loc29 * 100)) / 100;

                    if (Math.Floor(loc28) == Math.Floor(loc29))
                    {
                        loc18 = new List<double> { Math.Floor(loc11.x + loc13) };

                        if (loc28 == loc18[0] && loc29 > loc18[0])
                            loc18 = new List<double> { Math.Ceiling(loc11.x + loc13) };
                        else if (loc28 == loc18[0] && loc29 < loc18[0])
                            loc18 = new List<double> { Math.Floor(loc11.x + loc13) };
                        else if (loc29 == loc18[0] && loc28 > loc18[0])
                            loc18 = new List<double> { Math.Ceiling(loc11.x + loc13) };
                        else if (loc29 == loc18[0] && loc28 < loc18[0])
                            loc18 = new List<double> { Math.Floor(loc11.x + loc13) };
                    }
                    else if (Math.Ceiling(loc28) == Math.Ceiling(loc29))
                    {
                        loc18 = new List<double> { Math.Ceiling(loc11.x + loc13) };

                        if (loc28 == loc18[0] && loc29 > loc18[0])
                            loc18 = new List<double> { Math.Floor(loc11.x + loc13) };
                        else if (loc28 == loc18[0] && loc29 < loc18[0])
                            loc18 = new List<double> { Math.Ceiling(loc11.x + loc13) };
                        else if (loc29 == loc18[0] && loc28 > loc18[0])
                            loc18 = new List<double> { Math.Floor(loc11.x + loc13) };
                        else if (loc29 == loc18[0] && loc28 < loc18[0])
                            loc18 = new List<double> { Math.Ceiling(loc11.x + loc13) };
                    }
                    else if (((int)loc30 * 100) <= loc22)
                        loc18 = new List<double> { Math.Floor(loc29) };
                    else if (((int)loc31 * 100) >= loc23)
                        loc18 = new List<double> { Math.Floor(loc28) };
                    else
                        loc18 = new List<double> { Math.Floor(loc28), Math.Floor(loc29) };
                }

                if (loc19.Count > 0)
                {
                    for (var j = 0; j < loc19.Count; j++)
                    {
                        loc21 = new Point(Math.Floor(loc11.x + loc13), loc19[j]);
                        loc7.Add(loc21);
                    }
                }
                else if (loc18.Count > 0)
                {
                    for (var j = 0; j < loc18.Count; j++)
                    {
                        loc21 = new Point(loc18[j], Math.Floor(loc11.y + loc14));
                        loc7.Add(loc21);
                    }
                }
                else if (loc20 == 1)
                {
                    loc21 = new Point(Math.Floor(loc11.x + loc13), Math.Floor(loc11.y + loc14));
                    loc7.Add(loc21);
                }

                loc11.x = (loc11.x * 100 + loc13 * 100) / 100;
                loc11.y = (loc11.y * 100 + loc14 * 100) / 100;
            }

            return loc7;
        }

        public static int GetRange(int start, int end)
        {
            var startCell = CoordCells.GetCell(start);
            var endCell = CoordCells.GetCell(end);

            return Math.Abs(startCell.X - endCell.X) + Math.Abs(startCell.Y - endCell.Y);
        }
    }

    public class Point
    {
        public double x;
        public double y;

        public Point(double x, double y)
        {
            this.x = x;
            this.y = y;
        }
    }
    public class Point3D
    {
        public double x;
        public double y;
        public double z;

        public Point3D(double x, double y, double z)
        {
            this.x = x;
            this.y = y;
            this.z = z;
        }
    }
    
}
