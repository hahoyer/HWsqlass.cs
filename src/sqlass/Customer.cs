namespace sqlass.Tables
{
    sealed public partial class Customer
    {
        public int Id;
        public string Name;
        public Reference<Address> Address;
        public Reference<Address> DeliveryAddress;
    }
}


