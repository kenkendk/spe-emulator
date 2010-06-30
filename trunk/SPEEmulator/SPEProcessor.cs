using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SPEEmulator
{
    public delegate void StatusEventDelegate(SPEProcessor sender);

    /// <summary>
    /// Describes the possible states for the SPE
    /// </summary>
    public enum SPEState
    {
        /// <summary>
        /// Indicates that the SPE is has not yet started
        /// </summary>
        NotStarted,
        /// <summary>
        /// Indicates that the SPE is running
        /// </summary>
        Running,
        /// <summary>
        /// Indicates that the SPE is paused
        /// </summary>
        Paused,
        /// <summary>
        /// Indicates that the SPE has completed its run
        /// </summary>
        Terminated
    }

    /// <summary>
    /// Class that simulates a single complete SPE unit
    /// </summary>
    public class SPEProcessor
    {
        #region Default Constants
        /// <summary>
        /// The size of the SPE LS
        /// </summary>
        private const int DEFAULT_LS_SIZE = 256 * 1024;
        /// <summary>
        /// The size of the SPE mbox buffer
        /// </summary>
        private const int DEFAULT_MBOX_SIZE = 4;
        #endregion

        #region Private variables
        /// <summary>
        /// The lock to protect shared structures
        /// </summary>
        private object m_lock = new object();
        /// <summary>
        /// The current SPE state
        /// </summary>
        private SPEState m_state = SPEState.NotStarted;
        /// <summary>
        /// The SPE local store
        /// </summary>
        private byte[] m_ls;
        /// <summary>
        /// The SPE outgoing normal mbox
        /// </summary>
        private Queue<uint> m_outMbox;
        /// <summary>
        /// The SPE outgoing interupt mbox
        /// </summary>
        private Queue<uint> m_outIntrMbox;
        /// <summary>
        /// The SPE ingoing mbox
        /// </summary>
        private Queue<uint> m_inMbox;

        /// <summary>
        /// The size of the outgoing normal mbox buffer
        /// </summary>
        private int m_outMboxSize;
        /// <summary>
        /// The size of the outgoing interrupt mbox buffer
        /// </summary>
        private int m_outIntrMboxSize;
        /// <summary>
        /// The size of the ingoing mbox buffer
        /// </summary>
        private int m_inMboxSize;

        /// <summary>
        /// An event that is signalled when a queue is ready
        /// </summary>
        private System.Threading.ManualResetEvent m_event;

        /// <summary>
        /// The SPU execution unit
        /// </summary>
        private SPU m_spu;
        /// <summary>
        /// The MFC unit
        /// </summary>
        private MFC m_mfc;
        #endregion

        #region Public Events
        /// <summary>
        /// Signals that the SPE is now running
        /// </summary>
        public event StatusEventDelegate SPEStarted;
        /// <summary>
        /// Signals that the SPE is now stopped
        /// </summary>
        public event StatusEventDelegate SPEStopped;
        /// <summary>
        /// Signals that the SPE is now paused
        /// </summary>
        public event StatusEventDelegate SPEPaused;
        /// <summary>
        /// Signals that the SPE is now resumed
        /// </summary>
        public event StatusEventDelegate SPEResumed;

        /// <summary>
        /// Signals that the SPE has written its outgoing normal mailbox
        /// </summary>
        public event StatusEventDelegate MboxWritten;
        /// <summary>
        /// Signals that the SPE has written its outgoing interrupt mailbox
        /// </summary>
        public event StatusEventDelegate IntrMboxWritten;
        /// <summary>
        /// Signals that the SPE has read its ingoing mailbox
        /// </summary>
        public event StatusEventDelegate InMboxRead;
        #endregion

        #region Public methods
        /// <summary>
        /// Constructs a new SPE unit
        /// </summary>
        /// <param name="LS_Size">The size of the LS in bytes</param>
        /// <param name="mboxsize">The number of mailbox messages that can be stored in the outgoing normal mailbox</param>
        /// <param name="intrmboxsize">The number of mailbox messages that can be stored in the outgoing interrupt mailbox</param>
        /// <param name="inmboxsize">The number of mailbox messages that can be stored in the ingoing mailbox</param>
        public SPEProcessor(uint LS_Size = DEFAULT_LS_SIZE, uint mboxsize = DEFAULT_MBOX_SIZE, uint intrmboxsize = DEFAULT_MBOX_SIZE, uint inmboxsize = DEFAULT_MBOX_SIZE)
        {
            m_ls = new byte[LS_Size];
            m_outMboxSize = (int)mboxsize;
            m_outIntrMboxSize = (int)intrmboxsize;
            m_inMboxSize = (int)inmboxsize;

            m_outMbox = new Queue<uint>(m_outMboxSize);
            m_outIntrMbox = new Queue<uint>(m_outIntrMboxSize);
            m_inMbox = new Queue<uint>(m_inMboxSize);

            m_spu = new SPU(this);
            m_mfc = new MFC();
        }

        /// <summary>
        /// Gets the current SPE state
        /// </summary>
        public SPEState State { get { lock (m_lock) return m_state; } }

        /// <summary>
        /// Gets the SPE LS
        /// </summary>
        public byte[] LS { get { return m_ls; } }

        /// <summary>
        /// Gets the Local Storage Limit Register value
        /// </summary>
        public uint LSLR { get { return /*(0x8000 << 1) - 1;*/ 0xffffffffu; } } //TODO: Calculate using m_ls.Length

        /// <summary>
        /// Gets the SPU instance
        /// </summary>
        public SPU SPU { get { return m_spu; } }

        /// <summary>
        /// Starts the SPE, executing the datastream presented
        /// </summary>
        public void Start()
        {
            lock (m_lock)
            {
                if (m_state != SPEState.NotStarted)
                    throw new InvalidOperationException(string.Format("Cannot start the SPE while state is {0}", m_state));

                m_state = SPEState.Running;
            }

            if (SPEStarted != null)
                SPEStarted(this);

            m_spu.Run();
            //m_mfc.Start();

        }

        /// <summary>
        /// Stops the SPE execution
        /// </summary>
        public void Stop()
        {
            lock (m_lock)
            {
                if (m_state == SPEState.NotStarted)
                    return;

                if (m_state == SPEState.Terminated)
                    return;

                m_state = SPEState.Terminated;
            }

            //m_spu.Stop();
            //m_mfc.Stop();

            if (SPEStopped != null)
                SPEStopped(this);
        }

        /// <summary>
        /// Pauses the SPE execution
        /// </summary>
        public void Pause()
        {
            lock (m_lock)
            {
                if (m_state == SPEState.NotStarted || m_state == SPEState.Terminated)
                    throw new InvalidOperationException(string.Format("Cannot pause the SPE while state is {0}", m_state));

                if (m_state == SPEState.Paused)
                    return;

                m_state = SPEState.Paused;
            }
            
            //m_spu.Pause();
            //m_mfc.Pause();

            if (SPEPaused != null)
                SPEPaused(this);
        }

        /// <summary>
        /// Resumes the SPE execution
        /// </summary>
        public void Resume()
        {
            lock (m_lock)
            {
                if (m_state == SPEState.NotStarted || m_state == SPEState.Terminated)
                    throw new InvalidOperationException(string.Format("Cannot resume the SPE while state is {0}", m_state));

                if (m_state == SPEState.Running)
                    return;

                m_state = SPEState.Running;
            }

            //m_spu.Resume();
            //m_mfc.Resume();

            if (SPEResumed != null)
                SPEResumed(this);
        }

        /// <summary>
        /// Writes a message to the mailbox
        /// </summary>
        /// <param name="value">The value to write</param>
        /// <param name="block">True if the call should block until the message can be written, false otherwise</param>
        /// <returns>True if the message was put, false otherwise</returns>
        public bool WriteMbox(uint value, bool block = true)
        {
            while (true)
            {
                lock (m_lock)
                {
                    if (m_inMbox.Count >= m_inMboxSize)
                    {
                        if (!block)
                            return false;
                    }
                    else
                    {
                        m_inMbox.Enqueue(value);
                        return true;
                    }
                }

                m_event.WaitOne(1000);
            }

            throw new InvalidProgramException("Not supposed to get here");
        }

        /// <summary>
        /// Reads a message from the SPE's outgoing normal mbox
        /// </summary>
        /// <param name="block">True if the call should block until a message is read, false otherwise</param>
        /// <returns>The value read, or null if no value was read</returns>
        public uint? ReadMbox(bool block = true)
        {
            return ReadMbox(m_outMbox, block);
        }

        /// <summary>
        /// Reads a message from the SPE's outgoing interrupt mailbox
        /// </summary>
        /// <param name="block">True if the call should block until a message is read, false otherwise</param>
        /// <returns>The value read, or null if no value was read</returns>
        public uint? ReadIntrMbox(bool block = true)
        {
            return ReadMbox(m_outIntrMbox, block);
        }

        /// <summary>
        /// Gets the number of unread messages SPE's outgoing mailbox
        /// </summary>
        public int MboxSize
        {
            get
            {
                lock (m_lock)
                    return m_outMbox.Count;
            }
        }

        /// <summary>
        /// Gets the number of unread messages SPE's outgoing interrupt mailbox
        /// </summary>
        public int MboxIntrSize
        {
            get
            {
                lock (m_lock)
                    return m_outMbox.Count;
            }
        }

        /// <summary>
        /// Gets the number of avalible messages in the SPE's ingoing mailbox queue
        /// </summary>
        public int InMboxSize
        {
            get
            {
                lock (m_lock)
                    return m_inMboxSize - m_inMbox.Count;
            }
        }


        #endregion

        #region Private methods

        /// <summary>
        /// Reads a mbox message from a queue, possibly blocking
        /// </summary>
        /// <param name="mbox">The queue to read from</param>
        /// <param name="block">True if the request should block, false otherwise</param>
        /// <returns>The value read, or null if no value was read.</returns>
        private uint? ReadMbox(Queue<uint> mbox, bool block)
        {
            while (true)
            {
                lock (m_lock)
                {
                    if (mbox.Count <= 0)
                    {
                        if (!block)
                            return null;
                    }
                    else
                    {
                        return mbox.Dequeue();
                    }
                }

                m_event.WaitOne(1000);
            }

            throw new InvalidProgramException("Not supposed to get here");
        }
        #endregion
    }
}
