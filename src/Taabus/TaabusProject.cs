using System;
using System.Collections.Generic;
using System.Linq;
using Taabus;
using Taabus.Data;
using Taabus.UserInterface;

public static class TaabusProject
{
    public static Taabus.TaabusProject Project()
    {
        return new Taabus.TaabusProject 
            {
                Project = new Project {Servers = new Server[] {new Server {ConnectionString = "Data Source=ANNE\\OJB_NET;Integrated Security=true"}}},
                ExpansionDescriptions = new ExpansionDescription[] 
                    {
                        new ExpansionDescription 
                        {
                            Id = "ConnectionString=Data Source=ANNE\\OJB_NET;Integrated Security=true",
                            IsExpanded = true,
                            Nodes = new ExpansionDescription[] 
                                {
                                    new ExpansionDescription 
                                    {
                                        Id = "cwg_adsalesng_devtest",
                                        IsExpanded = true,
                                        Nodes = new ExpansionDescription[] 
                                            {
                                                new ExpansionDescription 
                                                {
                                                    Id = "as_AccessLevel",
                                                    IsExpanded = true,
                                                    Nodes = new ExpansionDescription[] {}
                                                },
                                                new ExpansionDescription 
                                                {
                                                    Id = "as_AchievementCalendar",
                                                    IsExpanded = true,
                                                    Nodes = new ExpansionDescription[] {}
                                                },
                                                new ExpansionDescription 
                                                {
                                                    Id = "as_AchievementLevel",
                                                    IsExpanded = true,
                                                    Nodes = new ExpansionDescription[] {}
                                                },
                                                new ExpansionDescription 
                                                {
                                                    Id = "as_AchievementPeriod",
                                                    IsExpanded = true,
                                                    Nodes = new ExpansionDescription[] {}
                                                },
                                                new ExpansionDescription 
                                                {
                                                    Id = "as_AchievementTopic",
                                                    IsExpanded = true,
                                                    Nodes = new ExpansionDescription[] {}
                                                },
                                                new ExpansionDescription 
                                                {
                                                    Id = "as_AchPeriodDocOrg",
                                                    IsExpanded = true,
                                                    Nodes = new ExpansionDescription[] {}
                                                },
                                                new ExpansionDescription 
                                                {
                                                    Id = "as_AchPeriodDocument",
                                                    IsExpanded = true,
                                                    Nodes = new ExpansionDescription[] {}
                                                },
                                                new ExpansionDescription 
                                                {
                                                    Id = "as_AchPeriodJournal",
                                                    IsExpanded = true,
                                                    Nodes = new ExpansionDescription[] 
                                                        {
                                                            new ExpansionDescription 
                                                            {
                                                                Id = "as_AchPeriodJournal3_fk",
                                                                IsExpanded = true,
                                                                Nodes = new ExpansionDescription[] {}
                                                            }
                                                        }
                                                }
                                            }
                                    }
                                }
                        }
                    },
                Selection = new String[] 
                    {
                        "ConnectionString=Data Source=ANNE\\OJB_NET;Integrated Security=true",
                        "cwg_adsalesng_devtest",
                        "as_AchPeriodDocument"
                    }
            };
    }
}