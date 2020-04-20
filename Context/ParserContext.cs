using System;
using System.Collections.Generic;
using System.Text;

namespace MT940Parser.Context
{
    public class ParserContext
    {
        public event EventHandler ContextChanged;
        private Dictionary<string, object> dict = new Dictionary<string, object>();
        public object this[string i]
        {
            get => dict[i];
            set 
            {
                dict[i] = value;
                ContextChanged.Invoke(this, null);
            }
        }
    }
}
