using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Security.Cryptography;
using System.Collections;

namespace Psl.Chase.Utils
{
    /// <summary>
    /// Summary description for RSA.
    /// </summary>
    public class RSA : IDisposable
    {
        private RSACryptoServiceProvider _rsa = null;
        private string _publicKey = String.Empty;
        private string _publicPrivateKey = String.Empty;
        /// <summary>
        /// Optimal Asymmetric Encryption Padding. It is only supported on Windows XP and later operating System
        /// </summary>
        private bool _oaep = false;
        private int _keySize = 1024;
        UnicodeEncoding _utf16Format = new UnicodeEncoding();
        public RSA()
        {
        }

        public RSA(bool oaep)
        {
            _oaep = oaep;
        }

        public RSA(int keySize)
        {
            if (this.CheckKeySize(keySize))
                this._keySize = keySize;
            else
                throw new CryptographicException("Invalid KeySize. The valid key size is 384 - 16,384 with skip size 8");
        }

        public RSA(bool oaep, int keySize)
        {
            _oaep = oaep;
            if (this.CheckKeySize(keySize))
                this._keySize = keySize;
            else
                throw new CryptographicException("Invalid KeySize. The valid key size is 384 - 16,384 with skip size 8");
        }
        /// <summary>
        /// Public Key and Public-Private Keys will be stored
        /// </summary>
        //public void GenerateKeys()
        //{
        //    RSACryptoServiceProvider rsa = new RSACryptoServiceProvider(_keySize);
        //    _publicKey = rsa.ToXmlString(false);
        //    _publicPrivateKey = rsa.ToXmlString(true);	
        //    rsa.Clear();

        //}

        public string EncryptString(string inputString, int dwKeySize, string xmlString)
        {
            dwKeySize = _keySize;
            // TODO: Add Proper Exception Handlers
            RSACryptoServiceProvider rsaCryptoServiceProvider = new RSACryptoServiceProvider(dwKeySize);
            rsaCryptoServiceProvider.FromXmlString(xmlString);
            int keySize = dwKeySize / 8;
            byte[] bytes = Encoding.UTF32.GetBytes(inputString);
            // The hash function in use by the .NET RSACryptoServiceProvider here is SHA1
            // int maxLength = ( keySize ) - 2 - ( 2 * SHA1.Create().ComputeHash( rawBytes ).Length );
            int maxLength = keySize - 42;
            int dataLength = bytes.Length;
            int iterations = dataLength / maxLength;
            StringBuilder stringBuilder = new StringBuilder();
            for (int i = 0; i <= iterations; i++)
            {
                byte[] tempBytes = new byte[(dataLength - maxLength * i > maxLength) ? maxLength : dataLength - maxLength * i];
                Buffer.BlockCopy(bytes, maxLength * i, tempBytes, 0, tempBytes.Length);
                byte[] encryptedBytes = rsaCryptoServiceProvider.Encrypt(tempBytes, true);
                // Be aware the RSACryptoServiceProvider reverses the order of encrypted bytes after encryption and before decryption.
                // If you do not require compatibility with Microsoft Cryptographic API (CAPI) and/or other vendors.
                // Comment out the next line and the corresponding one in the DecryptString function.
                Array.Reverse(encryptedBytes);
                // Why convert to base 64?
                // Because it is the largest power-of-two base printable using only ASCII characters
                stringBuilder.Append(Convert.ToBase64String(encryptedBytes));
            }
            return stringBuilder.ToString();
        }

