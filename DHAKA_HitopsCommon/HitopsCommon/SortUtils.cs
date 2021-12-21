#region

using System;
using System.Collections;

#endregion

namespace HitopsCommon
{
    public static class SortUtils
    {
        /// <summary>
        /// Quick-Sort Array by value int
        /// </summary>
        /// <param name="List"></param>
        public static void QuickSort(int[] List)
        {
            QuickSortClass.Sort(List);
        }

        /// <summary>
        /// Quick-Sort ArrayList by Hashtable value
        /// </summary>
        /// <param name="List"></param>
        /// <param name="SortKey">Int형식의 값을 지정</param>
        /// <returns></returns>
        public static ArrayList QuickSort(ArrayList List, String SortKey)
        {
            return QuickSortClass.Sort(List, SortKey);
        }

        private static class QuickSortClass
        {
            public static void Sort(int[] aList)
            {
                Sort(aList, 0, aList.Length - 1);
            }
            private static void Sort(int[] aList, int iLeft, int iRight)
            {
                //base case: list is empty
                if (iLeft >= iRight) return;
                //recursion step: make partition and sort recursively
                int iMid = Partition(aList, iLeft, iRight);
                Sort(aList, iLeft, iMid - 1);
                Sort(aList, iMid + 1, iRight);
            }
            //partition the array from Left+1 to Right with pivot a[Left]
            private static int Partition(int[] aList, int iLeft, int iRight)
            {
                int iUp = iLeft + 1; 		//pointer on left side
                int iDown = iRight;   	//pointer on right side
                int p = aList[iLeft]; 	//pivot Element
                //move pointer to center or swap if on wrong sides
                while (iUp <= iDown)
                {
                    if (aList[iUp] <= p) iUp++;
                    else if (aList[iDown] > p) iDown--;
                    else Swap(aList, iUp, iDown);
                }
                //swap pivot Element between partitions
                Swap(aList, iLeft, iDown);
                //return position of pivot Element
                return iDown;
            }
            //swap two Elements in the array
            private static void Swap(int[] aList, int i, int j)
            {
                int h = aList[i];
                aList[i] = aList[j];
                aList[j] = h;
            }

            private static ArrayList aList = new ArrayList();
            private static String sSortKey = "";
            public static ArrayList Sort(ArrayList reqList, String reqSortKey)
            {
                aList = reqList;
                sSortKey = reqSortKey;
                Sort(0, aList.Count - 1);
                return aList;
            }
            private static void Sort(int iLeft, int iRight)
            {
                //base case: list is empty
                if (iLeft >= iRight) return;
                //recursion step: make partition and sort recursively
                int iMid = Partition(iLeft, iRight);
                Sort(iLeft, iMid - 1);
                Sort(iMid + 1, iRight);
            }
            //partition the array from Left+1 to Right with pivot a[Left]
            private static int Partition(int iLeft, int iRight)
            {
                int iUp = iLeft + 1;		//pointer on left side
                int iDown = iRight;		//pointer on right side
                Hashtable pMap = (Hashtable)aList[iLeft]; //pivot Element
                //move pointer to center or swap if on wrong sides
                while (iUp <= iDown)
                {
                    if (CommFunc.ConvertToInt(((Hashtable)aList[iUp])[sSortKey].ToString()) <= CommFunc.ConvertToInt(pMap[sSortKey].ToString()))
                        iUp++;
                    else if (CommFunc.ConvertToInt(((Hashtable)aList[iDown])[sSortKey].ToString()) > CommFunc.ConvertToInt(pMap[sSortKey].ToString()))
                        iDown--;
                    else
                        Swap(iUp, iDown);
                }
                //swap pivot Element between partitions
                Swap(iLeft, iDown);
                //return position of pivot Element
                return iDown;
            }
            //swap two Elements in the ArrayList
            private static void Swap(int i, int j)
            {
                Hashtable sMap = (Hashtable)aList[i];
                aList[i] = (Hashtable)aList[j];
                aList[j] = (Hashtable)sMap;
            }
        }
    }
}
