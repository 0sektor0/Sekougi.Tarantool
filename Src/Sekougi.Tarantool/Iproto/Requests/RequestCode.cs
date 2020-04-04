namespace Sekougi.Tarantool.Iproto.Requests
{
    // To be continued
    public enum RequestCode
    {
        Ok = 0x00,
        Select = 0x01,
        Insert = 0x02,
        Replace = 0x03,
        Update = 0x04,
        Delete = 0x05,
        CallDeprecated = 0x06,
        Auth = 0x07,
        Eval = 0x08,
        Upsert = 0x09,
        Call = 0x0a,
        Execute = 0x0b,
        Nop = 0x0c,
        Prepare = 0x0d,
        Ping = 0x40,
        Join = 0x41,
        Subscribe = 0x42,
        VoteDeprecated = 0x43,
        Vote = 0x44,
        FetchSnapshot = 0x45,
        Register = 0x46
    }
}