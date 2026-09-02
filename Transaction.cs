namespace BankAccountSystem
{
    // A record of a single money movement. This is the link between the
    // TransactionLog subsystem and the accounts it acts upon.
    public class Transaction
    {
        public int TransactionId { get; private set; }
        public int AccountNumber { get; private set; }
        public string Kind { get; private set; }   // "DEPOSIT" or "WITHDRAW"
        public double Amount { get; private set; }
        public string Timestamp { get; private set; }

        public Transaction(int transactionId, int accountNumber, string kind, double amount)
        {
            TransactionId = transactionId;
            AccountNumber = accountNumber;
            Kind = kind;
            Amount = amount;
            Timestamp = System.DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss");
        }

        public void Display()
        {
            System.Console.WriteLine(
                "  Txn #" + TransactionId +
                " | Acct #" + AccountNumber +
                " | " + Kind +
                " | Amount: " + Amount.ToString("0.00") +
                " | " + Timestamp);
        }
    }
}
