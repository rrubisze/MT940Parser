using Raptorious.SharpMt940Lib.Mt940Format;
using System;
using System.Collections.Generic;
using System.Text;

namespace MT940Parser
{
    public class IngFormat : IMt940Format
    {
        public Separator Header => new Separator("940");

        public Separator Trailer => new Separator("-");
    }

}
