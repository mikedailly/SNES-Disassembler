using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SNESDisassembler
{
    // *********************************************************************************************************
    /// <summary>
    ///     Symbol definition type
    /// </summary>
        // *********************************************************************************************************
    public enum eSymbolType
    {
        /// <summary>We are a label</summary>
        Label,
        /// <summary>Defained as an equate</summary>
        Equate,


        /// <summary>Used when searching</summary>
        Any
    }

    /// <summary>
    ///     A single symbol
    /// </summary>
    public class Symbol
    {
        /// <summary>Symbol name</summary>
        public string  name;
        /// <summary>Symbol value</summary>
        public Int64 value;
        /// <summary>Symbol type</summary>
        public eSymbolType type;

        // *********************************************************************************************************
        /// <summary>
        ///     Create a symbol
        /// </summary>
        /// <param name="name">the symbol name</param>
        /// <param name="value">the symbol value</param>
        /// <param name="type">the symbol type</param>
        // *********************************************************************************************************
        public Symbol(string name, long value, eSymbolType type = eSymbolType.Label)
        {
            this.name = name;
            this.value = value;
            this.type = type;
        }
    }
}
