
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

namespace Book
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
        //    void Start() 
        //    {
        //    
        //    }
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
        }

        [ContextMenu("播放关闭书本动画")]
        public void PlayCloseBook()
        {
            if (mClosePd != null)
            {
                mClosePd.Play();
            }
        }


        [ContextMenu("播放上一页动画")]
        public void PlayFront()
        {
            mFrontPd.Play();
        }

        [ContextMenu("播放下一页动画")]
        public void PlayNext()
        {
            mNextPd.Play();
        }
        #endregion
    }
}