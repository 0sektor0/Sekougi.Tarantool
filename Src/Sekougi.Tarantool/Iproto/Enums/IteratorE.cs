namespace Sekougi.Tarantool.Iproto.Enums
{
    public enum IteratorE
    {
        //Tree
        Eq,
        Req,
        Gt,
        Ge,
        All = 2,
        Lt,
        Le,
        
        //Bitset
        BitsAllSet,
        BitsAnySet,
        BitsAllNotSet,
        
        //Rtree
        Overlaps,
        Neighbor,
    }
}