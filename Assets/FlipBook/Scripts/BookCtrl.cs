
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


using Framework.Unity.Tools;
using System.Collections.Generic;
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
        private Text mTempText;

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

#if UNITY_EDITOR
        private void OnValidate()
        {
            var tempScript = GameObject.FindObjectOfType<EventSystem>();
            if (tempScript == null)
            {
                var tempObj = new GameObject("EventSystem");
                tempObj.AddComponent<EventSystem>();
                tempObj.AddComponent<StandaloneInputModule>();
            }
        }
#endif

        #endregion

        #region Private Methods

        private void RestoreBook()
        {
            OpenEvent();
            RestoreOpenText();
        }

        private void RestoreOpenText()
        {
            RefreshLeftPage();
            RefreshRightPage();
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

        private void ResetLeftPageAnim()
        {
            mFrontPd.gameObject.SetActive(false);
            /*
            var tempScript = mFrontPd.GetComponent<AlembicStreamPlayer>();
            if (tempScript != null)
            {
                tempScript.currentTime = 0f;
            }
            */
        }

        private void ResetRightPageAnim()
        {
            mNextPd.gameObject.SetActive(false);
            /*
            var tempScript = mNextPd.GetComponent<AlembicStreamPlayer>();
            if (tempScript != null)
            {
                tempScript.currentTime = 0f;
            }
            */
        }

        private void RefreshLeftPage()
        {
            mOpenText1.text = mBook.GetPageContent(mCurPage);
            mOpenPageText1.text = (mCurPage + 1).ToString();
        }

        private void RefreshRightPage()
        {
            mOpenText2.text = mBook.GetPageContent(mCurPage + 1);
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

                CloseEvent();
                Invoke("OpenEvent", (float)(mOpenPd.duration - mOpenPd.initialTime));
            }

            mCurPage = 0;
            /// 显示左页和右页内容
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

                /// 将要翻动的左页显示
                mFrontPd.gameObject.SetActive(true);
                mFrontPd.Play();
                Invoke("ResetLeftPageAnim", tempIntervalTime);

                RestoreOpenText();
                /// 要翻动的左页文字刷新
                mPreText1.text = mBook.GetPageContent(mCurPage - 1);
                mPreText2.text = mBook.GetPageContent(mCurPage);
                mPrePageText1.text = (mCurPage).ToString();
                mPrePageText2.text = (mCurPage + 1).ToString();
                Invoke("RefreshLeftPage", 0.15f);
                Invoke("RefreshRightPage", tempIntervalTime - 0.15f);

                mCurPage -= 2;
                if (mCurPage < 0)
                {
                    mCurPage = 0;
                }
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

            /// 将要翻动的右页显示
            mNextPd.gameObject.SetActive(true);
            mNextPd.Play();
            Invoke("ResetRightPageAnim", tempIntervalTime);

            RestoreOpenText();

            /// 要翻动的右页文字刷新
            mNextText1.text = mBook.GetPageContent(mCurPage + 1);
            mNextText2.text = mBook.GetPageContent(mCurPage + 2);
            mNextPageText1.text = (mCurPage + 2).ToString();
            mNextPageText2.text = (mCurPage + 3).ToString();
            Invoke("RefreshRightPage", 0.15f);
            Invoke("RefreshLeftPage", tempIntervalTime - 0.15f);

            mCurPage += 2;
            bool tempIsLast = mCurPage >= mBook.PageNum;
            if (tempIsLast)
            {
                mCurPage = mBook.PageNum;
            }
        }

        public List<string> GetGontent(Text text, string content)
        {
            List<string> tempList = new List<string>();
            while (content != null && content.Length != 0)
            {
                text.text = content;
                text.cachedTextGenerator.Populate(text.text, text.GetGenerationSettings(text.rectTransform.rect.size));
                int tempCount = text.cachedTextGenerator.vertexCount / 4 - 1;
                string tempPageContent = content.Substring(0, tempCount);
                tempList.Add(tempPageContent);

                content = content.Substring(tempCount);
            }
            text.text = null;
            return tempList;
        }

        public void TestBook()
        {
            mTempText = GameObjectUtils.FindComponent<Text>(gameObject, "TempCanvas/Text");

            string tempStr = @"<color=#FF0000FF><b><size=45>　　　　中国共产党
　　第十七次全国代表大会
　　　胡锦涛的十七大报告</size></b></color>
　　（2007年10月15日）
　　同志们：
　　现在，我代表第十六届中央委员会向大会作报告。
　　中国共产党第十七次全国代表大会，是在我国改革发展关键阶段召开的一次十分重要的大会。大会的主题是：高举中国特色社会主义伟大旗帜，以邓小平理论和“三个代表”重要思想为指导，深入贯彻落实科学发展观，继续解放思想，坚持改革开放，推动科学发展，促进社会和谐，为夺取全面建设小康社会新胜利而奋斗。
　　中国特色社会主义伟大旗帜，是当代中国发展进步的旗帜，是全党全国各族人民团结奋斗的旗帜。解放思想是发展中国特色社会主义的一大法宝，改革开放是发展中国特色社会主义的强大动力，科学发展、社会和谐是发展中国特色社会主义的基本要求，全面建设小康社会是党和国家到二〇二〇年的奋斗目标，是全国各族人民的根本利益所在。
　　当今世界正在发生广泛而深刻的变化，当代中国正在发生广泛而深刻的变革。机遇前所未有，挑战也前所未有，机遇大于挑战。全党必须坚定不移地高举中国特色社会主义伟大旗帜，带领人民从新的历史起点出发，抓住和用好重要战略机遇期，求真务实，锐意进取，继续全面建设小康社会、加快推进社会主义现代化，完成时代赋予的崇高使命。
　　一、过去五年的工作
　　十六大以来的五年是不平凡的五年。面对复杂多变的国际环境和艰巨繁重的改革发展任务，党带领全国各族人民，高举邓小平理论和“三个代表”重要思想伟大旗帜，战胜各种困难和风险，开创了中国特色社会主义事业新局面，开拓了马克思主义中国化新境界。
　　十六大确立“三个代表”重要思想的指导地位，作出全面建设小康社会的战略决策。为贯彻十六大精神，中央召开七次全会，分别就深化机构改革、完善社会主义市场经济体制、加强党的执政能力建设、制定“十一五”规划、构建社会主义和谐社会等关系全局的重大问题作出决定和部署，提出并贯彻科学发展观等重大战略思想，推动党和国家工作取得新的重大成就。
　　经济实力大幅提升。经济保持平稳快速发展，国内生产总值年均增长百分之十以上，经济效益明显提高，财政收入连年显著增加，物价基本稳定。社会主义新农村建设扎实推进，区域发展协调性增强。创新型国家建设进展良好，自主创新能力较大提高。能源、交通、通信等基础设施和重点工程建设成效显著。载人航天飞行成功实现。能源资源节约和生态环境保护取得新进展。“十五”计划胜利完成，“十一五”规划进展顺利。
　　改革开放取得重大突破。农村综合改革逐步深化，农业税、牧业税、特产税全部取消，支农惠农政策不断加强。国有资产管理体制、国有企业和金融、财税、投资、价格、科技等领域改革取得重大进展。非公有制经济进一步发展。市场体系不断健全，宏观调控继续改善，政府职能加快转变。进出口总额大幅增加，实施“走出去”战略迈出坚实步伐，开放型经济进入新阶段。
　　人民生活显著改善。城乡居民收入较大增加，家庭财产普遍增多。城乡居民最低生活保障制度初步建立，贫困人口基本生活得到保障。居民消费结构优化，衣食住行用水平不断提高，享有的公共服务明显增强。
　　民主法制建设取得新进步。政治体制改革稳步推进。人民代表大会制度、中国共产党领导的多党合作和政治协商制度、民族区域自治制度不断完善，基层民主活力增强。人权事业健康发展。爱国统一战线发展壮大。中国特色社会主义法律体系基本形成，依法治国基本方略切实贯彻。行政管理体制、司法体制改革不断深化。
　　文化建设开创新局面。社会主义核心价值体系建设扎实推进，马克思主义理论研究和建设工程成效明显。思想道德建设广泛开展，全社会文明程度进一步提高。文化体制改革取得重要进展，文化事业和文化产业快速发展，人民精神文化生活更加丰富。全民健身和竞技体育取得新成绩。
　　社会建设全面展开。各级各类教育迅速发展，农村免费义务教育全面实现。就业规模日益扩大。社会保障体系建设进一步加强。抗击非典取得重大胜利，公共卫生体系和基本医疗服务不断健全，人民健康水平不断提高。社会管理逐步完善，社会大局稳定，人民安居乐业。
　　国防和军队建设取得历史性成就。中国特色军事变革加速推进，裁减军队员额二十万任务顺利完成，军队革命化、现代化、正规化建设全面加强，履行新世纪新阶段历史使命能力显著提高。
　　港澳工作和对台工作进一步加强。香港、澳门保持繁荣稳定，与内地经贸关系更加紧密。两岸政党交流成功开启，人员往来和经济文化交流达到新水平。制定反分裂国家法，坚决维护国家主权和领土完整。
　　全方位外交取得重大进展。坚持独立自主的和平外交政策，各项外交工作积极开展，同各国的交流合作广泛加强，在国际事务中发挥重要建设性作用，为全面建设小康社会争取了良好国际环境。
　　党的建设新的伟大工程扎实推进。党的执政能力建设和先进性建设深入进行。理论创新和理论武装卓有成效。保持共产党员先进性教育活动取得重大成果。党内民主不断扩大。领导班子和干部队伍建设特别是干部教育培训取得重要进展，人才工作进一步加强，干部人事制度改革和组织制度创新不断深入。党风廉政建设和反腐败斗争成效明显。
　　在看到成绩的同时，也要清醒认识到，我们的工作与人民的期待还有不小差距，前进中还面临不少困难和问题，突出的是：经济增长的资源环境代价过大；城乡、区域、经济社会发展仍然不平衡；农业稳定发展和农民持续增收难度加大；劳动就业、社会保障、收入分配、教育卫生、居民住房、安全生产、司法和社会治安等方面关系群众切身利益的问题仍然较多，部分低收入群众生活比较困难；思想道德建设有待加强；党的执政能力同新形势新任务不完全适应，对改革发展稳定一些重大实际问题的调查研究不够深入；一些基层党组织软弱涣散；少数党员干部作风不正，形式主义、官僚主义问题比较突出，奢侈浪费、消极腐败现象仍然比较严重。我们要高度重视这些问题，继续认真加以解决。
　　总起来说，这五年，是改革开放和全面建设小康社会取得重大进展的五年，是我国综合国力大幅提升和人民得到更多实惠的五年，是我国国际地位和影响显著提高的五年，是党的创造力、凝聚力、战斗力明显增强和全党全国各族人民团结更加紧密的五年。实践充分证明，十六大和十六大以来中央作出的各项重大决策是完全正确的。
　　五年来的成就，是全党全国各族人民共同奋斗的结果。我代表中共中央，向全国各族人民，向各民主党派、各人民团体和各界爱国人士，向香港特别行政区同胞、澳门特别行政区同胞和台湾同胞以及广大侨胞，向一切关心和支持中国现代化建设的各国朋友，表示衷心的感谢！
";
　　
            List<string> tempList = GetGontent(mTempText, tempStr);
            mBook = new Book(tempList.Count);
            for (int i = 0; i < tempList.Count; i++)
            {
                mBook.SetPageContent(i, tempList[i]);
            }


            /*
            int tempCount = 10;
            mBook = new Book(tempCount);
            for (int i = 0; i < tempCount; i++)
            {
                string tempText = string.Format("你好，当前页码是{0}", i + 1);
                mBook.SetPageContent(i, tempText);
            }
            */
        }
        #endregion
    }
}