using System;



namespace Sekougi.Tarantool.Model.Enums
{
    public static class IndexTypeConverter
    {
        public static IndexTypeE FromString(string indexTypeString)
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