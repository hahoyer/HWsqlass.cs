using sqlass.Tables;

namespace sqlass
{
    sealed public partial class Context
    {
        public Table<Customer> Customer;
        public Table<Address> Address;

        public Context()
        {
            Customer = new Table<Customer>(this,"Customer");
            Address = new Table<Address>(this,"Address");
        }
    }
}

