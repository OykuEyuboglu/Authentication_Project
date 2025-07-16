using Microsoft.AspNetCore.Cryptography.KeyDerivation;
using System.Security.Cryptography;

namespace authentication_project.Handlers
{
    public class PasswordHashHandler
    {
        private static int _iterationCount = 100000;
        private static RandomNumberGenerator _randomNumberGenerator = RandomNumberGenerator.Create();

        public static string HashPassword(string password)
        {
            int saltSize = 128 / 8;
            var salt = new byte[saltSize];
            _randomNumberGenerator.GetBytes(salt);

            var subkey = KeyDerivation.Pbkdf2(password, salt, KeyDerivationPrf.HMACSHA512, _iterationCount, 256 / 8);
            var outputBytes = new byte[13 + salt.Length + subkey.Length];

            outputBytes[0] = 0x01;

            WriteNetworkByteOrder(outputBytes, 1, (uint)KeyDerivationPrf.HMACSHA512);
            WriteNetworkByteOrder(outputBytes, 5, (uint)_iterationCount);
            WriteNetworkByteOrder(outputBytes, 9, (uint)saltSize);
            Buffer.BlockCopy(salt, 0, outputBytes, 13, salt.Length);
            Buffer.BlockCopy(subkey, 0, outputBytes, 13 + saltSize, subkey.Length);

            return Convert.ToBase64String(outputBytes);
        }

        public static bool VerifyPassword(string password, string hash)
        {
            try
            {
                var hashedPassword = Convert.FromBase64String(hash);

                if (hashedPassword.Length < 13) return false;


                var keyDerivationPrf = (KeyDerivationPrf)ReadNetworkByteOrder(hashedPassword, 1);
                var iterationCount = (int)ReadNetworkByteOrder(hashedPassword, 5);
                var saltLength = (int)ReadNetworkByteOrder(hashedPassword, 9);

                if (saltLength < 16) return false;

                var salt = new byte[saltLength];
                Buffer.BlockCopy(hashedPassword, 13, salt, 0, saltLength);
                var subkey = new byte[256 / 8];
                Buffer.BlockCopy(hashedPassword, 13 + saltLength, subkey, 0, subkey.Length);

                var expectedSubkey = KeyDerivation.Pbkdf2(password, salt, keyDerivationPrf, iterationCount, subkey.Length);
                return CryptographicOperations.FixedTimeEquals(subkey, expectedSubkey);
            }
            catch
            {
                return false;
            }
        }

        private static void WriteNetworkByteOrder(byte[] bytes, int offset, uint value)
        {
            bytes[offset] = (byte)(value >> 24);
            bytes[offset + 1] = (byte)(value >> 16);
            bytes[offset + 2] = (byte)(value >> 8);
            bytes[offset + 3] = (byte)(value);
        }

        private static uint ReadNetworkByteOrder(byte[] bytes, int offset)
        {
            return (uint)bytes[offset] << 24 |
                   (uint)bytes[offset + 1] << 16 |
                   (uint)bytes[offset + 2] << 8 |
                   (uint)bytes[offset + 3];
        }
    }
}

