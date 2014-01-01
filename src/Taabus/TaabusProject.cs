using System;
using System.Collections.Generic;
using System.Linq;

public static class V13
{
    public static Taabus.Configuration Data()
    {
        return new Taabus.Configuration 
            {
                ExpansionDescriptions = new Taabus.UserInterface.ExpansionDescription[] 
                    {
                        new Taabus.UserInterface.ExpansionDescription 
                        {
                            Id = "ConnectionString=Data Source=ANNE\\OJB_NET;Integrated Security=true",
                            IsExpanded = true,
                            Nodes = new Taabus.UserInterface.ExpansionDescription[] 
                                {
                                    new Taabus.UserInterface.ExpansionDescription 
                                    {
                                        Id = "cwg_adsalesng_devtest",
                                        IsExpanded = true,
                                        Nodes = new Taabus.UserInterface.ExpansionDescription[] 
                                            {
                                                new Taabus.UserInterface.ExpansionDescription 
                                                {
                                                    Id = "as_AchievementLevel",
                                                    IsExpanded = true,
                                                    Nodes = new Taabus.UserInterface.ExpansionDescription[] {}
                                                },
                                                new Taabus.UserInterface.ExpansionDescription 
                                                {
                                                    Id = "as_AchievementPeriod",
                                                    IsExpanded = true,
                                                    Nodes = new Taabus.UserInterface.ExpansionDescription[] {}
                                                },
                                                new Taabus.UserInterface.ExpansionDescription 
                                                {
                                                    Id = "as_AchievementTopic",
                                                    IsExpanded = true,
                                                    Nodes = new Taabus.UserInterface.ExpansionDescription[] {}
                                                },
                                                new Taabus.UserInterface.ExpansionDescription 
                                                {
                                                    Id = "as_AchPeriodDocOrg",
                                                    IsExpanded = true,
                                                    Nodes = new Taabus.UserInterface.ExpansionDescription[] {}
                                                },
                                                new Taabus.UserInterface.ExpansionDescription 
                                                {
                                                    Id = "as_AchPeriodDocument",
                                                    IsExpanded = true,
                                                    Nodes = new Taabus.UserInterface.ExpansionDescription[] {}
                                                },
                                                new Taabus.UserInterface.ExpansionDescription 
                                                {
                                                    Id = "as_AchPeriodJournal",
                                                    IsExpanded = true,
                                                    Nodes = new Taabus.UserInterface.ExpansionDescription[] {}
                                                }
                                            }
                                    },
                                    new Taabus.UserInterface.ExpansionDescription 
                                    {
                                        Id = "cwg_avaresco_local",
                                        IsExpanded = false,
                                        Nodes = new Taabus.UserInterface.ExpansionDescription[] 
                                            {
                                                new Taabus.UserInterface.ExpansionDescription 
                                                {
                                                    Id = "ApplicationRole",
                                                    IsExpanded = true,
                                                    Nodes = new Taabus.UserInterface.ExpansionDescription[] {}
                                                }
                                            }
                                    }
                                }
                        }
                    },
                Selection = new System.String[] 
                    {
                        "ConnectionString=Data Source=ANNE\\OJB_NET;Integrated Security=true",
                        "cwg_adsalesng_devtest"
                    },
                Servers = new Taabus.Data.Server[] {new Taabus.Data.Server {ConnectionString = "Data Source=ANNE\\OJB_NET;Integrated Security=true"}},
                WorkspaceItems = new Taabus.WorkspaceItem[] {}
            };
    }
}