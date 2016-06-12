using System;

namespace CycleBuffer
{
    public class CycleBuffer<T>
    {
        private T[] m_Buffer;
        private int m_Offset;
        private int m_Length;

        public CycleBuffer(int pCapacity)
        {
            m_Buffer = new T[pCapacity];
            m_Length = 0;
        }

        public int Offset { get { return m_Offset; } }
        public int Length { get { return m_Length; } }

        public T[] Read(int pCount)
        {
            if (pCount > m_Length)
                throw new ArgumentOutOfRangeException("There is not enough data to read.");

            T[] result = new T[pCount];
            if (pCount + m_Offset <= m_Buffer.Length)
            {
                Buffer.BlockCopy(m_Buffer, m_Offset, result, 0, pCount);
            }
            else // need to read twice
            {
                int firstRead = m_Buffer.Length - m_Offset;
                Buffer.BlockCopy(m_Buffer, m_Offset, result, 0, firstRead);
                Buffer.BlockCopy(m_Buffer, 0, result, firstRead, pCount - firstRead);
            }

            // Delete old data which has been read.
            Delete(pCount);

            return result;
        }


        public void Write(T[] pData)
        {
            if (pData.Length + m_Length > m_Buffer.Length)
                throw new ArgumentOutOfRangeException("There is not enough space the data.");

            if (pData.Length + (m_Offset + m_Length) <= m_Buffer.Length)
            {
                Buffer.BlockCopy(pData, 0, m_Buffer, m_Offset + m_Length, pData.Length);
            }
            else
            {
                int firstWrite = m_Buffer.Length - (m_Offset + m_Length);
                Buffer.BlockCopy(pData, 0, m_Buffer, (m_Offset + m_Length), firstWrite);
                Buffer.BlockCopy(pData, firstWrite, m_Buffer, 0, pData.Length - firstWrite);
            }

            m_Length += pData.Length;
        }

        public void Delete(int pCount)
        {
            if (pCount > m_Length)
                throw new ArgumentOutOfRangeException("There is not enough bytes to delete");

            m_Offset = (m_Offset + pCount) % m_Buffer.Length;
            m_Length -= pCount;
        }

    }
}
