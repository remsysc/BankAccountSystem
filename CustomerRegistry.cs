namespace BankAccountSystem
{
    // SUBSYSTEM 1: manages customers
    public class CustomerRegistry
    {
        private Customer[] _customers;
        private int _count;
        private int _nextId;

        public CustomerRegistry()
        {
            _customers = new Customer[4];
            _count = 0;
            _nextId = 1;
        }

        public int Count
        {
            get { return _count; }
        }

        // ADD: create and store a new customer, returning its generated id.
        public int Add(string name, string phone)
        {
            EnsureCapacity();
            Customer customer = new Customer(_nextId, name, phone);
            _customers[_count] = customer;
            _count++;
            _nextId++;
            return customer.CustomerId;
        }

        // VIEW: print every customer.
        public void ViewAll()
        {
            if (_count == 0)
            {
                System.Console.WriteLine("  (no customers yet)");
                return;
            }

            for (int i = 0; i < _count; i++)
            {
                _customers[i].Display();
            }
        }

        // SEARCH by id. Returns the customer or null if not found.
        public Customer FindById(int customerId)
        {
            for (int i = 0; i < _count; i++)
            {
                if (_customers[i].CustomerId == customerId)
                {
                    return _customers[i];
                }
            }
            return null;
        }

        // SEARCH by (partial) name. Prints all matches.
        public void SearchByName(string term)
        {
            if (term == null)
            {
                term = "";
            }
            term = term.Trim().ToLower();

            int matches = 0;
            for (int i = 0; i < _count; i++)
            {
                string name = _customers[i].Name.ToLower();
                if (name.Contains(term))
                {
                    _customers[i].Display();
                    matches++;
                }
            }

            if (matches == 0)
            {
                System.Console.WriteLine("  (no customers matched \"" + term + "\")");
            }
        }

        // REMOVE by id. Returns true if a customer was removed.
        public bool Remove(int customerId)
        {
            int index = -1;
            for (int i = 0; i < _count; i++)
            {
                if (_customers[i].CustomerId == customerId)
                {
                    index = i;
                    break;
                }
            }

            if (index == -1)
            {
                return false;
            }

            // Compact the array: shift later elements left by one.
            for (int i = index; i < _count - 1; i++)
            {
                _customers[i] = _customers[i + 1];
            }
            _customers[_count - 1] = null;
            _count--;
            return true;
        }

        // Grow the backing array by doubling when it is full.
        private void EnsureCapacity()
        {
            if (_count < _customers.Length)
            {
                return;
            }

            Customer[] bigger = new Customer[_customers.Length * 2];
            for (int i = 0; i < _count; i++)
            {
                bigger[i] = _customers[i];
            }
            _customers = bigger;
        }
    }
}
