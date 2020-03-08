using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Huffman
{
    public class CompressionsCollection
    {
        public string originalName { get; set; }
        public string path { get; set; }
        public int compressionReason { get; set; } // razon de compresion 
        public int compressionFactor { get; set; }  //factor de compresion 
        public int reduction { get; set; }

        public CompressionsCollection(string name, string path, int cr, int cf, int redPercentage) {
            originalName = name;
            this.path = path;
            compressionReason = cr;
            compressionFactor = cf;
            reduction = redPercentage;
        }
    }
}
