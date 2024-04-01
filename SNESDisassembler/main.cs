/* main.c
 * DisPel 65816 Disassembler
 * James Churchill
 * Created 20000924
 */


using Microsoft.VisualBasic.Devices;
using System;
using System.Text;
using static System.Windows.Forms.VisualStyles.VisualStyleElement.ToolTip;

class SNESDissassembler
{
    public StringBuilder Dissassembly { get; set; } = new StringBuilder();
    public List<List<string>> Segments{ get; set; } = new List<List<string>>();

    Dissassemble65816 disasm = new Dissassemble65816();


    string usage()
	{
		return "\nDisPel v1 by James Churchill/pelrun (C)2001-2011\n" +
			"65816/SNES Disassembler\n" +
			"Usage: dispel [-n] [-t] [-h] [-l] [-s] [-i] [-a] [-x] [-e] [-p]\n" +
			"              [-b <bank>|-r <startaddr>-<endaddr>] [-g <origin>]\n" +
			"              [-d <width>] [-o <outfile>] <infile>\n\n" +
			"Options: (numbers are hex-only, no prefixes)\n" +
			" -n                Skip $200 byte SMC header\n" +
			" -t                Don't output addresses/hex dump.\n" +
			" -h/-l             Force HiROM/LoROM memory mapping.\n" +
			" -s/-i             Force enable/disable shadow ROM addresses (see readme.)\n" +
			" -a                Start in 8-bit accumulator mode. Default is 16-bit.\n" +
			" -x                Start in 8-bit X/Y mode. Default is 16-bit.\n" +
			" -e                Turn off bank-boundary enforcement. (see readme.)\n" +
			" -p                Split subroutines by placing blank lines after RTS,RTL,RTI\n" +
			" -b <bank>         Disassemble bank <bank> only. Overrides -r.\n" +
			" -r <start>-<end>  Disassemble block from <start> to <end>.\n" +
			"                     Omit -<end> to disassemble to end of file.\n" +
			" -g <origin>       Set origin of disassembled code (see readme.)\n" +
			" -d <width>        No disassembly - produce a hexdump with <width> bytes/line.\n" +
			" -o <outfile>      Set file to redirect output to. Default is stdout.\n" +
			" <infile>          File to disassemble.\n";
	}

	/* Snes9x Hi/LoROM autodetect code */

	int AllASCII(byte[] b, int index, int size)
	{
		int i;
		for (i = 0; i < size; i++)
		{
			if (b[index + i] < 32 || b[index + i] > 126)
			{
				return 0;
			}
		}
		return 1;
	}

	int ScoreHiROM(byte[] data)
	{
		int score = 0;

		if ((data[0xFFDC] + data[0xFFDD] * 256 + data[0xFFDE] + data[0xFFDF] * 256) == 0xFFFF)
		{
			score += 2;
		}

		if (data[0xFFDA] == 0x33)
		{
			score += 2;
		}
		if ((data[0xFFD5] & 0xf) < 4)
		{
			score += 2;
		}
		if ((data[0xFFFD] & 0x80) == 0)
		{
			score -= 4;
		}
		if ((1 << (data[0xFFD7] - 7)) > 48)
		{
			score -= 1;
		}
		if (AllASCII(data, 0xFFB0, 6) == 0)
		{
			score -= 1;
		}
		if (AllASCII(data, 0xFFC0, 20) == 0)
		{
			score -= 1;
		}

		return (score);
	}

	int ScoreLoROM(byte[] data)
	{
		int score = 0;

		if ((data[0x7FDC] + data[0x7FDD] * 256 + data[0x7FDE] + data[0x7FDF] * 256) == 0xFFFF)
		{
			score += 2;
		}
		if (data[0x7FDA] == 0x33)
		{
			score += 2;
		}
		if ((data[0x7FD5] & 0xf) < 4)
		{
			score += 2;
		}
		if ((data[0x7FFD] & 0x80) == 0)
		{
			score -= 4;
		}
		if ((1 << (data[0x7FD7] - 7)) > 48)
		{
			score -= 1;
		}
		if (AllASCII(data, 0x7FB0, 6) == 0)
		{
			score -= 1;
		}
		if (AllASCII(data, 0x7FC0, 20) == 0)
		{
			score -= 1;
		}

		return (score);
	}

	int hexdump(byte[] data, Int64 pos, Int64 rpos, Int64 len, StringBuilder inst, int dwidth)
	{
		int i;

		inst.Append(string.Format("{0:X2}/{1:X4}\t", (pos >> 16) & 0xFF, pos & 0xFFFF));
		inst.Append(" ");

		for (i = 0; i < dwidth && i + rpos < len; i++)
		{
			inst.Append(string.Format("{0:X2}", data[rpos + i]));
		}
		return dwidth;
	}

