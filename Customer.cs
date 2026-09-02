namespace BankAccountSystem
{
    // A customer of the bank. ENCAPSULATION: fields are private and exposed
    // through validated properties.
    public class Customer
    {
        private string _name;
        private string _phone;

        public int CustomerId { get; private set; }

        public string Name
        {
            get { return _name; }
            set
            {
                // Reject empty names; keep the previous value if invalid.
                if (value != null && value.Trim().Length > 0)
                {
                    _name = value.Trim();
                }
            }
        }

        public string Phone
        {
            get { return _phone; }
            set
            {
                if (value != null)
                {
                    _phone = value.Trim();
                }
            }
        }

        public Customer(int customerId, string name, string phone)
        {
            CustomerId = customerId;
            _name = "Unknown";
            _phone = "";
            Name = name;
            Phone = phone;
        }

        public void Display()
        {
            System.Console.WriteLine(
                "  Customer #" + CustomerId +
                " | Name: " + Name +
                " | Phone: " + Phone);
        }
    }
}
