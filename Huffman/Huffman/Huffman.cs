using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Huffman
{
	public class Node : IComparable
	{
		public char ch;
		public int freq;
		public Node Left = null;
		public Node Right = null;

		Node(char ch, int freq)
		{
			this.ch = ch;
			this.freq = freq;
		}

		public Node(char ch, int freq, Node left, Node right)
		{
			this.ch = ch;
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
		public Dictionary<char, string> huffCode = new Dictionary<char, string>();
		public Node root;

		public void encode(Node root, string str, Dictionary<char, string> huffman)
		{
			if (root == null)
			{
				return;
			}

			if (root.Left == null && root.Right == null)
			{
				huffman.Add(root.ch, str);
			}

			encode(root.Left, str + "0", huffman);
			encode(root.Right, str + "1", huffman);
		}

		public void decode(Node root, ref int index, string str, ref string result)
		{
			if (root == null)
			{
				return;
			}
			if (root.Left == null && root.Right == null)
			{
				result += root.ch.ToString();
				return;
			}

			index++;

			if (str[index] == '0')
			{
				decode(root.Left, ref index, str, ref result);
			}
			else
			{
				decode(root.Right, ref index, str, ref result);
			}
		}

		public void BuildHuffman(string text)
		{
			Dictionary<char, int> freq = new Dictionary<char, int>();

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
				KeyValuePair<char, int> pair = freq.ElementAt(i);
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
				pList.Add(new Node('\0', sum, left, right));
				pList.Sort();
			}
			this.root = pList.ElementAt(0);
			encode(this.root, "", huffCode);
		}
	}
}
