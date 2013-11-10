using System;
using System.Collections.Generic;
using System.Linq;
using Taabus;

public static class TaabusProject
{
    public static Project Data()
    {
        return new Project
        {
            Servers = new Server[]
            {
                new Server{ConnectionString = "Data Source=ANNE\\OJB_NET;Integrated Security=True"},
            }
        };
    }
}
