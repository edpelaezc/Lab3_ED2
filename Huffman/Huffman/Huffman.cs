using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.IO;
using System.Text;

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
			string folder = @"C:\Compressions\";
			string fullPath = folder + newName;
			WriteTree(pList, fullPath);

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
		}

		public void BuildHuffman(Dictionary<byte, int> freq)
		{
			List<Node> pList = new List<Node>();

			for (int i = 0; i < freq.Count; i++)
			{
				KeyValuePair<byte, int> pair = freq.ElementAt(i);
				pList.Add(new Node(pair.Key, pair.Value, null, null));
			}

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
		}


		public void WriteFile(byte[] text, string newName, string name)
		{
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

			byte[] compressed = ConvertToByte(content);
			using (FileStream writer = new FileStream(fullPath, FileMode.Open))
			{
				byte[] ToWrite = ConvertToByte(content);
				for (int i = 0; i < ToWrite.Length; i++)
				{
					byte[] temp = { ToWrite[i] };				
					writer.Seek(0, SeekOrigin.End);
					writer.Write(temp, 0, 1);
				}
			}

			double compressedBytes = compressed.Length;
			double originalBytes = text.Length;
			double rc = compressedBytes / originalBytes;
			double fc = originalBytes / compressedBytes;
			double percentage = rc * 100;
			
			CompressionsCollection newElement = new CompressionsCollection(name, fullPath, rc, fc, percentage.ToString("N2") + "%");
			Data.Instance.archivos.Insert(0, newElement);
		}

		byte[] ConvertToByte(string b)
		{
			BitArray bits = new BitArray(b.Select(x => x == '1').ToArray());

			byte[] ret = ToByteArray(bits);

			return ret;
		}

		byte[] ToByteArray(BitArray bits)
		{
			int numBytes = bits.Count / 8;
			if (bits.Count % 8 != 0) numBytes++;

			byte[] bytes = new byte[numBytes];
			int byteIndex = 0, bitIndex = 0;

			for (int i = 0; i < bits.Count; i++)
			{
				if (bits[i])
					bytes[byteIndex] |= (byte)(1 << (7 - bitIndex));

				bitIndex++;
				if (bitIndex == 8)
				{
					bitIndex = 0;
					byteIndex++;
				}
			}

			return bytes;
		}

		void WriteTree(List<Node> pList, string fullpath)
		{
			foreach (var item in pList)
			{
				byte[] temp = { item.byt };
				using (FileStream writer = new FileStream(fullpath, FileMode.OpenOrCreate))
				{
					writer.Seek(0, SeekOrigin.End);
					if (ByteGenerator.ConvertToString(temp) == '\n'.ToString())
					{
						writer.Write(ByteGenerator.ConvertToBytes("NTR"), 0, 3);
					}
					else if (ByteGenerator.ConvertToString(temp) == '\r'.ToString())
					{
						writer.Write(ByteGenerator.ConvertToBytes("NDL"), 0, 3);
					}
					else
					{
						writer.Write(temp, 0, 1);
					}
					writer.Seek(0, SeekOrigin.End);
					writer.Write(ByteGenerator.ConvertToBytes(item.freq.ToString()), 0, item.freq.ToString().Length);
					writer.Seek(0, SeekOrigin.End);
					writer.Write(ByteGenerator.ConvertToBytes("@@@"), 0, 3);
				}
			}
		}


		public void decode(Node root, ref int index, BitArray str, ref string result)
		{
			if (root == null)
			{
				return;
			}
			if (root.Left == null && root.Right == null)
			{
				byte[] toStr = { root.byt };
				result += ByteGenerator.ConvertToString(toStr);
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

		public void DecodeFile(byte[] text, string newName, string name)
		{
			string txt = ByteGenerator.ConvertToString(text);
			string[] nodes = txt.Split("@@@");

			int start = (nodes.Length - 1) * 3;

			Dictionary<byte, int> freq = new Dictionary<byte, int>();

			for (int i = 0; i < nodes.Length - 1; i++)
			{
				string val = nodes[i];

				string character = "";
				string integer = "";

				if (int.TryParse(val[0].ToString(), out _))
				{
					character += val[0];
					for (int j = 1; j < val.Length; j++)
					{
						integer += val[j];
						start++;
					}
				}
				else
				{
					for (int j = 0; j < val.Length; j++)
					{
						if (!int.TryParse(val[j].ToString(), out _))
						{
							character += val[j];
						}
						else
						{
							integer += val[j];
							start++;
						}
					}
				}

				byte[] bt;
				if (character == "NTR")
				{
					bt = ByteGenerator.ConvertToBytes('\n'.ToString());
				}
				else if (character == "NDL")
				{
					bt = ByteGenerator.ConvertToBytes('\r'.ToString());
				}
				else
				{
					bt = ByteGenerator.ConvertToBytes(character.ToString());
				}
				start += character.Length;

				if (!freq.ContainsKey(bt[0]))
				{
					freq.Add(bt[0], Convert.ToInt32(integer));
				}
			}

			BuildHuffman(freq);

			BitArray result = ToBitArray(text.Skip(start).ToArray());
			int index = -1;
			string decoded = "";
			while (index < result.Length - 1)
			{
				decode(this.root, ref index, result, ref decoded);
			}

			string folder = @"C:\Compressions\";
			string fullPath = folder + newName;

			using (StreamWriter sw = new StreamWriter(fullPath))
			{
				sw.Write(decoded);
			}
		}

		private BitArray ToBitArray(byte[] bytes)
		{
			string strAllbin = "";

			for (int i = 0; i < bytes.Length; i++)
			{
				byte byteindx = bytes[i];

				string strBin = Convert.ToString(byteindx, 2); // Convert from Byte to Bin
				strBin = strBin.PadLeft(8, '0');  // Zero Pad

				strAllbin += strBin;
			}

			BitArray ba = new BitArray(strAllbin.Select(x => x == '1').ToArray());
			return ba;
		}
	}
}
