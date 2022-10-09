public class VigenereEnDecoder
{
    // Переменная с алфавитом по умолчанию (русский)
    const string defaultAlphabet = "АБВГДЕЁЖЗИЙКЛМНОПРСТУФХЦЧШЩЪЫЬЭЮЯ";
    readonly string letters;

    // Добавляем конструктор для возможности поменять алфавит, если он не передан, ставим по умолчанию
    public VigenereEnDecoder(string? alphabet = null)
    {
        letters = string.IsNullOrEmpty(alphabet) ? defaultAlphabet : alphabet;
    }

    // Генерация повторяющегося ключа
    private string GetRepeatKey(string s, int n)
    {
        var p = s;
        while (p.Length < n)
        {
            p += p;
        }

        return p.Substring(0, n);
    }

    private string Vigenere(string text, string password, bool encrypting = true)
    {
        // Создаем в алфавит в нижнем регистре
        var lowerAlphabet = letters.ToLower();
        // Генерируем повторяющийся ключ
        var gamma = GetRepeatKey(password, text.Length);
        // Создаем переменную, куда будем добавлять зашифрованные символы
        var retVal = "";
        var q = letters.Length;

        // Проходимся циклом посимвольно
        for (int i = 0; i < text.Length; i++)
        {
            var letterIndex = letters.IndexOf(text[i]);
            var codeIndex = letters.IndexOf(gamma[i]);

            // Если символ найден, шифруем его
            if (letterIndex > 0)
            {
                retVal += letters[(q + letterIndex + ((encrypting ? 1 : -1) * codeIndex)) % q].ToString();
                // Если найден, идем к следующему символу, перейдя к след. итерации цикла
                continue;
            }
            // Если не найден, пытаемся найти в алфавите с нижним регистром
            else
            {
                var lowerLetterIndex = lowerAlphabet.IndexOf(text[i]);
                var lowerCodeIndex = lowerAlphabet.IndexOf(gamma[i]);

                if (lowerLetterIndex > 0)
                {
                    retVal += lowerAlphabet[(q + lowerLetterIndex + ((encrypting ? 1 : -1) * lowerCodeIndex)) % q].ToString();
                    // Если найден, идем к следующему символу, перейдя к след. итерации цикла
                    continue;
                }
            }

            // Если не найден ни в одном из алфавитов, то добавляем его в неизменном виде
            retVal += text[i].ToString();
        }

        return retVal;
    }

    // Фукнция для шифрования текста
    public string Encrypt(string plainMessage, string password)
        => Vigenere(plainMessage, password);

    // Фукнция для дешифрования текста
    public string Decrypt(string encryptedMessage, string password)
        => Vigenere(encryptedMessage, password, false);
}


// Входная точка программы
class Program
{
    static void Main(string[] args)
    {
        // Создаем экземпляр класса шифровальщика
        var encoder = new VigenereEnDecoder();
        Console.Write("Введите текст: ");
        // Получаем данные с клавиатуры
        var message = Console.ReadLine();
        Console.Write("Введите ключ: ");
        // Получаем данные с клавиатуры
        var password = Console.ReadLine();
        // Создаем переменную и присваиваем ей зашифрованное сообщение
        var encryptedText = encoder.Encrypt(message, password);

        // Выводим результаты
        Console.WriteLine("Зашифрованное сообщение: {0}", encryptedText);
        Console.WriteLine("Расшифрованное сообщение: {0}", encoder.Decrypt(encryptedText, password));

        // Ожидаем от пользователя ввода любого символа, чтобы консоль сама не закрылась
        Console.ReadLine();
    }
}
