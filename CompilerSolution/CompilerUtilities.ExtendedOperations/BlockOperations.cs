using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace CompilerUtilities.ExtendedOperations
{
    internal static class BlockOperations
    {
        public static List<(int Open, int Close)> FindAllBlocks<T>(this IEnumerable<T> collection, T openElem, T closeElem)
        {
            var blockIndexPairs = new List<(int Open, int Close)>();
            var sourceSequence = collection as T[] ?? collection.ToArray();
            var blockSequence = DeleteNonBlockElements(sourceSequence, openElem, closeElem);

            while (blockSequence.Count > 0)
            {
                var blockPair = FindFirstBlock(blockSequence, openElem, closeElem);

                if (!IsCorrectPair(sourceSequence, blockPair))
                    throw new InvalidDataException($"Didn't expected complete block. OpenIndex: {blockPair.Open}, CloseIndex: {blockPair.Close}");

                blockIndexPairs.Add(blockPair);
                DeleteBlock(blockSequence, blockPair);
            }

            return blockIndexPairs;
        }

        private static List<(T Value, int Index)> DeleteNonBlockElements<T>(IEnumerable<T> collection, T openElem,
            T closeElem)
        {
            var numeratedSequence =
                collection.Select((elem, index) => ((T Value, int Index)) (elem, index));
            var withoutNonBlockSequence =
                numeratedSequence.Where(elem => elem.Value.Equals(openElem) || elem.Value.Equals(closeElem));
            return  withoutNonBlockSequence.ToList();
        }

        private static (int Open, int Close) FindFirstBlock<T>(List<(T Value, int Index)> blockSequence, T openElem, T closeElem)
        {
            if (!blockSequence.All(elem => elem.Value.Equals(openElem) || elem.Value.Equals(closeElem)))
                throw new InvalidDataException("Detected not valid block elements");

            var nestingAcc = 0;
            var openIndex = blockSequence.FindIndex(elem => elem.Value.Equals(openElem));

            var firstClose = blockSequence.FindIndex(elem => elem.Value.Equals(closeElem));
            if (firstClose < openIndex)
                throw new InvalidDataException($"Close elem detected before open (first openIndex {blockSequence[openIndex].Index}; first closeIndex {blockSequence[firstClose].Index})");

            var closeIndex = -1;

            if (openIndex != -1)
            {
                for (int i = openIndex + 1; i < blockSequence.Count; i++)
                {
                    bool isClose = blockSequence[i].Value.Equals(closeElem);

                    if (isClose && nestingAcc == 0)
                    {
                        closeIndex = i;
                        break;
                    }

                    if (isClose)
                        nestingAcc--;
                    else
                        nestingAcc++;
                }
            }

            if (openIndex == -1)
                return (-1, -1);

            if (closeIndex == -1)
                return (blockSequence[openIndex].Index, -1);

            return (blockSequence[openIndex].Index, blockSequence[closeIndex].Index);
        }

        private static bool IsCorrectPair<T>(IEnumerable<T> sequence, (int Open, int Close) blockPair)
        {
            var collectionSize = sequence.Count();
            return blockPair.Open < collectionSize && blockPair.Open >= 0
                && blockPair.Close < collectionSize && blockPair.Close >= 0;
        }

        private static void DeleteBlock<T>(List<(T Value, int Index)> blockSequence, (int Open, int Close) blockPair)
        {
            blockSequence.RemoveAll(elem => elem.Index == blockPair.Open);
            blockSequence.RemoveAll(elem => elem.Index == blockPair.Close);
        }
    }
}
