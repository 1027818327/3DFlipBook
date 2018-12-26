
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
using UnityEngine.EventSystems;
using UnityEngine.Playables;
using UnityEngine.UI;
using UTJ.Alembic;

namespace FlipBook
{
    public class BookCtrl : MonoBehaviour
    {
        #region Fields
        private AlembicStreamPlayer mAsp;
        private EventSystem mEventSystem;

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

        private void RestoreBook()
        {
            if (mAsp == null)
            {
                mAsp = GetComponentInChildren<AlembicStreamPlayer>();
            }

            mFrontPd.Stop();
            mNextPd.Stop();
            // 重置动画
            mAsp.currentTime = 1.033333f;

            RestoreOpenText();
        }

        private void RestoreOpenText()
        {
            mOpenText1.text = mBook.GetPageContent(mCurPage);
            mOpenText2.text = mBook.GetPageContent(mCurPage + 1);
            mOpenPageText1.text = (mCurPage + 1).ToString();
            mOpenPageText2.text = (mCurPage + 2).ToString();
        }

        private void CloseEvent()
        {
            mEventSystem = EventSystem.current;
            if (mEventSystem != null)
            {
                mEventSystem.enabled = false;
            }
        }

        private void OpenEvent()
        {
            if (mEventSystem != null)
            {
                mEventSystem.enabled = true;
            }
        }
        #endregion

        #region Protected & Public Methods
        [ContextMenu("播放打开书本动画")]
        public void PlayOpenBook()
        {
            if (mOpenPd != null)
            {
                mOpenPd.Play();

                CloseEvent();
                Invoke("OpenEvent", (float)(mOpenPd.duration - mOpenPd.initialTime));
            }

            mCurPage = 0;
            /// 显示第一页和第二页内容
            RestoreOpenText();
        }

        [ContextMenu("播放关闭书本动画")]
        public void PlayCloseBook()
        {
            if (mCurPage < 0)
            {
                Debuger.LogError("当前书本已经关闭");
                return;
            }

            if (mClosePd != null)
            {
                mClosePd.Play();
                CloseEvent();
                Invoke("OpenEvent", (float)(mClosePd.duration - mClosePd.initialTime));
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
            if (mCurPage < 0)
            {
                Debuger.LogError("请先打开书，才能翻一页");
                return;
            }

            if (mCurPage > 0)
            {
                float tempIntervalTime = (float)(mFrontPd.duration - mFrontPd.initialTime);
                Invoke("RestoreBook", tempIntervalTime);
                CloseEvent();
                Invoke("OpenEvent", tempIntervalTime);
                mFrontPd.Play();

                RestoreOpenText();

                mCurPage -= 2;
                if (mCurPage < 0)
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
            if (mCurPage < 0)
            {
                Debuger.LogError("请先打开书，才能翻一页");
                return;
            }

            if (mCurPage >= mBook.PageNum - 2)
            {
                return;
            }

            float tempIntervalTime = (float)(mNextPd.duration - mNextPd.initialTime);
            Invoke("RestoreBook", tempIntervalTime);
            CloseEvent();
            Invoke("OpenEvent", tempIntervalTime);

            mNextPd.Play();

            RestoreOpenText();

            mCurPage += 2;
            bool tempIsLast = mCurPage >= mBook.PageNum;
            if (tempIsLast)
            {
                mCurPage = mBook.PageNum;
            }

            /// 显示第一页和第二页内容
            mNextText1.text = mBook.GetPageContent(mCurPage);
            mNextText2.text = mBook.GetPageContent(mCurPage + 1);
            mNextPageText1.text = (mCurPage + 1).ToString();
            mNextPageText2.text = (mCurPage + 2).ToString();
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