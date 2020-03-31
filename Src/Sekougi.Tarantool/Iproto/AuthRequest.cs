using Sekougi.MessagePack;
using System;
using System.Text;


namespace Sekougi.Tarantool.Iproto
{
    public class AuthRequest : IRequest
    {
        private const int IPROTO_USER_NAME_KEY = 0x23;
        private const string AUTH_MECHANISM = "chap-sha1";

        private string _userName;
        private byte[] _scramble;
        private int _syncId;
        
        public RequestCode Code => RequestCode.Auth;


        public AuthRequest(string userName, string password, ReadOnlySpan<byte> base64Salt)
        {
            _userName = userName;
            _scramble = CalculateScramble(base64Salt, password);
        }
        
        public void Serialize(MessagePackWriter writer)
        {
            writer.WriteDictionaryHeader(1);
            writer.Write(_userName, Encoding.UTF8);
            
            writer.WriteArrayHeader(2);
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