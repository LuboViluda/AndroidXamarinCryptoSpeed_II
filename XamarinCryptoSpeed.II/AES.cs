using System;
using System.Security.Cryptography;
using Java.Security;
using System.IO;
using System.Text;
using Android.Util;
using Android.Widget;
using Android.Content;

namespace XamarinCryptoSpeed.II
{
	public class AES
	{
		byte[] key = null;
		byte[] iv = new byte[16]; 	// AES has 16 bits counter
		int keyLength { get; set; }

		public AES (int parKeyLength)
		{
			keyLength = parKeyLength;
			key = new byte[keyLength / 8];
			key = CommonAuxiliaryCode.GenerateKey(parKeyLength / 8);
			CommonAuxiliaryCode.GenerateDummyBytes(iv);
		}

		public void testAES(String cipherName, Context appContext)
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
					using (var AES = new AesCryptoServiceProvider())
					{
						AES.KeySize = 128;

						AES.Key = key;
						AES.IV = iv;
						AES.Mode = CipherMode.CBC;

						using (var cs = new CryptoStream(ms, AES.CreateEncryptor(), CryptoStreamMode.Write))
						{
							startEncryption = DateTime.Now.ToFileTime();
							cs.Write(b1, 0, b1.Length);
							endEncryption = DateTime.Now.ToFileTime();
							cs.Close();
						}
						b2 = ms.ToArray();
					}
				}


				using (MemoryStream ms = new MemoryStream ()) {
					using (var AES = new AesCryptoServiceProvider()) {
						AES.KeySize = 128;
						AES.Key = key;
						AES.IV = iv;
						AES.Mode = CipherMode.CBC;

						using (var cs = new CryptoStream (ms, AES.CreateDecryptor (), CryptoStreamMode.Write)) {
							startDecryption = DateTime.Now.ToFileTime();
							cs.Write (b2, 0, b2.Length);
							endDecryption = DateTime.Now.ToFileTime();
							cs.Close ();
						}
						b3 = ms.ToArray ();
					}
				}	
					// comparation, builds buffers
					if (CommonAuxiliaryCode.CmpByteArrayShowResult (b1, b3, cipherName + " attempt: " + i)) {
						encTime [i] = (((double)endEncryption / 10000.0) - ((double)startEncryption) / 10000.0);
						sumEncryption += encTime [i];
						decTime [i] = (((double)endDecryption / 10000.0) - ((double)startDecryption) / 10000.0);
						sumDecryption += decTime [i];
						Log.Info (Constants.TAG, cipherName + " attempt : " + i + " ended successful time enc: " + encTime [i] + " dec : " + decTime [i]);
						bufferEncryption.Append (encTime [i] + ",");
						bufferDecryption.Append (decTime [i] + ",");
						b3 [0] = 0;
					} else {   // shoudn't happen, skipp test
						Log.Error (Constants.TAG, cipherName + " plain text and plain text after enc/den differs !!!");
						return;
					}
				}
				double encR = sumEncryption / ((double)Constants.REPETITION);
				double decR = sumDecryption / ((double)Constants.REPETITION);
				Log.Info (Constants.TAG, "Test " + cipherName + " by provider: " + " ended succesfully");
				Log.Info (Constants.TAG, "Averange values: " + encR + "," + decR);
				Toast.MakeText (appContext, "ENC time: " + encR + " DEC time: " + decR, ToastLength.Short).Show ();
				CommonAuxiliaryCode.WriteToFile (cipherName + ".C#.D." + Constants.SIZE + "x"
					+ Constants.REPETITION + ".csv", bufferDecryption.ToString ());
				CommonAuxiliaryCode.WriteToFile (cipherName + ".C#.E." + Constants.SIZE + "x"
					+ Constants.REPETITION + ".csv", bufferEncryption.ToString ());
			}

	}
}

/*
 					aesAlg.Key = key;
					aesAlg.IV = iv;
					//aesAlg2.Key = key;
					//aesAlg2.IV = iv;


					ICryptoTransform encryptor = aesAlg.CreateEncryptor (aesAlg.Key, aesAlg.IV);
					//byte[] iv2 = new byte[16];
					//Array.Copy(iv, iv2, iv.Length);
					//encryption
					using (MemoryStream msEncrypt = new MemoryStream ()) {
						using (CryptoStream csEncrypt = new CryptoStream (msEncrypt, encryptor, CryptoStreamMode.Write)) {
							using (StreamWriter swEncrypt = new StreamWriter (csEncrypt)) {
								//Write all data to the stream.
								swEncrypt.Write (b1);
							}
							startEncryption = DateTime.Now.ToFileTime ();
							b2 = msEncrypt.ToArray ();
							endEncryption = DateTime.Now.ToFileTime ();
						}
					}

					ICryptoTransform decryptor = aesAlg.CreateDecryptor (aesAlg.Key, aesAlg.IV);
					//aesAlg.Key = key;
					//aesAlg.IV = iv2;
					// decryption
					using (MemoryStream msDecrypt = new MemoryStream (b2)) {
						using (CryptoStream csDecrypt = new CryptoStream (msDecrypt, decryptor, CryptoStreamMode.Read)) {
							string temp = null;
							using (StreamReader srDecrypt = new StreamReader (csDecrypt)) {
								startDecryption = DateTime.Now.ToFileTime ();
								temp = srDecrypt.ReadToEnd ();
								endDecryption = DateTime.Now.ToFileTime ();
								b3 = CommonAuxiliaryCode.GetBytes (temp);
							}
						}
					}
					*/