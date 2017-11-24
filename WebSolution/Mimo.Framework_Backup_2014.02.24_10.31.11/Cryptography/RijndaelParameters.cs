namespace Mimo.Framework.Cryptography
{
    public class RijndaelParameters
    {
        private byte [] _key;
        private byte[] _iv;

        /// <summary>
        /// Gets or sets the key for the symmetric algorithm
        /// </summary>
        public byte[] Key
        {
            get { return _key; }
            set { _key = value; }
        }

        /// <summary>
        /// Gets or sets the initialization vector for
        /// the symmetric algorithm.
        /// </summary>
        public byte[] IV
        {
            get { return _iv; }
            set { _iv = value; }
        }
    }
}
