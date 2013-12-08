using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using HtmlAgilityPack;
using Dol.Base;

namespace dolspider.Spiders.GVO.Quest.Handlers
{
    public static class PageHandler
    {
        public static IList<Dol.Base.Quest> GetQuestList(HtmlDocument doc)
        {
            var questList = PageListHandler.GetQuestHTMLList(doc).Select<HtmlNode, Dol.Base.Quest>(node =>
            {
                return QuestHandler.GetQuest(node);
            }).ToList<Dol.Base.Quest>();
            return questList;
        }
    }
}
