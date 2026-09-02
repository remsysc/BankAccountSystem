namespace BankAccountSystem
{
    // INHERITANCE: CheckingAccount IS-A Account with a different policy.
    public class CheckingAccount : Account
    {
        private const double OverdraftLimit = 500.0; // may go this far negative

        public CheckingAccount(int accountNumber, int ownerCustomerId, double openingBalance)
            : base(accountNumber, ownerCustomerId, openingBalance)
        {
        }

        public override string AccountType
        {
            get { return "CHECKING"; }
        }

        // POLYMORPHISM: a basic checking account earns no interest.
        public override double CalculateInterest()
        {
            return 0.0;
        }

        // A checking account may overdraw down to the overdraft limit.
        protected override bool CanWithdraw(double amount)
        {
            return (Balance - amount) >= -OverdraftLimit;
        }

        public override void Display()
        {
            base.Display();
            System.Console.WriteLine(
                "         Rule: overdraft allowed down to -" + OverdraftLimit.ToString("0.00") +
                " | No interest");
        }
    }
}
