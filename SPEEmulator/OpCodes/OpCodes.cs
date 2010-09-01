using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SPEEmulator.OpCodes
{
    //This lists all supported operations on the SPU

    /// <summary>Load Quadword (d-form)</summary>
    public class lqd : Bases.RI10  { public lqd() : base("0011 0100") { } }
    /// <summary>Load Quadword (x-form)</summary>
    public class lqx : Bases.RR    { public lqx() : base("0011 1000 100") { } }
    /// <summary>Load Quadword (a-form)</summary>
    public class lqa : Bases.RI16  { public lqa() : base("0011 0000 1") { } }
    /// <summary>Load Quadword Instruction Relative (a-form)</summary>
    public class lqr : Bases.RI16  { public lqr() : base("0011 0011 1") { } }
    /// <summary>Store Quadword (d-form)</summary>
    public class stqd : Bases.RI10 { public stqd() : base("0010 0100") { } }
    /// <summary>Store Quadword (x-form)</summary>
    public class stqx : Bases.RR   { public stqx() : base("0010 1000 100") { } }
    /// <summary>Store Quadword (a-form)</summary>
    public class stqa : Bases.RI16 { public stqa() : base("0010 0000 1") { } }
    /// <summary>Store Quadword Instruction Relative (a-form)</summary>
    public class stqr : Bases.RI16 { public stqr() : base("0010 0011 1") { } }
    /// <summary>Generate Constrols for Byte Insertion (d-form)</summary>
    public class cbd : Bases.RI7   { public cbd() : base("0011 1110 100") { } }
    /// <summary>Generate Constrols for Byte Insertion (x-form)</summary>
    public class cbx : Bases.RR    { public cbx() : base("0011 1010 100") { } }
    /// <summary>Generate Constrols for Halfword Insertion (d-form)</summary>
    public class chd : Bases.RI7   { public chd() : base("0011 1110 101") { } }
    /// <summary>Generate Constrols for Halfword Insertion (x-form)</summary>
    public class chx : Bases.RR    { public chx() : base("0011 1010 101") { } }
    /// <summary>Generate Constrols for Word Insertion (d-form)</summary>
    public class cwd : Bases.RI7   { public cwd() : base("0011 1110 110") { } }
    /// <summary>Generate Constrols for Word Insertion (x-form)</summary>
    public class cwx : Bases.RR    { public cwx() : base("0011 1010 110") { } }
    /// <summary>Generate Constrols for Doubleword Insertion (d-form)</summary>
    public class cdd : Bases.RI7   { public cdd() : base("0011 1110 111") { } }
    /// <summary>Generate Constrols for Doubleword Insertion (x-form)</summary>
    public class cdx : Bases.RR    { public cdx() : base("0011 1010 111") { } }

    /// <summary>Immediate Load Halfword</summary>
    public class ilh : Bases.RI16   { public ilh() : base("0100 0001 1") { } }
    /// <summary>Immediate Load Halfword Upper</summary>
    public class ilhu : Bases.RI16  { public ilhu() : base("0100 0001 0") { } }
    /// <summary>Immediate Load Word</summary>
    public class il : Bases.RI16    { public il() : base("0100 0000 1") { } }
    /// <summary>Immediate Load Address</summary>
    public class ila : Bases.RI18 { public ila() : base("0100 001") { } }
    /// <summary>Immediate Or Halfword Lower</summary>
    public class iohl : Bases.RI16  { public iohl() : base("0110 0000 1") { } }
    /// <summary>Form Select Mask for Bytes Immediate</summary>
    public class fsmbi : Bases.RI16 { public fsmbi() : base("0011 0010 1") { } }

    /// <summary>Add Halfword</summary>
    public class ah : Bases.RR { public ah() : base("0001 1001 000") { } }
    /// <summary>Add Halfword Immediate</summary>
    public class ahi : Bases.RI10 { public ahi() : base("0001 1101") { } }
    /// <summary>Add Word</summary>
    public class a : Bases.RR { public a() : base("0001 1000 000") { } }
    /// <summary>Add Word Immediate</summary>
    public class ai : Bases.RI10 { public ai() : base("0001 1100") { } }
    /// <summary>Substract from Halfword</summary>
    public class sfh : Bases.RR { public sfh() : base("0000 1001 000") { } }
    /// <summary>Substract from Halword Immediate</summary>
    public class sfhi : Bases.RI10 { public sfhi() : base("0000 1101") { } }
    /// <summary>Substract from Word</summary>
    public class sf : Bases.RR { public sf() : base("0000 1000 000") { } }
    /// <summary></summary>
    public class sfi : Bases.RI10 { public sfi() : base("0000 1100") { } }
    /// <summary>Add Extended</summary>
    public class addx : Bases.RR { public addx() : base("0110 1000 000") { } }
    /// <summary>Carry Generate</summary>
    public class cg : Bases.RR { public cg() : base("0001 1000 010") { } }
    /// <summary>Carry Generate Extended</summary>
    public class cgx : Bases.RR { public cgx() : base("0110 1000 010") { } }
    /// <summary>Substract from Extended</summary>
    public class sfx : Bases.RR { public sfx() : base("0110 1000 001") { } }
    /// <summary>Borrow Generate</summary>
    public class bg : Bases.RR { public bg() : base("0000 1000 010") { } }
    /// <summary>Borrow Generate Extended</summary>
    public class bgx : Bases.RR { public bgx() : base("0110 1000 011") { } }
    /// <summary>Multiply</summary>
    public class mpy : Bases.RR { public mpy() : base("0111 1000 100") { } }
    /// <summary>Multiply Unsigned</summary>
    public class mpyu : Bases.RR { public mpyu() : base("0111 1001 100") { } }
    /// <summary>Multiply Immediate</summary>
    public class mpyi : Bases.RI10 { public mpyi() : base("0111 0100") { } }
    /// <summary>Multiply Unsigned Immediate</summary>
    public class mpyui : Bases.RI10 { public mpyui() : base("0111 0101") { } }
    /// <summary>Multiply and Add</summary>
    public class mpya : Bases.RRR { public mpya() : base("1100") { } }
    /// <summary>Multiply High</summary>
    public class mpyh : Bases.RR { public mpyh() : base("0111 1000 101") { } }
    /// <summary>Multiply and Shift Right</summary>
    public class mpys : Bases.RR { public mpys() : base("0111 1000 111") { } }
    /// <summary>Multiply High High</summary>
    public class mpyhh : Bases.RR { public mpyhh() : base("0111 1000 110") { } }
    /// <summary>Multiply High High and Add</summary>
    public class mpyhha : Bases.RR { public mpyhha() : base("0110 1000 110") { } }
    /// <summary>Multiply High High Unsigned</summary>
    public class mpyhhu : Bases.RR { public mpyhhu() : base("0111 1001 110") { } }
    /// <summary>Multiply High High Unsigned and Add</summary>
    public class mpyhhau : Bases.RR { public mpyhhau() : base("0110 1001 110") { } }
    /// <summary>Count Leading Zeros</summary>
    public class clz : Bases.R { public clz() : base("0101 0100 101") { } }
    /// <summary>Count Ones in Bytes</summary>
    public class cntb : Bases.R { public cntb() : base("0101 0110 100") { } }
    /// <summary>Form Select Mask for Bytes</summary>
    public class fsmb : Bases.R { public fsmb() : base("0011 0110 110") { } }
    /// <summary>Form Select Mask for Halfwords</summary>
    public class fsmh : Bases.R { public fsmh() : base("00110110 101") { } }
    /// <summary>Form Select Mask for Words</summary>
    public class fsm : Bases.R { public fsm() : base("0011 0110 100") { } }
    /// <summary>Gather Bits from Bytes</summary>
    public class gbb : Bases.R { public gbb() : base("0011 0110 010") { } }
    /// <summary>Gather Bits from Halfwords</summary>
    public class gbh : Bases.R { public gbh() : base("0111 0110 001") { } }
    /// <summary>Gather bits from words</summary>
    public class gb : Bases.R { public gb() : base("0011 0110 000") { } }
    /// <summary>Average Bytes</summary>
    public class avgb : Bases.RR { public avgb() : base("0001 1010 011") { } }
    /// <summary>Absolute Differences of Bytes</summary>
    public class absdb : Bases.RR { public absdb() : base("0000 1010 011") { } }
    /// <summary>Sum bytes into Halfwords</summary>
    public class sumb : Bases.RR { public sumb() : base("0100 1010 011") { } }
    /// <summary>Extend Sign Bute to Halfword</summary>
    public class xsbh : Bases.R { public xsbh() : base("0101 0110 110") { } }
    /// <summary>Extend Sign Halword to Word</summary>
    public class xshw : Bases.R { public xshw() : base("0101 0101 110") { } }
    /// <summary>Extend Sign Word to Doubleword</summary>
    public class xswd : Bases.R { public xswd() : base("0101 0100 110") { } }
    /// <summary>And</summary>
    public class and : Bases.RR { public and() : base("0001 1000 001") { } }
    /// <summary>And with Complement</summary>
    public class andc : Bases.RR { public andc() : base("0101 1000 001") { } }
    /// <summary>And Byte Immediate</summary>
    public class andbi : Bases.RI10 { public andbi() : base("0001 0110") { } }
    /// <summary>And Halfword Immediate</summary>
    public class andhi : Bases.RI10 { public andhi() : base("0001 0101") { } }
    /// <summary>And Word Immediate</summary>
    public class andi : Bases.RI10 { public andi() : base("0001 0100") { } }
    /// <summary>Or</summary>
    public class or : Bases.RR { public or() : base("0000 1000 001") { } }
    /// <summary>Or with Complement</summary>
    public class orc : Bases.RR { public orc() : base("0101 1001 001") { } }
    /// <summary>Or Byte Immediate</summary>
    public class orbi : Bases.RI10 { public orbi() : base("0000 0110") { } }
    /// <summary>Or Halfword Immediate</summary>
    public class orhi : Bases.RI10 { public orhi() : base("0000 0101") { } }
    /// <summary>Or Word Immediate</summary>
    public class ori : Bases.RI10 { public ori() : base("0000 0100") { } }
    /// <summary>Or Across</summary>
    public class orx : Bases.R { public orx() : base("0011 1110 000") { } }
    /// <summary>Exclusive Or</summary>
    public class xor : Bases.RR { public xor() : base("0100 1000 001") { } }
    /// <summary>Exclusive Or Byte Immediate</summary>
    public class xorbi : Bases.RI10 { public xorbi() : base("0100 0110") { } }
    /// <summary>Exclusive Or Halfword Immediate</summary>
    public class xorhi : Bases.RI10 { public xorhi() : base("0100 0101") { } }
    /// <summary>Exclusive Or Word Immediate</summary>
    public class xori : Bases.RI10 { public xori() : base("0100 0100") { } }
    /// <summary>Nand</summary>
    public class nand : Bases.RR { public nand() : base("0001 1001 001") { } }
    /// <summary>Nor</summary>
    public class nor : Bases.RR { public nor() : base("0000 1001 001") { } }
    /// <summary>Equivalent</summary>
    public class eqv : Bases.RR { public eqv() : base("0100 1001 001") { } }
    /// <summary>Select Bits</summary>
    public class selb : Bases.RRR { public selb() : base("1000") { } }
    /// <summary>Shuffle Bytes</summary>
    public class shufb : Bases.RRR { public shufb() : base("1011") { } }

    /// <summary>Shift Left Halfword</summary>
    public class shlh : Bases.RR { public shlh() : base("0000 1011 111") { } }
    /// <summary>Shift Left Halfword Immediate</summary>
    public class shlhi : Bases.RI7 { public shlhi() : base("0000 1111 111") { } }
    /// <summary>Shift Left Word</summary>
    public class shl : Bases.RR { public shl() : base("0000 1011 011") { } }
    /// <summary>Shift Left Word Immediate</summary>
    public class shli : Bases.RI7 { public shli() : base("0000 1111 011") { } }
    /// <summary>Shift Left Quadword by Bits</summary>
    public class shlqbi : Bases.RR { public shlqbi() : base("0011 1011 011") { } }
    /// <summary>Shift Left Quadword by Bits Immediate</summary>
    public class shlqbii : Bases.RI7 { public shlqbii() : base("0011 1111 011") { } }
    /// <summary>Shift Left Quadword by Bytes</summary>
    public class shlqby : Bases.RR { public shlqby() : base("0011 1011 111") { } }
    /// <summary>Shift Left Quadword by Bytes Immediate</summary>
    public class shlqbyi : Bases.RI7 { public shlqbyi() : base("0011 1111 111") { } }
    /// <summary>Shift left Quadword by Bytes from Bit Shift Count</summary>
    public class shlqbybi : Bases.RR { public shlqbybi() : base("0011 1001 111") { } }
    /// <summary>Rotate Halfword</summary>
    public class roth : Bases.RR { public roth() : base("0000 1011 100") { } }
    /// <summary>Rotate Halfword Immediate</summary>
    public class rothi : Bases.RI7 { public rothi() : base("0000 1111 100") { } }
    /// <summary>Rotate Word</summary>
    public class rot : Bases.RR { public rot() : base("0000 1011 000") { } }
    /// <summary>Rotate Word Immediate</summary>
    public class roti : Bases.RI7 { public roti() : base("0000 1111 000") { } }
    /// <summary>Rotate Quadword by Bytes</summary>
    public class rotqby : Bases.RR { public rotqby() : base("0011 1011 100") { } }
    /// <summary>Rotate Quadword by Bytes Immediate</summary>
    public class rotqbyi : Bases.RI7 { public rotqbyi() : base("0011 1111 100") { } }
    /// <summary>Rotate Quadword by Bytes from Bit Shift Count</summary>
    public class rotqbybi : Bases.RR { public rotqbybi() : base("0011 1001 100") { } }
    /// <summary>Rotate Quadword by Bits</summary>
    public class rotqbi : Bases.RR { public rotqbi() : base("0011 1011 000") { } }
    /// <summary>Rotate Quadword by Bits Immediate</summary>
    public class rotqbii : Bases.RI7 { public rotqbii() : base("0011 1111 000") { } }
    /// <summary>Rotate and Mask Halfword</summary>
    public class rothm : Bases.RR { public rothm() : base("0000 1011 101") { } }
    /// <summary>Rotate and Mask Halfword Immediate</summary>
    public class rothmi : Bases.RI7 { public rothmi() : base("0000 1111 101") { } }
    /// <summary>Rotate and Mask Word</summary>
    public class rotm : Bases.RR { public rotm() : base("0000 1011 001") { } }
    /// <summary>Rotate and Mask Word Immediate</summary>
    public class rotmi : Bases.RI7 { public rotmi() : base("0000 1111 001") { } }
    /// <summary>Rotate and Mask Quadword by Bytes</summary>
    public class rotqmby : Bases.RR { public rotqmby() : base("0011 1011 101") { } }
    /// <summary>Roate and Mask Quadword by Bytes Immediate</summary>
    public class rotqmbyi : Bases.RI7 { public rotqmbyi() : base("0011 1111 101") { } }
    /// <summary>Rotate and Mask Quadword Bytes from Bit Shift Count Required</summary>
    public class rotqmbybi : Bases.RR { public rotqmbybi() : base("0011 1001 101") { } }
    /// <summary>Rotate and Mask Quadword by Bits</summary>
    public class rotqmbi : Bases.RR { public rotqmbi() : base("0011 1011 001") { } }
    /// <summary>Rotate and Mask Quadword by Bits Immediate</summary>
    public class rotqmbii : Bases.RI7 { public rotqmbii() : base("0011 1111 001") { } }
    /// <summary>Rotate and Mask Algebraic Halfword</summary>
    public class rotmah : Bases.RR { public rotmah() : base("0000 1011 110") { } }
    /// <summary>Rotate and Mask Algebraic Halfword Immediate</summary>
    public class rotmahi : Bases.RI7 { public rotmahi() : base("0000 1111 110") { } }
    /// <summary>Rotate and Mask Algebraic Word</summary>
    public class rotma : Bases.RR { public rotma() : base("0000 1011 010") { } }
    /// <summary>Rotate and Mask Algebraic Word Immediate</summary>
    public class rotmai : Bases.RI7 { public rotmai() : base("0000 1111 010") { } }
    /// <summary>Halt If Equal</summary>
    public class heq : Bases.RR { public heq() : base("0111 1011 000") { } }
    /// <summary>Halt If Equal Immediate</summary>
    public class heqi : Bases.RI10 { public heqi() : base("0111 1111") { } }
    /// <summary>Half If Greater Than</summary>
    public class hgt : Bases.RR { public hgt() : base("0100 1011 000") { } }
    /// <summary>Halt If Greater Than Immediate</summary>
    public class hgti : Bases.RI10 { public hgti() : base("0100 1111") { } }
    /// <summary>Halt If logically Greater Than</summary>
    public class hlgt : Bases.RR { public hlgt() : base("0101 1011 000") { } }
    /// <summary>Halt If Logically Greater Than Immediate</summary>
    public class hlgti : Bases.RI10 { public hlgti() : base("0101 1111") { } }
    /// <summary>Compare Equal Byte</summary>
    public class ceqb : Bases.RR { public ceqb() : base("0111 1010 000") { } }
    /// <summary>Compare Equal Byte Immediate</summary>
    public class ceqbi : Bases.RI10 { public ceqbi() : base("0111 1110") { } }
    /// <summary>Compare Equal Halfword</summary>
    public class ceqh : Bases.RR { public ceqh() : base("0111 1001 000") { } }
    /// <summary>Compare Equal Halfword Immediate</summary>
    public class ceqhi : Bases.RI10 { public ceqhi() : base("0111 1101") { } }
    /// <summary>Compare Equal Word</summary>
    public class ceq : Bases.RR { public ceq() : base("0111 1000 000") { } }
    /// <summary>Compare Equal Word Immediate</summary>
    public class ceqi : Bases.RI10 { public ceqi() : base("0111 1100") { } }
    /// <summary>Compare Greater Than Byte</summary>
    public class cgtb : Bases.RR { public cgtb() : base("0010 1010 000") { } }
    /// <summary>Compare Greater Than Byte Immediate</summary>
    public class cgtbi : Bases.RI10 { public cgtbi() : base("0100 1110") { } }
    /// <summary>Compare Greater Than Halfword</summary>
    public class cgth : Bases.RR { public cgth() : base("0100 1001 000") { } }
    /// <summary>Compare Greater Than Halfword Immediate</summary>
    public class cgthi : Bases.RI10 { public cgthi() : base("0100 1101") { } }
    /// <summary>Compare Greater Than Word</summary>
    public class cgt : Bases.RR { public cgt() : base("0100 1000 000") { } }
    /// <summary>Compare Greater Than Word Immediate</summary>
    public class cgti : Bases.RI10 { public cgti() : base("0100 1100") { } }
    /// <summary>Compare Logical Greater Than Byte</summary>
    public class clgtb : Bases.RR { public clgtb() : base("0101 1010 000") { } }
    /// <summary>Compare Logical Greater Than Byte Immediate</summary>
    public class clgtbi : Bases.RI10 { public clgtbi() : base("0101 1110") { } }
    /// <summary>Compare Logical Greater Than Halfword</summary>
    public class clgth : Bases.RR { public clgth() : base("0101 1001 000") { } }
    /// <summary>Compare Logical Greater Than Halfword Immediate</summary>
    public class clgthi : Bases.RI10 { public clgthi() : base("0101 1101") { } }
    /// <summary>Compare Logical Greater Than Word</summary>
    public class clgt : Bases.RR { public clgt() : base("0101 1000 000") { } }
    /// <summary>Compare Logical Greater Than Word Immediate</summary>
    public class clgti : Bases.RI10 { public clgti() : base("0101 1100") { } }
    /// <summary>Branch Relative</summary>
    public class br : Bases.RI16 { public br() : base("0011 0010 0") { } } //NOTE: Target register is unused
    /// <summary>Branch Absolute</summary>
    public class bra : Bases.RI16 { public bra() : base("0011 0000 0") { } } //NOTE: Target register is unused
    /// <summary>Branch Relative and Set Link</summary>
    public class brsl : Bases.RI16 { public brsl() : base("0011 0011 0") { } }
    /// <summary>Branch Absolute and Set Link</summary>
    public class brasl : Bases.RI16 { public brasl() : base("0011 0001 0") { } }
    /// <summary>Branch Indirect</summary>
    public class bi : Bases.RInt { public bi() : base("0011 0101 000") { } } //NOTE: Target register is unused
    /// <summary>Interrupt Return</summary>
    public class iret : Bases.RInt { public iret() : base("0011 0101 010") { } } //NOTE: Target register is unused
    /// <summary>Branch Indirect and Set Link if External Data</summary>
    public class bisled : Bases.RInt { public bisled() : base("0011 0101 011") { } }
    /// <summary>Branch Indirect and Set Link</summary>
    public class bisl : Bases.RInt { public bisl() : base("0011 0101 001") { } }
    /// <summary>Branch If Not Zero Word</summary>
    public class brnz : Bases.RI16 { public brnz() : base("0010 0001 0") { } }
    /// <summary>Branch If Zero Word</summary>
    public class brz : Bases.RI16 { public brz() : base("0010 0000 0") { } }
    /// <summary>Branch If Not Zero Halfword</summary>
    public class brhnz : Bases.RI16 { public brhnz() : base("0010 0011 0") { } }
    /// <summary>Branch If Zero Halfword</summary>
    public class brhz : Bases.RI16 { public brhz() : base("0010 0010 0") { } }
    /// <summary>Branch Indirect If Zero</summary>
    public class biz : Bases.RInt { public biz() : base("0010 0101 000") { } }
    /// <summary>Branch Indirect If Not Zero</summary>
    public class binz : Bases.RInt { public binz() : base("0010 0101 001") { } }
    /// <summary>Branch Indirect If Zero Halfword</summary>
    public class bihz : Bases.RInt { public bihz() : base("0010 0101 010") { } }
    /// <summary>Branch Indirect If Not Zero Halfword</summary>
    public class bihnz : Bases.RInt { public bihnz() : base("0010 0101 011") { } }
    /// <summary>Hint for Branch (r-form)</summary>
    public partial class hbr : Bases.Instruction { public hbr() : base("0011 0101 100") { } }
    /// <summary>Hint for Branch (a-form)</summary>
    public class hbra : Bases.RBranch { public hbra() : base("0001 000") { } }
    /// <summary>Hint for Branch Relative</summary>
    public class hbrr : Bases.RBranch { public hbrr() : base("0001 001") { } }

    /// <summary>Floating Add</summary>
    public class fa : Bases.RR { public fa() : base("0101 1000 100") { } }
    /// <summary>Double Floating Add</summary>
    public class dfa : Bases.RR { public dfa() : base("0101 1001 100") { } }
    /// <summary>Floating Substract</summary>
    public class fs : Bases.RR { public fs() : base("0101 1000 101") { } }
    /// <summary>Double Floating Substract</summary>
    public class dfs : Bases.RR { public dfs() : base("0101 1001 101") { } }
    /// <summary>Floating Multiply</summary>
    public class fm : Bases.RR { public fm() : base("0101 1000 110") { } }
    /// <summary>Double Floating Multiply</summary>
    public class dfm : Bases.RR { public dfm() : base("0101 1001 110") { } }
    /// <summary>Floating Multiply and Add</summary>
    public class fma : Bases.RRR { public fma() : base("1110") { } }
    /// <summary>Double Floating Multiply and Add</summary>
    public class dfma : Bases.RR { public dfma() : base("0110 1011 100") { } }
    /// <summary>Floating Negative Multiply and Substract</summary>
    public class fnms : Bases.RRR { public fnms() : base("1101") { } }
    /// <summary>Double Floating Negative Multiply and Substract</summary>
    public class dfnms : Bases.RR { public dfnms() : base("0110 1011 110") { } }
    /// <summary>Floating Multiply and Substract</summary>
    public class fms : Bases.RRR { public fms() : base("1111") { } }
    /// <summary>Double Floating Multiply and Substract</summary>
    public class dfms : Bases.RR { public dfms() : base("0110 1011 101") { } }
    /// <summary>Double Floating Negative Multiply and Add</summary>
    public class dfnma : Bases.RR { public dfnma() : base("0110 1011 111") { } }
    /// <summary>Floating Reciprocal Estimate</summary>
    public class frest : Bases.R { public frest() : base("0011 0111 000") { } }
    /// <summary>Floating Reciprocal Absolute Square Root Estimate</summary>
    public class frsqest : Bases.R { public frsqest() : base("0011 0111 001") { } }
    /// <summary>Floating Interpolate</summary>
    public class fi : Bases.RR { public fi() : base("0111 1010 100") { } }
    /// <summary>Convert Signed Integer to Floating</summary>
    public class csflt : Bases.RI8 { public csflt() : base("0111 0110 10") { } }
    /// <summary>Convert Floating To Signed Integer</summary>
    public class cflts : Bases.RI8 { public cflts() : base("0111 0110 00") { } }
    /// <summary>Convert Unsigned Integer to Floating</summary>
    public class cuflt : Bases.RI8 { public cuflt() : base("0111 0110 11") { } }
    /// <summary>Convert Floating to Unsigned Integer</summary>
    public class cfltu : Bases.RI8 { public cfltu() : base("0111 0110 01") { } }
    /// <summary>Floating Round Double to Single</summary>
    public class frds : Bases.R { public frds() : base("0111 0111 001") { } }
    /// <summary>Floating Extend Single To Double</summary>
    public class fesd : Bases.R { public fesd() : base("0111 0111 000") { } }
    /// <summary>Double Floating Compare Equal</summary>
    public class dfceq : Bases.RR { public dfceq() : base("0111 1000 011") { } }
    /// <summary>Double Floating Compare Magnitude Equal</summary>
    public class dfcmeq : Bases.RR { public dfcmeq() : base("0111 1001 011") { } }
    /// <summary>Double Floating Compare Greater Than</summary>
    public class dfcgt : Bases.RR { public dfcgt() : base("0101 1000 011") { } }
    /// <summary>Double Floating Test Value</summary>
    public class dfcmgt : Bases.RR { public dfcmgt() : base("0111 0111 111") { } }
    /// <summary>Double Floating Compare Magnitude Greater Than</summary>
    public class dftsv : Bases.RI7 { public dftsv() : base("0101 1001 011") { } }
    /// <summary>Floating Compare Equal</summary>
    public class fceq : Bases.RR { public fceq() : base("0111 1000 010") { } }
    /// <summary>Floating Compare Magnitude Equal</summary>
    public class fcmeq : Bases.RR { public fcmeq() : base("0111 1001 010") { } }
    /// <summary>Floating Compare Greater Than</summary>
    public class fcgt : Bases.RR { public fcgt() : base("0101 1000 010") { } }
    /// <summary>Floating Compare Magnitude Greater Than</summary>
    public class fcmgt : Bases.RR { public fcmgt() : base("0101 1001 010") { } }
    /// <summary>Floating-Point Status and Control Register Write</summary>
    public class fscrwr : Bases.R { public fscrwr() : base("0111 0111 010") { } }
    /// <summary>Floating-Point Status and Control Register Read</summary>
    public class fscrrd : Bases.R0 { public fscrrd() : base("0111 0011 000") { } }

    /// <summary>Stop and Signal</summary>
    public partial class stop : Bases.Instruction { public stop() : base("0000 0000 000") { } }
    /// <summary>Stop and Signal with Dependencies</summary>
    public class stopd : Bases.RR { public stopd() : base("0010 1000 000") { } }
    /// <summary>No Operation (Load)</summary>
    public class lnop : Bases.Instruction { public lnop() : base("0000 0000 001") { } }
    /// <summary>No Operation (Execute)</summary>
    public class nop : Bases.R0 { public nop() : base("0100 0000 001") { } }
    /// <summary>Synchronize</summary>
    public partial class sync : Bases.Instruction { public sync() : base("0000 0000 010") { } }
    /// <summary>Synchronize data</summary>
    public class dsync : Bases.Instruction { public dsync() : base("0000 0000 011") { } }
    /// <summary>Move from Special-Purpose Register</summary>
    public class mfspr : Bases.R { public mfspr() : base("0000 0001 100") { } }
    /// <summary>Move to Special-Purpose Register</summary>
    public class mtspr : Bases.R { public mtspr() : base("0010 0001 100") { } }
    
    /// <summary>Read Channel</summary>
    public class rdch : Bases.R { public rdch() : base("0000 0001 101") { } }
    /// <summary>Read Channel Count</summary>
    public class rchcnt : Bases.R { public rchcnt() : base("0000 0001 111") { } }
    /// <summary>Write Channel</summary>
    public class wrch : Bases.R { public wrch() : base("0010 0001 101") { } }
}
