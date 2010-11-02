using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SPEEmulator.OpCodes.Bases
{
    #region Enumerations
    /// <summary>
    /// All known opcodes
    /// </summary>
    public enum Mnemonic
    {
        /// <summary>
        /// Add Word
        /// </summary>
        a,
        /// <summary>
        /// Absolute Differences of Bytes
        /// </summary>
        absdb,
        /// <summary>
        /// Add Extended
        /// </summary>
        addx,
        /// <summary>
        /// Add Halfword
        /// </summary>
        ah,
        /// <summary>
        /// Add Halfword Immediate
        /// </summary>
        ahi,
        /// <summary>
        /// Add Word Immediate
        /// </summary>
        ai,
        /// <summary>
        /// And
        /// </summary>
        and,
        /// <summary>
        /// And Byte Immediate
        /// </summary>
        andbi,
        /// <summary>
        /// And with Complement
        /// </summary>
        andc,
        /// <summary>
        /// And Halfword Immediate
        /// </summary>
        andhi,
        /// <summary>
        /// And Word Immediate
        /// </summary>
        andi,
        /// <summary>
        /// Average Bytes
        /// </summary>
        avgb,
        /// <summary>
        /// Borrow Generate
        /// </summary>
        bg,
        /// <summary>
        /// Borrow Generate Extended
        /// </summary>
        bgx,
        /// <summary>
        /// Branch Indirect
        /// </summary>
        bi,
        /// <summary>
        /// Branch Indirect If Not Zero Halfword
        /// </summary>
        bihnz,
        /// <summary>
        /// Branch Indirect If Zero Halfword
        /// </summary>
        bihz,
        /// <summary>
        /// Branch Indirect If Not Zero
        /// </summary>
        binz,
        /// <summary>
        /// Branch Indirect and Set Link
        /// </summary>
        bisl,
        /// <summary>
        /// Branch Indirect and Set Link if External Data
        /// </summary>
        bisled,
        /// <summary>
        /// Branch Indirect If Zero
        /// </summary>
        biz,
        /// <summary>
        /// Branch Relative
        /// </summary>
        br,
        /// <summary>
        /// Branch Absolute
        /// </summary>
        bra,
        /// <summary>
        /// Branch Absolute and Set Link
        /// </summary>
        brasl,
        /// <summary>
        /// Branch If Not Zero Halfword
        /// </summary>
        brhnz,
        /// <summary>
        /// Branch If Zero Halfword
        /// </summary>
        brhz,
        /// <summary>
        /// Branch If Not Zero Word
        /// </summary>
        brnz,
        /// <summary>
        /// Branch Relative and Set Link
        /// </summary>
        brsl,
        /// <summary>
        /// Branch If Zero Word
        /// </summary>
        brz,
        /// <summary>
        /// Generate Controls for Byte Insertion (d-form)
        /// </summary>
        cbd,
        /// <summary>
        /// Generate Controls for Byte Insertion (x-form)
        /// </summary>
        cbx,
        /// <summary>
        /// Generate Controls for Doubleword Insertion (d-form)
        /// </summary>
        cdd,
        /// <summary>
        /// Generate Controls for Doubleword Insertion (x-form)
        /// </summary>
        cdx,
        /// <summary>
        /// Compare Equal Word
        /// </summary>
        ceq,
        /// <summary>
        /// Compare Equal Byte
        /// </summary>
        ceqb,
        /// <summary>
        /// Compare Equal Byte Immediate
        /// </summary>
        ceqbi,
        /// <summary>
        /// Compare Equal Halfword
        /// </summary>
        ceqh,
        /// <summary>
        /// Compare Equal Halfword Immediate
        /// </summary>
        ceqhi,
        /// <summary>
        /// Compare Equal Word Immediate
        /// </summary>
        ceqi,
        /// <summary>
        /// Convert Floating to Signed Integer
        /// </summary>
        cflts,
        /// <summary>
        /// Convert Floating to Unsigned Integer
        /// </summary>
        cfltu,
        /// <summary>
        /// Carry Generate
        /// </summary>
        cg,
        /// <summary>
        /// Compare Greater Than Word
        /// </summary>
        cgt,
        /// <summary>
        /// Compare Greater Than Byte
        /// </summary>
        cgtb,
        /// <summary>
        /// Compare Greater Than Byte Immediate
        /// </summary>
        cgtbi,
        /// <summary>
        /// Compare Greater Than Halfword
        /// </summary>
        cgth,
        /// <summary>
        /// Compare Greater Than Halfword Immediate
        /// </summary>
        cgthi,
        /// <summary>
        /// Compare Greater Than Word Immediate
        /// </summary>
        cgti,
        /// <summary>
        /// Carry Generate Extended
        /// </summary>
        cgx,
        /// <summary>
        /// Generate Controls for Halfword Insertion (d-form)
        /// </summary>
        chd,
        /// <summary>
        /// Generate Controls for Halfword Insertion (x-form)
        /// </summary>
        chx,
        /// <summary>
        /// Compare Logical Greater Than Word
        /// </summary>
        clgt,
        /// <summary>
        /// Compare Logical Greater Than Byte
        /// </summary>
        clgtb,
        /// <summary>
        /// Compare Logical Greater Than Byte Immediate
        /// </summary>
        clgtbi,
        /// <summary>
        /// Compare Logical Greater Than Halfword
        /// </summary>
        clgth,
        /// <summary>
        /// Compare Logical Greater Than Halfword Immediate
        /// </summary>
        clgthi,
        /// <summary>
        /// Compare Logical Greater Than Word Immediate
        /// </summary>
        clgti,
        /// <summary>
        /// Count Leading Zeros
        /// </summary>
        clz,
        /// <summary>
        /// Count Ones in Bytes
        /// </summary>
        cntb,
        /// <summary>
        /// Convert Signed Integer to Floating
        /// </summary>
        csflt,
        /// <summary>
        /// Convert Unsigned Integer to Floating
        /// </summary>
        cuflt,
        /// <summary>
        /// Generate Controls for Word Insertion (d-form)
        /// </summary>
        cwd,
        /// <summary>
        /// Generate Controls for Word Insertion (x-form)
        /// </summary>
        cwx,
        /// <summary>
        /// Double Floating Add
        /// </summary>
        dfa,
        /// <summary>
        /// Double Floating Compare Equal
        /// </summary>
        dfceq,
        /// <summary>
        /// Double Floating Compare Greater Than
        /// </summary>
        dfcgt,
        /// <summary>
        /// Double Floating Compare Magnitude Equal
        /// </summary>
        dfcmeq,
        /// <summary>
        /// Double Floating Compare Magnitude Greater Than
        /// </summary>
        dfcmgt,
        /// <summary>
        /// Double Floating Multiply
        /// </summary>
        dfm,
        /// <summary>
        /// Double Floating Multiply and Add
        /// </summary>
        dfma,
        /// <summary>
        /// Double Floating Multiply and Subtract
        /// </summary>
        dfms,
        /// <summary>
        /// Double Floating Negative Multiply and Add
        /// </summary>
        dfnma,
        /// <summary>
        /// Double Floating Multiply and Subtract
        /// </summary>
        dfnms,
        /// <summary>
        /// Double Floating Subtract
        /// </summary>
        dfs,
        /// <summary>
        /// Double Floating Test Special Value
        /// </summary>
        dftsv,
        /// <summary>
        /// Synchronize Data
        /// </summary>
        dsync,
        /// <summary>
        /// Equivalent
        /// </summary>
        eqv,
        /// <summary>
        /// Floating Add
        /// </summary>
        fa,
        /// <summary>
        /// Floating Compare Equal
        /// </summary>
        fceq,
        /// <summary>
        /// Floating Compare Greater Than
        /// </summary>
        fcgt,
        /// <summary>
        /// Floating Compare Magnitude Equal
        /// </summary>
        fcmeq,
        /// <summary>
        /// Floating Compare Magnitude Greater Than
        /// </summary>
        fcmgt,
        /// <summary>
        /// Floating Extend Single to Double
        /// </summary>
        fesd,
        /// <summary>
        /// Floating Interpolate
        /// </summary>
        fi,
        /// <summary>
        /// Floating Multiply
        /// </summary>
        fm,
        /// <summary>
        /// Floating Multiply and Add
        /// </summary>
        fma,
        /// <summary>
        /// Floating Multiply and Subtract
        /// </summary>
        fms,
        /// <summary>
        /// Floating Negative Multiply and Subtract
        /// </summary>
        fnms,
        /// <summary>
        /// Floating Round Double to Single
        /// </summary>
        frds,
        /// <summary>
        /// Floating Reciprocal Estimate
        /// </summary>
        frest,
        /// <summary>
        /// Floating Reciprocal Absolute Square Root Estimate
        /// </summary>
        frsqest,
        /// <summary>
        /// Floating Subtract
        /// </summary>
        fs,
        /// <summary>
        /// Floating-Point Status and Control Register Write
        /// </summary>
        fscrrd,
        /// <summary>
        /// Floating-Point Status and Control Register Read
        /// </summary>
        fscrwr,
        /// <summary>
        /// Form Select Mask for Words
        /// </summary>
        fsm,
        /// <summary>
        /// Form Select Mask for Bytes
        /// </summary>
        fsmb,
        /// <summary>
        /// Form Select Mask for Bytes Immediate
        /// </summary>
        fsmbi,
        /// <summary>
        /// Form Select Mask for Halfwords
        /// </summary>
        fsmh,
        /// <summary>
        /// Gather Bits from Words
        /// </summary>
        gb,
        /// <summary>
        /// Gather Bits from Bytes
        /// </summary>
        gbb,
        /// <summary>
        /// Gather Bits from Halfwords
        /// </summary>
        gbh,
        /// <summary>
        /// Hint for Branch (r-form)
        /// </summary>
        hbr,
        /// <summary>
        /// Hint for Branch (a-form)
        /// </summary>
        hbra,
        /// <summary>
        /// Hint for Branch Relative
        /// </summary>
        hbrr,
        /// <summary>
        /// Halt If Equal
        /// </summary>
        heq,
        /// <summary>
        /// Halt If Equal Immediate
        /// </summary>
        heqi,
        /// <summary>
        /// Halt If Greater Than
        /// </summary>
        hgt,
        /// <summary>
        /// Halt If Greater Than Immediate
        /// </summary>
        hgti,
        /// <summary>
        /// Halt If Logically Greater Than
        /// </summary>
        hlgt,
        /// <summary>
        /// Halt If Logically Greater Than Immediate
        /// </summary>
        hlgti,
        /// <summary>
        /// Immediate Load Word
        /// </summary>
        il,
        /// <summary>
        /// Immediate Load Address
        /// </summary>
        ila,
        /// <summary>
        /// Immediate Load Halfword
        /// </summary>
        ilh,
        /// <summary>
        /// Immediate Load Halfword Upper
        /// </summary>
        ilhu,
        /// <summary>
        /// Immediate Or Halfword Lower
        /// </summary>
        iohl,
        /// <summary>
        /// Interrupt Return
        /// </summary>
        iret,
        /// <summary>
        /// No Operation (Load)
        /// </summary>
        lnop,
        /// <summary>
        /// Load Quadword (a-form)
        /// </summary>
        lqa,
        /// <summary>
        /// Load Quadword (d-form)
        /// </summary>
        lqd,
        /// <summary>
        /// Load Quadword Instruction Relative (a-form)
        /// </summary>
        lqr,
        /// <summary>
        /// Load Quadword (x-form)
        /// </summary>
        lqx,
        /// <summary>
        /// Move from Special-Purpose Register
        /// </summary>
        mfspr,
        /// <summary>
        /// Multiply
        /// </summary>
        mpy,
        /// <summary>
        /// Multiply and Add
        /// </summary>
        mpya,
        /// <summary>
        /// Multiply High
        /// </summary>
        mpyh,
        /// <summary>
        /// Multiply High High
        /// </summary>
        mpyhh,
        /// <summary>
        /// Multiply High High and Add
        /// </summary>
        mpyhha,
        /// <summary>
        /// Multiply High High Unsigned and Add
        /// </summary>
        mpyhhau,
        /// <summary>
        /// Multiply High High Unsigned
        /// </summary>
        mpyhhu,
        /// <summary>
        /// Multiply Immediate
        /// </summary>
        mpyi,
        /// <summary>
        /// Multiply and Shift Right
        /// </summary>
        mpys,
        /// <summary>
        /// Multiply Unsigned
        /// </summary>
        mpyu,
        /// <summary>
        /// Multiply Unsigned Immediate
        /// </summary>
        mpyui,
        /// <summary>
        /// Move to Special-Purpose Register
        /// </summary>
        mtspr,
        /// <summary>
        /// Nand
        /// </summary>
        nand,
        /// <summary>
        /// No Operation (Execute)
        /// </summary>
        nop,
        /// <summary>
        /// Nor
        /// </summary>
        nor,
        /// <summary>
        /// Or
        /// </summary>
        or,
        /// <summary>
        /// Or Byte Immediate
        /// </summary>
        orbi,
        /// <summary>
        /// Or with Complement
        /// </summary>
        orc,
        /// <summary>
        /// Or Halfword Immediate
        /// </summary>
        orhi,
        /// <summary>
        /// Or Word Immediate
        /// </summary>
        ori,
        /// <summary>
        /// Or Across
        /// </summary>
        orx,
        /// <summary>
        /// Read Channel Count
        /// </summary>
        rchcnt,
        /// <summary>
        /// Read Channel
        /// </summary>
        rdch,
        /// <summary>
        /// Rotate Word
        /// </summary>
        rot,
        /// <summary>
        /// Rotate Halfword
        /// </summary>
        roth,
        /// <summary>
        /// Rotate Halfword Immediate
        /// </summary>
        rothi,
        /// <summary>
        /// Rotate and Mask Halfword
        /// </summary>
        rothm,
        /// <summary>
        /// Rotate and Mask Halfword Immediate
        /// </summary>
        rothmi,
        /// <summary>
        /// Rotate Word Immediate
        /// </summary>
        roti,
        /// <summary>
        /// Rotate and Mask Word
        /// </summary>
        rotm,
        /// <summary>
        /// Rotate and Mask Algebraic Word
        /// </summary>
        rotma,
        /// <summary>
        /// Rotate and Mask Algebraic Halfword
        /// </summary>
        rotmah,
        /// <summary>
        /// Rotate and Mask Algebraic Halfword Immediate
        /// </summary>
        rotmahi,
        /// <summary>
        /// Rotate and Mask Algebraic Word Immediate
        /// </summary>
        rotmai,
        /// <summary>
        /// Rotate and Mask Word Immediate
        /// </summary>
        rotmi,
        /// <summary>
        /// Rotate Quadword by Bits
        /// </summary>
        rotqbi,
        /// <summary>
        /// Rotate Quadword by Bits Immediate
        /// </summary>
        rotqbii,
        /// <summary>
        /// Rotate Quadword by Bytes
        /// </summary>
        rotqby,
        /// <summary>
        /// Rotate Quadword by Bytes from Bit Shift Count
        /// </summary>
        rotqbybi,
        /// <summary>
        /// Rotate Quadword by Bytes Immediate
        /// </summary>
        rotqbyi,
        /// <summary>
        /// Rotate and Mask Quadword by Bits
        /// </summary>
        rotqmbi,
        /// <summary>
        /// Rotate and Mask Quadword by Bits Immediate
        /// </summary>
        rotqmbii,
        /// <summary>
        /// Rotate and Mask Quadword by Bytes
        /// </summary>
        rotqmby,
        /// <summary>
        /// Rotate and Mask Quadword Bytes from Bit Shift Count
        /// </summary>
        rotqmbybi,
        /// <summary>
        /// Rotate and Mask Quadword by Bytes Immediate
        /// </summary>
        rotqmbyi,
        /// <summary>
        /// Select Bits 
        /// </summary>
        selb,
        /// <summary>
        /// Subtract from Word
        /// </summary>
        sf,
        /// <summary>
        /// Subtract from Halfword
        /// </summary>
        sfh,
        /// <summary>
        /// Subtract from Halfword Immediate
        /// </summary>
        sfhi,
        /// <summary>
        /// Subtract from Word Immediate
        /// </summary>
        sfi,
        /// <summary>
        /// Subtract from Extended
        /// </summary>
        sfx,
        /// <summary>
        /// Shift Left Word
        /// </summary>
        shl,
        /// <summary>
        /// Shift Left Halfword
        /// </summary>
        shlh,
        /// <summary>
        /// Shift Left Halfword Immediate
        /// </summary>
        shlhi,
        /// <summary>
        /// Shift Left Word Immediate
        /// </summary>
        shli,
        /// <summary>
        /// Shift Left Quadword by Bits
        /// </summary>
        shlqbi,
        /// <summary>
        /// Shift Left Quadword by Bits Immediate
        /// </summary>
        shlqbii,
        /// <summary>
        /// Shift Left Quadword by Bytes
        /// </summary>
        shlqby,
        /// <summary>
        /// Shift Left Quadword by Bytes from Bit Shift Count
        /// </summary>
        shlqbybi,
        /// <summary>
        /// Shift Left Quadword by Bytes Immediate
        /// </summary>
        shlqbyi,
        /// <summary>
        /// Shuffle Bytes
        /// </summary>
        shufb,
        /// <summary>
        /// Stop and Signal
        /// </summary>
        stop,
        /// <summary>
        /// Stop and Signal with Dependencies
        /// </summary>
        stopd,
        /// <summary>
        /// Store Quadword (a-form)
        /// </summary>
        stqa,
        /// <summary>
        /// Store Quadword (d-form)
        /// </summary>
        stqd,
        /// <summary>
        /// Store Quadword Instruction Relative (a-form)
        /// </summary>
        stqr,
        /// <summary>
        /// Store Quadword (x-form)
        /// </summary>
        stqx,
        /// <summary>
        /// Sum Bytes into Halfwords
        /// </summary>
        sumb,
        /// <summary>
        /// Synchronize
        /// </summary>
        sync,
        /// <summary>
        /// Write Channel
        /// </summary>
        wrch,
        /// <summary>
        /// Exclusive Or 
        /// </summary>
        xor,
        /// <summary>
        /// Exclusive Or Byte Immediate
        /// </summary>
        xorbi,
        /// <summary>
        /// Exclusive Or Halfword Immediate
        /// </summary>
        xorhi,
        /// <summary>
        /// Exclusive Or Word Immediate
        /// </summary>
        xori,
        /// <summary>
        /// Extend Sign Byte to Halfword
        /// </summary>
        xsbh,
        /// <summary>
        /// Extend Sign Halfword to Word
        /// </summary>
        xshw,
        /// <summary>
        /// Extend Sign Word to Doubleword
        /// </summary>
        xswd,
    }
    #endregion

    /// <summary>
    /// The base class for all opcodes
    /// </summary>
    public class Instruction
    {
        /// <summary>
        /// The size of a register number, 7 bits
        /// </summary>
        protected const int REGISTER_SIZE = 7;

        /// <summary>
        /// The mask for a register number, 7 bits
        /// </summary>
        protected const int REGISTER_MASK = 0x7f;

        /// <summary>
        /// The internal opcode
        /// </summary>
        protected Mnemonic m_mnemonic;

        /// <summary>
        /// The opcode bit signature
        /// </summary>
        protected uint m_signature;

        /// <summary>
        /// The opcode mask
        /// </summary>
        protected uint m_mask;

        /// <summary>
        /// The current binary value this opcode represents
        /// </summary>
        protected uint m_value;

        /// <summary>
        /// Gets the Mnemonic for the opcode
        /// </summary>
        public Mnemonic Mnemonic { get { return m_mnemonic; } }

        /// <summary>
        /// Gets the current binary value for this instruction
        /// </summary>
        public uint Value 
        { 
            get { return m_value;}
            set
            {
                System.Diagnostics.Trace.Assert((value & m_mask) == m_signature);
                m_value = value;
            }
        }

        /// <summary>
        /// Gets the opcode bitmask
        /// </summary>
        public uint Mask { get { return m_mask; } }

        /// <summary>
        /// Constructs an opcode base class
        /// </summary>
        /// <param name="mnemonic">The mnemonic for the opcode</param>
        /// <param name="signature">The bit signature for the opcode</param>
        /// <param name="mask">The bitmask used to signal significant bits in the signature</param>
        protected Instruction(uint signature, uint mask)
        {
            if (!Enum.TryParse<Mnemonic>(this.GetType().Name, true, out m_mnemonic))
                throw new Exception(string.Format("Unknown Mnemonic {0}", this.GetType().Name));

            m_signature = signature;
            m_mask = mask;
            m_value = m_signature;

            System.Diagnostics.Trace.Assert((m_signature & ~m_mask) == 0);
        }

        /// <summary>
        /// Constructs an opcode base class
        /// </summary>
        /// <param name="bitpattern">A string representation of the bitpattern to match</param>
        protected Instruction(string bitpattern)
            : this(ParsePattern(bitpattern), GenerateMask(bitpattern))
        {
        }

        /// <summary>
        /// Parses a bitstring into an uint, value is assumed to be aligned to MSB, and any missing bits are filled with 0
        /// </summary>
        /// <param name="value">The bitpattern to calculate the value for</param>
        /// <returns>The unsigned integer representing the value</returns>
        private static uint ParsePattern(string value)
        {
            System.Diagnostics.Trace.Assert(value != null); 
            System.Diagnostics.Trace.Assert(value.Length <= 32);

            value = value.Replace(" ", "");
            value += new string('0', 32 - value.Length);
            uint res = 0;
            foreach (char c in value)
            {
                res = res << 1;
                res |= (uint)(c == '1' ? 1 : 0);
            }
            return res;
        }

        /// <summary>
        /// Produces a bit mask that matches the n MSB bits, where n is the length of the string
        /// </summary>
        /// <param name="value">The bitstring to mathc</param>
        /// <returns>The unsigned integer bit mask</returns>
        private static uint GenerateMask(string value)
        {
            return ParsePattern(new string('1', value.Replace(" ", "").Length));
        }

        public override string ToString()
        {
            return Mnemonic.ToString();
        }
    }
}
