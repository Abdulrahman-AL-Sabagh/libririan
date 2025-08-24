namespace Libririan;

public class SelctableCli
{
    public string selectableValue(string[] options)
    {
        int index = 0;
        ConsoleKey key;
        do
        {
            Console.Clear();
            drawText(options,index);
            key = Console.ReadKey(true).Key;
            switch (key)
            {
                case ConsoleKey.UpArrow when index > 0:
                    index--;
                    break;
                case ConsoleKey.DownArrow when index < options.Length - 1:
                    index++;
                    break;
            }
        } while (key != ConsoleKey.Enter);
        
        return options[index];
    }

    
    private void drawText(string[] options, int index)
    {
        for (int i = 0; i < options.Length; i++)
        {
            if (i == index)
            {
                Console.ForegroundColor = ConsoleColor.Green;
                Console.WriteLine($"> [{options[i]}]");
                Console.ResetColor();
            }
            else
            {
                Console.WriteLine($"> [{options[i]}]");
            }
        }
    }
}