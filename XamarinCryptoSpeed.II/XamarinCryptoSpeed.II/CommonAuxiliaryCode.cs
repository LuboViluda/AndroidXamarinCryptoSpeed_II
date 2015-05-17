using System;
using System.Linq;
using Java.Security;
using Android.Util;
using Java.Security.Cert;
using Javax.Crypto;

using System.IO;
using System.Security.Cryptography;

namespace XamarinCryptoSpeed
{
	public class CommonAuxiliaryCode
	{
		public static byte[] GenerateKey(int keyLength)
		{	// generate secure password 
			byte[] bytes = new byte[keyLength];
			byte[] salt = new byte[8];	// salt at least 8 bytes
			var rngCsp = new RNGCryptoServiceProvider();
			rngCsp.GetBytes(bytes);
			rngCsp.GetBytes(salt);
			var rfc = new Rfc2898DeriveBytes(bytes, salt, 8192);	//8192 - was used in Android to generate file enc. dec. passwords before scrypt
			return rfc.GetBytes(keyLength);
		}

		public static byte[] GetBytes(string str)
		{
			byte[] bytes = new byte[str.Length * sizeof(char)];
			System.Buffer.BlockCopy(str.ToCharArray(), 0, bytes, 0, bytes.Length);
			return bytes;
		}

		// generate dummy bytes
		public static void GenerateDummyBytes(byte[] array)
		{
			Random random = new Random();
			random.NextBytes (array);
		}

		public static Boolean CmpByteArrayShowResult(byte[] array1, byte[] array2, String comparedAlg)
		{
			return Enumerable.SequenceEqual(array1, array2);
		}

		public static void WriteToFile(String filename, String data) 
		{
			try 
			{
				File.AppendAllText("/storage/emulated/0/Xamarin_" + filename, data);
			}
			catch (IOException e) 
			{
				Log.Error("Exception", "File write failed: " + e.ToString());
			}
		}

		public static KeyStore.PrivateKeyEntry GetPrivateKeyEntry(String alias)  
		{
			try
			{
				KeyStore ks = KeyStore.GetInstance("AndroidKeyStore");
				ks.Load(null);
				return (KeyStore.PrivateKeyEntry) ks.GetEntry(alias, null);
			}
			catch(KeyStoreException e)
			{
				Log.Error ("Exception", "File write failed: " + e);
			}
			catch(IOException e)
			{
				Log.Error ("Exception", "File write failed: " + e);
			}
			catch(CertificateException e)
			{
				Log.Error ("Exception", "File write failed: " + e);
			}
			catch(NoSuchAlgorithmException e)
			{
				Log.Error ("Exception", "File write failed: " + e);
			}
			catch(UnrecoverableEntryException e)
			{
				Log.Error ("Exception", "File write failed: " + e);
			}
			return null;
		}

	}
}
