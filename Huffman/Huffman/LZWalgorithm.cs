using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Huffman
{
	public class LZWalgorithm
	{
		public Dictionary<string, int> InitialDictionary = new Dictionary<string, int>();
		private int key = 1;

		private void FillDictionary(byte[] text)
		{						
			for (int i = 0; i < text.Length; i++)
			{
				string character = ByteGenerator.ConvertToString(new byte[] { text[i] });
				if (!InitialDictionary.ContainsKey(character))
				{
					InitialDictionary.Add(character, this.key);
					this.key++;
				}
			}
		}

		public void Compress(byte[] text)
		{
			FillDictionary(text);

			string w = "";
			string k = "";
			for (int i = 0; i < text.Length; i++)
			{
				k = ByteGenerator.ConvertToString(new byte[] { text[i] });

				if (InitialDictionary.ContainsKey(w+k))
				{
					w += k;
				}
				else
				{
					// escribir codigo de w										
					InitialDictionary.Add(w+k, this.key);
					this.key++;
					w = k;
				}
			}

			// escribir codigo de w
		}

		private void WriteInitialDictionary()
		{

		}
	}
}