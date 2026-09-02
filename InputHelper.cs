namespace BankAccountSystem
{
    // Small helper to read and parse console input safely.
    public static class InputHelper
    {
        public static string ReadText(string prompt)
        {
            System.Console.Write(prompt);
            string line = System.Console.ReadLine();
            if (line == null)
            {
                return "";
            }
            return line.Trim();
        }

        public static int ReadInt(string prompt)
        {
            while (true)
            {
                string text = ReadText(prompt);
                int value;
                if (int.TryParse(text, out value))
                {
                    return value;
                }
                System.Console.WriteLine("  Please enter a whole number.");
            }
        }

        // Reads a non-negative amount, re-prompting until valid.
        public static double ReadAmount(string prompt)
        {
            while (true)
            {
                string text = ReadText(prompt);
                double value;
                if (double.TryParse(text, out value) && value >= 0)
                {
                    return value;
                }
                System.Console.WriteLine("  Please enter a valid non-negative number.");
            }
        }
    }
}
