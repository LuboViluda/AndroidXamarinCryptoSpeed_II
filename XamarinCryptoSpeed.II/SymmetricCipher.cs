using System;
using Android.Util;
using Android.Widget;
using System.Text;
using System.IO;
using System.Security.Cryptography;
using Android.Content;
using Mono.Security;
using Mono.Security.Cryptography;

namespace XamarinCryptoSpeed.II
{
	public class SymmetricCipher
	{
		byte[] key = null;
		byte[] iv = null; 	// AES has 16 bytes counter, TripleDES 8 bytes
		int keyLength;
		String cipherName = null;
		Context appContext;

		public SymmetricCipher (String parCipherName, int parKeyLength, int parIvLength, Context parAppContext)
		{
			cipherName = parCipherName;
			keyLength = parKeyLength;
			key = CommonAuxiliaryCode.GenerateKey(parKeyLength / 8);
			iv = new byte[parIvLength];
			CommonAuxiliaryCode.GenerateDummyBytes(iv);
			appContext = parAppContext;
		}

		public void TestSymmetricCipher()
		{
			byte[] b1 = new byte[Constants.SIZE];
			byte[] b2 = null;
			byte[] b3 = null;
			long startEncryption ,endEncryption, startDecryption, endDecryption;
			double[] encTime = new double[50];
			double[] decTime = new double[50];
			double sumEncryption, sumDecryption;
			sumDecryption = sumEncryption = 0;
			// buffers for log written into device sd card
			StringBuilder bufferEncryption = new StringBuilder();
			StringBuilder bufferDecryption = new StringBuilder();

			CommonAuxiliaryCode.GenerateDummyBytes(b1);
			Log.Info(Constants.TAG, cipherName + "test start");
			for (int i = 0; i < Constants.REPETITION; i++) 
			{
				using (MemoryStream ms = new MemoryStream())
				{
					using (var symCipher = SymmetricCipher.GetCipher(cipherName))
					{
						symCipher.KeySize = 128;

						symCipher.Key = key;
						symCipher.IV = iv;
						symCipher.Mode = CipherMode.CBC;

						using (var cs = new CryptoStream(ms, symCipher.CreateEncryptor(), CryptoStreamMode.Write))
						{
							startEncryption = DateTime.Now.ToFileTime();
							cs.Write(b1, 0, b1.Length);
							endEncryption = DateTime.Now.ToFileTime();
							cs.Close();
						}
						b2 = ms.ToArray();
					}
				}
					
				using (MemoryStream ms = new MemoryStream ()) 
				{
					using (var symCipher = SymmetricCipher.GetCipher(cipherName)) 
					{
						symCipher.KeySize = 128;
						symCipher.Key = key;
						symCipher.IV = iv;
						symCipher.Mode = CipherMode.CBC;

						using (var cs = new CryptoStream (ms, symCipher.CreateDecryptor (), CryptoStreamMode.Write)) {
							startDecryption = DateTime.Now.ToFileTime();
							cs.Write (b2, 0, b2.Length);
							endDecryption = DateTime.Now.ToFileTime();
							cs.Close ();
						}
						b3 = ms.ToArray ();
					}
				}	
				// comparation, builds buffers
				if (CommonAuxiliaryCode.CmpByteArrayShowResult (b1, b3, cipherName + " attempt: " + i)) 
				{
					encTime [i] = (((double)endEncryption / 10000.0) - ((double)startEncryption) / 10000.0);
					sumEncryption += encTime [i];
					decTime [i] = (((double)endDecryption / 10000.0) - ((double)startDecryption) / 10000.0);
					sumDecryption += decTime [i];
					Log.Info (Constants.TAG, cipherName + "-" + keyLength + " attempt : " + i + " ended successful time enc: " + encTime [i] + " dec : " + decTime [i]);
					bufferEncryption.Append (encTime [i] + ",");
					bufferDecryption.Append (decTime [i] + ",");
					b3 [0] = 0;
				} else 
				{   // shoudn't happen, skipp test
					Log.Error (Constants.TAG, cipherName + " plain text and plain text after enc/den differs !!!");
					return;
				}
			}
			double encR = sumEncryption / ((double)Constants.REPETITION);
			double decR = sumDecryption / ((double)Constants.REPETITION);
			Log.Info (Constants.TAG, "Test " + cipherName + " by provider: " + " ended succesfully");
			Log.Info (Constants.TAG, "Averange values: " + encR + "," + decR);
			Toast.MakeText (appContext, "ENC time: " + encR + " DEC time: " + decR, ToastLength.Short).Show ();
			CommonAuxiliaryCode.WriteToFile (cipherName + "-" + keyLength + ".C#.D." + Constants.SIZE + "x"
				+ Constants.REPETITION + ".csv", bufferDecryption.ToString ());
			CommonAuxiliaryCode.WriteToFile (cipherName + "-" + keyLength +".C#.E." + Constants.SIZE + "x"
				+ Constants.REPETITION + ".csv", bufferEncryption.ToString ());
		}

		public static SymmetricAlgorithm GetCipher (String algorithmName)
		{
			if(algorithmName.Equals("TripleDES"))
			{
				return new TripleDESCryptoServiceProvider();
			}
			if(algorithmName.Equals("AES"))
			{
				return new AesCryptoServiceProvider();
			}
			if(algorithmName.Equals("ARC4"))
			{
				return new ARC4Managed();	
			}
			Log.Error(Constants.TAG, "SymmetricCipher Error, GetCipher asked for no supported algorithm");
			return null;
		}
		
			
	}
}

