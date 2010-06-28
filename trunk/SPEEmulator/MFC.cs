using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SPEEmulator
{
    class MFC
    {
        #region Enumerations
        /// <summary>
        /// A list of all documented MFC channels
        /// </summary>
        public enum Channels
        {
            /// <summary>
            /// Write multisource synchronization request
            /// </summary>
            MFC_WrMSSyncReq = 9,
            /// <summary>
            /// Read tag mask
            /// </summary>
            MFC_RdTagMask = 12,
            /// <summary>
            /// Write local memory address command parameter
            /// </summary>
            MFC_LSA = 16,
            /// <summary>
            /// Write high order DMA effective address command parameter
            /// </summary>
            MFC_EAH = 17,
            /// <summary>
            /// Write low order DMA effective address command parameter
            /// </summary>
            MFC_EAL = 18,
            /// <summary>
            /// Write DMA transfer size command parameter
            /// </summary>
            MFC_Size = 19,
            /// <summary>
            /// Write tag identifier command parameter
            /// </summary>
            MFC_TagID = 20,
            /// <summary>
            /// Write and enqueue DMA command with associated class ID
            /// </summary>
            MFC_Cmd = 21,
            /// <summary>
            /// Write tag mask
            /// </summary>
            MFC_WrTagMask = 22,
            /// <summary>
            /// Write request for conditional or unconditional tag status update
            /// </summary>
            MFC_WrTagUpdate = 23,
            /// <summary>
            /// Read tag status with mask applied
            /// </summary>
            MFC_RdTagStat = 24,
            /// <summary>
            /// Read DMA list stall-and-notify status
            /// </summary>
            MFC_RdListStallStat = 25,
            /// <summary>
            /// Write DMA list stall-and-notify acknowledge
            /// </summary>
            MFC_WrListStallAck = 26,
            /// <summary>
            /// Read completion status of last completed immediate MFC atomic update command (see the Synergistic Processor Unit Channels section of Cell Broadband Engine Architecture) 
            /// </summary>
            MFC_RdAtomicStat = 27
        }
        #endregion
    }
}