        public string DecryptString(string inputString, int dwKeySize, string xmlString)
        {
            try
            {
                dwKeySize = _keySize;
                //  xmlString = this.PublicKey;
                // TODO: Add Proper Exception Handlers
                RSACryptoServiceProvider rsaCryptoServiceProvider = new RSACryptoServiceProvider(dwKeySize);
                rsaCryptoServiceProvider.FromXmlString(xmlString);
                int base64BlockSize = ((dwKeySize / 8) % 3 != 0) ? (((dwKeySize / 8) / 3) * 4) + 4 : ((dwKeySize / 8) / 3) * 4;
                int iterations = inputString.Length / base64BlockSize;
                ArrayList arrayList = new ArrayList();
                for (int i = 0; i < iterations; i++)
                {
                    byte[] encryptedBytes = Convert.FromBase64String(inputString.Substring(base64BlockSize * i, base64BlockSize));
                    // Be aware the RSACryptoServiceProvider reverses the order of encrypted bytes after encryption and before decryption.
                    // If you do not require compatibility with Microsoft Cryptographic API (CAPI) and/or other vendors.
                    // Comment out the next line and the corresponding one in the EncryptString function.
                    Array.Reverse(encryptedBytes);
                    arrayList.AddRange(rsaCryptoServiceProvider.Decrypt(encryptedBytes, true));
                }
                return Encoding.UTF32.GetString(arrayList.ToArray(Type.GetType("System.Byte")) as byte[]);
            }
            catch (Exception ex)
            {
                string msg = ex.Message;
                string stackTrace = ex.StackTrace;
                string total = msg + stackTrace;
                return string.Empty;
            }
        }
        public void GenerateKeys()
        {
            RSACryptoServiceProvider RSAProvider = new RSACryptoServiceProvider();
            _publicPrivateKey = RSAProvider.ToXmlString(true);
            _publicKey = RSAProvider.ToXmlString(false);
        }

        //public string RSAEncrypt(string data, string key)
        //{
        //    try
        //    {				
        //        byte[] dataByte = this.GetBytes(data, key);
        //        byte[] cipherByte = _rsa.Encrypt(dataByte, _oaep);
        //        return Convert.ToBase64String(cipherByte); 			
        //    }
        //    catch 
        //    {
        //        throw;	 
        //    }

        //}

        //public string RSADecrypt(string cipher, string key)
        //{
        //    try
        //    {
        //        byte[] cipherByte = this.GetBytes(cipher, key);
        //        byte[] dataByte = _rsa.Decrypt(cipherByte, _oaep);			
        //        return  Convert.ToBase64String(cipherByte);	
        //    }
        //    catch (Exception ex)
        //    {
        //        System.Windows.Forms.MessageBox.Show(ex.Message);
        //        return null;
        //    }

        //}

        /// <summary>
        /// The data will be encrypted block vise
        /// </summary>
        /// <param name="data"></param>
        /// <param name="key"></param>
        /// <returns></returns>
        //public string RSAEncryptBlock(string data, string key)
        //{
        //    byte[] dataByte = this.GetBytes(data, key);
        //    byte[] encoded;
        //    //Determine the optimum block size for encryption.
        //    int blockSize = 0;
        //    if (_rsa.KeySize == 1024)
        //    {
        //        //High encryption capabilities are in place.
        //        blockSize = 16;
        //    }
        //    else
        //    {
        //        //High encryption capabilities are not in place.
        //        blockSize = 5;
        //    }
        //    //Create the memory stream where the enrypted data will be created
        //    using( MemoryStream  ms = new MemoryStream())
        //    {
        //        byte[] rawBlock, encryptedBlock;
        //        for(int i=0 ; i < dataByte.Length ; i += blockSize)
        //        {
        //            if ((dataByte.Length - i) > blockSize)
        //            {
        //                rawBlock = new byte[blockSize];
        //            }
        //            else
        //            {
        //                rawBlock = new byte[dataByte.Length - i];
        //            }
        //            //Copy a block of data
        //            Buffer.BlockCopy(dataByte, i, rawBlock, 0, rawBlock.Length);

        //            //Encrypt the block of data.
        //            encryptedBlock = _rsa.Encrypt(rawBlock, _oaep);

        //            //Write the block of data
        //            ms.Write(encryptedBlock, 0, encryptedBlock.Length);

