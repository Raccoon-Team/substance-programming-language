using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CompilerUtilities.ExtendedOperations
{
    static class BlockOperations
    {
        public static List<(int Open, int Close)> FindAllBlocks<T>(this IEnumerable<T> collection, T openElem, T closeElem)
        {
            List<(int Open, int Close)> BlocksId = new List<(int Open, int Close)>();
            var blockCollection = collection.Select((elem, index) => ((T Value, int Index))(elem, index))
                                            .Where(elem => elem.Value.Equals(openElem) || elem.Value.Equals(closeElem))
                                            .ToList();

            while (blockCollection.Count > 0)
            {
                var pair = FindFirstBlock(blockCollection, openElem, closeElem);
                if (pair.Open == -1 || pair.Close == -1)
                    throw new InvalidDataException($"Didn't expected complete block. OpenIndex: {pair.Open}, CloseIndex: {pair.Close}");
                BlocksId.Add(pair);
                blockCollection.RemoveAll(elem => elem.Index == pair.Open);
                blockCollection.RemoveAll(elem => elem.Index == pair.Close);
            }

            return BlocksId;
        }

        private static (int Open, int Close) FindFirstBlock<T>(List<(T Value, int Index)> blockCollection, T openElem, T closeElem)
        {
            if (!blockCollection.All(elem => elem.Value.Equals(openElem) || elem.Value.Equals(closeElem)))
                throw new InvalidDataException($"Detected not valid block elements");

            var nestingAcc = 0;
            var openIndex = blockCollection.FindIndex(elem => elem.Value.Equals(openElem));

            var firstClose = blockCollection.FindIndex(elem => elem.Value.Equals(closeElem));
            if (firstClose < openIndex)
                throw new InvalidDataException($"Close elem detected before open (first openIndex {blockCollection[openIndex].Index}; first closeIndex {blockCollection[firstClose].Index})");

            var closeIndex = -1;

            if (openIndex != -1)
            {
                for (int i = openIndex + 1; i < blockCollection.Count; i++)
                {
                    bool isClose = blockCollection[i].Value.Equals(closeElem);
                    if (isClose && nestingAcc == 0)
                    {
                        closeIndex = i;
                        break;
                    }
                    if (isClose)
                    {
                        nestingAcc--;
                    }
                    else
                    {
                        nestingAcc++;
                    }
                }
            }

            if (openIndex == -1)
                return (-1, -1);
            if (closeIndex == -1)
                return (blockCollection[openIndex].Index, -1);

            return (blockCollection[openIndex].Index, blockCollection[closeIndex].Index);
        }
    }
}
