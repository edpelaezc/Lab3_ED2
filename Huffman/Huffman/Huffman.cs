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
	}
}
