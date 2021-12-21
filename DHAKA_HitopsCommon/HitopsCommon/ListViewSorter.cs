#region

using System;
using System.Collections;
using System.Windows.Forms;

#endregion

namespace HitopsCommon
{
    public class ListViewSorter : IComparer
    {
        ArrayList mSortList;        //컬럼 인덱스
        ArrayList mSortDir;         //DESC/ASC
        ArrayList mSortType;        //N이면 숫자형식, S이면 스트링 형식 비교

        public ListViewSorter(ArrayList _mSortList, ArrayList _mSortDir, ArrayList _mSortType)
        {
            mSortList = _mSortList;
            mSortDir = _mSortDir;
            mSortType = _mSortType;
        }

        public int Compare(object x, object y)
        {
            int i, mRet, CurrentColumn;
            String strDir, strType;

            ListViewItem rowA = (ListViewItem)x;
            ListViewItem rowB = (ListViewItem)y;

            for (i = 0; i < mSortList.Count; i++)
            {
                CurrentColumn = Convert.ToInt32(mSortList[i]);
                strDir = mSortDir[i].ToString();
                strType = mSortType[i].ToString();

                if (strType == "N")
                {
                    int iA, iB;

                    iA = Convert.ToInt32(rowA.SubItems[CurrentColumn].Text);
                    iB = Convert.ToInt32(rowB.SubItems[CurrentColumn].Text);

                    if(iA==iB)
                    {
                        mRet = 0;
                    }else
                    {
                        if(iA>iB) mRet = 1;
                        else mRet = -1;
                    }
                }else
                {
                    mRet = String.Compare(rowA.SubItems[CurrentColumn].Text, rowB.SubItems[CurrentColumn].Text);
                }
                
                if (mRet != 0) return mRet;
            }

            return 0;
        }
    }
}