	public int main(string[] args)
	{
		string infile, outfile;
		StringBuilder inst = new StringBuilder();
		byte[] dmem = new byte[4];
		byte flag = 0;
		Int64 len, pos = 0, origin = 0x1000000, start = 0, end = 0, rpos;
		bool hirom = false;
		bool DontChangeHirom = false;
		bool shadow = false;
		bool DontChangeShadow = false;
		bool bound = true;
        eDissassemblyOptions tsrc = eDissassemblyOptions.None;
		UInt32 bank = 0x100, i;
		long tmp;
		int dwidth = 0,skip=0;
        long offset;
        int hiscore, loscore;

		outfile = "";

		// Parse the commandline

		if (args.Length < 2)
		{
			usage();
			Environment.Exit(1);
		}

		for (i = 1; i < (args.Length - 1); i++)
		{
			string arg = args[i];

			switch (args[i])
			{
				case "-n":
                    skip = 1;
					break;
				case "-t":
					tsrc = (eDissassemblyOptions) tsrc|eDissassemblyOptions.DontOutputAddress;
					break;
				case "-h":
					hirom = true;
					DontChangeHirom = true;
					break;
				case "-l":
					hirom = false;
					DontChangeHirom = true;
					break;
				case "-s":
					shadow = true;
					DontChangeShadow = true;
					break;
				case "-i":
					shadow = false;
					DontChangeShadow = true;
					break;
				case "-a":
					flag |= 0x20;
					break;
				case "-x":
					flag |= 0x10;
					break;
				case "-e":
					bound = false;
					break;
				case "-p":
                    tsrc = (eDissassemblyOptions)tsrc | eDissassemblyOptions.SplitFunctions;
					break;
				case "-d":
					i++;
					// No disassembly - produce HEXDUMP "x" bytes wide
					/*if ((sscanf(argv[i], "%2X", &dwidth) == 0) || dwidth==0)
					{
						usage();
						printf("\n-d requires a hex value between 01 and FF after it.\n");
						exit(1);
					}*/
					break;
				case "-b":
					// Disassemble bank 
					i++;
					/*if (sscanf(argv[i], "%2X", &bank) == 0)
					{
						usage();
						printf("\n-b requires a 1-byte hex value after it.\n");
						exit(1);
					}*/
					break;
				case "-r":
					// Disassemble from START to END
					i++;
					/*
					if (sscanf(argv[i], "%6lX-%6lX", &start, &end) == 0)
					{
						usage();
						printf("\n-a requires at least one hex value after it.\n");
						exit(1);
					}*/
					break;
				case "-g":
					// Origin
					i++;
					/*if (sscanf(argv[i], "%6lX", &origin) == 0)
					{
						usage();
						printf("\n-r requires one hex value after it.\n");
						exit(1);
					}*/
					break;
				case "-o":
					i++;
					outfile = args[i];
					//strcpy(outfile, argv[i]);
					break;
				default:
					usage();
					Console.WriteLine("\nUnknown option: " + args[i] + "\n");
					Environment.Exit(1);
					break;
			}
		}
		infile = args[args.Length - 1];

		// read the SNES ROM
		byte[]? data = null;
		try
		{
			data = File.ReadAllBytes(infile);
		}
		catch (Exception e)
		{
			Console.WriteLine("Error reading file - " + infile);
			Environment.Exit(1);
		}
		len = data.Length;

		// Skip SMC header?
		if (skip != 0)
		{
			byte[] NewData = new byte[data.Length - 0x200];
			Array.Copy(data, 0x200, NewData, 0, data.Length - 0x200);
			data = NewData;
        }


		// Autodetect the HiROM/LoROM state
		if (DontChangeHirom == false)
		{
			hiscore = ScoreHiROM(data);
			loscore = ScoreLoROM(data);
			if (hiscore > loscore)
			{
				//			fprintf(stderr,"Autodetected HiROM image.\n");
				hirom = true;
			}
			else
			{
				//			fprintf(stderr,"Autodetected LoROM image.\n");
				hirom = false;
			}
		}

		// Unmangle the address options

		pos = start;


		// Shadow ROM is a feature of the SNES hardware -the entire rom image is
		// mirrored at bank 80 onwards.When game code executes in those banks the
		// hardware runs at a higher clock - rate than when it's running at the lower
		// copy. (I think it's called FastROM and SlowROM in other documentation - for
		// obvious reasons.Note I'm not talking about Fast/SlowROM *protection* here.)

		// If shadow addresses given, convert to unshadowed and set shadow on.
		if (((bank == 0x100) && ((start & 0x800000) != 0)) || ((bank & 0x80) != 0))
		{
			shadow = true;
		}

		// If HiROM addresses given, convert to normal and set hirom on.
		if (((bank == 0x100) && ((start & 0x400000) != 0)) || ((bank & 0x40) != 0))
		{
			hirom = true;
		}
		bank &= 0x13F;
		start &= 0x3FFFFF;
		end &= 0x3FFFFF;


		start = 0x18000;
        end = 0x38000;

		// Autodetect shadow
		if (DontChangeShadow == false)
		{
			//		fprintf(stderr,"%02X\n",data[hirom?0xFFD5:0x7FD5]);
			int off = 0;
			if (hirom) off = 0xFFD5; else off = 0x7FD5;

			if ((data[off] & 0x30) != 0)
			{
				shadow = true;
			}
			else
			{
				shadow = false;
			}
		}

		// If the bank byte is set, apply it to the address range
		if (bank < 0x100)
		{
			if (hirom == true)
			{
				pos = bank << 16;
				start = pos;
				end = start | 0xFFFF;
			}
			else
			{
				pos = (bank << 16) + 0x8000;
				start = bank * 0x8000;
				end = start + 0x7FFF;
			}
		}
		else
		{
			if (hirom == false)
			{
				// Convert the addresses to offsets
				if ((start & 0xFFFF) < 0x8000)
				{
					start += 0x8000;
				}
				pos = start;
				start = ((start >> 16) & 0xFF) * 0x8000 + (start & 0x7FFF);
				end = ((end >> 16) & 0xFF) * 0x8000 + (end & 0x7FFF);
			}
		}

		// If end isn't after start, set end to end-of-file.
		if (end <= start)
		{
			end = len - 1;
		}

		// If new origin set, apply it.
		if (origin < 0x1000000)
		{
			pos = origin;
		}

		// If shadow set, apply it
		if (shadow)
		{
			pos |= 0x800000;
		}

		// If hirom, apply the mapping
		if (hirom)
		{
			pos |= 0x400000;
		}

		/*
		#ifdef _DEBUG
			fprintf(stderr,"Start: $%06X End: $%06X Pos: $%06X\n", start, end, pos);
			fprintf(stderr,"Input: %s\nOutput: %s\n", infile, outfile);
			if(shadow)
			{
				fprintf(stderr,"Autodetected FastROM.\n");
			}
			else
			{
				fprintf(stderr,"Autodetected SlowROM.\n");
			}
		#endif
		*/
		// Begin disassembly

		rpos = start;

		while (rpos < len && rpos <= end)
		{
			// copy some data to the staging area
			Array.Copy(data, rpos, dmem, 0, 4);
			//memcpy(dmem, data+rpos, 4);

			// disassemble one instruction, or produce one line of hexdump
			List<string> segments = new List<string>();
            if (dwidth == 0)
			{
				offset = disasm.disasm(dmem, pos, ref flag, inst, tsrc, out segments);
				Segments.Add(segments);
            }
			else
			{
				offset = hexdump(data, pos, rpos, len, inst, dwidth);
			}


			// Check for a file/block overrun
			if ((rpos + offset) > len || (rpos + offset) > (end + 1))
			{
				// print out remaining bytes and finish
				/*
				fprintf(fout, "%02lX/%04lX:\t", (pos >> 16) & 0xFF, pos & 0xFFFF);
				for (i = rpos; i < len && i <= end; i++)
				{
					fprintf(fout, "%02X", data[rpos]);
				}
				fprintf(fout, "\n");
				*/
				break;
			}

			// Check for a bank overrun
			if (bound && ((pos & 0xFFFF) + offset) > 0x10000)
			{
				// print out remaining bytes

				//fprintf(fout, "%02lX/%04lX:\t", (pos >> 16) & 0xFF, pos & 0xFFFF);
				tmp = 0x10000 - (pos & 0xffff);
				/*for (i = 0; i < tmp; i++)
				{
					fprintf(fout, "%02X", data[rpos + i]);
				}
				fprintf(fout, "\n");*/
				// Move to next bank
				if (!hirom)
				{
					pos = (pos & 0xFF0000) + 0x18000;
				}
				else
				{
					pos += tmp;
				}
				rpos += tmp;
				continue;
			}

			//fprintf(fout, "%s\n", inst);

			// Move to next instruction
			if (!hirom && ((pos & 0xFFFF) + offset) > 0xFFFF)
			{
				pos = (pos & 0xFF0000) + 0x18000;
			}
			else
			{
				pos += offset;
			}
			rpos += offset;
		}

        Dissassembly = inst;
        return 0;
	}
}

