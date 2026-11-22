using System;
using System.IO;
using FF8.JSM;

namespace Memoria.EventEngine.EV
{
    public static class EVOperands
    {
        // m prefixes a VM argument mask. Digits are unsigned widths, a-d signed widths.
        // Layouts cross-checked against Hades Workshop Database_Script.cpp and the EV corpus.
        public static String For(Jsm.Opcode opcode) => opcode switch
        {
            Jsm.Opcode.NOP or Jsm.Opcode.Return or Jsm.Opcode.pad0a or Jsm.Opcode.rsv0c or Jsm.Opcode.pad0e or Jsm.Opcode.pad0f or
            Jsm.Opcode.pad11 or Jsm.Opcode.pad13 or Jsm.Opcode.pad15 or Jsm.Opcode.pad17 or Jsm.Opcode.pad19 or Jsm.Opcode.CLRDIST or
            Jsm.Opcode.CC or Jsm.Opcode.UCOFF or Jsm.Opcode.UCON or Jsm.Opcode.PRINT1 or Jsm.Opcode.PRINTF or Jsm.Opcode.WAITANIM or
            Jsm.Opcode.ENDANIM or Jsm.Opcode.WAITSEQ or Jsm.Opcode.ENDSEQ or Jsm.Opcode.DEBUGCC or Jsm.Opcode.WATCH or Jsm.Opcode.WAITTURN or
            Jsm.Opcode.NOINITMES or Jsm.Opcode.DASHOFF or Jsm.Opcode.PPRINT or Jsm.Opcode.PPRINTF or Jsm.Opcode.MAPID or Jsm.Opcode.BGCLOCK or
            Jsm.Opcode.BGCUNLOCK or Jsm.Opcode.TRACKADD or Jsm.Opcode.PRINTQUAD or Jsm.Opcode.SHADOWON or Jsm.Opcode.SHADOWOFF or Jsm.Opcode.SHADOWUNLOCK or
            Jsm.Opcode.RAISE or Jsm.Opcode.SLEEPINH or Jsm.Opcode.PREJUMP or Jsm.Opcode.POSTJUMP or Jsm.Opcode.MOVQ or Jsm.Opcode.MOVJ or
            Jsm.Opcode.MJPOS or Jsm.Opcode.MENUON or Jsm.Opcode.MENUOFF or Jsm.Opcode.DELETEALLCARD or Jsm.Opcode.RESETMAPNAME or Jsm.Opcode.EYE or
            Jsm.Opcode.AIM or Jsm.Opcode.GLOBALCLEAR or Jsm.Opcode.DEBUGSAVE or Jsm.Opcode.DEBUGLOAD or Jsm.Opcode.PUSHHIDE or Jsm.Opcode.POPSHOW or
            Jsm.Opcode.JUMP3 or Jsm.Opcode.INCFROG or Jsm.Opcode.BEND or Jsm.Opcode.SYNCPARTY or Jsm.Opcode.VRP or Jsm.Opcode.CLOSEALL or
            Jsm.Opcode.SLEEPON or Jsm.Opcode.DASHON or Jsm.Opcode.GAMEOVER => "",
            Jsm.Opcode.NEW or Jsm.Opcode.NEW2 or Jsm.Opcode.NEW3 => "11",
            Jsm.Opcode.REQ or Jsm.Opcode.REQSW or Jsm.Opcode.REQEW or Jsm.Opcode.ATTACH or Jsm.Opcode.BGAWAIT or Jsm.Opcode.BGARANGE or
            Jsm.Opcode.CLEARCOLOR or Jsm.Opcode.DTURN or Jsm.Opcode.VIBTRACK1 or Jsm.Opcode.VIBTRACK => "m111",
            Jsm.Opcode.REPLY or Jsm.Opcode.REPLYSW or Jsm.Opcode.REPLYEW or Jsm.Opcode.ROTXZ or Jsm.Opcode.MESHSHOW or Jsm.Opcode.MESHHIDE or
            Jsm.Opcode.AFRAME or Jsm.Opcode.AMODE or Jsm.Opcode.TURNA or Jsm.Opcode.TURN or Jsm.Opcode.BGLACTIVE or Jsm.Opcode.BGAANIME or
            Jsm.Opcode.BGAACTIVE or Jsm.Opcode.SETROW or Jsm.Opcode.BGAFLAG or Jsm.Opcode.MENU or Jsm.Opcode.DDIR or Jsm.Opcode.NECKID or
            Jsm.Opcode.TEXPLAY or Jsm.Opcode.TEXPLAY1 or Jsm.Opcode.TEXSTOP or Jsm.Opcode.BGVSET or Jsm.Opcode.BGAVISIBLE or Jsm.Opcode.BGIACTIVEF or
            Jsm.Opcode.RAIN or Jsm.Opcode.CLEARSTATUS or Jsm.Opcode.WINPOSE or Jsm.Opcode.BGLLOOPTYPE or Jsm.Opcode.BGAFRAME or Jsm.Opcode.CLEARAP or
            Jsm.Opcode.MAXAP or Jsm.Opcode.VIBRANGE or (Jsm.Opcode)0x102 or (Jsm.Opcode)0x103 or (Jsm.Opcode)0x104 or (Jsm.Opcode)0x106 or
            (Jsm.Opcode)0x108 or (Jsm.Opcode)0x10A => "m11",
            Jsm.Opcode.SONGFLAG or Jsm.Opcode.DELETE or Jsm.Opcode.CLOSE or Jsm.Opcode.WAIT or Jsm.Opcode.MOVA or Jsm.Opcode.MSPEED or
            Jsm.Opcode.BGIMASK or Jsm.Opcode.DIRE or Jsm.Opcode.BTLCMD or Jsm.Opcode.OBJINDEX or Jsm.Opcode.ASPEED or Jsm.Opcode.STARTSEQ or
            Jsm.Opcode.NECKFLAG or Jsm.Opcode.DETACH or Jsm.Opcode.WAITMES or Jsm.Opcode.MROT or Jsm.Opcode.ENCRATE or Jsm.Opcode.FICON or
            Jsm.Opcode.TIMERCONTROL or Jsm.Opcode.SETCAM or Jsm.Opcode.SHADOWAMP or Jsm.Opcode.TIMERDISPLAY or Jsm.Opcode.AUTOTURN or Jsm.Opcode.CFLAG or
            Jsm.Opcode.DRET or Jsm.Opcode.MOVT or Jsm.Opcode.TSPEED or Jsm.Opcode.SPEEDTH or Jsm.Opcode.TURNDS or Jsm.Opcode.BGI or
            Jsm.Opcode.GETSCREEN or Jsm.Opcode.PRETEND or Jsm.Opcode.DWAITTURN or Jsm.Opcode.DWAITANIM or Jsm.Opcode.PARTYDELETE or Jsm.Opcode.OVAL or
            Jsm.Opcode.BTLSEQ or Jsm.Opcode.VIBSTART or Jsm.Opcode.VIBACTIVE or Jsm.Opcode.VIBFLAG => "m1",
            Jsm.Opcode.POS or Jsm.Opcode.MOVE or Jsm.Opcode.TRACKSTART or Jsm.Opcode.TRACK or Jsm.Opcode.SHADOWOFFSET or Jsm.Opcode.TURNTO or
            Jsm.Opcode.MOVH => "mbb",
            Jsm.Opcode.BGVPORT => "m1bbbb",
            Jsm.Opcode.MES or Jsm.Opcode.MESN => "m112",
            Jsm.Opcode.FMV => "m1b1",
            Jsm.Opcode.ENCOUNT or Jsm.Opcode.HINT => "ma2",
            Jsm.Opcode.MAPJUMP or Jsm.Opcode.AIDLE or Jsm.Opcode.AWALK or Jsm.Opcode.ARUN or Jsm.Opcode.ANIM or Jsm.Opcode.ASLEEP or
            Jsm.Opcode.TIMERSET or Jsm.Opcode.ATURNL or Jsm.Opcode.ATURNR or Jsm.Opcode.DISCCHANGE or Jsm.Opcode.MINIGAME or Jsm.Opcode.FULLMEMBER or
            Jsm.Opcode.WMAPJUMP or Jsm.Opcode.CHRSET or Jsm.Opcode.CHRCLEAR or Jsm.Opcode.MESB or Jsm.Opcode.VIBRATE => "m2",
            Jsm.Opcode.MODEL or Jsm.Opcode.ITEMADD or Jsm.Opcode.ITEMDELETE or Jsm.Opcode.CHOOSEPARAM or Jsm.Opcode.BGIACTIVET => "m21",
            Jsm.Opcode.LOCATE or Jsm.Opcode.TWIST or Jsm.Opcode.SHADOWSCALE or (Jsm.Opcode)0x100 or (Jsm.Opcode)0x101 or (Jsm.Opcode)0x109 or
            (Jsm.Opcode)0x10B or (Jsm.Opcode)0x10C or (Jsm.Opcode)0x10E or (Jsm.Opcode)0x111 => "maa",
            Jsm.Opcode.ENCSCENE or Jsm.Opcode.BGVDEFINE => "m12222",
            Jsm.Opcode.BTLSET or Jsm.Opcode.BGARATE or Jsm.Opcode.WPRM => "m1b",
            Jsm.Opcode.RADIUS => "ma11",
            Jsm.Opcode.STOP or Jsm.Opcode.SHADOWLOCK or Jsm.Opcode.AICON or Jsm.Opcode.SETSIGNAL or Jsm.Opcode.HEREON => "ma",
            Jsm.Opcode.BGSMOVE or Jsm.Opcode.POS3 or Jsm.Opcode.MOVE3 or Jsm.Opcode.ATTACHOFFSET or Jsm.Opcode.MOVE3H => "mbbb",
            Jsm.Opcode.BGLCOLOR or Jsm.Opcode.IDLESPEED or Jsm.Opcode.CHRCOLOR => "m1111",
            Jsm.Opcode.BGLMOVE or Jsm.Opcode.CHRFX or Jsm.Opcode.DPOS3 => "m1bbb",
            Jsm.Opcode.BGLLOOP or Jsm.Opcode.BGLPARALLAX => "m11bb",
            Jsm.Opcode.BGLORIGIN or Jsm.Opcode.DPOS or Jsm.Opcode.BGVALPHA => "m1bb",
            Jsm.Opcode.MESVALUE or Jsm.Opcode.PARTYMENU or Jsm.Opcode.SETKEYMASK or Jsm.Opcode.CLEARKEYMASK or Jsm.Opcode.DANIM or Jsm.Opcode.PLAYERNAME or
            Jsm.Opcode.SETHP or Jsm.Opcode.SETMP or (Jsm.Opcode)0x105 => "m12",
            Jsm.Opcode.BGSSCROLL => "mbb11",
            Jsm.Opcode.BGSRELEASE => "ma1",
            Jsm.Opcode.BGCACTIVE => "m1a1",
            Jsm.Opcode.BGCHEIGHT or Jsm.Opcode.SETMAPNAME => "mb",
            Jsm.Opcode.SEPV => "mbbba",
            Jsm.Opcode.SEPVA => "m1a",
            Jsm.Opcode.ENCOUNT2 => "maa2",
            Jsm.Opcode.BGLATTACH => "m1baa111",
            Jsm.Opcode.AJUMP => "m2aa",
            Jsm.Opcode.MESA or Jsm.Opcode.MESAN => "m1112",
            Jsm.Opcode.CHRSCALE => "m1aaa",
            Jsm.Opcode.DRADIUS => "maaaa",
            Jsm.Opcode.SPS => "m11bbb",
            Jsm.Opcode.FLDSND0 => "m22",
            Jsm.Opcode.FLDSND1 => "m22c",
            Jsm.Opcode.FLDSND2 => "m22ca",
            Jsm.Opcode.FLDSND3 => "m22caa",
            Jsm.Opcode.GILADD or Jsm.Opcode.GILDELETE => "mc",
            Jsm.Opcode.SPS2 => "m111bb",
            Jsm.Opcode.SETVY3 => "mbbb1",
            Jsm.Opcode.BGLSCROLLOFFSET => "m11bb1",
            Jsm.Opcode.WIPERGB => "m111111",
            Jsm.Opcode.JOIN => "m11111",
            (Jsm.Opcode)0x107 => "m122",
            (Jsm.Opcode)0x10D => "mab",
            (Jsm.Opcode)0x10F or (Jsm.Opcode)0x110 => "maaa",
            _ => throw new InvalidDataException($"Opcode 0x{(Int32)opcode:X} has no fixed operand layout.")
        };

        public static Boolean UsesExpression(Jsm.Opcode opcode, Byte mask, Int32 index)
        {
            // The engine reads request tags and DELETE's UID with geti, bypassing getv1.
            Int32 operandLimit = opcode switch
            {
                Jsm.Opcode.REQ or Jsm.Opcode.REQSW or Jsm.Opcode.REQEW => 2,
                Jsm.Opcode.REPLY or Jsm.Opcode.REPLYSW or Jsm.Opcode.REPLYEW => 1,
                Jsm.Opcode.DELETE => 0,
                _ => 8
            };
            return index < operandLimit && (mask & (1 << index)) != 0;
        }

        public static Int32 Width(Char kind) => kind >= 'a' ? kind - 'a' + 1 : kind - '0';

        public static Boolean IsSigned(Char kind) => kind >= 'a';
    }
}
