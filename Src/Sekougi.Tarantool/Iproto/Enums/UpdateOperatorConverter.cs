using System.Collections.Generic;



namespace Sekougi.Tarantool.Iproto.Enums
{
    public static class UpdateOperatorConverter
    {
        public const string DELETION_CODE = "#";
        public const string SPLICE_CODE = ":";
        
        private static Dictionary<UpdateOperatorE, string> _operationsStrings;


        static UpdateOperatorConverter()
        {
            _operationsStrings = new Dictionary<UpdateOperatorE, string>
            {
                {UpdateOperatorE.Subtraction, "-"},
                {UpdateOperatorE.Addition, "+"},
                {UpdateOperatorE.And, "&"},
                {UpdateOperatorE.Or, "|"},
                {UpdateOperatorE.Xor, "^"},
                {UpdateOperatorE.Insertion, "!"},
                {UpdateOperatorE.Assignment, "="},
            };
        }
        
        public static string ToString(UpdateOperatorE updateOperator)
        {
            return _operationsStrings[updateOperator];
        }
    }
}