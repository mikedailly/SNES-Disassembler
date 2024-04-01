// ************************************************************************************************************************************************
// 65816.cs
// C version of	65816/6502 module for DisPel
//				James Churchill
//				Created 230900
//				Last Modified 240900
//					
// Port to C# by Mike Dailly 31/3/24
//
// ************************************************************************************************************************************************




using System.Text;

// ************************************************************************************************************************************************
/// <summary>
///		Dissassembly creation options
/// </summary>
// ************************************************************************************************************************************************
[Flags]
public enum eDissassemblyOptions
{
	None = 0,
    /// <summary>Don't output addresses/hex dump</summary>
    DontOutputAddress = 1,
    /// <summary>Split subroutines by placing blank lines after RTS,RTL,RTI</summary>
    SplitFunctions = 2
}

public class Dissassemble65816
{
    // ************************************************************************************************************************************************
    /// <summary>
    ///		disassembles a single instruction
    /// </summary>
    /// <param name="mem">memory for disassembly</param>
    /// <param name="pos">"address" of the instruction</param>
    /// <param name="flag">current processor state</param>
    /// <param name="inst">String buffer</param>
    /// <param name="tsrc">1 if addresses/hex dump is to be suppressed</param>
    /// <returns>number of bytes to advance, or 0 for error</returns>
    // ************************************************************************************************************************************************
    public int disasm(byte[] mem, Int64 pos, ref byte flag, StringBuilder inst, eDissassemblyOptions tsrc, out List<string> segments)
	{
		// temp buffers to hold instruction,parameters and hex
		StringBuilder ibuf = new StringBuilder(128);
		StringBuilder pbuf = new StringBuilder(128);
		StringBuilder hbuf = new StringBuilder(128);
		segments = new List<string>();

        // variables to hold the instruction increment and signed params
        int offset, sval, i;

		// Parse out instruction mnemonic

		switch (mem[0])
		{
			// ADC
			case 0x69:
			case 0x6D:
			case 0x6F:
			case 0x65:
			case 0x72:
			case 0x67:
			case 0x7D:
			case 0x7F:
			case 0x79:
			case 0x75:
			case 0x61:
			case 0x71:
			case 0x77:
			case 0x63:
			case 0x73:
				ibuf.Append("adc");
				break;
			// AND
			case 0x29:
			case 0x2D:
			case 0x2F:
			case 0x25:
			case 0x32:
			case 0x27:
			case 0x3D:
			case 0x3F:
			case 0x39:
			case 0x35:
			case 0x21:
			case 0x31:
			case 0x37:
			case 0x23:
			case 0x33:
				ibuf.Append("and");
				break;
			// ASL
			case 0x0A:
			case 0x0E:
			case 0x06:
			case 0x1E:
			case 0x16:
				ibuf.Append("asl");
				break;
			// BCC
			case 0x90:
				ibuf.Append("bcc");
				break;
			// BCS
			case 0xB0:
				ibuf.Append("bcs");
				break;
			// BEQ
			case 0xF0:
				ibuf.Append("beq");
				break;
			// BNE
			case 0xD0:
				ibuf.Append("bne");
				break;
			// BMI
			case 0x30:
				ibuf.Append("bmi");
				break;
			// BPL
			case 0x10:
				ibuf.Append("bpl");
				break;
			// BVC
			case 0x50:
				ibuf.Append("bvc");
				break;
			// BVS
			case 0x70:
				ibuf.Append("bvs");
				break;
			// BRA
			case 0x80:
				ibuf.Append("bra");
				break;
			// BRL
			case 0x82:
				ibuf.Append("brl");
				break;
			// BIT
			case 0x89:
			case 0x2C:
			case 0x24:
			case 0x3C:
			case 0x34:
				ibuf.Append("bit");
				break;
			// BRK
			case 0x00:
				ibuf.Append("brk");
				break;
			// CLC
			case 0x18:
				ibuf.Append("clc");
				break;
			// CLD
			case 0xD8:
				ibuf.Append("cld");
				break;
			// CLI
			case 0x58:
				ibuf.Append("cli");
				break;
			// CLV
			case 0xB8:
				ibuf.Append("clv");
				break;
			// SEC
			case 0x38:
				ibuf.Append("sec");
				break;
			// SED
			case 0xF8:
				ibuf.Append("sed");
				break;
			// SEI
			case 0x78:
				ibuf.Append("sei");
				break;
			// CMP
			case 0xC9:
			case 0xCD:
			case 0xCF:
			case 0xC5:
			case 0xD2:
			case 0xC7:
			case 0xDD:
			case 0xDF:
			case 0xD9:
			case 0xD5:
			case 0xC1:
			case 0xD1:
			case 0xD7:
			case 0xC3:
			case 0xD3:
				ibuf.Append("cmp");
				break;
			// COP
			case 0x02:
				ibuf.Append("cop");
				break;
			// CPX
			case 0xE0:
			case 0xEC:
			case 0xE4:
				ibuf.Append("cpx");
				break;
			// CPY
			case 0xC0:
			case 0xCC:
			case 0xC4:
				ibuf.Append("cpy");
				break;
			// DEC
			case 0x3A:
			case 0xCE:
			case 0xC6:
			case 0xDE:
			case 0xD6:
				ibuf.Append("dec");
				break;
			// DEX
			case 0xCA:
				ibuf.Append("dex");
				break;
			// DEY
			case 0x88:
				ibuf.Append("dey");
				break;
			// EOR
			case 0x49:
			case 0x4D:
			case 0x4F:
			case 0x45:
			case 0x52:
			case 0x47:
			case 0x5D:
			case 0x5F:
			case 0x59:
			case 0x55:
			case 0x41:
			case 0x51:
			case 0x57:
			case 0x43:
			case 0x53:
				ibuf.Append("eor");
				break;
			// INC
			case 0x1A:
			case 0xEE:
			case 0xE6:
			case 0xFE:
			case 0xF6:
				ibuf.Append("inc");
				break;
			// INX
			case 0xE8:
				ibuf.Append("inx");
				break;
			// INY
			case 0xC8:
				ibuf.Append("iny");
				break;
			// JMP
			case 0x4C:
			case 0x6C:
			case 0x7C:
			case 0x5C:
			case 0xDC:
				ibuf.Append("jmp");
				break;
			// JSR
			case 0x22:
			case 0x20:
			case 0xFC:
				ibuf.Append("jsr");
				break;
			// LDA
			case 0xA9:
			case 0xAD:
			case 0xAF:
			case 0xA5:
			case 0xB2:
			case 0xA7:
			case 0xBD:
			case 0xBF:
			case 0xB9:
			case 0xB5:
			case 0xA1:
			case 0xB1:
			case 0xB7:
			case 0xA3:
			case 0xB3:
				ibuf.Append("lda");
				break;
			// LDX
			case 0xA2:
			case 0xAE:
			case 0xA6:
			case 0xBE:
			case 0xB6:
				ibuf.Append("ldx");
				break;
			// LDY
			case 0xA0:
			case 0xAC:
			case 0xA4:
			case 0xBC:
			case 0xB4:
				ibuf.Append("ldy");
				break;
			// LSR
			case 0x4A:
			case 0x4E:
			case 0x46:
			case 0x5E:
			case 0x56:
				ibuf.Append("lsr");
				break;
			// MVN
			case 0x54:
				ibuf.Append("mvn");
				break;
			// MVP
			case 0x44:
				ibuf.Append("mvp");
				break;
			// NOP
			case 0xEA:
				ibuf.Append("nop");
				break;
			// ORA
			case 0x09:
			case 0x0D:
			case 0x0F:
			case 0x05:
			case 0x12:
			case 0x07:
			case 0x1D:
			case 0x1F:
			case 0x19:
			case 0x15:
			case 0x01:
			case 0x11:
			case 0x17:
			case 0x03:
			case 0x13:
				ibuf.Append("ora");
				break;
			// PEA
			case 0xF4:
				ibuf.Append("pea");
				break;
			// PEI
			case 0xD4:
				ibuf.Append("pei");
				break;
			// PER
			case 0x62:
				ibuf.Append("per");
				break;
			// PHA
			case 0x48:
				ibuf.Append("pha");
				break;
			// PHP
			case 0x08:
				ibuf.Append("php");
				break;
			// PHX
			case 0xDA:
				ibuf.Append("phx");
				break;
			// PHY
			case 0x5A:
				ibuf.Append("phy");
				break;
			// PLA
			case 0x68:
				ibuf.Append("pla");
				break;
			// PLP
			case 0x28:
				ibuf.Append("plp");
				break;
			// PLX
			case 0xFA:
				ibuf.Append("plx");
				break;
			// PLY
			case 0x7A:
				ibuf.Append("ply");
				break;
			// PHB
			case 0x8B:
				ibuf.Append("phb");
				break;
			// PHD
			case 0x0B:
				ibuf.Append("phd");
				break;
			// PHK
			case 0x4B:
				ibuf.Append("phk");
				break;
			// PLB
			case 0xAB:
				ibuf.Append("plb");
				break;
			// PLD
			case 0x2B:
				ibuf.Append("pld");
				break;
			// REP
			case 0xC2:
				ibuf.Append("rep");
				break;
			// ROL
			case 0x2A:
			case 0x2E:
			case 0x26:
			case 0x3E:
			case 0x36:
				ibuf.Append("rol");
				break;
			// ROR
			case 0x6A:
			case 0x6E:
			case 0x66:
			case 0x7E:
			case 0x76:
				ibuf.Append("ror");
				break;
			// RTI
			case 0x40:
				{
					ibuf.Append("rti");
					if ((tsrc & eDissassemblyOptions.SplitFunctions) != 0)
					{
						ibuf.Append(Environment.NewLine);
					}
					break;
				}
			// RTL
			case 0x6B:
				{
					ibuf.Append("rtl");
					if ((tsrc & eDissassemblyOptions.SplitFunctions) != 0)
					{
						ibuf.Append(Environment.NewLine);
					}
					break;
				}
			// RTS
			case 0x60:
				{
					ibuf.Append("rts");
					if ((tsrc & eDissassemblyOptions.SplitFunctions) != 0)
					{
						ibuf.Append(Environment.NewLine);
					}
					break;
				}
			// SBC
			case 0xE9:
			case 0xED:
			case 0xEF:
			case 0xE5:
			case 0xF2:
			case 0xE7:
			case 0xFD:
			case 0xFF:
			case 0xF9:
			case 0xF5:
			case 0xE1:
			case 0xF1:
			case 0xF7:
			case 0xE3:
			case 0xF3:
				ibuf.Append("sbc");
				break;
			// SEP
			case 0xE2:
				ibuf.Append("sep");
				break;
			// STA
			case 0x8D:
			case 0x8F:
			case 0x85:
			case 0x92:
			case 0x87:
			case 0x9D:
			case 0x9F:
			case 0x99:
			case 0x95:
			case 0x81:
			case 0x91:
			case 0x97:
			case 0x83:
			case 0x93:
				ibuf.Append("sta");
				break;
			// STP
			case 0xDB:
				ibuf.Append("stp");
				break;
			// STX
			case 0x8E:
			case 0x86:
			case 0x96:
				ibuf.Append("stx");
				break;
			// STY
			case 0x8C:
			case 0x84:
			case 0x94:
				ibuf.Append("sty");
				break;
			// STZ
			case 0x9C:
			case 0x64:
			case 0x9E:
			case 0x74:
				ibuf.Append("stz");
				break;
			// TAX
			case 0xAA:
				ibuf.Append("tax");
				break;
			// TAY
			case 0xA8:
				ibuf.Append("tay");
				break;
			// TXA
			case 0x8A:
				ibuf.Append("txa");
				break;
			// TYA
			case 0x98:
				ibuf.Append("tya");
				break;
			// TSX
			case 0xBA:
				ibuf.Append("tsx");
				break;
			// TXS
			case 0x9A:
				ibuf.Append("txs");
				break;
			// TXY
			case 0x9B:
				ibuf.Append("txy");
				break;
			// TYX
			case 0xBB:
				ibuf.Append("tyx");
				break;
			// TCD
			case 0x5B:
				ibuf.Append("tcd");
				break;
			// TDC
			case 0x7B:
				ibuf.Append("tdc");
				break;
			// TCS
			case 0x1B:
				ibuf.Append("tcs");
				break;
			// TSC
			case 0x3B:
				ibuf.Append("tsc");
				break;
			// TRB
			case 0x1C:
			case 0x14:
				ibuf.Append("trb");
				break;
			// TSB
			case 0x0C:
			case 0x04:
				ibuf.Append("tsb");
				break;
			// WAI
			case 0xCB:
				ibuf.Append("wai");
				break;
			// WDM
			case 0x42:
				ibuf.Append("wdm");
				break;
			// XBA
			case 0xEB:
				ibuf.Append("xba");
				break;
			// XCE
			case 0xFB:
				ibuf.Append("xce");
				break;
			default:
				// Illegal
				ibuf.Append(string.Format("Illegal instruction: {0:X2}", mem[0]));
				break;
		};




		// Parse out parameter list
		switch (mem[0])
		{
			// Absolute
			case 0x0C:
			case 0x0D:
			case 0x0E:
			case 0x1C:
			case 0x20:
			case 0x2C:
			case 0x2D:
			case 0x2E:
			case 0x4C:
			case 0x4D:
			case 0x4E:
			case 0x6D:
			case 0x6E:
			case 0x8C:
			case 0x8D:
			case 0x8E:
			case 0x9C:
			case 0xAC:
			case 0xAD:
			case 0xAE:
			case 0xCC:
			case 0xCD:
			case 0xCE:
			case 0xEC:
			case 0xED:
			case 0xEE:
				{
					pbuf.Append(string.Format("${0:X4}", ((int)mem[1]) + ((int)mem[2]) * 256));
					offset = 3;
					break;
				}
			// Absolute Indexed Indirect
			case 0x7C:
			case 0xFC:
				{
					pbuf.Append(string.Format("(${0:X4},x)", ((int)mem[1]) + ((int)mem[2]) * 256));
					offset = 3;
					break;
				}
			// Absolute Indexed, X
			case 0x1D:
			case 0x1E:
			case 0x3C:
			case 0x3D:
			case 0x3E:
			case 0x5D:
			case 0x5E:
			case 0x7D:
			case 0x7E:
			case 0x9D:
			case 0x9E:
			case 0xBC:
			case 0xBD:
			case 0xDD:
			case 0xDE:
			case 0xFD:
			case 0xFE:
				{
					pbuf.Append(string.Format("${0:X4},x", ((int)mem[1]) + ((int)mem[2]) * 256));
					offset = 3;
					break;
				}
			// Absolute Indexed, Y
			case 0x19:
			case 0x39:
			case 0x59:
			case 0x79:
			case 0x99:
			case 0xB9:
			case 0xBE:
			case 0xD9:
			case 0xF9:
				{
					pbuf.Append(string.Format("${0:X4},y", ((int)mem[1]) + ((int)mem[2]) * 256));
					offset = 3;
					break;
				}
			// Absolute Indirect
			case 0x6C:
				{
					pbuf.Append(string.Format("(${0:X4})", ((int)mem[1]) + ((int)mem[2]) * 256));
					offset = 3;
					break;
				}
			// Absolute Indirect Long
			case 0xDC:
				{
					pbuf.Append(string.Format("[${0:X4}]", ((int)mem[1]) + ((int)mem[2]) * 256));
					offset = 3;
					break;
				}
			// Absolute Long
			case 0x0F:
			case 0x22:
			case 0x2F:
			case 0x4F:
			case 0x5C:
			case 0x6F:
			case 0x8F:
			case 0xAF:
			case 0xCF:
			case 0xEF:
				{
					pbuf.Append(string.Format("${0:X6}", ((int)mem[1]) + (((int)mem[2]) * 256) + (((int)mem[3]) * 65536)));
					offset = 4;
					break;
				}
			// Absolute Long Indexed, X
			case 0x1F:
			case 0x3F:
			case 0x5F:
			case 0x7F:
			case 0x9F:
			case 0xBF:
			case 0xDF:
			case 0xFF:
				{
					pbuf.Append(string.Format("${0:X6},x", ((int)mem[1]) + (((int)mem[2]) * 256) + (((int)mem[3]) * 65536)));
					offset = 4;
					break;
				}
			// Accumulator
			case 0x0A:
			case 0x1A:
			case 0x2A:
			case 0x3A:
			case 0x4A:
			case 0x6A:
				{
					pbuf.Append("a");
					offset = 1;
					break;
				}
			// Block Move
			case 0x44:
			case 0x54:
				{
					pbuf.Append(string.Format("${0:X2},${0:X2}", ((int)mem[1]), ((int)mem[2])));
					offset = 3;
					break;
				}
			// Direct Page
			case 0x04:
			case 0x05:
			case 0x06:
			case 0x14:
			case 0x24:
			case 0x25:
			case 0x26:
			case 0x45:
			case 0x46:
			case 0x64:
			case 0x65:
			case 0x66:
			case 0x84:
			case 0x85:
			case 0x86:
			case 0xA4:
			case 0xA5:
			case 0xA6:
			case 0xC4:
			case 0xC5:
			case 0xC6:
			case 0xE4:
			case 0xE5:
			case 0xE6:
				{
					pbuf.Append(string.Format("${0:X2}", (int)mem[1]));
					offset = 2;
					break;
				}
			// Direct Page Indexed, X
			case 0x15:
			case 0x16:
			case 0x34:
			case 0x35:
			case 0x36:
			case 0x55:
			case 0x56:
			case 0x74:
			case 0x75:
			case 0x76:
			case 0x94:
			case 0x95:
			case 0xB4:
			case 0xB5:
			case 0xD5:
			case 0xD6:
			case 0xF5:
			case 0xF6:
				{
					pbuf.Append(string.Format("${0:X2},x", (int)mem[1]));
					offset = 2;
					break;
				}
			// Direct Page Indexed, Y
			case 0x96:
			case 0xB6:
				{
					pbuf.Append(string.Format("${0:X2},y", (int)mem[1]));
					offset = 2;
					break;
				}
			// Direct Page Indirect
			case 0x12:
			case 0x32:
			case 0x52:
			case 0x72:
			case 0x92:
			case 0xB2:
			case 0xD2:
			case 0xF2:
				{
					pbuf.Append(string.Format("(${0:X2})", (int)mem[1]));
					offset = 2;
					break;
				}
			// Direct Page Indirect Long
			case 0x07:
			case 0x27:
			case 0x47:
			case 0x67:
			case 0x87:
			case 0xA7:
			case 0xC7:
			case 0xE7:
				{
					pbuf.Append(string.Format("[${0:X2}]", (int)mem[1]));
					offset = 2;
					break;
				}
			// Direct Page Indexed Indirect, X
			case 0x01:
			case 0x21:
			case 0x41:
			case 0x61:
			case 0x81:
			case 0xA1:
			case 0xC1:
			case 0xE1:
				{
					pbuf.Append(string.Format("(${0:X2},x)", (int)mem[1]));
					offset = 2;
					break;
				}
			// Direct Page Indirect Indexed, Y
			case 0x11:
			case 0x31:
			case 0x51:
			case 0x71:
			case 0x91:
			case 0xB1:
			case 0xD1:
			case 0xF1:
				{
					pbuf.Append(string.Format("(${0:X2}),y", (int)mem[1]));
					offset = 2;
					break;
				}
			// Direct Page Indirect Long Indexed, Y
			case 0x17:
			case 0x37:
			case 0x57:
			case 0x77:
			case 0x97:
			case 0xB7:
			case 0xD7:
			case 0xF7:
				{
					pbuf.Append(string.Format("[${0:X2}],y", (int)mem[1]));
					offset = 2;
					break;
				}
			// Stack (Pull)
			case 0x28:
			case 0x2B:
			case 0x68:
			case 0x7A:
			case 0xAB:
			case 0xFA:
			// Stack (Push)
			case 0x08:
			case 0x0B:
			case 0x48:
			case 0x4B:
			case 0x5A:
			case 0x8B:
			case 0xDA:
			// Stack (RTL)
			case 0x6B:
			// Stack (RTS)
			case 0x60:
			// Stack/RTI
			case 0x40:
			// Implied
			case 0x18:
			case 0x1B:
			case 0x38:
			case 0x3B:
			case 0x58:
			case 0x5B:
			case 0x78:
			case 0x7B:
			case 0x88:
			case 0x8A:
			case 0x98:
			case 0x9A:
			case 0x9B:
			case 0xA8:
			case 0xAA:
			case 0xB8:
			case 0xBA:
			case 0xBB:
			case 0xC8:
			case 0xCA:
			case 0xCB:
			case 0xD8:
			case 0xDB:
			case 0xE8:
			case 0xEA:
			case 0xEB:
			case 0xF8:
			case 0xFB:
				offset = 1;
				break;
			// Program Counter Relative
			case 0x10:
			case 0x30:
			case 0x50:
			case 0x70:
			case 0x80:
			case 0x90:
			case 0xB0:
			case 0xD0:
			case 0xF0:
				{
					// Calculate the signed value of the param
					sval = (mem[1] > 127) ? (mem[1] - 256) : mem[1];
					pbuf.Append(string.Format("${0:X4}", (pos + sval + 2) & 0xffff));
					offset = 2;
					break;
				}
			// Stack (Program Counter Relative Long)
			case 0x62:
			// Program Counter Relative Long
			case 0x82:
				{
					// Calculate the signed value of the param
					sval = ((int)mem[1]) + (((int)mem[2]) * 256);
					sval = (sval > 32767) ? (sval - 65536) : sval;
					pbuf.Append(string.Format("${0:X4}", (pos + sval + 3) & 0xffff));
					offset = 3;
					break;
				}
			// Stack Relative Indirect Indexed, Y
			case 0x13:
			case 0x33:
			case 0x53:
			case 0x73:
			case 0x93:
			case 0xB3:
			case 0xD3:
			case 0xF3:
				{
					pbuf.Append(string.Format("(${0:X2},s),y", (int)mem[1]));
					offset = 2;
					break;
				}
			// Stack (Absolute)
			case 0xF4:
				{
					pbuf.Append(string.Format("${0:X4}", ((int)mem[1]) + (((int)mem[2]) * 256)));
					offset = 3;
					break;
				}
			// Stack (Direct Page Indirect)
			case 0xD4:
				{
					pbuf.Append(string.Format("(${0:X2})", ((int)mem[1])));
					offset = 2;
					break;
				}
			// Stack Relative
			case 0x03:
			case 0x23:
			case 0x43:
			case 0x63:
			case 0x83:
			case 0xA3:
			case 0xC3:
			case 0xE3:
				{
					pbuf.Append(string.Format("${0:X2},s", (int)mem[1]));
					offset = 2;
					break;
				}
			// WDM mode
			case 0x42:
			// Stack/Interrupt
			case 0x00:
			case 0x02:
				{
					pbuf.Append(string.Format("${0:X2}", (int)mem[1]));
					offset = 2;
					break;
				}
			// Immediate (Invariant)
			case 0xC2:
				// REP following
				{
					flag = (byte)((int)flag & ~mem[1]);
					pbuf.Append(string.Format("#${0:X2}", (int)mem[1]));
					offset = 2;
					break;
				}
			case 0xE2:
				// SEP following
				{
					flag = (byte)((int)flag | mem[1]);
					pbuf.Append(string.Format("#${0:X2}", (int)mem[1]));
					offset = 2;
					break;
				}
			// Immediate (A size dependent)
			case 0x09:
			case 0x29:
			case 0x49:
			case 0x69:
			case 0x89:
			case 0xA9:
			case 0xC9:
			case 0xE9:
				if ((flag & 0x20) != 0)
				{
					// 8Bit A
					pbuf.Append(string.Format("#${0:X2}", (int)mem[1]));
					offset = 2;
				}
				else
				{
					// 16Bit A
					pbuf.Append(string.Format("#${0:X4}", ((int)mem[1]) + (((int)mem[2]) * 256)));
					offset = 3;
				}
				break;
			// Immediate (X/Y size dependent)
			case 0xA0:
			case 0xA2:
			case 0xC0:
			case 0xE0:
				if (((flag) & 0x10) != 0)
				{
					// 8Bit index reg
					pbuf.Append(string.Format("#${0:X2}", (int)mem[1]));
					offset = 2;
				}
				else
				{
					// 16Bit index reg
					pbuf.Append(string.Format("#${0:X4}", ((int)mem[1]) + (((int)mem[2]) * 256)));
					offset = 3;
				}
				break;
			default:
				pbuf.Append(string.Format("Unhandled Addressing Mode: {0:2X}" + Environment.NewLine, mem[0]));
				return -1;
		};


		// Generate hex output
		for (i = 0; i < offset; i++)
		{
			hbuf.Append(string.Format("{0:X2}", mem[i]));
		}
		for (i = offset * 2; i < 8; i++)
		{
			hbuf.Append(" ");
		}

		// Generate whole disassembly line
		if ((tsrc & eDissassemblyOptions.DontOutputAddress) == 0)
		{
			inst.Append(string.Format("{0:X2}{1:X4}:\t{2}\t{3} {4}", (pos >> 16) & 0xFF, pos & 0xFFFF, hbuf.ToString(), ibuf.ToString(), pbuf.ToString()));
			string add = string.Format("{0:X2}{1:X4}", (pos >> 16) & 0xFF, pos & 0xFFFF);

			segments.Add(add);
            segments.Add(hbuf.ToString());
            segments.Add(ibuf.ToString());
            segments.Add(pbuf.ToString());
        }
        else
		{
			segments.Add("");
            segments.Add("");
            segments.Add(ibuf.ToString());
            segments.Add(pbuf.ToString());

            inst.Append(ibuf.ToString());
			inst.Append(" ");
			inst.Append(pbuf.ToString());
		}
		inst.AppendLine();

		return offset;
	}
}
