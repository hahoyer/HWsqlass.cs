// Generated by HWClassLibrary.sqlass.T4.Context
// Timestamp: 2011-10-31T23:19:12.7903799+01:00
#region Generated Code

using HWClassLibrary.sqlass;

namespace sqlass.Tables
{
    sealed public partial class Address 
        : ISQLSupportProvider        
        , ISQLKeyProvider<int>    
    { 
        public int Id; 
        public string Text;

        ISQLSupport ISQLSupportProvider.SQLSupport{get{return new SQLSupport.Address(this);}} 
        int ISQLKeyProvider<int>.SQLKey { get { return Id; } }    
    }
}

namespace sqlass.SQLSupport
{
    public partial class Address: ISQLSupport
    {
        readonly Tables.Address _target;
        public Address(Tables.Address target){_target = target;}

        string ISQLSupport.Insert
        {
            get
            {
                var result = "insert into Address values("; 
                result += _target.Id.SQLFormat();
                result += ", "; 
                result += _target.Text.SQLFormat();
                result += ")";   

                return result;
            }
        }
    }
}

#endregion Generated Code
