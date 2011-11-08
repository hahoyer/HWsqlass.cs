// Generated by HWClassLibrary.sqlass.T4.Context
// Timestamp: 2011-11-06T14:22:55.8648353+01:00
#region Generated Code

using System;
using System.Collections.Generic;
using System.Linq;
using HWClassLibrary.Debug;
using HWClassLibrary.sqlass;

namespace sqlass.Tables
{
    using sqlass;

    sealed public partial class Customer 
        : ISQLSupportProvider        
        , ISQLKeyProvider<System.Int32>    
    { 
        public System.Int32 Id; 
        public System.String Name; 
        public Microsoft.VisualStudio.TextTemplatingE472DFC81C0348FB2CD282B8525F7355.GeneratedTextTransformation+Address Address; 
        public Microsoft.VisualStudio.TextTemplatingE472DFC81C0348FB2CD282B8525F7355.GeneratedTextTransformation+Address DeliveryAddress;

        ISQLSupport ISQLSupportProvider.SQLSupport{get{return new SQLSupport.Customer(this);}} 
        System.Int32 ISQLKeyProvider<System.Int32>.SQLKey { get { return Id; } }    
    }
}

namespace sqlass.SQLSupport
{
    using sqlass;

    public partial class Customer: ISQLSupport
    {
        readonly Tables.Customer _target;
        public Customer(Tables.Customer target){_target = target;}

        string ISQLSupport.Insert
        {
            get
            {
                var result = "insert into ..Customer values("; 
                result += _target.Id.SQLFormat();
                result += ", "; 
                result += _target.Name.SQLFormat();
                result += ", "; 
                result += _target.Address.SQLFormat();
                result += ", "; 
                result += _target.DeliveryAddress.SQLFormat();
                result += ")";   

                return result;
            }
        }
    }
}

#endregion Generated Code
