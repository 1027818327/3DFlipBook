
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
using UTJ.Alembic;

namespace FlipBook
{
    public class BookCtrl : MonoBehaviour
    {
        #region Fields
        private AlembicStreamPlayer mAsp;

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

        public Text mOpenText1;
        public Text mOpenText2;
        public Text mOpenPageText1;
        public Text mOpenPageText2;

        public Text mNextText1;
        public Text mNextText2;
        public Text mNextPageText1;
        public Text mNextPageText2;

        public Text mPreText1;
        public Text mPreText2;
        public Text mPrePageText1;
        public Text mPrePageText2;

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
        private void RestoreOpenText()
        {
            mOpenText1.text = mBook.GetPageContent(mCurPage);
            mOpenText2.text = mBook.GetPageContent(mCurPage + 1);
            mOpenPageText1.text = (mCurPage + 1).ToString();
            mOpenPageText2.text = (mCurPage + 2).ToString();
        }
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
            RestoreOpenText();
        }

        [ContextMenu("播放关闭书本动画")]
        public void PlayCloseBook()
        {
            if (mClosePd != null)
            {
                mClosePd.Play();
            }

            if (mCurPage >= 0)
            {
                RestoreOpenText();
            }

            mCurPage = -1;
            /// 文本清空
            mNextText1.text = null;
            mNextText2.text = null;
            mPreText1.text = null;
            mPreText2.text = null;
        }


        [ContextMenu("播放上一页动画")]
        public void PlayFront()
        {
            if (mCurPage > 0)
            {
                Invoke("RestoreBook", (float)(mFrontPd.duration - mFrontPd.initialTime));
                mFrontPd.Play();

                RestoreOpenText();

                mCurPage -= 2;
                bool tempIsFirst = mCurPage < 0;
                if (tempIsFirst)
                {
                    mCurPage = 0;
                }

                /// 显示第一页和第二页内容
                mPreText1.text = mBook.GetPageContent(mCurPage);
                mPreText2.text = mBook.GetPageContent(mCurPage + 1);
                mPrePageText1.text = (mCurPage + 1).ToString();
                mPrePageText2.text = (mCurPage + 2).ToString();
            }
        }

        [ContextMenu("播放下一页动画")]
        public void PlayNext()
        {
            if (mCurPage >= mBook.PageNum - 2)
            {
                return;
            }

            Invoke("RestoreBook", (float)(mNextPd.duration - mNextPd.initialTime));
            mNextPd.Play();

            RestoreOpenText();

            mCurPage += 2;
            bool tempIsLast = mCurPage >= mBook.PageNum;
            if (tempIsLast)
            {
                mCurPage = mBook.PageNum;
            }

            //Debuger.Log("当前页码是"+mCurPage.ToString());

            /// 显示第一页和第二页内容
            mNextText1.text = mBook.GetPageContent(mCurPage);
            mNextText2.text = mBook.GetPageContent(mCurPage + 1);
            mNextPageText1.text = (mCurPage + 1).ToString();
            mNextPageText2.text = (mCurPage + 2).ToString();
        }

        public void RestoreBook()
        {
            if (mAsp == null)
            {
                mAsp = GetComponentInChildren<AlembicStreamPlayer>();
            }

            mFrontPd.Stop();
            mNextPd.Stop();
            mAsp.currentTime = 3.333333f;

            RestoreOpenText();
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