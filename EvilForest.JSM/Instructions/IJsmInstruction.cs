﻿﻿using System;
using System.Collections.Generic;
using System.Linq;
using Albeoris.Framework.Collections;
 using FF8.Core;
using FF8.JSM.Format;
using Memoria.EventEngine.EV;

 namespace FF8.JSM.Instructions
{
    public abstract class JsmInstruction : IJsmInstruction, IFormattableScript
    {
        public Boolean IsStandalone { get; set; }

        protected JsmInstruction()
        {
        }

        public virtual void Format(ScriptWriter sw, IScriptFormatterContext formatterContext, IServices services)
        {
            sw.AppendLine(this.ToString());
        }

        public virtual IAwaitable Execute(IServices services)
        {
            return TestExecute(services);
        }

        public virtual IAwaitable TestExecute(IServices services)
        {
            throw new NotImplementedException($"The instruction {GetType()} is not implemented yet. Please override \"{nameof(Execute)}\" method if you know the correct behavior or \"{nameof(TestExecute)}\" method for test environment.");
        }

        public static JsmInstruction TryMake(Jsm.Opcode opcode, EVScriptMaker maker, IStack<IJsmExpression> stack)
        {
            if (Factories.TryGetValue(opcode, out var make))
            {
                JsmInstructionReader reader = new JsmInstructionReader(maker, stack);
                return make(reader);
            }

            return null;
        }


        private delegate JsmInstruction Make(JsmInstructionReader reader);

