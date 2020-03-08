﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Huffman
{
    public class CompressionsCollection
    {
        public string originalName { get; set; }
        public string path { get; set; }
        public string compressionReason { get; set; } // razon de compresion 
        public string compressionFactor { get; set; }  //factor de compresion 
        public string reduction { get; set; }
    }
}
