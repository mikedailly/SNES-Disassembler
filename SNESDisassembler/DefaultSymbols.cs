using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SNESDisassembler
{
    public class DefaultSymbols
    {
        public static SymbolManager Symbols;
        public static void Init(SymbolManager manager)
        {
            Symbols = manager;
        }

        public static void Add(string name, Int64 value, eSymbolType type = eSymbolType.Equate)
        {
            Symbols.Add(value, name, type);
        }


        public static void AddDefaults()
        {
            Add("IniDisp", 0x2100);
            Add("ScreenCtrl", 0x2100);
            Add("OBJsel", 0x2101);
            Add("OBJAttr", 0x2101);
            Add("OAMAddress", 0x2102);
            Add("OAMAddL", 0x2102);
            Add("OAMAddressL", 0x2102);
            Add("OAMAddH", 0x2103);
            Add("OAMAddressH",0x2103);
            Add("OAMData",0x2104);
            Add("BGMode",0x2105);
            Add("Mosaic",0x2106);
            Add("MosaicMode",0x2106);

            Add("BG1Sc",0x2107);
            Add("BG2Sc",0x2108);
            Add("BG3Sc",0x2109);
            Add("BG4Sc",0x210A);
            Add("BG12NBA",0x210B);
            Add("BG34NBA",0x210C);
            Add("BG1HOfs",0x210D);
            Add("BG1Hoffset",0x210D);
            Add("BG1VOfs",0x210E);
            Add("BG1Voffset",0x210E);
            Add("BG2Hofs",0x210F);
            Add("BG2Hoffset",0x210F);
            Add("BG2Voffset",0x2110);
            Add("BG2Vofs",0x2110);
            Add("BG3Hofs",0x2111);
            Add("BG3Hoffset",0x2111);
            Add("BG3Vofs",0x2112);
            Add("BG3Voffset",0x2112);
            Add("BG4Hofs",0x2113);
            Add("BG4Hoffset",0x2113);
            Add("BG4Vofs",0x2114);
            Add("BG4Voffset",0x2114);
            Add("VMainC",0x2115);
            Add("VRAMAddrSM",0x2115);
            Add("VRAMAddress",0x2116);
            Add("VMAddL",0x2116);
            Add("VRAMAddL",0x2116);
            Add("VRAMAdd",0x2117);
            Add("VMAddH",0x2117);
            Add("VRAMData",0x2118);
            Add("VRAMDataL",0x2118);
            Add("VMDataL",0x2118);
            Add("VRAMDataH",0x2119);
            Add("VMDataH",0x2119);

            Add("M7Sel",0x211A);
            Add("VRAMFlipOver",0x211A);
            Add("Xply1",0x211B);
            Add("Xply16",0x211B);
            Add("M7A",0x211B);
            Add("MatParamA",0x211B);
            Add("Xply2",0x211C);
            Add("Xply8",0x211C);
            Add("M7B",0x211C);
            Add("MatParamB",0x211C);
            Add("M7C",0x211D);
            Add("MatParamC",0x211D);
            Add("M7D",0x211E);
            Add("MatParamD",0x211E);
            Add("M7X",0x211F);
            Add("CentrePosX",0x211F);
            Add("M7Y",0x2120);
            Add("CentrePosY",0x2120);
            Add("CGAdd",0x2121);
            Add("CGRAMAddress",0x2121);
            Add("CGRAMData",0x2122);
            Add("CGData",0x2122);
            Add("W12sel",0x2123);
            Add("BG12Window",0x2123);
            Add("W34sel",0x2124);
            Add("BG34Window",0x2124);
            Add("WOBJsel",0x2125);
            Add("ColorWindow",0x2125);

            Add("Wh0",0x2126);
            Add("WinH0Position",0x2126);
            Add("Wh1",0x2127);
            Add("WinH1Position",0x2127);
            Add("Wh2",0x2128);
            Add("WinH2Position",0x2128);
            Add("Wh3",0x2129);
            Add("WinH3Position",0x2129);
            Add("WBglog",0x212a);
            Add("WinLogic1",0x212a);
            Add("Wobjlog",0x212b);
            Add("WinLogic2",0x212b);


            Add("TM",0x212c);
            Add("ThruMain",0x212c);
            Add("TS",0x212d);
            Add("ThruSub",0x212d);
            Add("TMW",0x212e);
            Add("ThruMainWin",0x212e);
            Add("TSW",0x212f);
            Add("ThruSubWin",0x212f);

            Add("CGSWSel",0x2130);
            Add("WinControl",0x2130);
            Add("CGAdSub",0x2131);
            Add("ADDSUBEnable",0x2131);
            Add("ColData",0x2132);
            Add("ColConstData",0x2132);
            Add("SetIni",0x2133);
            Add("ColControl",0x2133);

            Add("Answer",0x2134);
            //Add("MPYLow		equ	Answer);
            Add("MPYMid",0x2135);
            Add("MPYHigh",0x2136);


            Add("SLHV",0x2137);
            //Add("HVLatchR",0x2137);
            Add("OAMDataR",0x2138);
            Add("VRAMDataR",0x2139);
            Add("VRAMDataHR",0x213a);
            Add("CGDataR",0x213b);
            Add("OPhct",0x213c);
            Add("OPvct",0x213d);
            Add("STAT77",0x213e);
            Add("STAT78",0x213f);
            Add("APUIO0",0x2140);
            Add("APUIO1",0x2141);
            Add("APUIO2",0x2142);
            Add("APUIO3",0x2143);
            Add("WMData",0x2180);
            Add("WMadd",0x2181);
            Add("WMaddM",0x2182);
            Add("WMaddH",0x2183);
            Add("PORT0",0x02140);
            Add("PORT1",0x02141);
            Add("PORT2",0x02142);
            Add("PORT3", 0x02143);

            // Write only registers
            Add("NMITimEn",0x4200);
            Add("IntEnable",0x4200);
            Add("WRIO",0x4201);
            Add("IOPortOut",0x4201);
            Add("WrMpyA", 0x4202);
            Add("MultiplicandA",0x4202);
            Add("WrMpyB",0x4203);
            Add("MultiplicandB",0x4203);
            Add("WRdivL",0x4204);
            Add("Div16",0x4204);
            Add("DividendCL",0x4204);
            Add("WRdivM",0x4205);
            Add("DividendCH",0x4205);
            Add("Div8",0x4206);
            Add("WRdivH",0x4206);
            Add("DivisorB",0x4206);
            Add("WrDivB", 0x4206);

            Add("HCountTimerL	",0x4207);
            Add("HCountTimerH",0x4208);
            Add("VCountTimerL",0x4209);
            Add("VCountTimerH",0x420a);
            Add("MDMAen",0x420b);
            Add("HDMAEnable",0x420c);
            Add("MemSel",0x420d);

            // Read only registers
            Add("RdNMI",0x4210);
            Add("BlankFlagR",0x4210);
            Add("TimeUp",0x4211);
            Add("TimerFlagR",0x4211);
            Add("HVBjoy",0x4212);
            Add("VHBlankFlagR",0x4212);
            Add("IOPortIn",0x4213);
            Add("QuotentLR",0x4214);
            Add("QuotentHR",0x4215);
            Add("ProductLR",0x4216);
            Add("ProductHR",0x4217);
            Add("JoyCtrl1LR",0x4218);
            Add("JoyCtrl1HR",0x4219);
            Add("JoyCtrl2LR",0x421a);
            Add("JoyCtrl2HR",0x421b);
            Add("JoyCtrl3LR",0x421c);
            Add("JoyCtrl3HR",0x421d);
            Add("JoyCtrl4LR",0x421e);
            Add("JoyCtrl4HR", 0x421f);


            // DMA Channel control
            Add("CH0Ctrl",0x4300);			
            Add("CH0BAddress",0x4301);			
            Add("CH0A1TableAddrL",0x4302);			
            Add("CH0A1TableAddrH",0x4303);			
            Add("CH0A1TableBank",0x4304);			
            Add("CH0DataSizeL",0x4305);			
            Add("CH0DataSizeH",0x4306);			
            Add("CH0DataBank",0x4307);			
            Add("CH0A2TableAddrL",0x4308);			
            Add("CH0A2TableAddrH",0x4309);			
            Add("CH0A2LineNumber",0x430a);			



            Add("CH1Ctrl",0x4310);
            Add("CH1BAddress",0x4311);			
            Add("CH1A1TableAddrL",0x4312);			
            Add("CH1A1TableAddrH",0x4313);			
            Add("CH1A1TableBank",0x4314);			
            Add("CH1DataSizeL",0x4315);			
            Add("CH1DataSizeH",0x4316);			
            Add("CH1DataBank",0x4317);			
            Add("CH1A2TableAddrL",0x4318);			
            Add("CH1A2TableAddrH",0x4319);			
            Add("CH1A2LineNumber",0x431a);			

            Add("CH2Ctrl",0x4320);			
            Add("CH2BAddress",0x4321);			
            Add("CH2A1TableAddrL",0x4322);			
            Add("CH2A1TableAddrH",0x4323);			
            Add("CH2A1TableBank",0x4324);			
            Add("CH2DataSizeL",0x4325);			
            Add("CH2DataSizeH",0x4326);			
            Add("CH2DataBank",0x4327);			
            Add("CH2A2TableAddrL",0x4328);			
            Add("CH2A2TableAddrH",0x4329);			
            Add("CH2A2LineNumber",0x432a);			

            Add("CH3Ctrl",0x4330);			
            Add("CH3BAddress",0x4331);			
            Add("CH3A1TableAddrL",0x4332);			
            Add("CH3A1TableAddrH",0x4333);			
            Add("CH3A1TableBank",0x4334);			
            Add("CH3DataSizeL",0x4335);			
            Add("CH3DataSizeH",0x4336);			
            Add("CH3DataBank",0x4337);			
            Add("CH3A2TableAddrL",0x4338);			
            Add("CH3A2TableAddrH",0x4339);			
            Add("CH3A2LineNumber",0x433a);			

            Add("CH4Ctrl",0x4340);			
            Add("CH4BAddress",0x4341);			
            Add("CH4A1TableAddrL",0x4342);			
            Add("CH4A1TableAddrH",0x4343);			
            Add("CH4A1TableBank",0x4344);			
            Add("CH4DataSizeL",0x4345);			
            Add("CH4DataSizeH",0x4346);			
            Add("CH4DataBank",0x4347);			
            Add("CH4A2TableAddrL",0x4348);			
            Add("CH4A2TableAddrH",0x4349);			
            Add("CH4A2LineNumber",0x434a);			

            Add("CH5Ctrl",0x4350);			
            Add("CH5BAddress",0x4351);			
            Add("CH5A1TableAddrL",0x4352);			
            Add("CH5A1TableAddrH",0x4353);			
            Add("CH5A1TableBank",0x4354);			
            Add("CH5DataSizeL",0x4355);			
            Add("CH5DataSizeH",0x4356);			
            Add("CH5DataBank",0x4357);			
            Add("CH5A2TableAddrL",0x4358);			
            Add("CH5A2TableAddrH",0x4359);			
            Add("CH5A2LineNumber",0x435a);			

            Add("CH6Ctrl",0x4360);			
            Add("CH6BAddress",0x4361);			
            Add("CH6A1TableAddrL",0x4362);			
            Add("CH6A1TableAddrH",0x4363);			
            Add("CH6A1TableBank",0x4364);			
            Add("CH6DataSizeL",0x4365);			
            Add("CH6DataSizeH",0x4366);			
            Add("CH6DataBank",0x4367);			
            Add("CH6A2TableAddrL",0x4368);			
            Add("CH6A2TableAddrH",0x4369);			
            Add("CH6A2LineNumber",0x436a);			

            Add("CH7Ctrl",0x4370);			
            Add("CH7BAddress",0x4371);			
            Add("CH7A1TableAddrL",0x4372);			
            Add("CH7A1TableAddrH",0x4373);			
            Add("CH7A1TableBank",0x4374);			
            Add("CH7DataSizeL",0x4375);			
            Add("CH7DataSizeH",0x4376);			
            Add("CH7DataBank",0x4377);			
            Add("CH7A2TableAddrL",0x4378);			
            Add("CH7A2TableAddrH",0x4379);			
            Add("CH7A2LineNumber",0x437a);			

            Add("SNASMTrapEntry",0x708008);
            Add("SNASMBreakEntry",0x70800b);

            Add("BRKVector",0xffe6);
            Add("NMIVector",0xffea);
            Add("RSTVector",0xfffc);
            Add("IRQVector",0xfffe);
            Add("ResetVector",0xfffc);
            Add("PPU",0x2100);
        }
    }
}
