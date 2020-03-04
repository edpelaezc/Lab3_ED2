using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Huffman
{
	public class Node
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
	}
	public class Huffman
	{
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

		public void decode(Node root, int index, string str)
		{
			str = "";
			if (root == null)
			{
				return;
			}
			if (root.Left == null && root.Right == null)
			{
				str += root.ch.ToString();
				return;
			}

			index++;

			if (str[index] == '0')
			{
				decode(root.Left, index, str);
			}
			else
			{
				decode(root.Right, index, str);
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
				freq.Add(text[i], freq[text[i]] + 1);
			}

			SortedList<int, Node> pList = new SortedList<int, Node>();

			for (int i = 0; i < freq.Count; i++)
			{
				KeyValuePair<char, int> pair = freq.ElementAt(i);
				pList.Add(pair.Value, new Node(pair.Key, pair.Value, null, null));
			}

			

			while (pList.Count != 1)
			{
				KeyValuePair<int, Node> pair = pList.ElementAt(pList.Count - 1);
				pList.RemoveAt(pList.Count - 1);
				Node left = pair.Value;

				pair = pList.ElementAt(pList.Count - 1);
				pList.RemoveAt(pList.Count - 1);
				Node right = pair.Value;

				int sum = left.freq + right.freq;
				pList.Add(sum, new Node('0', sum, left, right));

			}
			KeyValuePair<int, Node> root = pList.ElementAt(pList.Count - 1);
			Node nRoot = root.Value;

			Dictionary<char, string> huffCode = new Dictionary<char, string>();
			encode(nRoot, "", huffCode);


		}	
	}
}
