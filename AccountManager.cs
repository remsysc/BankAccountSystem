namespace BankAccountSystem
{
    // SUBSYSTEM 2: manages accounts. The array is typed as the base class
    // Account[], so it holds SavingsAccount and CheckingAccount together and
    // calls their overridden methods polymorphically.
    public class AccountManager
    {
        private Account[] _accounts;
        private int _count;
        private int _nextNumber;

        public AccountManager()
        {
            _accounts = new Account[4];
            _count = 0;
            _nextNumber = 1001; // human-friendly starting account number
        }

        public int Count
        {
            get { return _count; }
        }

        // ADD: creates a savings or checking account for a customer.
        // kind: "S" for savings, "C" for checking.
        public int Add(string kind, int ownerCustomerId, double openingBalance)
        {
            EnsureCapacity();

            Account account;
            if (kind == "S" || kind == "s")
            {
                account = new SavingsAccount(_nextNumber, ownerCustomerId, openingBalance);
            }
            else
            {
                account = new CheckingAccount(_nextNumber, ownerCustomerId, openingBalance);
            }

            _accounts[_count] = account;
            _count++;
            _nextNumber++;
            return account.AccountNumber;
        }

        // VIEW: polymorphic Display() picks the right subclass output.
        public void ViewAll()
        {
            if (_count == 0)
            {
                System.Console.WriteLine("  (no accounts yet)");
                return;
            }

            for (int i = 0; i < _count; i++)
            {
                _accounts[i].Display();
            }
        }

        // SEARCH by account number.
        public Account FindByNumber(int accountNumber)
        {
            for (int i = 0; i < _count; i++)
            {
                if (_accounts[i].AccountNumber == accountNumber)
                {
                    return _accounts[i];
                }
            }
            return null;
        }

        // VIEW accounts owned by one customer.
        public void ViewByCustomer(int customerId)
        {
            int matches = 0;
            for (int i = 0; i < _count; i++)
            {
                if (_accounts[i].OwnerCustomerId == customerId)
                {
                    _accounts[i].Display();
                    matches++;
                }
            }

            if (matches == 0)
            {
                System.Console.WriteLine("  (customer #" + customerId + " has no accounts)");
            }
        }

        // POLYMORPHISM in action: apply each account's own interest rule.
        public void ApplyInterestToAll()
        {
            if (_count == 0)
            {
                System.Console.WriteLine("  (no accounts to apply interest to)");
                return;
            }

            for (int i = 0; i < _count; i++)
            {
                double interest = _accounts[i].CalculateInterest();
                if (interest > 0)
                {
                    _accounts[i].Deposit(interest);
                    System.Console.WriteLine(
                        "  Acct #" + _accounts[i].AccountNumber +
                        " (" + _accounts[i].AccountType + ") earned interest: " +
                        interest.ToString("0.00"));
                }
                else
                {
                    System.Console.WriteLine(
                        "  Acct #" + _accounts[i].AccountNumber +
                        " (" + _accounts[i].AccountType + ") earned no interest.");
                }
            }
        }

        // Returns the account numbers owned by a customer, as a right-sized
        // array. Used by the main system to cascade transaction cleanup.
        public int[] GetAccountNumbersByCustomer(int customerId)
        {
            int matches = 0;
            for (int i = 0; i < _count; i++)
            {
                if (_accounts[i].OwnerCustomerId == customerId)
                {
                    matches++;
                }
            }

            int[] result = new int[matches];
            int index = 0;
            for (int i = 0; i < _count; i++)
            {
                if (_accounts[i].OwnerCustomerId == customerId)
                {
                    result[index] = _accounts[i].AccountNumber;
                    index++;
                }
            }
            return result;
        }

        // REMOVE a single account by number.
        public bool Remove(int accountNumber)
        {
            int index = -1;
            for (int i = 0; i < _count; i++)
            {
                if (_accounts[i].AccountNumber == accountNumber)
                {
                    index = i;
                    break;
                }
            }

            if (index == -1)
            {
                return false;
            }

            for (int i = index; i < _count - 1; i++)
            {
                _accounts[i] = _accounts[i + 1];
            }
            _accounts[_count - 1] = null;
            _count--;
            return true;
        }

        // REMOVE every account owned by a customer (cascade on customer delete).
        // Returns how many accounts were removed.
        public int RemoveByCustomer(int customerId)
        {
            int removed = 0;
            int i = 0;
            while (i < _count)
            {
                if (_accounts[i].OwnerCustomerId == customerId)
                {
                    for (int j = i; j < _count - 1; j++)
                    {
                        _accounts[j] = _accounts[j + 1];
                    }
                    _accounts[_count - 1] = null;
                    _count--;
                    removed++;
                    // do not advance i; a new element now sits at position i
                }
                else
                {
                    i++;
                }
            }
            return removed;
        }

        private void EnsureCapacity()
        {
            if (_count < _accounts.Length)
            {
                return;
            }

            Account[] bigger = new Account[_accounts.Length * 2];
            for (int i = 0; i < _count; i++)
            {
                bigger[i] = _accounts[i];
            }
            _accounts = bigger;
        }
    }
}
