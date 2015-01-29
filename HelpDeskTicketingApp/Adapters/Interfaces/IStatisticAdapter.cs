using HelpDeskTicketingApp.Adapters.DataAdapters;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HelpDeskTicketingApp.Adapters.Interfaces
{
    public interface IStatisticAdapter
    {
        Dictionary<string, int> GeneralIssueStatistic();

        List<GeneralTechStats> GeneralTechStatistic();
        List<GeneralIssueTypeStats> GeneralIssueTypesStatistic();

    }
}
