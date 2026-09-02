namespace BankAccountSystem
{
    // MAIN SYSTEM: owns the three subsystems and coordinates the workflows
    public class BankManager
    {
        private CustomerRegistry _customers;
        private AccountManager _accounts;
        private TransactionLog _transactions;

        public BankManager()
        {
            _customers = new CustomerRegistry();
            _accounts = new AccountManager();
            _transactions = new TransactionLog();
        }

        public void Run()
        {
            bool running = true;
            while (running)
            {
                ShowMainMenu();
                int choice = InputHelper.ReadInt("Choose an option: ");
                System.Console.WriteLine();

                if (choice == 1)
                {
                    CustomerMenu();
                }
                else if (choice == 2)
                {
                    AccountMenu();
                }
                else if (choice == 3)
                {
                    TransactionMenu();
                }
                else if (choice == 4)
                {
                    _accounts.ApplyInterestToAll();
                }
                else if (choice == 0)
                {
                    running = false;
                    System.Console.WriteLine("Goodbye!");
                }
                else
                {
                    System.Console.WriteLine("Unknown option.");
                }

                System.Console.WriteLine();
            }
        }

        private void ShowMainMenu()
        {
            System.Console.WriteLine("==============================================");
            System.Console.WriteLine("        BANK ACCOUNT MANAGEMENT SYSTEM        ");
            System.Console.WriteLine("==============================================");
            System.Console.WriteLine(" 1. Customers  (" + _customers.Count + ")");
            System.Console.WriteLine(" 2. Accounts   (" + _accounts.Count + ")");
            System.Console.WriteLine(" 3. Transactions (" + _transactions.Count + ")");
            System.Console.WriteLine(" 4. Apply interest to all accounts");
            System.Console.WriteLine(" 0. Exit");
            System.Console.WriteLine("----------------------------------------------");
        }

        // ---------------- Customer subsystem menu ----------------
        private void CustomerMenu()
        {
            System.Console.WriteLine("--- CUSTOMERS: 1)Add 2)View 3)Search 4)Remove ---");
            int choice = InputHelper.ReadInt("Choose: ");
            System.Console.WriteLine();

            if (choice == 1)
            {
                string name = InputHelper.ReadText("Name: ");
                string phone = InputHelper.ReadText("Phone: ");
                int id = _customers.Add(name, phone);
                System.Console.WriteLine("Added customer #" + id + ".");
            }
            else if (choice == 2)
            {
                _customers.ViewAll();
            }
            else if (choice == 3)
            {
                string term = InputHelper.ReadText("Search name contains: ");
                _customers.SearchByName(term);
            }
            else if (choice == 4)
            {
                int id = InputHelper.ReadInt("Customer id to remove: ");
                // INTERCONNECTION: removing a customer cascades to their
                // accounts, and each removed account's transactions are purged.
                Customer target = _customers.FindById(id);
                if (target == null)
                {
                    System.Console.WriteLine("No customer #" + id + ".");
                    return;
                }

                int purgedTxns = PurgeTransactionsForCustomerAccounts(id);
                int removedAccounts = _accounts.RemoveByCustomer(id);
                _customers.Remove(id);

                System.Console.WriteLine(
                    "Removed customer #" + id + ", " + removedAccounts +
                    " account(s), and " + purgedTxns + " transaction(s).");
            }
            else
            {
                System.Console.WriteLine("Unknown option.");
            }
        }

        // Removes transactions for every account the customer owns.
        private int PurgeTransactionsForCustomerAccounts(int customerId)
        {
            int purged = 0;
            int[] owned = _accounts.GetAccountNumbersByCustomer(customerId);
            for (int i = 0; i < owned.Length; i++)
            {
                purged = purged + _transactions.RemoveByAccount(owned[i]);
            }
            return purged;
        }

        // ---------------- Account subsystem menu ----------------
        private void AccountMenu()
        {
            System.Console.WriteLine("--- ACCOUNTS: 1)Add 2)View 3)Search 4)Remove ---");
            int choice = InputHelper.ReadInt("Choose: ");
            System.Console.WriteLine();

            if (choice == 1)
            {
                int ownerId = InputHelper.ReadInt("Owner customer id: ");
                if (_customers.FindById(ownerId) == null)
                {
                    System.Console.WriteLine("No customer #" + ownerId + ". Create the customer first.");
                    return;
                }

                string kind = InputHelper.ReadText("Type - (S)avings or (C)hecking: ");
                double opening = InputHelper.ReadAmount("Opening balance: ");
                int number = _accounts.Add(kind, ownerId, opening);
                System.Console.WriteLine("Opened account #" + number + " for customer #" + ownerId + ".");
            }
            else if (choice == 2)
            {
                _accounts.ViewAll();
            }
            else if (choice == 3)
            {
                int number = InputHelper.ReadInt("Account number: ");
                Account found = _accounts.FindByNumber(number);
                if (found == null)
                {
                    System.Console.WriteLine("No account #" + number + ".");
                }
                else
                {
                    found.Display();
                }
            }
            else if (choice == 4)
            {
                int number = InputHelper.ReadInt("Account number to remove: ");
                if (_accounts.FindByNumber(number) == null)
                {
                    System.Console.WriteLine("No account #" + number + ".");
                    return;
                }
                int purged = _transactions.RemoveByAccount(number);
                _accounts.Remove(number);
                System.Console.WriteLine("Removed account #" + number + " and " + purged + " transaction(s).");
            }
            else
            {
                System.Console.WriteLine("Unknown option.");
            }
        }

        // ---------------- Transaction subsystem menu ----------------
        private void TransactionMenu()
        {
            System.Console.WriteLine("--- TRANSACTIONS: 1)Deposit 2)Withdraw 3)View all 4)Search by account ---");
            int choice = InputHelper.ReadInt("Choose: ");
            System.Console.WriteLine();

            if (choice == 1)
            {
                int number = InputHelper.ReadInt("Account number: ");
                Account account = _accounts.FindByNumber(number);
                if (account == null)
                {
                    System.Console.WriteLine("No account #" + number + ".");
                    return;
                }
                double amount = InputHelper.ReadAmount("Deposit amount: ");
                if (account.Deposit(amount))
                {
                    _transactions.Record(number, "DEPOSIT", amount);
                    System.Console.WriteLine("Deposited " + amount.ToString("0.00") +
                        ". New balance: " + account.Balance.ToString("0.00"));
                }
                else
                {
                    System.Console.WriteLine("Deposit rejected (amount must be positive).");
                }
            }
            else if (choice == 2)
            {
                int number = InputHelper.ReadInt("Account number: ");
                Account account = _accounts.FindByNumber(number);
                if (account == null)
                {
                    System.Console.WriteLine("No account #" + number + ".");
                    return;
                }
                double amount = InputHelper.ReadAmount("Withdraw amount: ");
                // POLYMORPHISM: Withdraw enforces the subclass-specific rule.
                if (account.Withdraw(amount))
                {
                    _transactions.Record(number, "WITHDRAW", amount);
                    System.Console.WriteLine("Withdrew " + amount.ToString("0.00") +
                        ". New balance: " + account.Balance.ToString("0.00"));
                }
                else
                {
                    System.Console.WriteLine("Withdrawal rejected by the account's rules.");
                }
            }
            else if (choice == 3)
            {
                _transactions.ViewAll();
            }
            else if (choice == 4)
            {
                int number = InputHelper.ReadInt("Account number: ");
                _transactions.SearchByAccount(number);
            }
            else
            {
                System.Console.WriteLine("Unknown option.");
            }
        }
    }
}