        private static readonly Dictionary<Jsm.Opcode, Make> Factories = new Dictionary<Jsm.Opcode, Make>
        {
            {Jsm.Opcode.NOP, NOP.Create},
            {Jsm.Opcode.JMP, JMP.Create},
            {Jsm.Opcode.JMP_IF, r => JMP_IF.Create(false, r)},
            {Jsm.Opcode.JMP_IFN, r => JMP_IF.Create(true, r)},
            {Jsm.Opcode.Return, JsmReturn.Create},
//{Jsm.Opcode.EXPR, EXPR.Create},
//{Jsm.Opcode.JMP_SWITCHEX, JMP_SWITCHEX.Create},
            {Jsm.Opcode.NEW, NEW.Create},
            {Jsm.Opcode.NEW2, NEW2.Create},
            {Jsm.Opcode.NEW3, NEW3.Create},
//{Jsm.Opcode.pad0a, pad0a.Create},
            {Jsm.Opcode.JMP_SWITCHEX, r => JMP_SWITCH.Create(r, isEx: true)},
            {Jsm.Opcode.JMP_SWITCH, r => JMP_SWITCH.Create(r, isEx: false)},
//{Jsm.Opcode.rsv0c, rsv0c.Create},
//{Jsm.Opcode.rsv0d, rsv0d.Create},
//{Jsm.Opcode.pad0e, pad0e.Create},
//{Jsm.Opcode.pad0f, pad0f.Create},
            {Jsm.Opcode.REQ, REQ.Create},
//{Jsm.Opcode.pad11, pad11.Create},
            {Jsm.Opcode.REQSW, REQSW.Create},
//{Jsm.Opcode.pad13, pad13.Create},
            {Jsm.Opcode.REQEW, REQEW.Create},
//{Jsm.Opcode.pad15, pad15.Create},
            {Jsm.Opcode.REPLY, REPLY.Create},
//{Jsm.Opcode.pad17, pad17.Create},
            {Jsm.Opcode.REPLYSW, REPLYSW.Create},
//{Jsm.Opcode.pad19, pad19.Create},
            {Jsm.Opcode.REPLYEW, REPLYEW.Create},
            {Jsm.Opcode.SONGFLAG, SONGFLAG.Create},
            {Jsm.Opcode.DELETE, DELETE.Create},
            {Jsm.Opcode.POS, POS.Create},
            {Jsm.Opcode.BGVPORT, BGVPORT.Create},
            {Jsm.Opcode.MES, MES.Create},
            {Jsm.Opcode.MESN, MESN.Create},
            {Jsm.Opcode.CLOSE, CLOSE.Create},
            {Jsm.Opcode.WAIT, WAIT.Create},
            {Jsm.Opcode.MOVE, MOVE.Create},
            {Jsm.Opcode.MOVA, MOVA.Create},
            {Jsm.Opcode.CLRDIST, CLRDIST.Create},
            {Jsm.Opcode.MSPEED, MSPEED.Create},
            {Jsm.Opcode.BGIMASK, BGIMASK.Create},
            {Jsm.Opcode.FMV, FMV.Create},
            {Jsm.Opcode.QUAD, QUAD.Create},
            {Jsm.Opcode.ENCOUNT, ENCOUNT.Create},
            {Jsm.Opcode.MAPJUMP, MAPJUMP.Create},
            {Jsm.Opcode.CC, CC.Create},
            {Jsm.Opcode.UCOFF, UCOFF.Create},
            {Jsm.Opcode.UCON, UCON.Create},
            {Jsm.Opcode.MODEL, MODEL.Create},
            {Jsm.Opcode.PRINT1, PRINT1.Create},
            {Jsm.Opcode.PRINTF, PRINTF.Create},
            {Jsm.Opcode.LOCATE, LOCATE.Create},
            {Jsm.Opcode.AIDLE, AIDLE.Create},
            {Jsm.Opcode.AWALK, AWALK.Create},
            {Jsm.Opcode.ARUN, ARUN.Create},
            {Jsm.Opcode.DIRE, DIRE.Create},
            {Jsm.Opcode.ROTXZ, ROTXZ.Create},
            {Jsm.Opcode.BTLCMD, BTLCMD.Create},
            {Jsm.Opcode.MESHSHOW, MESHSHOW.Create},
            {Jsm.Opcode.MESHHIDE, MESHHIDE.Create},
            {Jsm.Opcode.OBJINDEX, OBJINDEX.Create},
            {Jsm.Opcode.ENCSCENE, ENCSCENE.Create},
            {Jsm.Opcode.AFRAME, AFRAME.Create},
            {Jsm.Opcode.ASPEED, ASPEED.Create},
            {Jsm.Opcode.AMODE, AMODE.Create},
            {Jsm.Opcode.ANIM, ANIM.Create},
            {Jsm.Opcode.WAITANIM, WAITANIM.Create},
            {Jsm.Opcode.ENDANIM, ENDANIM.Create},
            {Jsm.Opcode.STARTSEQ, STARTSEQ.Create},
            {Jsm.Opcode.WAITSEQ, WAITSEQ.Create},
            {Jsm.Opcode.ENDSEQ, ENDSEQ.Create},
            {Jsm.Opcode.DEBUGCC, DEBUGCC.Create},
            {Jsm.Opcode.NECKFLAG, NECKFLAG.Create},
            {Jsm.Opcode.ITEMADD, ITEMADD.Create},
            {Jsm.Opcode.ITEMDELETE, ITEMDELETE.Create},
            {Jsm.Opcode.BTLSET, BTLSET.Create},
            {Jsm.Opcode.RADIUS, RADIUS.Create},
            {Jsm.Opcode.ATTACH, ATTACH.Create},
            {Jsm.Opcode.DETACH, DETACH.Create},
            {Jsm.Opcode.WATCH, WATCH.Create},
            {Jsm.Opcode.STOP, STOP.Create},
            {Jsm.Opcode.WAITTURN, WAITTURN.Create},
            {Jsm.Opcode.TURNA, TURNA.Create},
            {Jsm.Opcode.ASLEEP, ASLEEP.Create},
            {Jsm.Opcode.NOINITMES, NOINITMES.Create},
            {Jsm.Opcode.WAITMES, WAITMES.Create},
            {Jsm.Opcode.MROT, MROT.Create},
            {Jsm.Opcode.TURN, TURN.Create},
            {Jsm.Opcode.ENCRATE, ENCRATE.Create},
            {Jsm.Opcode.BGSMOVE, BGSMOVE.Create},
            {Jsm.Opcode.BGLCOLOR, BGLCOLOR.Create},
            {Jsm.Opcode.BGLMOVE, BGLMOVE.Create},
            {Jsm.Opcode.BGLACTIVE, BGLACTIVE.Create},
            {Jsm.Opcode.BGLLOOP, BGLLOOP.Create},
            {Jsm.Opcode.BGLPARALLAX, BGLPARALLAX.Create},
            {Jsm.Opcode.BGLORIGIN, BGLORIGIN.Create},
            {Jsm.Opcode.BGAANIME, BGAANIME.Create},
            {Jsm.Opcode.BGAACTIVE, BGAACTIVE.Create},
            {Jsm.Opcode.BGARATE, BGARATE.Create},
            {Jsm.Opcode.SETROW, SETROW.Create},
            {Jsm.Opcode.BGAWAIT, BGAWAIT.Create},
            {Jsm.Opcode.BGAFLAG, BGAFLAG.Create},
            {Jsm.Opcode.BGARANGE, BGARANGE.Create},
            {Jsm.Opcode.MESVALUE, MESVALUE.Create},
            {Jsm.Opcode.TWIST, TWIST.Create},
            {Jsm.Opcode.FICON, FICON.Create},
            {Jsm.Opcode.TIMERSET, TIMERSET.Create},
            {Jsm.Opcode.DASHOFF, DASHOFF.Create},
            {Jsm.Opcode.CLEARCOLOR, CLEARCOLOR.Create},
            {Jsm.Opcode.PPRINT, PPRINT.Create},
            {Jsm.Opcode.PPRINTF, PPRINTF.Create},
            {Jsm.Opcode.MAPID, MAPID.Create},
            {Jsm.Opcode.BGSSCROLL, BGSSCROLL.Create},
            {Jsm.Opcode.BGSRELEASE, BGSRELEASE.Create},
            {Jsm.Opcode.BGCACTIVE, BGCACTIVE.Create},
            {Jsm.Opcode.BGCHEIGHT, BGCHEIGHT.Create},
            {Jsm.Opcode.BGCLOCK, BGCLOCK.Create},
            {Jsm.Opcode.BGCUNLOCK, BGCUNLOCK.Create},
            {Jsm.Opcode.MENU, MENU.Create},
            {Jsm.Opcode.TRACKSTART, TRACKSTART.Create},
            {Jsm.Opcode.TRACK, TRACK.Create},
            {Jsm.Opcode.TRACKADD, TRACKADD.Create},
            {Jsm.Opcode.PRINTQUAD, PRINTQUAD.Create},
            {Jsm.Opcode.ATURNL, ATURNL.Create},
            {Jsm.Opcode.ATURNR, ATURNR.Create},
            {Jsm.Opcode.CHOOSEPARAM, CHOOSEPARAM.Create},
            {Jsm.Opcode.TIMERCONTROL, TIMERCONTROL.Create},
            {Jsm.Opcode.SETCAM, SETCAM.Create},
            {Jsm.Opcode.SHADOWON, SHADOWON.Create},
            {Jsm.Opcode.SHADOWOFF, SHADOWOFF.Create},
            {Jsm.Opcode.SHADOWSCALE, SHADOWSCALE.Create},
            {Jsm.Opcode.SHADOWOFFSET, SHADOWOFFSET.Create},
            {Jsm.Opcode.SHADOWLOCK, SHADOWLOCK.Create},
            {Jsm.Opcode.SHADOWUNLOCK, SHADOWUNLOCK.Create},
            {Jsm.Opcode.SHADOWAMP, SHADOWAMP.Create},
            {Jsm.Opcode.IDLESPEED, IDLESPEED.Create},
            {Jsm.Opcode.DDIR, DDIR.Create},
            {Jsm.Opcode.CHRFX, CHRFX.Create},
            {Jsm.Opcode.SEPV, SEPV.Create},
            {Jsm.Opcode.SEPVA, SEPVA.Create},
            {Jsm.Opcode.NECKID, NECKID.Create},
            {Jsm.Opcode.ENCOUNT2, ENCOUNT2.Create},
            {Jsm.Opcode.TIMERDISPLAY, TIMERDISPLAY.Create},
            {Jsm.Opcode.RAISE, RAISE.Create},
            {Jsm.Opcode.CHRCOLOR, CHRCOLOR.Create},
            {Jsm.Opcode.SLEEPINH, SLEEPINH.Create},
            {Jsm.Opcode.AUTOTURN, AUTOTURN.Create},
            {Jsm.Opcode.BGLATTACH, BGLATTACH.Create},
            {Jsm.Opcode.CFLAG, CFLAG.Create},
            {Jsm.Opcode.AJUMP, AJUMP.Create},
            {Jsm.Opcode.MESA, MESA.Create},
            {Jsm.Opcode.MESAN, MESAN.Create},
            {Jsm.Opcode.DRET, DRET.Create},
            {Jsm.Opcode.MOVT, MOVT.Create},
            {Jsm.Opcode.TSPEED, TSPEED.Create},
            {Jsm.Opcode.BGIACTIVET, BGIACTIVET.Create},
            {Jsm.Opcode.TURNTO, TURNTO.Create},
            {Jsm.Opcode.PREJUMP, PREJUMP.Create},
            {Jsm.Opcode.POSTJUMP, POSTJUMP.Create},
            {Jsm.Opcode.MOVQ, MOVQ.Create},
            {Jsm.Opcode.CHRSCALE, CHRSCALE.Create},
            {Jsm.Opcode.MOVJ, MOVJ.Create},
            {Jsm.Opcode.POS3, POS3.Create},
            {Jsm.Opcode.MOVE3, MOVE3.Create},
            {Jsm.Opcode.DRADIUS, DRADIUS.Create},
            {Jsm.Opcode.MJPOS, MJPOS.Create},
            {Jsm.Opcode.MOVH, MOVH.Create},
            {Jsm.Opcode.SPEEDTH, SPEEDTH.Create},
            {Jsm.Opcode.TURNDS, TURNDS.Create},
            {Jsm.Opcode.BGI, BGI.Create},
            {Jsm.Opcode.GETSCREEN, GETSCREEN.Create},
            {Jsm.Opcode.MENUON, MENUON.Create},
            {Jsm.Opcode.MENUOFF, MENUOFF.Create},
            {Jsm.Opcode.DISCCHANGE, DISCCHANGE.Create},
            {Jsm.Opcode.DPOS3, DPOS3.Create},
            {Jsm.Opcode.MINIGAME, MINIGAME.Create},
            {Jsm.Opcode.DELETEALLCARD, DELETEALLCARD.Create},
            {Jsm.Opcode.SETMAPNAME, SETMAPNAME.Create},
            {Jsm.Opcode.RESETMAPNAME, RESETMAPNAME.Create},
            {Jsm.Opcode.PARTYMENU, PARTYMENU.Create},
            {Jsm.Opcode.SPS, SPS.Create},
            {Jsm.Opcode.FULLMEMBER, FULLMEMBER.Create},
            {Jsm.Opcode.PRETEND, PRETEND.Create},
            {Jsm.Opcode.WMAPJUMP, WMAPJUMP.Create},
            {Jsm.Opcode.EYE, EYE.Create},
            {Jsm.Opcode.AIM, AIM.Create},
            {Jsm.Opcode.SETKEYMASK, SETKEYMASK.Create},
            {Jsm.Opcode.CLEARKEYMASK, CLEARKEYMASK.Create},
            {Jsm.Opcode.DTURN, DTURN.Create},
            {Jsm.Opcode.DWAITTURN, DWAITTURN.Create},
            {Jsm.Opcode.DANIM, DANIM.Create},
            {Jsm.Opcode.DWAITANIM, DWAITANIM.Create},
            {Jsm.Opcode.DPOS, DPOS.Create},
            {Jsm.Opcode.TEXPLAY, TEXPLAY.Create},
            {Jsm.Opcode.TEXPLAY1, TEXPLAY1.Create},
            {Jsm.Opcode.TEXSTOP, TEXSTOP.Create},
            {Jsm.Opcode.BGVSET, BGVSET.Create},
            {Jsm.Opcode.WPRM, WPRM.Create},
            {Jsm.Opcode.FLDSND0, FLDSND0.Create},
            {Jsm.Opcode.FLDSND1, FLDSND1.Create},
            {Jsm.Opcode.FLDSND2, FLDSND2.Create},
            {Jsm.Opcode.FLDSND3, FLDSND3.Create},
            {Jsm.Opcode.BGVDEFINE, BGVDEFINE.Create},
            {Jsm.Opcode.BGAVISIBLE, BGAVISIBLE.Create},
            {Jsm.Opcode.BGIACTIVEF, BGIACTIVEF.Create},
            {Jsm.Opcode.CHRSET, CHRSET.Create},
            {Jsm.Opcode.CHRCLEAR, CHRCLEAR.Create},
            {Jsm.Opcode.GILADD, GILADD.Create},
            {Jsm.Opcode.GILDELETE, GILDELETE.Create},
            {Jsm.Opcode.MESB, MESB.Create},
            {Jsm.Opcode.GLOBALCLEAR, GLOBALCLEAR.Create},
            {Jsm.Opcode.DEBUGSAVE, DEBUGSAVE.Create},
            {Jsm.Opcode.DEBUGLOAD, DEBUGLOAD.Create},
            {Jsm.Opcode.ATTACHOFFSET, ATTACHOFFSET.Create},
            {Jsm.Opcode.PUSHHIDE, PUSHHIDE.Create},
            {Jsm.Opcode.POPSHOW, POPSHOW.Create},
            {Jsm.Opcode.AICON, AICON.Create},
            {Jsm.Opcode.RAIN, RAIN.Create},
            {Jsm.Opcode.CLEARSTATUS, CLEARSTATUS.Create},
            {Jsm.Opcode.SPS2, SPS2.Create},
            {Jsm.Opcode.WINPOSE, WINPOSE.Create},
            {Jsm.Opcode.JUMP3, JUMP3.Create},
            {Jsm.Opcode.PARTYDELETE, PARTYDELETE.Create},
            {Jsm.Opcode.PLAYERNAME, PLAYERNAME.Create},
            {Jsm.Opcode.OVAL, OVAL.Create},
            {Jsm.Opcode.INCFROG, INCFROG.Create},
            {Jsm.Opcode.BEND, BEND.Create},
            {Jsm.Opcode.SETVY3, SETVY3.Create},
            {Jsm.Opcode.SETSIGNAL, SETSIGNAL.Create},
            {Jsm.Opcode.BGLSCROLLOFFSET, BGLSCROLLOFFSET.Create},
            {Jsm.Opcode.BTLSEQ, BTLSEQ.Create},
            {Jsm.Opcode.BGLLOOPTYPE, BGLLOOPTYPE.Create},
            {Jsm.Opcode.BGAFRAME, BGAFRAME.Create},
            {Jsm.Opcode.MOVE3H, MOVE3H.Create},
            {Jsm.Opcode.SYNCPARTY, SYNCPARTY.Create},
            {Jsm.Opcode.VRP, VRP.Create},
            {Jsm.Opcode.CLOSEALL, CLOSEALL.Create},
            {Jsm.Opcode.WIPERGB, WIPERGB.Create},
            {Jsm.Opcode.BGVALPHA, BGVALPHA.Create},
            {Jsm.Opcode.SLEEPON, SLEEPON.Create},
            {Jsm.Opcode.HEREON, HEREON.Create},
            {Jsm.Opcode.DASHON, DASHON.Create},
            {Jsm.Opcode.SETHP, SETHP.Create},
            {Jsm.Opcode.SETMP, SETMP.Create},
            {Jsm.Opcode.CLEARAP, CLEARAP.Create},
            {Jsm.Opcode.MAXAP, MAXAP.Create},
            {Jsm.Opcode.GAMEOVER, GAMEOVER.Create},
            {Jsm.Opcode.VIBSTART, VIBSTART.Create},
            {Jsm.Opcode.VIBACTIVE, VIBACTIVE.Create},
            {Jsm.Opcode.VIBTRACK1, VIBTRACK1.Create},
            {Jsm.Opcode.VIBTRACK, VIBTRACK.Create},
            {Jsm.Opcode.VIBRATE, VIBRATE.Create},
            {Jsm.Opcode.VIBFLAG, VIBFLAG.Create},
            {Jsm.Opcode.VIBRANGE, VIBRANGE.Create},
            {Jsm.Opcode.HINT, HINT.Create},
            {Jsm.Opcode.JOIN, JOIN.Create},
            {Jsm.Opcode.EXT, EXT.Create}
        };
    }

    public interface IJsmInstruction
    {
    }

    public interface INameProvider
    {
        public Boolean TryGetDisplayName(IScriptFormatterContext formatterContext, out String displayName);
        
        public Boolean TryGetAsciiName(IScriptFormatterContext formatterContext, out String displayName)
        {
            if (TryGetDisplayName(formatterContext, out displayName))
            {
                displayName = new String(displayName.Where(Char.IsLetterOrDigit).ToArray());
                return true;
            }

            displayName = default;
            return false;

        }
    }
}