using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.IO;

namespace Huffman
{
	public class Node : IComparable
	{
		public byte byt;
		public int freq;
		public Node Left = null;
		public Node Right = null;

		Node(byte ch, int freq)
		{
			this.byt = ch;
			this.freq = freq;
		}

		public Node(byte ch, int freq, Node left, Node right)
		{
			this.byt = ch;
			this.freq = freq;
			this.Left = left;
			this.Right = right;
		}

		public int CompareTo(object obj)
		{
			var iobj = (Node)obj;
			return freq.CompareTo(iobj.freq);
		}
	}
	public class Huffman
	{
		public Dictionary<byte, string> huffCode = new Dictionary<byte, string>();
		public Node root;
		private bool[] bitF = { false };
		private bool[] bitT = { true };
		public void encode(Node root, string str, Dictionary<byte, string> huffman)
		{
			if (root == null)
			{
				return;
			}

			if (root.Left == null && root.Right == null)
			{
				huffman.Add(root.byt, str);
			}

			BitArray bit = new BitArray(bitF);

			encode(root.Left, str + "0", huffman);
			encode(root.Right, str + "1", huffman);
		}

		/// <summary>
		/// 
		/// </summary>
		/// <param name="root"></param>
		/// <param name="index"></param>
		/// <param name="str">BitArray str = new BitArray(byte[])</param>
		/// <param name="result"></param>
		public void decode(Node root, ref int index, BitArray str, ref string result)
		{
			if (root == null)
			{
				return;
			}
			if (root.Left == null && root.Right == null)
			{
				result += root.byt.ToString();
				return;
			}

			index++;

			if (str[index] == false)
			{
				decode(root.Left, ref index, str, ref result);
			}
			else
			{
				decode(root.Right, ref index, str, ref result);
			}
		}

		public void BuildHuffman(byte[] text, string newName)
		{
			Dictionary<byte, int> freq = new Dictionary<byte, int>();

			for (int i = 0; i < text.Length; i++)
			{
				if (!freq.ContainsKey(text[i]))
				{
					freq.Add(text[i], 0);
				}
				freq.TryGetValue(text[i], out int x);
				freq[text[i]] = x + 1;
			}

			List<Node> pList = new List<Node>();

			for (int i = 0; i < freq.Count; i++)
			{
				KeyValuePair<byte, int> pair = freq.ElementAt(i);
				pList.Add(new Node(pair.Key, pair.Value, null, null));
			}

			pList.Sort();
			while (pList.Count != 1)
			{
				Node pair = pList[0];
				pList.RemoveAt(0);
				Node left = pair;

				pair = pList.ElementAt(0);
				pList.RemoveAt(0);
				Node right = pair;

				int sum = left.freq + right.freq;
				pList.Add(new Node(0, sum, left, right));
				pList.Sort();
			}
			this.root = pList.ElementAt(0);
			encode(this.root, "", huffCode);

			//escribir archivo binario 
			string folder = @"C:\Compressions\";
			string fullPath = folder + newName;

			// crear el directorio
			DirectoryInfo directory = Directory.CreateDirectory(folder);

			string content = "";
			foreach (var item in text)
			{
				content += huffCode[item];
			}

			using (FileStream writer = new FileStream(fullPath, FileMode.OpenOrCreate))
			{
				byte[] ToWrite = ConvertToByte(content);
				foreach (var item in ToWrite)
				{
					if (item != 0)
					{
						writer.WriteByte(item);
					}					
				}
			}
			
			Console.WriteLine("OK");
		}

		byte[] ConvertToByte(string b)
		{
			BitArray bits = new BitArray(b.ToList().ConvertAll<bool>(x => x == '1').ToArray());

			byte[] ret = new byte[bits.Length];
			bits.CopyTo(ret, 0);

			return ret;
		}
	}
}