        //        }

        //        ms.Position = 0;
        //        encoded = new byte[ms.Length];	
        //        ms.Read(encoded, 0, encoded.Length);
        //    }
        //    return  Convert.ToBase64String(encoded, 0, encoded.Length);
        //}
        /// <summary>
        /// The data will be decrypted block vise
        /// </summary>
        /// <param name="data"></param>
        /// <param name="key"></param>
        /// <returns></returns>
        //public string RSADecryptBlock(string data, string key)
        //{
        //    byte[] dataByte = this.GetBytes(data, key);
        //    byte[] decoded ;
        //    //Determine the bloack size for decrypting
        //    int keySize = _rsa.KeySize / 8;
        //    //Create the memory stream where the decrypted data will be created
        //    using( MemoryStream  ms = new MemoryStream())
        //    {
        //        byte[] rawBlock, decryptedBlock;
        //        for(int i=0 ; i < dataByte.Length ; i += keySize)
        //        {
        //            if ((dataByte.Length - i) > keySize)
        //            {
        //                rawBlock = new byte[keySize];
        //            }
        //            else
        //            {
        //                rawBlock = new byte[dataByte.Length - i];
        //            }
        //            //Copy a block of data
        //            Buffer.BlockCopy(dataByte, i, rawBlock, 0, rawBlock.Length);

        //            //Decrypt the block of data.
        //            decryptedBlock = _rsa.Decrypt(rawBlock, _oaep);

        //            //Write the block of data
        //            ms.Write(decryptedBlock, 0, decryptedBlock.Length);

        //        }

        //        ms.Position = 0;
        //        decoded = new byte[ms.Length];	
        //        ms.Read(decoded, 0, decoded.Length);
        //    }
        //    return Convert.ToBase64String(decoded, 0, decoded.Length); 


        //}


        //public string PublicKey
        //{
        //    get{return this._publicKey;}
        //}

        //public string PublicPrivateKey
        //{
        //    get{return this._publicPrivateKey;}
        //}

        /// <summary>
        /// Get/Set Optimal Asymmetric Encryption Padding.
        /// It is only supported on Windows XP and later operating System.
        /// Default value is false
        /// </summary>
        //public bool OAEP
        //{
        //    set{this._oaep = value;}
        //    get{return this._oaep;}
        //}

        /// <summary>
        /// Get/Set the Key Size.
        /// The valid key size is 384 - 16,384(in 8 bit inc)
        /// </summary>
        //public int KeySize
        //{
        //    set
        //    {
        //        if (this.CheckKeySize(value))
        //            this._keySize = value;				
        //        else
        //            throw new CryptographicException("Invalid KeySize. The valid key size is 364 - 16,384");
        //    }
        //    get{return this._keySize;}
        //} 

        /// <summary>
        /// Checking whether key size is valid or not
        /// </summary>
        /// <param name="size"></param>
        /// <returns></returns>
        private bool CheckKeySize(int size)
        {
            if ((size >= 384) && (size <= 16384))
            {
                if (size % 8 == 0)
                    return true;
                else
                    return false;
            }
            else
                return false;
        }

        /// <summary>
        /// Creates a new instance of a RSACryptoServiceProvider if not present
        /// Load a key
        /// Convert string to bytes using UTF16Format
        /// </summary>
        /// <param name="data"></param>
        /// <param name="key"></param>
        /// <returns></returns>
        //private byte[] GetBytes(string data, string key)
        //{
        //    if (_rsa == null)
        //        _rsa = new RSACryptoServiceProvider(_keySize);
        //    _rsa.FromXmlString(key);
        //    return Encoding.Unicode.GetBytes(data);


        //}
        #region IDisposable Members

        public void Dispose()
        {
            // TODO:  Add RSA.Dispose implementation
            if (this._rsa != null)
            {
                this._rsa.Clear();
            }
        }

        #endregion
    }
}
