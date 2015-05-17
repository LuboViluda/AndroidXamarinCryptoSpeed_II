using System;
using System.Security.Cryptography;
using System.Text;
using Android.Util;

namespace XamarinCryptoSpeed.II
{
	public static class Hash
	{
		public static void TestHash (String algorithmName)
		{
			byte[] b1 = new byte[Constants.SIZE];
			byte[] b2 = null;
			CommonAuxiliaryCode.GenerateDummyBytes(b1);

			long startHash ,endHash;
			double[] hashTime = new double[Constants.REPETITION];
			double hashSum = 0;
			StringBuilder buffer = new StringBuilder();

			HashAlgorithm mg = LoadHashClass(algorithmName);
			if (null == mg) 
			{	// error log alreary written by loadhashClass method, skipp test
				return;
			}

			Log.Info(Constants.TAG, algorithmName + " test start with file size: " + Constants.SIZE + " bytes x times: " + Constants.REPETITION);
			for(int i = 0; i < Constants.REPETITION; i++)
			{
				startHash = DateTime.Now.ToFileTime();
				b2 = mg.ComputeHash(b1);
				endHash = DateTime.Now.ToFileTime();
				if(b2 != null)
				{
					hashTime[i] = (((double) endHash / 10000.0) - ((double) startHash) / 10000.0);
					hashSum += hashTime[i];
					Log.Info(Constants.TAG, algorithmName + " attempt : " + i + " ended successful time : " + hashTime[i]);
					buffer.Append(hashTime[i]);
					buffer.Append (","); 	//csv
					b2 = null;
				}
			}
			double hashAvr = hashSum / ((double) Constants.REPETITION);
			Log.Info(Constants.TAG, "Test " + algorithmName + " finished, avrg. time : " + hashAvr);
			Log.Info(Constants.TAG, "Test " + algorithmName + " by provider: " + "with file size: " +  Constants.SIZE + 
				" bytesxtimes: " + Constants.REPETITION);
			Log.Info(Constants.TAG, "ended succesfully\n Averange values: " + hashAvr);
			CommonAuxiliaryCode.WriteToFile(algorithmName + ".C#." + Constants.SIZE + "x" + Constants.REPETITION + ".csv", buffer.ToString());
		}

	
		private static HashAlgorithm LoadHashClass(String algorithmName)
		{
			if(algorithmName.Equals("MD5"))
			{
				return System.Security.Cryptography.MD5.Create();
			}
			if(algorithmName.Equals("SHA1"))
			{
				return System.Security.Cryptography.SHA1.Create();
			}
			if(algorithmName.Equals("MD5Mono")) 
			{
				return new MD5CryptoServiceProvider();
			}
			if(algorithmName.Equals("SHA1Mono"))
			{
				return new SHA1CryptoServiceProvider ();
			}
			Log.Error(Constants.TAG, "Test skipped, called no supported algorithm : " + algorithmName);
			return null;
		}
	
	}
}

