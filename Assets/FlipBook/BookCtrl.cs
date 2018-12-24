
#region 版权信息
/*
 * -----------------------------------------------------------
 *  Copyright (c) KeJun All rights reserved.
 * -----------------------------------------------------------
 *		描述: 
 *      创建者：陈伟超
 *      创建时间: 2018/12/12
 *  
 */
#endregion


using UnityEngine;
using UnityEngine.Playables;
using UnityEngine.UI;

namespace FlipBook
{
    public class BookCtrl : MonoBehaviour
    {
        #region Fields
        /// <summary>
        /// 上一页
        /// </summary>
        public PlayableDirector mFrontPd;
        /// <summary>
        /// 下一页
        /// </summary>
        public PlayableDirector mNextPd;

        /// <summary>
        /// 打开书本
        /// </summary>
        public PlayableDirector mOpenPd;
        /// <summary>
        /// 关闭书本
        /// </summary>
        public PlayableDirector mClosePd;

        /// <summary>
        /// 当前第几页，[0, Max + 1]
        /// </summary>
        private int mCurPage = -1;
        public Book mBook;

        public Text mText1;
        public Text mText2;

        #endregion

        #region Properties

        #endregion

        #region Unity Messages
        //    void Awake()
        //    {
        //
        //    }
        //    void OnEnable()
        //    {
        //
        //    }
        //
        void Start()
        {
            TestBook();
        }
        //    
        //    void Update() 
        //    {
        //    
        //    }
        //
        //    void OnDisable()
        //    {
        //
        //    }
        //
        //    void OnDestroy()
        //    {
        //
        //    }

        #endregion

        #region Private Methods

        #endregion

        #region Protected & Public Methods
        [ContextMenu("播放打开书本动画")]
        public void PlayOpenBook()
        {
            if (mOpenPd != null)
            {
                mOpenPd.Play();
            }

            mCurPage = 0;
            /// 显示第一页和第二页内容
            mText1.text = mBook.GetPageContent(mCurPage);
            mText2.text = mBook.GetPageContent(mCurPage + 1);
        }

        [ContextMenu("播放关闭书本动画")]
        public void PlayCloseBook()
        {
            if (mClosePd != null)
            {
                mClosePd.Play();
            }

            mCurPage = -1;
            /// 文本清空
            mText1.text = mBook.GetPageContent(0);
            mText2.text = mBook.GetPageContent(1);
        }


        [ContextMenu("播放上一页动画")]
        public void PlayFront()
        {
            if (mCurPage > 0)
            {
                mFrontPd.Play();

                mCurPage--;
                bool tempIsFirst = mCurPage < 0;
                if (tempIsFirst)
                {
                    mCurPage = 0;
                }

                /// 显示第一页和第二页内容
                mText1.text = mBook.GetPageContent(mCurPage);
                mText2.text = mBook.GetPageContent(mCurPage + 1);
            }
        }

        [ContextMenu("播放下一页动画")]
        public void PlayNext()
        {
            if (mCurPage >= mBook.PageNum)
            {
                return;
            }
            mNextPd.Play();
            mCurPage++;
            bool tempIsLast = mCurPage >= mBook.PageNum;
            if (tempIsLast)
            {
                mCurPage = mBook.PageNum;
            }

            Debuger.Log("当前页码是"+mCurPage.ToString());

            /// 显示第一页和第二页内容
            mText1.text = mBook.GetPageContent(mCurPage);
            mText2.text = mBook.GetPageContent(mCurPage + 1);
        }

        public void TestBook()
        {
            int tempCount = 10;
            mBook = new Book(tempCount);
            for (int i = 0; i < tempCount; i++)
            {
                string tempText = string.Format("你好，当前页码是{0}", i + 1);
                mBook.SetPageContent(i, tempText);
            }
        }
        #endregion
    }
}