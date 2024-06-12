namespace TradingBell.WebCat.EasyAsk.Impl
{
    using TradingBell.WebCat.EasyAsk;
    using System;
    using System.Collections.Generic;

    internal class GroupedPageLayout
    {
        private int m_endRow = -1;
        private IGroupedResult m_groupEnd = null;
        private IGroupedResult m_groupStart = null;
        private int m_startRow = -1;

        private GroupedPageLayout()
        {
        }

        public IGroupedResult getEndGroup()
        {
            return this.m_groupEnd;
        }

        public int getEndRow()
        {
            return this.m_endRow;
        }

        public IGroupedResult getStartGroup()
        {
            return this.m_groupStart;
        }

        public int getStartRow()
        {
            return this.m_startRow;
        }

        public static IList<GroupedPageLayout> layoutPages(int pageSize, IList<IGroupedResult> groups, bool breakGroups)
        {
            IGroupedResult result;
            GroupedPageLayout layout;
            List<GroupedPageLayout> list = new List<GroupedPageLayout>();
            if (0 < pageSize)
            {
                for (int i = 0; i < groups.Count; i++)
                {
                    result = groups[i];
                    IGroupedResult result2 = result;
                    int num2 = 1;
                    int num3 = 0;
                    int num4 = result.getNumberOfRows();
                    while (null != result)
                    {
                        while ((num3 + num4) < pageSize)
                        {
                            num3 += num4;
                            if ((i + 1) < groups.Count)
                            {
                                result = groups[i++];
                                num4 = result.getNumberOfRows();
                            }
                            else
                            {
                                layout = new GroupedPageLayout {
                                    m_groupStart = result2,
                                    m_startRow = num2,
                                    m_groupEnd = result,
                                    m_endRow = result.getNumberOfRows()
                                };
                                list.Add(layout);
                                result = null;
                                break;
                            }
                        }
                        if (null != result)
                        {
                            layout = new GroupedPageLayout {
                                m_groupStart = result2,
                                m_startRow = num2,
                                m_groupEnd = result
                            };
                            if (breakGroups)
                            {
                                if (result == result2)
                                {
                                    layout.m_endRow = ((pageSize - num3) + num2) - 1;
                                }
                                else
                                {
                                    layout.m_endRow = pageSize - num3;
                                }
                            }
                            else
                            {
                                layout.m_endRow = result.getNumberOfRows();
                            }
                            list.Add(layout);
                            num3 = 0;
                            if (layout.m_startRow == result.getNumberOfRows())
                            {
                                if ((i + 1) < groups.Count)
                                {
                                    result = groups[i++];
                                    num4 = result.getNumberOfRows();
                                    num2 = 1;
                                    result2 = result;
                                }
                                else
                                {
                                    result = null;
                                }
                            }
                            else
                            {
                                result2 = result;
                                num2 = layout.m_endRow + 1;
                                num4 = result.getNumberOfRows() - (num2 - 1);
                            }
                        }
                    }
                }
                return list;
            }
            if (0 < groups.Count)
            {
                layout = new GroupedPageLayout();
                result = groups[0];
                layout.m_groupStart = result;
                layout.m_startRow = 1;
                result = groups[groups.Count - 1];
                layout.m_groupEnd = result;
                layout.m_endRow = result.getNumberOfRows();
                list.Add(layout);
            }
            return list;
        }
    }
}

