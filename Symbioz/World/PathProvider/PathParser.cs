using Symbioz.Enums;
using System;
using System.Collections.Generic;

namespace Symbioz.World.PathProvider
{
    public class PathParser
    {
        public static short ReadCell(short cell)
        {
            return (short)(cell & 4095);
        }
        public static sbyte GetDirection(int result)
        {
            return (sbyte)(result >> 12);
        }
        public static Dictionary<short, DirectionsEnum> ReturnDispatchedCells(List<short> keys)
        {
            var cells = new Dictionary<short, DirectionsEnum>();
            keys.ForEach(x => cells.Add((short)(x & 4095), (DirectionsEnum)(x >> 12)));
            return cells;
        }
        public static int GetCellXCoord(int cellid)
        {
            int num = 15;
            return checked(cellid - (num - 1) *GetCellYCoord(cellid)) / num;
        }
        public static int GetCellYCoord(int cellid)
        {
            int num = 15;
            checked
            {
                int num2 = cellid / (num * 2 - 1);
                int num3 = cellid - num2 * (num * 2 - 1);
                int num4 = num3 % num;
                return num2 - num4;
            }
        }

    }
}
