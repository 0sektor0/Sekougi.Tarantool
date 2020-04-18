using System;
using Sekougi.Tarantool.Model.Enums;

namespace Sekougi.Tarantool.Model
{
    public class ModelUtils
    {
        public static IndexTypeE GetIndexTypeFromString(string indexTypeString)
        {
            switch (indexTypeString)
            {
                case "tree":
                    return IndexTypeE.Tree;
                
                case "hash":
                    return IndexTypeE.Hash;
                
                case "bitset":
                    return IndexTypeE.Bitset;
                
                case "rtree":
                    return IndexTypeE.RTree;
                
                default:
                    throw new InvalidCastException($"unrecognized index type: {indexTypeString}");
            }
        }
    }
}