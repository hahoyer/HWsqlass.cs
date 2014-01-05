using System;
using System.Collections.Generic;
using System.Linq;

public static class V13
{
    public static Taabus.Configuration Data()
    {
        return new Taabus.Configuration 
            {
                ExpansionDescriptions = new Taabus.External.ExpansionDescription[] 
                    {
                        new Taabus.External.ExpansionDescription 
                        {
                            Id = "ConnectionString=Data Source=ANNE\\OJB_NET;Integrated Security=true",
                            IsExpanded = true,
                            Nodes = new Taabus.External.ExpansionDescription[] 
                                {
                                    new Taabus.External.ExpansionDescription 
                                    {
                                        Id = "cwg_adsalesng_devtest",
                                        IsExpanded = true,
                                        Nodes = new Taabus.External.ExpansionDescription[] 
                                            {
                                                new Taabus.External.ExpansionDescription 
                                                {
                                                    Id = "as_AccessLevel",
                                                    IsExpanded = true,
                                                    Nodes = new Taabus.External.ExpansionDescription[] {}
                                                },
                                                new Taabus.External.ExpansionDescription 
                                                {
                                                    Id = "as_AchPeriodDocOrg",
                                                    IsExpanded = true,
                                                    Nodes = new Taabus.External.ExpansionDescription[] {}
                                                }
                                            }
                                    }
                                }
                        }
                    },
                Selection = new System.String[] 
                    {
                        "ConnectionString=Data Source=ANNE\\OJB_NET;Integrated Security=true",
                        "cwg_adsalesng_devtest",
                        "as_AccessLevel"
                    },
                Servers = new Taabus.Data.Server[] {new Taabus.Data.Server {ConnectionString = "Data Source=ANNE\\OJB_NET;Integrated Security=true"}},
                Items = new Taabus.External.Item[] 
                    {
                        new Taabus.External.Item 
                        {
                            Type = "Taabus.UserInterface.CardView",
                                    X = 130,
                                    Y = 105,
                                    Width = 96,
                                    Height = 24
                                ,
                            Data = new Taabus.External.DataItem 
                                {
                                    ServerId = "ANNE\\OJB_NET",
                                    DataBaseId = "cwg_adsalesng_devtest",
                                    TypeId = "as_AccessLevel"
                                }
                        }
                    }
            };
    }
}