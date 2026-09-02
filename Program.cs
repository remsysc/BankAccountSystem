namespace BankAccountSystem
{
    // Entry point: builds the main system and runs the menu loop.
    public class Program
    {
        public static void Main(string[] args)
        {
            BankManager bank = new BankManager();
            bank.Run();
        }
    }
}
