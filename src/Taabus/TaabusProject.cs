using System;
using System.Collections.Generic;
using System.Linq;
using Taabus;

public static class TaabusProject
{
    public static Project Project()
    {
        return new Project
        {
            Servers = new Server[]
            {
                new Server{ConnectionString = "Data Source=ANNE\\OJB_NET;Integrated Security=True"},
            }
        };
    }

    public static ExpansionDescription[] ExpansionDescriptions()
    {
        return new ExpansionDescription[]{        new ExpansionDescription
        {
            Id = "ConnectionString=Data Source=ANNE\\OJB_NET;Integrated Security=True",
            IsExpanded = true,
            Nodes = new ExpansionDescription[]{        new ExpansionDescription
        {
            Id = "cwg_adsalesng_devtest",
            IsExpanded = true,
            Nodes = new ExpansionDescription[]{},
        }},
        }};
    }

    public static string[] Selection()
    {
        return new string[]{"ConnectionString=Data Source=ANNE\\OJB_NET;Integrated Security=True",
"cwg_adsalesng_devtest",
"as_AchPeriodJournal"};
    }
}
