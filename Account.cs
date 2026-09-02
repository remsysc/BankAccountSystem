namespace BankAccountSystem
{
    // ABSTRACTION: Account defines WHAT every account can do,
    // but leaves the specific rules (interest, withdrawal policy) to subclasses.
    public abstract class Account
    {
        // ENCAPSULATION: balance is private; the outside world can only
        // change it through Deposit/Withdraw, which enforce the rules.
        private double _balance;

        public int AccountNumber { get; private set; }
        public int OwnerCustomerId { get; private set; }

        public double Balance
        {
            get { return _balance; }
        }

        protected Account(int accountNumber, int ownerCustomerId, double openingBalance)
        {
            AccountNumber = accountNumber;
            OwnerCustomerId = ownerCustomerId;

            // Guard against a negative opening balance.
            if (openingBalance < 0)
            {
                _balance = 0;
            }
            else
            {
                _balance = openingBalance;
            }
        }

        // A short label used when listing accounts. Subclasses must define it.
        public abstract string AccountType { get; }

        // POLYMORPHISM: each account type computes interest its own way.
        public abstract double CalculateInterest();

        // Deposit is shared behavior. Returns true if it succeeded.
        public bool Deposit(double amount)
        {
            if (amount <= 0)
            {
                return false;
            }

            _balance = _balance + amount;
            return true;
        }

        // POLYMORPHISM: withdrawal rules differ per account type,
        // so the actual limit check is delegated to CanWithdraw.
        public bool Withdraw(double amount)
        {
            if (amount <= 0)
            {
                return false;
            }

            if (!CanWithdraw(amount))
            {
                return false;
            }

            _balance = _balance - amount;
            return true;
        }

        // Subclasses decide whether a withdrawal is allowed.
        protected abstract bool CanWithdraw(double amount);

        // Allows subclasses to apply computed interest to the balance.
        protected void CreditInterest(double interest)
        {
            if (interest > 0)
            {
                _balance = _balance + interest;
            }
        }

        // POLYMORPHISM: subclasses extend this to add their own detail line.
        public virtual void Display()
        {
            System.Console.WriteLine(
                "  [" + AccountType + "] Acct #" + AccountNumber +
                " | Owner Customer #" + OwnerCustomerId +
                " | Balance: " + Balance.ToString("0.00"));
        }
    }
}
