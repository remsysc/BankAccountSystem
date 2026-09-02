namespace BankAccountSystem
{
    // SUBSYSTEM 3: keeps a history of transactions using a plain array.
    public class TransactionLog
    {
        private Transaction[] _transactions;
        private int _count;
        private int _nextId;

        public TransactionLog()
        {
            _transactions = new Transaction[8];
            _count = 0;
            _nextId = 1;
        }

        public int Count
        {
            get { return _count; }
        }

        // ADD (record) a transaction against an account.
        public int Record(int accountNumber, string kind, double amount)
        {
            EnsureCapacity();
            Transaction txn = new Transaction(_nextId, accountNumber, kind, amount);
            _transactions[_count] = txn;
            _count++;
            _nextId++;
            return txn.TransactionId;
        }

        // VIEW: print all transactions.
        public void ViewAll()
        {
            if (_count == 0)
            {
                System.Console.WriteLine("  (no transactions yet)");
                return;
            }

            for (int i = 0; i < _count; i++)
            {
                _transactions[i].Display();
            }
        }

        // SEARCH: print all transactions for one account.
        public void SearchByAccount(int accountNumber)
        {
            int matches = 0;
            for (int i = 0; i < _count; i++)
            {
                if (_transactions[i].AccountNumber == accountNumber)
                {
                    _transactions[i].Display();
                    matches++;
                }
            }

            if (matches == 0)
            {
                System.Console.WriteLine("  (no transactions for account #" + accountNumber + ")");
            }
        }

        // REMOVE: drop every transaction tied to an account (used when the
        // account itself is removed) and return how many were dropped.
        public int RemoveByAccount(int accountNumber)
        {
            int removed = 0;
            int i = 0;
            while (i < _count)
            {
                if (_transactions[i].AccountNumber == accountNumber)
                {
                    for (int j = i; j < _count - 1; j++)
                    {
                        _transactions[j] = _transactions[j + 1];
                    }
                    _transactions[_count - 1] = null;
                    _count--;
                    removed++;
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
            if (_count < _transactions.Length)
            {
                return;
            }

            Transaction[] bigger = new Transaction[_transactions.Length * 2];
            for (int i = 0; i < _count; i++)
            {
                bigger[i] = _transactions[i];
            }
            _transactions = bigger;
        }
    }
}
