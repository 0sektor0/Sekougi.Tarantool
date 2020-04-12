using Sekougi.MessagePack;
using System;
using System.Text;
using Sekougi.Tarantool.Iproto.Enums;


namespace Sekougi.Tarantool.Iproto.Requests
{
    public class AuthRequest : RequestBase
    {
        private const int USER_NAME_KEY = 0x23;
        private const int AUTH_TUPLE_KEY = 0x21;
        private const string AUTH_MECHANISM = "chap-sha1";

        private string _userName;
        private byte[] _scramble;
        
        public override CommandE Code => CommandE.Auth;


        public AuthRequest(string userName, string password, ReadOnlySpan<byte> base64Salt)
        {
            _userName = userName;
            _scramble = CalculateScramble(base64Salt, password);
        }
        
        protected override void SerializeBody(MessagePackWriter writer)
        {
            writer.WriteDictionaryLength(2);
            
            // username 
            writer.Write(USER_NAME_KEY);
            writer.Write(_userName, Encoding.UTF8);
            
            // tuple with auth mechanism and scramble 
            writer.Write(AUTH_TUPLE_KEY);
            writer.WriteArrayLength(2);
            writer.Write(AUTH_MECHANISM, Encoding.UTF8);
            writer.WriteBinary(_scramble);
        }

        private static byte[] CalculateScramble(ReadOnlySpan<byte> base64Salt, string password)
        {
            var encodedSalt = Encoding.ASCII.GetString(base64Salt);
            var decodedSalt = Convert.FromBase64String(encodedSalt);
            
            var first20SaltBytes = new byte[20];
            Array.Copy(decodedSalt, first20SaltBytes, 20);

            var step1 = Sha1Utils.Hash(password);
            var step2 = Sha1Utils.Hash(step1);
            var step3 = Sha1Utils.Hash(first20SaltBytes, step2);
            var scrambleBytes = Sha1Utils.Xor(step1, step3);

            return scrambleBytes;
        }
    }
}