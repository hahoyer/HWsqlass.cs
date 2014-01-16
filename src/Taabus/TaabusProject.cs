using System;
using System.Collections.Generic;
using System.Linq;
using Taabus;
using Taabus.Data;
using Taabus.External;

public static class V13
{
    public static Configuration Data()
    {
        return new Configuration
        {
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
                                    Id = "as_AchPeriodDocOrg",
                                    IsExpanded = true,
                                    Nodes = new ExpansionDescription[] {}
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
                "as_AccountType"
            },
            Servers = new Server[] {new Server {ConnectionString = "Data Source=ANNE\\OJB_NET;Integrated Security=true"}},
            Items = new Taabus.External.Item[]
            {
                new Taabus.External.Item
                {
                    Data =
                        new Taabus.External.CardItem
                        {
                            Data = new Taabus.External.TypeItem
                            {
                                ServerId = "ANNE\\OJB_NET",
                                DataBaseId = "cwg_adsalesng_devtest",
                                TypeId = "as_AccessLevel"
                            }
                        },
                    X = 130,
                    Y = 105,
                    Width = 96,
                    Height = 24,
                    Type = "Taabus.UserInterface.CardView"
                },
                new Taabus.External.Item
                {
                    Data =
                        new Taabus.External.CardItem
                        {
                            Data= new Taabus.External.TypeItem
                            {
                                ServerId = "ANNE\\OJB_NET",
                                DataBaseId = "cwg_adsalesng_devtest",
                                TypeId = "as_AccountType"
                            }
                        },
                    X = 292,
                    Y = 222,
                    Width = 99,
                    Height = 24,
                    Type = "Taabus.UserInterface.CardView",
                    Id = 1
                },
                new Taabus.External.Item
                {
                    Data =
                        new Taabus.External.TableVieItem
                        {
                            Data=new Id {Value = 1}
                        },
                    X = 292,
                    Y = 258,
                    Width = 297,
                    Height = 240,
                    Type = "Taabus.UserInterface.TableView"
                }
            }
        };
    }
}