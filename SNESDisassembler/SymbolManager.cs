using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SNESDisassembler
{
    /// <summary>
    ///     Global symbol manager
    /// </summary>
    public class SymbolManager
    {
        /// <summary>Look up table</summary>
        Dictionary<Int64, List<Symbol>> SymbolTable;


        // ************************************************************************************************************
        /// <summary>
        ///     Create a new symbol manager
        /// </summary>
        // ************************************************************************************************************
        public SymbolManager() 
        {
            SymbolTable = new Dictionary<Int64, List<Symbol>>();
        }

        // ************************************************************************************************************
        /// <summary>
        ///     Add a symbol to the pool
        /// </summary>
        /// <param name="_symbol">Symbol to addd</param>
        // ************************************************************************************************************
        public void Add(Symbol _symbol)
        {
            List<Symbol> list = null;
            if( !SymbolTable.TryGetValue(_symbol.value, out list) )
            {
                list = new List<Symbol>();
                SymbolTable.Add(_symbol.value, list);
            }
            list.Add( _symbol);
        }

        // ************************************************************************************************************
        /// <summary>
        ///     Add a symbol to the pool
        /// </summary>
        /// <param name="_symbol">Symbol to addd</param>
        // ************************************************************************************************************
        public void Add(Int64 value, string text, eSymbolType type = eSymbolType.Label)
        {
            Symbol sym = new Symbol(text, value, type);
            Add(sym);
        }


        public Symbol? Find(Int64 value, eSymbolType prefered_type = eSymbolType.Any)
        {
            List<Symbol> list;
            if (SymbolTable.TryGetValue(value, out list))
            {
                if (list != null && list.Count >= 0)
                {
                    if (prefered_type == eSymbolType.Any)
                    {
                        return list[0];
                    }
                    else
                    {
                        // search for the right symbol type - or return the first one found
                        Symbol s = list[0];
                        foreach (Symbol sym in list)
                        {
                            if (sym.type == prefered_type)
                            {
                                return sym;
                            }
                        }
                        return s;
                    }
                }
            }
            return null;
        }

        public string Lookup(Int64 value, int width, eSymbolType prefered_type = eSymbolType.Any)
        {
            Symbol sym = Find(value, prefered_type);
            if(sym==null)
            {
                switch(width)
                {
                    case 2:
                        return string.Format("${0:X2}", value);
                    case 4:
                        return string.Format("${0:X4}", value);
                    case 6:
                        return string.Format("${0:X6}", value);
                    case 8:
                    default:
                        return string.Format("${0:X8}", value);
                }

            }
            else
            {
                return sym.name;
            }
        }

    }
}
