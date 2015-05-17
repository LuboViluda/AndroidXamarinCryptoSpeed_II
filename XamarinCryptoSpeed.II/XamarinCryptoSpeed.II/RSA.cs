using System;
using System.Text;
using Android.Util;
using Java.Security;
using System.Security.Cryptography;
using Android.Content;
using Android.Widget;

namespace XamarinCryptoSpeed.II
{
	public class RSA
	{
		// override standard common constant
		static readonly int SIG_SIZE = 20;
		static readonly int SIG_REPETITION = 200;

		int keyLength;

		RSACryptoServiceProvider rsa = null;
		RSAParameters privKey;
		RSAParameters pubKey;


		public RSA (int parKeyLength)
		{
			keyLength = parKeyLength;
			rsa = new RSACryptoServiceProvider(keyLength);
			privKey = rsa.ExportParameters(true);
			pubKey = rsa.ExportParameters(false);
		}

		public void TestSignature(Context appContext)
		{
			byte[] b1 =  new byte[SIG_SIZE];
			byte[] b2 = null;
			long startSign ,endSign, startVerify, endVerify;
			double[] signTime = new double[SIG_REPETITION];
			double[] verifyTime = new double[SIG_REPETITION];
			double sumSign = 0;
			double sumVerify = 0;
			StringBuilder bufferSign = new StringBuilder();
			StringBuilder bufferVerify = new StringBuilder();
			CommonAuxiliaryCode.GenerateDummyBytes(b1);

			for(int i =0; i < SIG_REPETITION; i++)
			{
				try
				{	
					rsa.ImportParameters(privKey);
					startSign = DateTime.Now.ToFileTime();
					b2 = rsa.SignData(b1, CryptoConfig.MapNameToOID("SHA1"));
					endSign = DateTime.Now.ToFileTime();

					Boolean success;
					rsa.ImportParameters(pubKey);
					startVerify = DateTime.Now.ToFileTime();
					success = rsa.VerifyData(b1, CryptoConfig.MapNameToOID("SHA1"), b2);
					endVerify = DateTime.Now.ToFileTime();
					if (success)
					{
						signTime[i] = (((double) endSign / 10000.0) - ((double) startSign)/ 10000.0);
						sumSign += signTime[i];
						verifyTime[i] =  (((double) endVerify / 10000.0) - ((double) startVerify)/ 10000.0);
						sumVerify += verifyTime[i];
						Log.Info(Constants.TAG,  "RSA" + keyLength + " attempt " + i + " ended successful time sign: " + signTime[i] + " verify : " + verifyTime[i]);
						bufferSign.Append(signTime[i] + ",");
						bufferVerify.Append(verifyTime[i] + ",");
					}
					else
					{   // shoudn't happen, skipp test
						Log.Error(Constants.TAG, "RSA" + keyLength + " plain text and plain text after enc/den differs !!!");
						return;
					}
				}  
				catch (SignatureException e)
				{
					e.PrintStackTrace();
				}
				catch(InvalidKeyException e)
				{
					e.PrintStackTrace();
				}
			}
			double encR =  sumSign / ((double) SIG_REPETITION);
			double decR =  sumVerify / ((double) SIG_REPETITION);
			Log.Info(Constants.TAG, "RSA" + keyLength + " by provider: " + "AES C# system" + " ended succesfully");
			Log.Info(Constants.TAG, "Averange values: " + encR + "," + decR);
			Toast.MakeText(appContext, "SIGN time: " + encR + " VERIFY time: " + decR, ToastLength.Short ).Show();
			CommonAuxiliaryCode.WriteToFile("RSA.S." + keyLength + "." + SIG_SIZE + "x" 
				+ SIG_REPETITION  + ".csv", bufferSign.ToString());
			CommonAuxiliaryCode.WriteToFile("RSA" + ".V." + keyLength + "." + SIG_SIZE + "x" 
				+ SIG_REPETITION  +".csv", bufferVerify.ToString());
		}
	}
}

