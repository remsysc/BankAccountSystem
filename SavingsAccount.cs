namespace BankAccountSystem
{
    // INHERITANCE: SavingsAccount IS-A Account and reuses its balance logic.
    public class SavingsAccount : Account
    {
        private const double InterestRate = 0.025;   // 2.5% interest
        private const double MinimumBalance = 100.0;  // must stay above this

        public SavingsAccount(int accountNumber, int ownerCustomerId, double openingBalance)
            : base(accountNumber, ownerCustomerId, openingBalance)
        {
        }

        public override string AccountType
        {
            get { return "SAVINGS"; }
        }

        // POLYMORPHISM: savings earns interest on its current balance.
        public override double CalculateInterest()
        {
            return Balance * InterestRate;
        }

        // A savings account may not drop below the minimum balance.
        protected override bool CanWithdraw(double amount)
        {
            return (Balance - amount) >= MinimumBalance;
        }

        public override void Display()
        {
            base.Display();
            System.Console.WriteLine(
                "         Rule: keep at least " + MinimumBalance.ToString("0.00") +
                " | Interest rate: " + (InterestRate * 100).ToString("0.0") + "%");
        }
    }
}
