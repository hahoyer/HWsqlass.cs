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
                        "as_AchievementLevel"
                    },
                Servers = new Taabus.Data.Server[] {new Taabus.Data.Server {ConnectionString = "Data Source=ANNE\\OJB_NET;Integrated Security=true"}},
                Items = new Taabus.External.Item[] 
                    {
                        new Taabus.External.Item 
                        {
                            Data = new Taabus.External.TableView 
                                {
                                    Data = new Taabus.External.Id {Value = 0},
                                    ColumnConfig = new Taabus.External.ColumnConfig[] 
                                        {
                                            new Taabus.External.ColumnConfig 
                                            {
                                                Name = "#",
                                                Width = 28
                                            },
                                            new Taabus.External.ColumnConfig 
                                            {
                                                Name = "AccessLevelId",
                                                Width = 33
                                            },
                                            new Taabus.External.ColumnConfig 
                                            {
                                                Name = "MessageId",
                                                Width = 44
                                            },
                                            new Taabus.External.ColumnConfig 
                                            {
                                                Name = "Inserted",
                                                Width = 80
                                            },
                                            new Taabus.External.ColumnConfig 
                                            {
                                                Name = "Updated",
                                                Width = 14
                                            },
                                            new Taabus.External.ColumnConfig 
                                            {
                                                Name = "UserId",
                                                Width = 39
                                            },
                                            new Taabus.External.ColumnConfig 
                                            {
                                                Name = "Timestamp",
                                                Width = 100
                                            }
                                        }
                                },
                            X = 577,
                            Y = 197,
                            Width = 288,
                            Height = 240
                        },
                        new Taabus.External.Item 
                        {
                            Data = new Taabus.External.TableView 
                                {
                                    Data = new Taabus.External.Id {Value = 0},
                                    ColumnConfig = new Taabus.External.ColumnConfig[] 
                                        {
                                            new Taabus.External.ColumnConfig 
                                            {
                                                Name = "#",
                                                Width = 31
                                            },
                                            new Taabus.External.ColumnConfig 
                                            {
                                                Name = "AccessLevelId",
                                                Width = 100
                                            },
                                            new Taabus.External.ColumnConfig 
                                            {
                                                Name = "MessageId",
                                                Width = 100
                                            },
                                            new Taabus.External.ColumnConfig 
                                            {
                                                Name = "Inserted",
                                                Width = 100
                                            },
                                            new Taabus.External.ColumnConfig 
                                            {
                                                Name = "Updated",
                                                Width = 100
                                            },
                                            new Taabus.External.ColumnConfig 
                                            {
                                                Name = "UserId",
                                                Width = 100
                                            },
                                            new Taabus.External.ColumnConfig 
                                            {
                                                Name = "Timestamp",
                                                Width = 100
                                            }
                                        }
                                },
                            X = 386,
                            Y = -1,
                            Width = 288,
                            Height = 240
                        },
                        new Taabus.External.Item 
                        {
                            Id = 0,
                            Data = new Taabus.External.TypeItemView 
                                {
                                    Data = new Taabus.External.TypeItem 
                                        {
                                            ServerId = "ANNE\\OJB_NET",
                                            DataBaseId = "cwg_adsalesng_devtest",
                                            TypeId = "as_AccessLevel"
                                        }
                                },
                            X = 205,
                            Y = 119,
                            Width = 96,
                            Height = 24
                        },
                        new Taabus.External.Item 
                        {
                            Id = 1,
                            Data = new Taabus.External.TypeItemView 
                                {
                                    Data = new Taabus.External.TypeItem 
                                        {
                                            ServerId = "ANNE\\OJB_NET",
                                            DataBaseId = "cwg_adsalesng_devtest",
                                            TypeId = "as_AccountType"
                                        }
                                },
                            X = 35,
                            Y = 354,
                            Width = 99,
                            Height = 24
                        },
                        new Taabus.External.Item 
                        {
                            Data = new Taabus.External.TableView 
                                {
                                    Data = new Taabus.External.Id {Value = 1},
                                    ColumnConfig = new Taabus.External.ColumnConfig[] 
                                        {
                                            new Taabus.External.ColumnConfig 
                                            {
                                                Name = "#",
                                                Width = 21
                                            },
                                            new Taabus.External.ColumnConfig 
                                            {
                                                Name = "AccountTypeId",
                                                Width = 21
                                            },
                                            new Taabus.External.ColumnConfig 
                                            {
                                                Name = "MessageId",
                                                Width = 26
                                            },
                                            new Taabus.External.ColumnConfig 
                                            {
                                                Name = "Inserted",
                                                Width = 107
                                            },
                                            new Taabus.External.ColumnConfig 
                                            {
                                                Name = "Updated",
                                                Width = 21
                                            },
                                            new Taabus.External.ColumnConfig 
                                            {
                                                Name = "UserId",
                                                Width = 23
                                            },
                                            new Taabus.External.ColumnConfig 
                                            {
                                                Name = "timestamp",
                                                Width = 72
                                            }
                                        }
                                },
                            X = 66,
                            Y = 568,
                            Width = 297,
                            Height = 240
                        }
                    }
            };
    }
}