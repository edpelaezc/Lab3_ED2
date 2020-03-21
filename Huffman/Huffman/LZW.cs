using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Text;
using System.IO;

namespace Huffman
{
    public class LZW
    {
        Dictionary<long, string> alphabet = new Dictionary<long, string>();
        string allText;
        string previous; // w 
        string actual; // k        
        int cont = 1;
        List<long> output = new List<long>();


        public void GetText(StringBuilder text) {
            allText = text.ToString();
            allText = allText.Replace("\r\n", "");
        }

        public void InitializeDictionary(StringBuilder text, string newName) {
            List<string> elements = allText.Select(c => c.ToString()).ToList();
            elements = elements.Distinct().ToList();

            for (int i = 0; i < elements.Count; i++)
            {
                alphabet.Add(cont, elements[i]);
                cont++;
            }
            cont--;

            string folder = @"C:\Compressions\";
            string fullPath = folder + newName;

            //escribir el diccionario inicial
            using (FileStream writer = new FileStream(fullPath, FileMode.OpenOrCreate))
            {
                foreach (var item in elements)
                {                    
                    byte[] ToWrite = Encoding.ASCII.GetBytes(item);
                    int aux = ToWrite[0];
                    string binary = Convert.ToString(aux, 2);
                    binary = binary.PadLeft(8, '0');
                    writer.Seek(0, SeekOrigin.End);
                    writer.Write(ToWrite, 0, 1);
                } 
                writer.Write(ByteGenerator.ConvertToBytes("%%%"), 0, 3);
            }

        }


        public void Compress(byte[] text, string newName, string name) {
            previous = "";
            for (int i = 0; i < allText.Length; i++)
            {
                actual = allText[i].ToString();
                string aux = previous + actual;
                if (alphabet.Values.Contains(aux)) // if wk is on the dictionary
                {
                    previous = previous + actual;
                }
                else
                {
                    List<string> cadenas = alphabet.Values.ToList();
                    //agregar codigo de w
                    for (int j = 0; j < alphabet.Count; j++)
                    {
                        if (cadenas[j] == previous)
                        {
                            output.Add(alphabet.Keys.ToList()[j]);
                            j = alphabet.Count; //parar de comparar.
                        }
                    }

                    //agregar wk al diccionario 
                    cont++;
                    aux = previous + actual;
                    alphabet.Add(cont, aux);

                    //w = k
                    previous = actual;
                }
            }

            //imprimir codigo de w 
            List<string> values = alphabet.Values.ToList();            
            for (int j = 0; j < alphabet.Count; j++)
            {
                if (values[j] == previous)
                {
                    output.Add(alphabet.Keys.ToList()[j]);
                    j = alphabet.Count; //parar de comparar.
                }
            }

            //OBTENCION DE CODIGOS DE SALIDA TERMINADA, imprimir en el archivo 

            string folder = @"C:\Compressions\";
            string fullPath = folder + newName;

            // crear el directorio
            DirectoryInfo directory = Directory.CreateDirectory(folder);
            
            using (FileStream writer = new FileStream(fullPath, FileMode.Open))
            {
                for (int j = 0; j < output.Count; j++)
                {
                    //byte[] ToWrite = Encoding.ASCII.GetBytes(output[j].ToString());                    
                    string binary = Convert.ToString(output[j], 2);
                    binary = binary.PadLeft(8, '0');
                    byte[] sequence = binary.Select(c => Convert.ToByte(c.ToString())).ToArray();                    
                    writer.Seek(0, SeekOrigin.End);
                    writer.Write(sequence, 0, sequence.Length);
                }
            }

            
            double compressedBytes = output.Count;
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

    }
}
