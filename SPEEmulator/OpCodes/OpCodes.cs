using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SPEEmulator.OpCodes
{
    //This lists all supported operations on the SPU
    //RI10: "uint rt, uint ra, uint i10" "RT = rt; RA = ra; I10 = i10;"

    /// <summary>Load Quadword (d-form)</summary>
    public class lqd : Bases.RI10
    {
        public lqd() : base("0011 0100") { }
        public lqd(uint rt, uint ra, uint i10) : base("0011 0100") { RT = rt; RA = ra; I10 = i10; }
    }
    /// <summary>Load Quadword (x-form)</summary>
    public class lqx : Bases.RR
    {
        public lqx() : base("0011 1000 100") { }
        public lqx(uint rt, uint ra, uint rb) : base("0011 1000 100") { RT = rt; RA = ra; RB = rb; }
    }
    /// <summary>Load Quadword (a-form)</summary>
    public class lqa : Bases.RI16
    {
        public lqa() : base("0011 0000 1") { }
        public lqa(uint rt, uint i16) : base("0011 0000 1") { RT = rt; I16 = i16; }
    }
    /// <summary>Load Quadword Instruction Relative (a-form)</summary>
    public class lqr : Bases.RI16
    {
        public lqr() : base("0011 0011 1") { }
        public lqr(uint rt, uint i16) : base("0011 0011 1") { RT = rt; I16 = i16; }
    }
    /// <summary>Store Quadword (d-form)</summary>
    public class stqd : Bases.RI10
    {
        public stqd() : base("0010 0100") { }
        public stqd(uint rt, uint ra, uint i10) : base("0010 0100") { RT = rt; RA = ra; I10 = i10; }
    }
    /// <summary>Store Quadword (x-form)</summary>
    public class stqx : Bases.RR
    {
        public stqx() : base("0010 1000 100") { }
        public stqx(uint rt, uint ra, uint rb) : base("0010 1000 100") { RT = rt; RA = ra; RB = rb; }
    }
    /// <summary>Store Quadword (a-form)</summary>
    public class stqa : Bases.RI16
    {
        public stqa() : base("0010 0000 1") { }
        public stqa(uint rt, uint i16) : base("0010 0000 1") { RT = rt; I16 = i16; }
    }
    /// <summary>Store Quadword Instruction Relative (a-form)</summary>
    public class stqr : Bases.RI16
    {
        public stqr() : base("0010 0011 1") { }
        public stqr(uint rt, uint i16) : base("0010 0011 1") { RT = rt; I16 = i16; }
    }
    /// <summary>Generate Constrols for Byte Insertion (d-form)</summary>
    public class cbd : Bases.RI7
    {
        public cbd() : base("0011 1110 100") { }
        public cbd(uint rt, uint ra, uint i7) : base("0011 1110 100") { RT = rt; RA = ra; I7 = i7; }
    }
    /// <summary>Generate Constrols for Byte Insertion (x-form)</summary>
    public class cbx : Bases.RR
    {
        public cbx() : base("0011 1010 100") { }
        public cbx(uint rt, uint ra, uint rb) : base("0011 1010 100") { RT = rt; RA = ra; RB = rb; }
    }
    /// <summary>Generate Constrols for Halfword Insertion (d-form)</summary>
    public class chd : Bases.RI7
    {
        public chd() : base("0011 1110 101") { }
        public chd(uint rt, uint ra, uint i7) : base("0011 1110 101") { RT = rt; RA = ra; I7 = i7; }
    }
    /// <summary>Generate Constrols for Halfword Insertion (x-form)</summary>
    public class chx : Bases.RR
    {
        public chx() : base("0011 1010 101") { }
        public chx(uint rt, uint ra, uint rb) : base("0011 1010 101") { RT = rt; RA = ra; RB = rb; }
    }
    /// <summary>Generate Constrols for Word Insertion (d-form)</summary>
    public class cwd : Bases.RI7
    {
        public cwd() : base("0011 1110 110") { }
        public cwd(uint rt, uint ra, uint i7) : base("0011 1110 110") { RT = rt; RA = ra; I7 = i7; }
    }
    /// <summary>Generate Constrols for Word Insertion (x-form)</summary>
    public class cwx : Bases.RR
    {
        public cwx() : base("0011 1010 110") { }
        public cwx(uint rt, uint ra, uint rb) : base("0011 1010 110") { RT = rt; RA = ra; RB = rb; }
    }
    /// <summary>Generate Constrols for Doubleword Insertion (d-form)</summary>
    public class cdd : Bases.RI7
    {
        public cdd() : base("0011 1110 111") { }
        public cdd(uint rt, uint ra, uint i7) : base("0011 1110 111") { RT = rt; RA = ra; I7 = i7; }
    }
    /// <summary>Generate Constrols for Doubleword Insertion (x-form)</summary>
    public class cdx : Bases.RR
    {
        public cdx() : base("0011 1010 111") { }
        public cdx(uint rt, uint ra, uint rb) : base("0011 1010 111") { RT = rt; RA = ra; RB = rb; }
    }

    /// <summary>Immediate Load Halfword</summary>
    public class ilh : Bases.RI16
    {
        public ilh() : base("0100 0001 1") { }
        public ilh(uint rt, uint i16) : base("0100 0001 1") { RT = rt; I16 = i16; }
    }
    /// <summary>Immediate Load Halfword Upper</summary>
    public class ilhu : Bases.RI16
    {
        public ilhu() : base("0100 0001 0") { }
        public ilhu(uint rt, uint i16) : base("0100 0001 0") { RT = rt; I16 = i16; }
    }
    /// <summary>Immediate Load Word</summary>
    public class il : Bases.RI16
    {
        public il() : base("0100 0000 1") { }
        public il(uint rt, uint i16) : base("0100 0000 1") { RT = rt; I16 = i16; }
    }
    /// <summary>Immediate Load Address</summary>
    public class ila : Bases.RI18
    {
        public ila() : base("0100 001") { }
        public ila(uint rt, uint i18) : base("0100 001") { RT = rt; I18 = i18; }
    }
    /// <summary>Immediate Or Halfword Lower</summary>
    public class iohl : Bases.RI16
    {
        public iohl() : base("0110 0000 1") { }
        public iohl(uint rt, uint i16) : base("0110 0000 1") { RT = rt; I16 = i16; }
    }
    /// <summary>Form Select Mask for Bytes Immediate</summary>
    public class fsmbi : Bases.RI16
    {
        public fsmbi() : base("0011 0010 1") { }
        public fsmbi(uint rt, uint i16) : base("0011 0010 1") { RT = rt; I16 = i16; }
    }

    /// <summary>Add Halfword</summary>
    public class ah : Bases.RR
    {
        public ah() : base("0001 1001 000") { }
        public ah(uint rt, uint ra, uint rb) : base("0001 1001 000") { RT = rt; RA = ra; RB = rb; }
    }
    /// <summary>Add Halfword Immediate</summary>
    public class ahi : Bases.RI10
    {
        public ahi() : base("0001 1101") { }
        public ahi(uint rt, uint ra, uint i10) : base("0001 1101") { RT = rt; RA = ra; I10 = i10; }
    }
    /// <summary>Add Word</summary>
    public class a : Bases.RR
    {
        public a() : base("0001 1000 000") { }
        public a(uint rt, uint ra, uint rb) : base("0001 1000 000") { RT = rt; RA = ra; RB = rb; }
    }
    /// <summary>Add Word Immediate</summary>
    public class ai : Bases.RI10
    {
        public ai() : base("0001 1100") { }
        public ai(uint rt, uint ra, uint i10) : base("0001 1100") { RT = rt; RA = ra; I10 = i10; }
    }
    /// <summary>Substract from Halfword</summary>
    public class sfh : Bases.RR
    {
        public sfh() : base("0000 1001 000") { }
        public sfh(uint rt, uint ra, uint rb) : base("0000 1001 000") { RT = rt; RA = ra; RB = rb; }
    }
    /// <summary>Substract from Halword Immediate</summary>
    public class sfhi : Bases.RI10
    {
        public sfhi() : base("0000 1101") { }
        public sfhi(uint rt, uint ra, uint i10) : base("0000 1101") { RT = rt; RA = ra; I10 = i10; }
    }
    /// <summary>Substract from Word</summary>
    public class sf : Bases.RR
    {
        public sf() : base("0000 1000 000") { }
        public sf(uint rt, uint ra, uint rb) : base("0000 1000 000") { RT = rt; RA = ra; RB = rb; }
    }
    /// <summary></summary>
    public class sfi : Bases.RI10
    {
        public sfi() : base("0000 1100") { }
        public sfi(uint rt, uint ra, uint i10) : base("0000 1100") { RT = rt; RA = ra; I10 = i10; }
    }
    /// <summary>Add Extended</summary>
    public class addx : Bases.RR
    {
        public addx() : base("0110 1000 000") { }
        public addx(uint rt, uint ra, uint rb) : base("0110 1000 000") { RT = rt; RA = ra; RB = rb; }
    }
    /// <summary>Carry Generate</summary>
    public class cg : Bases.RR
    {
        public cg() : base("0001 1000 010") { }
        public cg(uint rt, uint ra, uint rb) : base("0001 1000 010") { RT = rt; RA = ra; RB = rb; }
    }
    /// <summary>Carry Generate Extended</summary>
    public class cgx : Bases.RR
    {
        public cgx() : base("0110 1000 010") { }
        public cgx(uint rt, uint ra, uint rb) : base("0110 1000 010") { RT = rt; RA = ra; RB = rb; }
    }
    /// <summary>Substract from Extended</summary>
    public class sfx : Bases.RR
    {
        public sfx() : base("0110 1000 001") { }
        public sfx(uint rt, uint ra, uint rb) : base("0110 1000 001") { RT = rt; RA = ra; RB = rb; }
    }
    /// <summary>Borrow Generate</summary>
    public class bg : Bases.RR
    {
        public bg() : base("0000 1000 010") { }
        public bg(uint rt, uint ra, uint rb) : base("0000 1000 010") { RT = rt; RA = ra; RB = rb; }
    }
    /// <summary>Borrow Generate Extended</summary>
    public class bgx : Bases.RR
    {
        public bgx() : base("0110 1000 011") { }
        public bgx(uint rt, uint ra, uint rb) : base("0110 1000 011") { RT = rt; RA = ra; RB = rb; }
    }
    /// <summary>Multiply</summary>
    public class mpy : Bases.RR
    {
        public mpy() : base("0111 1000 100") { }
        public mpy(uint rt, uint ra, uint rb) : base("0111 1000 100") { RT = rt; RA = ra; RB = rb; }
    }
    /// <summary>Multiply Unsigned</summary>
    public class mpyu : Bases.RR
    {
        public mpyu() : base("0111 1001 100") { }
        public mpyu(uint rt, uint ra, uint rb) : base("0111 1001 100") { RT = rt; RA = ra; RB = rb; }
    }
    /// <summary>Multiply Immediate</summary>
    public class mpyi : Bases.RI10
    {
        public mpyi() : base("0111 0100") { }
        public mpyi(uint rt, uint ra, uint i10) : base("0111 0100") { RT = rt; RA = ra; I10 = i10; }
    }
    /// <summary>Multiply Unsigned Immediate</summary>
    public class mpyui : Bases.RI10
    {
        public mpyui() : base("0111 0101") {}
        public mpyui(uint rt, uint ra, uint i10) : base("0111 0101") { RT = rt; RA = ra; I10 = i10; }
    }
    /// <summary>Multiply and Add</summary>
    public class mpya : Bases.RRR
    {
        public mpya() : base("1100") { }
        public mpya(uint rt, uint ra, uint rb, uint rc) : base("1100") { RT = rt; RA = ra; RB = rb; RC = rc; }
    }
    /// <summary>Multiply High</summary>
    public class mpyh : Bases.RR
    {
        public mpyh() : base("0111 1000 101") { }
        public mpyh(uint rt, uint ra, uint rb) : base("0111 1000 101") { RT = rt; RA = ra; RB = rb; }
    }
    /// <summary>Multiply and Shift Right</summary>
    public class mpys : Bases.RR
    {
        public mpys() : base("0111 1000 111") { }
        public mpys(uint rt, uint ra, uint rb) : base("0111 1000 111") { RT = rt; RA = ra; RB = rb; }
    }
    /// <summary>Multiply High High</summary>
    public class mpyhh : Bases.RR
    {
        public mpyhh() : base("0111 1000 110") { }
        public mpyhh(uint rt, uint ra, uint rb) : base("0111 1000 110") { RT = rt; RA = ra; RB = rb; }
    }
    /// <summary>Multiply High High and Add</summary>
    public class mpyhha : Bases.RR
    {
        public mpyhha() : base("0110 1000 110") { }
        public mpyhha(uint rt, uint ra, uint rb) : base("0110 1000 110") { RT = rt; RA = ra; RB = rb; }
    }
    /// <summary>Multiply High High Unsigned</summary>
    public class mpyhhu : Bases.RR
    {
        public mpyhhu() : base("0111 1001 110") { }
        public mpyhhu(uint rt, uint ra, uint rb) : base("0111 1001 110") { RT = rt; RA = ra; RB = rb; }
    }
    /// <summary>Multiply High High Unsigned and Add</summary>
    public class mpyhhau : Bases.RR
    {
        public mpyhhau() : base("0110 1001 110") { }
        public mpyhhau(uint rt, uint ra, uint rb) : base("0110 1001 110") { RT = rt; RA = ra; RB = rb; }
    }
    /// <summary>Count Leading Zeros</summary>
    public class clz : Bases.R
    {
        public clz() : base("0101 0100 101") { }
        public clz(uint rt, uint ra) : base("0101 0100 101") { RT = rt; RA = ra; }
    }
    /// <summary>Count Ones in Bytes</summary>
    public class cntb : Bases.R
    {
        public cntb() : base("0101 0110 100") { }
        public cntb(uint rt, uint ra) : base("0101 0110 100") { RT = rt; RA = ra; }
    }
    /// <summary>Form Select Mask for Bytes</summary>
    public class fsmb : Bases.R
    {
        public fsmb() : base("0011 0110 110") { }
        public fsmb(uint rt, uint ra) : base("0011 0110 110") { RT = rt; RA = ra; }
    }
    /// <summary>Form Select Mask for Halfwords</summary>
    public class fsmh : Bases.R
    {
        public fsmh() : base("00110110 101") { }
        public fsmh(uint rt, uint ra) : base("00110110 101") { RT = rt; RA = ra; }
    }
    /// <summary>Form Select Mask for Words</summary>
    public class fsm : Bases.R
    {
        public fsm() : base("0011 0110 100") { }
        public fsm(uint rt, uint ra) : base("0011 0110 100") { RT = rt; RA = ra; }
    }
    /// <summary>Gather Bits from Bytes</summary>
    public class gbb : Bases.R
    {
        public gbb() : base("0011 0110 010") { }
        public gbb(uint rt, uint ra) : base("0011 0110 010") { RT = rt; RA = ra; }
    }
    /// <summary>Gather Bits from Halfwords</summary>
    public class gbh : Bases.R
    {
        public gbh() : base("0111 0110 001") { }
        public gbh(uint rt, uint ra) : base("0111 0110 001") { RT = rt; RA = ra; }
    }
    /// <summary>Gather bits from words</summary>
    public class gb : Bases.R
    {
        public gb() : base("0011 0110 000") { }
        public gb(uint rt, uint ra) : base("0011 0110 000") { RT = rt; RA = ra; }
    }
    /// <summary>Average Bytes</summary>
    public class avgb : Bases.RR
    {
        public avgb() : base("0001 1010 011") { }
        public avgb(uint rt, uint ra, uint rb) : base("0001 1010 011") { RT = rt; RA = ra; RB = rb; }
    }
    /// <summary>Absolute Differences of Bytes</summary>
    public class absdb : Bases.RR
    {
        public absdb() : base("0000 1010 011") { }
        public absdb(uint rt, uint ra, uint rb) : base("0000 1010 011") { RT = rt; RA = ra; RB = rb; }
    }
    /// <summary>Sum bytes into Halfwords</summary>
    public class sumb : Bases.RR
    {
        public sumb() : base("0100 1010 011") { }
        public sumb(uint rt, uint ra, uint rb) : base("0100 1010 011") { RT = rt; RA = ra; RB = rb; }
    }
    /// <summary>Extend Sign Bute to Halfword</summary>
    public class xsbh : Bases.R
    {
        public xsbh() : base("0101 0110 110") { }
        public xsbh(uint rt, uint ra) : base("0101 0110 110") { RT = rt; RA = ra; }
    }
    /// <summary>Extend Sign Halword to Word</summary>
    public class xshw : Bases.R
    {
        public xshw() : base("0101 0101 110") { }
        public xshw(uint rt, uint ra) : base("0101 0101 110") { RT = rt; RA = ra; }
    }
    /// <summary>Extend Sign Word to Doubleword</summary>
    public class xswd : Bases.R
    {
        public xswd() : base("0101 0100 110") { }
        public xswd(uint rt, uint ra) : base("0101 0100 110") { RT = rt; RA = ra; }
    }
    /// <summary>And</summary>
    public class and : Bases.RR
    {
        public and() : base("0001 1000 001") { }
        public and(uint rt, uint ra, uint rb) : base("0001 1000 001") { RT = rt; RA = ra; RB = rb; }
    }
    /// <summary>And with Complement</summary>
    public class andc : Bases.RR
    {
        public andc() : base("0101 1000 001") { }
        public andc(uint rt, uint ra, uint rb) : base("0101 1000 001") { RT = rt; RA = ra; RB = rb; }
    }
    /// <summary>And Byte Immediate</summary>
    public class andbi : Bases.RI10
    {
        public andbi() : base("0001 0110") { }
        public andbi(uint rt, uint ra, uint i10) : base("0001 0110") { RT = rt; RA = ra; I10 = i10; }
    }
    /// <summary>And Halfword Immediate</summary>
    public class andhi : Bases.RI10
    {
        public andhi() : base("0001 0101") { }
        public andhi(uint rt, uint ra, uint i10) : base("0001 0101") { RT = rt; RA = ra; I10 = i10; }
    }
    /// <summary>And Word Immediate</summary>
    public class andi : Bases.RI10
    {
        public andi() : base("0001 0100") { }
        public andi(uint rt, uint ra, uint i10) : base("0001 0100") { RT = rt; RA = ra; I10 = i10; }
    }
    /// <summary>Or</summary>
    public class or : Bases.RR
    {
        public or() : base("0000 1000 001") { }
        public or(uint rt, uint ra, uint rb) : base("0000 1000 001") { RT = rt; RA = ra; RB = rb; }
    }
    /// <summary>Or with Complement</summary>
    public class orc : Bases.RR
    {
        public orc() : base("0101 1001 001") { }
        public orc(uint rt, uint ra, uint rb) : base("0101 1001 001") { RT = rt; RA = ra; RB = rb; }
    }
    /// <summary>Or Byte Immediate</summary>
    public class orbi : Bases.RI10
    {
        public orbi() : base("0000 0110") { }
        public orbi(uint rt, uint ra, uint i10) : base("0000 0110") { RT = rt; RA = ra; I10 = i10; }
    }
    /// <summary>Or Halfword Immediate</summary>
    public class orhi : Bases.RI10
    {
        public orhi() : base("0000 0101") { }
        public orhi(uint rt, uint ra, uint i10) : base("0000 0101") { RT = rt; RA = ra; I10 = i10; }
    }
    /// <summary>Or Word Immediate</summary>
    public class ori : Bases.RI10
    {
        public ori() : base("0000 0100") { }
        public ori(uint rt, uint ra, uint i10) : base("0000 0100") { RT = rt; RA = ra; I10 = i10; }
    }
    /// <summary>Or Across</summary>
    public class orx : Bases.R
    {
        public orx() : base("0011 1110 000") { }
        public orx(uint rt, uint ra) : base("0011 1110 000") { RT = rt; RA = ra; }
    }
    /// <summary>Exclusive Or</summary>
    public class xor : Bases.RR
    {
        public xor() : base("0100 1000 001") { }
        public xor(uint rt, uint ra, uint rb) : base("0100 1000 001") { RT = rt; RA = ra; RB = rb; }
    }
    /// <summary>Exclusive Or Byte Immediate</summary>
    public class xorbi : Bases.RI10
    {
        public xorbi() : base("0100 0110") { }
        public xorbi(uint rt, uint ra, uint i10) : base("0100 0110") { RT = rt; RA = ra; I10 = i10; }
    }
    /// <summary>Exclusive Or Halfword Immediate</summary>
    public class xorhi : Bases.RI10
    {
        public xorhi() : base("0100 0101") { }
        public xorhi(uint rt, uint ra, uint i10) : base("0100 0101") { RT = rt; RA = ra; I10 = i10; }
    }
    /// <summary>Exclusive Or Word Immediate</summary>
    public class xori : Bases.RI10
    {
        public xori() : base("0100 0100") { }
        public xori(uint rt, uint ra, uint i10) : base("0100 0100") { RT = rt; RA = ra; I10 = i10; }
    }
    /// <summary>Nand</summary>
    public class nand : Bases.RR
    {
        public nand() : base("0001 1001 001") { }
        public nand(uint rt, uint ra, uint rb) : base("0001 1001 001") { RT = rt; RA = ra; RB = rb; }
    }
    /// <summary>Nor</summary>
    public class nor : Bases.RR
    {
        public nor() : base("0000 1001 001") { }
        public nor(uint rt, uint ra, uint rb) : base("0000 1001 001") { RT = rt; RA = ra; RB = rb; }
    }
    /// <summary>Equivalent</summary>
    public class eqv : Bases.RR
    {
        public eqv() : base("0100 1001 001") { }
        public eqv(uint rt, uint ra, uint rb) : base("0100 1001 001") { RT = rt; RA = ra; RB = rb; }
    }
    /// <summary>Select Bits</summary>
    public class selb : Bases.RRR
    {
        public selb() : base("1000") { }
        public selb(uint rt, uint ra, uint rb, uint rc) : base("1000") { RT = rt; RA = ra; RB = rb; RC = rc; }
    }
    /// <summary>Shuffle Bytes</summary>
    public class shufb : Bases.RRR
    {
        public shufb() : base("1011") { }
        public shufb(uint rt, uint ra, uint rb, uint rc) : base("1011") { RT = rt; RA = ra; RB = rb; RC = rc; }
    }

    /// <summary>Shift Left Halfword</summary>
    public class shlh : Bases.RR
    {
        public shlh() : base("0000 1011 111") { }
        public shlh(uint rt, uint ra, uint rb) : base("0000 1011 111") { RT = rt; RA = ra; RB = rb; }
    }
    /// <summary>Shift Left Halfword Immediate</summary>
    public class shlhi : Bases.RI7
    {
        public shlhi() : base("0000 1111 111") { }
        public shlhi(uint rt, uint ra, uint i7) : base("0000 1111 111") { RT = rt; RA = ra; I7 = i7; }
    }
    /// <summary>Shift Left Word</summary>
    public class shl : Bases.RR
    {
        public shl() : base("0000 1011 011") { }
        public shl(uint rt, uint ra, uint rb) : base("0000 1011 011") { RT = rt; RA = ra; RB = rb; }
    }
    /// <summary>Shift Left Word Immediate</summary>
    public class shli : Bases.RI7
    {
        public shli() : base("0000 1111 011") { }
        public shli(uint rt, uint ra, uint i7) : base("0000 1111 011") { RT = rt; RA = ra; I7 = i7; }
    }
    /// <summary>Shift Left Quadword by Bits</summary>
    public class shlqbi : Bases.RR
    {
        public shlqbi() : base("0011 1011 011") { }
        public shlqbi(uint rt, uint ra, uint rb) : base("0011 1011 011") { RT = rt; RA = ra; RB = rb; }
    }
    /// <summary>Shift Left Quadword by Bits Immediate</summary>
    public class shlqbii : Bases.RI7
    {
        public shlqbii() : base("0011 1111 011") { }
        public shlqbii(uint rt, uint ra, uint i7) : base("0011 1111 011") { RT = rt; RA = ra; I7 = i7; }
    }
    /// <summary>Shift Left Quadword by Bytes</summary>
    public class shlqby : Bases.RR
    {
        public shlqby() : base("0011 1011 111") { }
        public shlqby(uint rt, uint ra, uint rb) : base("0011 1011 111") { RT = rt; RA = ra; RB = rb; }
    }
    /// <summary>Shift Left Quadword by Bytes Immediate</summary>
    public class shlqbyi : Bases.RI7
    {
        public shlqbyi() : base("0011 1111 111") { }
        public shlqbyi(uint rt, uint ra, uint i7) : base("0011 1111 111") { RT = rt; RA = ra; I7 = i7; }
    }
    /// <summary>Shift left Quadword by Bytes from Bit Shift Count</summary>
    public class shlqbybi : Bases.RR
    {
        public shlqbybi() : base("0011 1001 111") { }
        public shlqbybi(uint rt, uint ra, uint rb) : base("0011 1001 111") { RT = rt; RA = ra; RB = rb; }
    }
    /// <summary>Rotate Halfword</summary>
    public class roth : Bases.RR
    {
        public roth() : base("0000 1011 100") { }
        public roth(uint rt, uint ra, uint rb) : base("0000 1011 100") { RT = rt; RA = ra; RB = rb; }
    }
    /// <summary>Rotate Halfword Immediate</summary>
    public class rothi : Bases.RI7
    {
        public rothi() : base("0000 1111 100") { }
        public rothi(uint rt, uint ra, uint i7) : base("0000 1111 100") { RT = rt; RA = ra; I7 = i7; }
    }
    /// <summary>Rotate Word</summary>
    public class rot : Bases.RR
    {
        public rot() : base("0000 1011 000") { }
        public rot(uint rt, uint ra, uint rb) : base("0000 1011 000") { RT = rt; RA = ra; RB = rb; }
    }
    /// <summary>Rotate Word Immediate</summary>
    public class roti : Bases.RI7
    {
        public roti() : base("0000 1111 000") { }
        public roti(uint rt, uint ra, uint i7) : base("0000 1111 000") { RT = rt; RA = ra; I7 = i7; }
    }
    /// <summary>Rotate Quadword by Bytes</summary>
    public class rotqby : Bases.RR
    {
        public rotqby() : base("0011 1011 100") { }
        public rotqby(uint rt, uint ra, uint rb) : base("0011 1011 100") { RT = rt; RA = ra; RB = rb; }
    }
    /// <summary>Rotate Quadword by Bytes Immediate</summary>
    public class rotqbyi : Bases.RI7
    {
        public rotqbyi() : base("0011 1111 100") { }
        public rotqbyi(uint rt, uint ra, uint i7) : base("0011 1111 100") { RT = rt; RA = ra; I7 = i7; }
    }
    /// <summary>Rotate Quadword by Bytes from Bit Shift Count</summary>
    public class rotqbybi : Bases.RR
    {
        public rotqbybi() : base("0011 1001 100") { }
        public rotqbybi(uint rt, uint ra, uint rb) : base("0011 1001 100") { RT = rt; RA = ra; RB = rb; }
    }
    /// <summary>Rotate Quadword by Bits</summary>
    public class rotqbi : Bases.RR
    {
        public rotqbi() : base("0011 1011 000") { }
        public rotqbi(uint rt, uint ra, uint rb) : base("0011 1011 000") { RT = rt; RA = ra; RB = rb; }
    }
    /// <summary>Rotate Quadword by Bits Immediate</summary>
    public class rotqbii : Bases.RI7
    {
        public rotqbii() : base("0011 1111 000") { }
        public rotqbii(uint rt, uint ra, uint i7) : base("0011 1111 000") { RT = rt; RA = ra; I7 = i7; }
    }
    /// <summary>Rotate and Mask Halfword</summary>
    public class rothm : Bases.RR
    {
        public rothm() : base("0000 1011 101") { }
        public rothm(uint rt, uint ra, uint rb) : base("0000 1011 101") { RT = rt; RA = ra; RB = rb; }
    }
    /// <summary>Rotate and Mask Halfword Immediate</summary>
    public class rothmi : Bases.RI7
    {
        public rothmi() : base("0000 1111 101") { }
        public rothmi(uint rt, uint ra, uint i7) : base("0000 1111 101") { RT = rt; RA = ra; I7 = i7; }
    }
    /// <summary>Rotate and Mask Word</summary>
    public class rotm : Bases.RR
    {
        public rotm() : base("0000 1011 001") { }
        public rotm(uint rt, uint ra, uint rb) : base("0000 1011 001") { RT = rt; RA = ra; RB = rb; }
    }
    /// <summary>Rotate and Mask Word Immediate</summary>
    public class rotmi : Bases.RI7
    {
        public rotmi() : base("0000 1111 001") { }
        public rotmi(uint rt, uint ra, uint i7) : base("0000 1111 001") { RT = rt; RA = ra; I7 = i7; }
    }
    /// <summary>Rotate and Mask Quadword by Bytes</summary>
    public class rotqmby : Bases.RR
    {
        public rotqmby() : base("0011 1011 101") { }
        public rotqmby(uint rt, uint ra, uint rb) : base("0011 1011 101") { RT = rt; RA = ra; RB = rb; }
    }
    /// <summary>Roate and Mask Quadword by Bytes Immediate</summary>
    public class rotqmbyi : Bases.RI7
    {
        public rotqmbyi() : base("0011 1111 101") { }
        public rotqmbyi(uint rt, uint ra, uint i7) : base("0011 1111 101") { RT = rt; RA = ra; I7 = i7; }
    }
    /// <summary>Rotate and Mask Quadword Bytes from Bit Shift Count Required</summary>
    public class rotqmbybi : Bases.RR
    {
        public rotqmbybi() : base("0011 1001 101") { }
        public rotqmbybi(uint rt, uint ra, uint rb) : base("0011 1001 101") { RT = rt; RA = ra; RB = rb; }
    }
    /// <summary>Rotate and Mask Quadword by Bits</summary>
    public class rotqmbi : Bases.RR
    {
        public rotqmbi() : base("0011 1011 001") { }
        public rotqmbi(uint rt, uint ra, uint rb) : base("0011 1011 001") { RT = rt; RA = ra; RB = rb; }
    }
    /// <summary>Rotate and Mask Quadword by Bits Immediate</summary>
    public class rotqmbii : Bases.RI7
    {
        public rotqmbii() : base("0011 1111 001") { }
        public rotqmbii(uint rt, uint ra, uint i7) : base("0011 1111 001") { RT = rt; RA = ra; I7 = i7; }
    }
    /// <summary>Rotate and Mask Algebraic Halfword</summary>
    public class rotmah : Bases.RR
    {
        public rotmah() : base("0000 1011 110") { }
        public rotmah(uint rt, uint ra, uint rb) : base("0000 1011 110") { RT = rt; RA = ra; RB = rb; }
    }
    /// <summary>Rotate and Mask Algebraic Halfword Immediate</summary>
    public class rotmahi : Bases.RI7
    {
        public rotmahi() : base("0000 1111 110") { }
        public rotmahi(uint rt, uint ra, uint i7) : base("0000 1111 110") { RT = rt; RA = ra; I7 = i7; }
    }
    /// <summary>Rotate and Mask Algebraic Word</summary>
    public class rotma : Bases.RR
    {
        public rotma() : base("0000 1011 010") { }
        public rotma(uint rt, uint ra, uint rb) : base("0000 1011 010") { RT = rt; RA = ra; RB = rb; }
    }
    /// <summary>Rotate and Mask Algebraic Word Immediate</summary>
    public class rotmai : Bases.RI7
    {
        public rotmai() : base("0000 1111 010") { }
        public rotmai(uint rt, uint ra, uint i7) : base("0000 1111 010") { RT = rt; RA = ra; I7 = i7; }
    }
    /// <summary>Halt If Equal</summary>
    public class heq : Bases.RR
    {
        public heq() : base("0111 1011 000") { }
        public heq(uint rt, uint ra, uint rb) : base("0111 1011 000") { RT = rt; RA = ra; RB = rb; }
    }
    /// <summary>Halt If Equal Immediate</summary>
    public class heqi : Bases.RI10
    {
        public heqi() : base("0111 1111") { }
        public heqi(uint rt, uint ra, uint i10) : base("0111 1111") { RT = rt; RA = ra; I10 = i10; }
    }
    /// <summary>Half If Greater Than</summary>
    public class hgt : Bases.RR
    {
        public hgt() : base("0100 1011 000") { }
        public hgt(uint rt, uint ra, uint rb) : base("0100 1011 000") { RT = rt; RA = ra; RB = rb; }
    }
    /// <summary>Halt If Greater Than Immediate</summary>
    public class hgti : Bases.RI10
    {
        public hgti() : base("0100 1111") { }
        public hgti(uint rt, uint ra, uint i10) : base("0100 1111") { RT = rt; RA = ra; I10 = i10; }
    }
    /// <summary>Halt If logically Greater Than</summary>
    public class hlgt : Bases.RR
    {
        public hlgt() : base("0101 1011 000") { }
        public hlgt(uint rt, uint ra, uint rb) : base("0101 1011 000") { RT = rt; RA = ra; RB = rb; }
    }
    /// <summary>Halt If Logically Greater Than Immediate</summary>
    public class hlgti : Bases.RI10
    {
        public hlgti() : base("0101 1111") { }
        public hlgti(uint rt, uint ra, uint i10) : base("0101 1111") { RT = rt; RA = ra; I10 = i10; }
    }
    /// <summary>Compare Equal Byte</summary>
    public class ceqb : Bases.RR
    {
        public ceqb() : base("0111 1010 000") { }
        public ceqb(uint rt, uint ra, uint rb) : base("0111 1010 000") { RT = rt; RA = ra; RB = rb; }
    }
    /// <summary>Compare Equal Byte Immediate</summary>
    public class ceqbi : Bases.RI10
    {
        public ceqbi() : base("0111 1110") { }
        public ceqbi(uint rt, uint ra, uint i10) : base("0111 1110") { RT = rt; RA = ra; I10 = i10; }
    }
    /// <summary>Compare Equal Halfword</summary>
    public class ceqh : Bases.RR
    {
        public ceqh() : base("0111 1001 000") { }
        public ceqh(uint rt, uint ra, uint rb) : base("0111 1001 000") { RT = rt; RA = ra; RB = rb; }
    }
    /// <summary>Compare Equal Halfword Immediate</summary>
    public class ceqhi : Bases.RI10
    {
        public ceqhi() : base("0111 1101") { }
        public ceqhi(uint rt, uint ra, uint i10) : base("0111 1101") { RT = rt; RA = ra; I10 = i10; }
    }
    /// <summary>Compare Equal Word</summary>
    public class ceq : Bases.RR
    {
        public ceq() : base("0111 1000 000") { }
        public ceq(uint rt, uint ra, uint rb) : base("0111 1000 000") { RT = rt; RA = ra; RB = rb; }
    }
    /// <summary>Compare Equal Word Immediate</summary>
    public class ceqi : Bases.RI10
    {
        public ceqi() : base("0111 1100") { }
        public ceqi(uint rt, uint ra, uint i10) : base("0111 1100") { RT = rt; RA = ra; I10 = i10; }
    }
    /// <summary>Compare Greater Than Byte</summary>
    public class cgtb : Bases.RR
    {
        public cgtb() : base("0010 1010 000") { }
        public cgtb(uint rt, uint ra, uint rb) : base("0010 1010 000") { RT = rt; RA = ra; RB = rb; }
    }
    /// <summary>Compare Greater Than Byte Immediate</summary>
    public class cgtbi : Bases.RI10
    {
        public cgtbi() : base("0100 1110") { }
        public cgtbi(uint rt, uint ra, uint i10) : base("0100 1110") { RT = rt; RA = ra; I10 = i10; }
    }
    /// <summary>Compare Greater Than Halfword</summary>
    public class cgth : Bases.RR
    {
        public cgth() : base("0100 1001 000") { }
        public cgth(uint rt, uint ra, uint rb) : base("0100 1001 000") { RT = rt; RA = ra; RB = rb; }
    }
    /// <summary>Compare Greater Than Halfword Immediate</summary>
    public class cgthi : Bases.RI10
    {
        public cgthi() : base("0100 1101") { }
        public cgthi(uint rt, uint ra, uint i10) : base("0100 1101") { RT = rt; RA = ra; I10 = i10; }
    }
    /// <summary>Compare Greater Than Word</summary>
    public class cgt : Bases.RR
    {
        public cgt() : base("0100 1000 000") { }
        public cgt(uint rt, uint ra, uint rb) : base("0100 1000 000") { RT = rt; RA = ra; RB = rb; }
    }
    /// <summary>Compare Greater Than Word Immediate</summary>
    public class cgti : Bases.RI10
    {
        public cgti() : base("0100 1100") { }
        public cgti(uint rt, uint ra, uint i10) : base("0100 1100") { RT = rt; RA = ra; I10 = i10; }
    }
    /// <summary>Compare Logical Greater Than Byte</summary>
    public class clgtb : Bases.RR
    {
        public clgtb() : base("0101 1010 000") { }
        public clgtb(uint rt, uint ra, uint rb) : base("0101 1010 000") { RT = rt; RA = ra; RB = rb; }
    }
    /// <summary>Compare Logical Greater Than Byte Immediate</summary>
    public class clgtbi : Bases.RI10
    {
        public clgtbi() : base("0101 1110") { }
        public clgtbi(uint rt, uint ra, uint i10) : base("0101 1110") { RT = rt; RA = ra; I10 = i10; }
    }
    /// <summary>Compare Logical Greater Than Halfword</summary>
    public class clgth : Bases.RR
    {
        public clgth() : base("0101 1001 000") { }
        public clgth(uint rt, uint ra, uint rb) : base("0101 1001 000") { RT = rt; RA = ra; RB = rb; }
    }
    /// <summary>Compare Logical Greater Than Halfword Immediate</summary>
    public class clgthi : Bases.RI10
    {
        public clgthi() : base("0101 1101") { }
        public clgthi(uint rt, uint ra, uint i10) : base("0101 1101") { RT = rt; RA = ra; I10 = i10; }
    }
    /// <summary>Compare Logical Greater Than Word</summary>
    public class clgt : Bases.RR
    {
        public clgt() : base("0101 1000 000") { }
        public clgt(uint rt, uint ra, uint rb) : base("0101 1000 000") { RT = rt; RA = ra; RB = rb; }
    }
    /// <summary>Compare Logical Greater Than Word Immediate</summary>
    public class clgti : Bases.RI10
    {
        public clgti() : base("0101 1100") { }
        public clgti(uint rt, uint ra, uint i10) : base("0101 1100") { RT = rt; RA = ra; I10 = i10; }
    }
    /// <summary>Branch Relative</summary>
    /// <remarks>Target register is unused</remarks>
    public class br : Bases.RI16
    {
        public br() : base("0011 0010 0") { }
        public br(uint rt, uint i16) : base("0011 0010 0") { RT = rt; I16 = i16; }
    } 
    /// <summary>Branch Absolute</summary>
    /// <remarks>Target register is unused</remarks>
    public class bra : Bases.RI16
    {
        public bra() : base("0011 0000 0") { }
        public bra(uint rt, uint i16) : base("0011 0000 0") { RT = rt; I16 = i16; }
    } 
    /// <summary>Branch Relative and Set Link</summary>
    public class brsl : Bases.RI16
    {
        public brsl() : base("0011 0011 0") { }
        public brsl(uint rt, uint i16) : base("0011 0011 0") { RT = rt; I16 = i16; }
    }
    /// <summary>Branch Absolute and Set Link</summary>
    public class brasl : Bases.RI16
    {
        public brasl() : base("0011 0001 0") { }
        public brasl(uint rt, uint i16) : base("0011 0001 0") { RT = rt; I16 = i16; }
    }
    /// <summary>Branch Indirect</summary>
    /// <remarks>Target register is unused</remarks>
    public class bi : Bases.RInt
    {
        public bi() : base("0011 0101 000") { }
        public bi(uint rc, uint ra) : base("0011 0101 000") { RT = rc; RA = ra; }
    }
    /// <summary>Interrupt Return</summary>
    /// <remarks>Target register is unused</remarks>
    public class iret : Bases.RInt
    {
        public iret() : base("0011 0101 010") { }
        public iret(uint rc, uint ra) : base("0011 0101 010") { RT = rc; RA = ra; }
    }
    /// <summary>Branch Indirect and Set Link if External Data</summary>
    public class bisled : Bases.RInt
    {
        public bisled() : base("0011 0101 011") { }
        public bisled(uint rc, uint ra) : base("0011 0101 011") { RT = rc; RA = ra; }
    }
    /// <summary>Branch Indirect and Set Link</summary>
    public class bisl : Bases.RInt
    {
        public bisl() : base("0011 0101 001") { }
        public bisl(uint rc, uint ra) : base("0011 0101 001") { RT = rc; RA = ra; }
    }
    /// <summary>Branch If Not Zero Word</summary>
    public class brnz : Bases.RI16
    {
        public brnz() : base("0010 0001 0") { }
        public brnz(uint rt, uint i16) : base("0010 0001 0") { RT = rt; I16 = i16; }
    }
    /// <summary>Branch If Zero Word</summary>
    public class brz : Bases.RI16
    {
        public brz() : base("0010 0000 0") { }
        public brz(uint rt, uint i16) : base("0010 0000 0") { RT = rt; I16 = i16; }
    }
    /// <summary>Branch If Not Zero Halfword</summary>
    public class brhnz : Bases.RI16
    {
        public brhnz() : base("0010 0011 0") { }
        public brhnz(uint rt, uint i16) : base("0010 0011 0") { RT = rt; I16 = i16; }
    }
    /// <summary>Branch If Zero Halfword</summary>
    public class brhz : Bases.RI16
    {
        public brhz() : base("0010 0010 0") { }
        public brhz(uint rt, uint i16) : base("0010 0010 0") { RT = rt; I16 = i16; }
    }
    /// <summary>Branch Indirect If Zero</summary>
    public class biz : Bases.RInt
    {
        public biz() : base("0010 0101 000") { }
        public biz(uint rc, uint ra) : base("0010 0101 000") { RT = rc; RA = ra; }
    }
    /// <summary>Branch Indirect If Not Zero</summary>
    public class binz : Bases.RInt
    {
        public binz() : base("0010 0101 001") { }
        public binz(uint rc, uint ra) : base("0010 0101 001") { RT = rc; RA = ra; }
    }
    /// <summary>Branch Indirect If Zero Halfword</summary>
    public class bihz : Bases.RInt
    {
        public bihz() : base("0010 0101 010") { }
        public bihz(uint rc, uint ra) : base("0010 0101 010") { RT = rc; RA = ra; }
    }
    /// <summary>Branch Indirect If Not Zero Halfword</summary>
    public class bihnz : Bases.RInt
    {
        public bihnz() : base("0010 0101 011") { }
        public bihnz(uint rc, uint ra) : base("0010 0101 011") { RT = rc; RA = ra; }
    }
    /// <summary>Hint for Branch (r-form)</summary>
    public partial class hbr : Bases.Instruction
    {
        public hbr() : base("0011 0101 100") { }
    }
    /// <summary>Hint for Branch (a-form)</summary>
    public class hbra : Bases.RBranch
    {
        public hbra() : base("0001 000") { }
    }
    /// <summary>Hint for Branch Relative</summary>
    public class hbrr : Bases.RBranch
    {
        public hbrr() : base("0001 001") { }
    }

    /// <summary>Floating Add</summary>
    public class fa : Bases.RR
    {
        public fa() : base("0101 1000 100") { }
        public fa(uint rt, uint ra, uint rb) : base("0101 1000 100") { RT = rt; RA = ra; RB = rb; }
    }
    /// <summary>Double Floating Add</summary>
    public class dfa : Bases.RR
    {
        public dfa() : base("0101 1001 100") { }
        public dfa(uint rt, uint ra, uint rb) : base("0101 1001 100") { RT = rt; RA = ra; RB = rb; }
    }
    /// <summary>Floating Substract</summary>
    public class fs : Bases.RR
    {
        public fs() : base("0101 1000 101") { }
        public fs(uint rt, uint ra, uint rb) : base("0101 1000 101") { RT = rt; RA = ra; RB = rb; }
    }
    /// <summary>Double Floating Substract</summary>
    public class dfs : Bases.RR
    {
        public dfs() : base("0101 1001 101") { }
        public dfs(uint rt, uint ra, uint rb) : base("0101 1001 101") { RT = rt; RA = ra; RB = rb; }
    }
    /// <summary>Floating Multiply</summary>
    public class fm : Bases.RR
    {
        public fm() : base("0101 1000 110") { }
        public fm(uint rt, uint ra, uint rb) : base("0101 1000 110") { RT = rt; RA = ra; RB = rb; }
    }
    /// <summary>Double Floating Multiply</summary>
    public class dfm : Bases.RR
    {
        public dfm() : base("0101 1001 110") { }
        public dfm(uint rt, uint ra, uint rb) : base("0101 1001 110") { RT = rt; RA = ra; RB = rb; }
    }
    /// <summary>Floating Multiply and Add</summary>
    public class fma : Bases.RRR
    {
        public fma() : base("1110") { }
        public fma(uint rt, uint ra, uint rb, uint rc) : base("1110") { RT = rt; RA = ra; RB = rb; RC = rc; }
    }
    /// <summary>Double Floating Multiply and Add</summary>
    public class dfma : Bases.RR
    {
        public dfma() : base("0110 1011 100") { }
        public dfma(uint rt, uint ra, uint rb) : base("0110 1011 100") { RT = rt; RA = ra; RB = rb; }
    }
    /// <summary>Floating Negative Multiply and Substract</summary>
    public class fnms : Bases.RRR
    {
        public fnms() : base("1101") { }
        public fnms(uint rt, uint ra, uint rb, uint rc) : base("1101") { RT = rt; RA = ra; RB = rb; RC = rc; }
    }
    /// <summary>Double Floating Negative Multiply and Substract</summary>
    public class dfnms : Bases.RR
    {
        public dfnms() : base("0110 1011 110") { }
        public dfnms(uint rt, uint ra, uint rb) : base("0110 1011 110") { RT = rt; RA = ra; RB = rb; }
    }
    /// <summary>Floating Multiply and Substract</summary>
    public class fms : Bases.RRR
    {
        public fms() : base("1111") { }
        public fms(uint rt, uint ra, uint rb, uint rc) : base("1111") { RT = rt; RA = ra; RB = rb; RC = rc; }
    }
    /// <summary>Double Floating Multiply and Substract</summary>
    public class dfms : Bases.RR
    {
        public dfms() : base("0110 1011 101") { }
        public dfms(uint rt, uint ra, uint rb) : base("0110 1011 101") { RT = rt; RA = ra; RB = rb; }
    }
    /// <summary>Double Floating Negative Multiply and Add</summary>
    public class dfnma : Bases.RR
    {
        public dfnma() : base("0110 1011 111") { }
        public dfnma(uint rt, uint ra, uint rb) : base("0110 1011 111") { RT = rt; RA = ra; RB = rb; }
    }
    /// <summary>Floating Reciprocal Estimate</summary>
    public class frest : Bases.R
    {
        public frest() : base("0011 0111 000") { }
        public frest(uint rt, uint ra) : base("0011 0111 000") { RT = rt; RA = ra; }
    }
    /// <summary>Floating Reciprocal Absolute Square Root Estimate</summary>
    public class frsqest : Bases.R
    {
        public frsqest() : base("0011 0111 001") { }
        public frsqest(uint rt, uint ra) : base("0011 0111 001") { RT = rt; RA = ra; }
    }
    /// <summary>Floating Interpolate</summary>
    public class fi : Bases.RR
    {
        public fi() : base("0111 1010 100") { }
        public fi(uint rt, uint ra, uint rb) : base("0111 1010 100") { RT = rt; RA = ra; RB = rb; }
    }
    /// <summary>Convert Signed Integer to Floating</summary>
    public class csflt : Bases.RI8
    {
        public csflt() : base("0111 0110 10") { }
        public csflt(uint rt, uint ra, uint i8) : base("0111 0110 10") { RT = rt; RA = ra; I8 = i8; }
    }
    /// <summary>Convert Floating To Signed Integer</summary>
    public class cflts : Bases.RI8
    {
        public cflts() : base("0111 0110 00") { }
        public cflts(uint rt, uint ra, uint i8) : base("0111 0110 00") { RT = rt; RA = ra; I8 = i8; }
    }
    /// <summary>Convert Unsigned Integer to Floating</summary>
    public class cuflt : Bases.RI8
    {
        public cuflt() : base("0111 0110 11") { }
        public cuflt(uint rt, uint ra, uint i8) : base("0111 0110 11") { RT = rt; RA = ra; I8 = i8; }
    }
    /// <summary>Convert Floating to Unsigned Integer</summary>
    public class cfltu : Bases.RI8
    {
        public cfltu() : base("0111 0110 01") { }
        public cfltu(uint rt, uint ra, uint i8) : base("0111 0110 01") { RT = rt; RA = ra; I8 = i8; }
    }
    /// <summary>Floating Round Double to Single</summary>
    public class frds : Bases.R
    {
        public frds() : base("0111 0111 001") { }
        public frds(uint rt, uint ra) : base("0111 0111 001") { RT = rt; RA = ra; }
    }
    /// <summary>Floating Extend Single To Double</summary>
    public class fesd : Bases.R
    {
        public fesd() : base("0111 0111 000") { }
        public fesd(uint rt, uint ra) : base("0111 0111 000") { RT = rt; RA = ra; }
    }
    /// <summary>Double Floating Compare Equal</summary>
    public class dfceq : Bases.RR
    {
        public dfceq() : base("0111 1000 011") { }
        public dfceq(uint rt, uint ra, uint rb) : base("0111 1000 011") { RT = rt; RA = ra; RB = rb; }
    }
    /// <summary>Double Floating Compare Magnitude Equal</summary>
    public class dfcmeq : Bases.RR
    {
        public dfcmeq() : base("0111 1001 011") { }
        public dfcmeq(uint rt, uint ra, uint rb) : base("0111 1001 011") { RT = rt; RA = ra; RB = rb; }
    }
    /// <summary>Double Floating Compare Greater Than</summary>
    public class dfcgt : Bases.RR
    {
        public dfcgt() : base("0101 1000 011") { }
        public dfcgt(uint rt, uint ra, uint rb) : base("0101 1000 011") { RT = rt; RA = ra; RB = rb; }
    }
    /// <summary>Double Floating Test Value</summary>
    public class dfcmgt : Bases.RR
    {
        public dfcmgt() : base("0111 0111 111") { }
        public dfcmgt(uint rt, uint ra, uint rb) : base("0111 0111 111") { RT = rt; RA = ra; RB = rb; }
    }
    /// <summary>Double Floating Compare Magnitude Greater Than</summary>
    public class dftsv : Bases.RI7
    {
        public dftsv() : base("0101 1001 011") { }
        public dftsv(uint rt, uint ra, uint i7) : base("0101 1001 011") { RT = rt; RA = ra; I7 = i7; }
    }
    /// <summary>Floating Compare Equal</summary>
    public class fceq : Bases.RR
    {
        public fceq() : base("0111 1000 010") { }
        public fceq(uint rt, uint ra, uint rb) : base("0111 1000 010") { RT = rt; RA = ra; RB = rb; }
    }
    /// <summary>Floating Compare Magnitude Equal</summary>
    public class fcmeq : Bases.RR
    {
        public fcmeq() : base("0111 1001 010") { }
        public fcmeq(uint rt, uint ra, uint rb) : base("0111 1001 010") { RT = rt; RA = ra; RB = rb; }
    }
    /// <summary>Floating Compare Greater Than</summary>
    public class fcgt : Bases.RR
    {
        public fcgt() : base("0101 1000 010") { }
        public fcgt(uint rt, uint ra, uint rb) : base("0101 1000 010") { RT = rt; RA = ra; RB = rb; }
    }
    /// <summary>Floating Compare Magnitude Greater Than</summary>
    public class fcmgt : Bases.RR
    {
        public fcmgt() : base("0101 1001 010") { }
        public fcmgt(uint rt, uint ra, uint rb) : base("0101 1001 010") { RT = rt; RA = ra; RB = rb; }
    }
    /// <summary>Floating-Point Status and Control Register Write</summary>
    public class fscrwr : Bases.R
    {
        public fscrwr() : base("0111 0111 010") { }
        public fscrwr(uint rt, uint ra) : base("0111 0111 010") { RT = rt; RA = ra; }
    }
    /// <summary>Floating-Point Status and Control Register Read</summary>
    public class fscrrd : Bases.R0
    {
        public fscrrd() : base("0111 0011 000") { }
        public fscrrd(uint rt) : base("0111 0011 000") { RT = rt; }
    }

    /// <summary>Stop and Signal</summary>
    public partial class stop : Bases.Instruction
    {
        public stop() : base("0000 0000 000") { }
        public stop(uint signal) : base("0000 0000 000") { StopAndSignalType = signal; }
    }
    /// <summary>Stop and Signal with Dependencies</summary>
    public class stopd : Bases.RR
    {
        public stopd() : base("0010 1000 000") { }
        public stopd(uint rt, uint ra, uint rb) : base("0010 1000 000") { RT = rt; RA = ra; RB = rb; }
    }
    /// <summary>No Operation (Load)</summary>
    public class lnop : Bases.Instruction
    {
        public lnop() : base("0000 0000 001") { }
    }
    /// <summary>No Operation (Execute)</summary>
    public class nop : Bases.R0
    {
        public nop() : base("0100 0000 001") { }
        public nop(uint rt) : base("0100 0000 001") { RT = rt; }
    }
    /// <summary>Synchronize</summary>
    public partial class sync : Bases.Instruction
    {
        public sync() : base("0000 0000 010") { }
    }
    /// <summary>Synchronize data</summary>
    public class dsync : Bases.Instruction
    {
        public dsync() : base("0000 0000 011") { }
    }
    /// <summary>Move from Special-Purpose Register</summary>
    public class mfspr : Bases.R
    {
        public mfspr() : base("0000 0001 100") { }
        public mfspr(uint rt, uint ra) : base("0000 0001 100") { RT = rt; RA = ra; }
    }
    /// <summary>Move to Special-Purpose Register</summary>
    public class mtspr : Bases.R
    {
        public mtspr() : base("0010 0001 100") { }
        public mtspr(uint rt, uint ra) : base("0010 0001 100") { RT = rt; RA = ra; }
    }

    /// <summary>Read Channel</summary>
    public class rdch : Bases.R
    {
        public rdch() : base("0000 0001 101") { }
        public rdch(uint rt, uint ra) : base("0000 0001 101") { RT = rt; RA = ra; }
    }
    /// <summary>Read Channel Count</summary>
    public class rchcnt : Bases.R
    {
        public rchcnt() : base("0000 0001 111") { }
        public rchcnt(uint rt, uint ra) : base("0000 0001 111") { RT = rt; RA = ra; }
    }
    /// <summary>Write Channel</summary>
    public class wrch : Bases.R
    {
        public wrch() : base("0010 0001 101") { }
        public wrch(uint rt, uint ra) : base("0010 0001 101") { RT = rt; RA = ra; }
    }
}
