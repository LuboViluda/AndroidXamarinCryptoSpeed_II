using System;

namespace XamarinCryptoSpeed
{
	public class Constants
	{
		// 10.24 MB, constant for symmetric ciphers to enc./denc.
		static public readonly int SIZE = 10240000;
		static public readonly int REPETITION = 50;
		static public readonly String TAG = "lubo.cryptospeedapp";
		static public readonly String ALIAS = "testKey";		
	}
}