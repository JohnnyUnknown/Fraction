using static System.Console;

internal class Program
{
    private static void Main(string[] args)
    {
        // Конструктор по умолчанию
        Fraction fraction = new Fraction();
        // Конструктор с параметрами
        Fraction fraction1 = new Fraction(2, 3);
        Fraction fraction2 = new Fraction(13);
        WriteLine($"Дробь 1 (конструктор по умолчанию):\t" + fraction);
        WriteLine($"Дробь 2 (с двумя параметрами (2, 3)):\t{fraction1.Numerator}/{fraction1.Denominator};");
        WriteLine($"Дробь 3 (с одним параметром (13)):\t" + fraction2);

        // Проверка работы свойств
        WriteLine("\nВведите значения дроби 1:");
        fraction.Input();
        WriteLine("\nВведите значения дроби 2:");
        fraction1.Input();
        WriteLine("\nДроби после ввода значений вручную:");
        WriteLine($"Дробь 1: " + fraction);
        WriteLine($"Дробь 2: " + fraction1);

        // Вывод результатов вычислений
        WriteLine($"\nСложение дробей:\t{fraction} + {fraction1} = {fraction + fraction1}");
        WriteLine($"Вычитание дробей:\t{fraction} - {fraction1} = {fraction - fraction1}");
        WriteLine($"Умножение дробей:\t{fraction} * {fraction1} = {fraction * fraction1}");
        WriteLine($"Деление дробей:\t\t{fraction} / {fraction1} = {fraction / fraction1}");

        // Реализация деконструктора. Декомпозиция объекта fraction2
        (int num, int denom) = fraction2;
        WriteLine($"\nРеализация деконструктора:\n" +
            $"Числитель дроби 3:\t{num};\nЗнаменатель дроби 3:\t{denom};");

        // Сравнение объектов перегруженными операторами
        WriteLine("\nСравнение дроби 1 и дроби 2:");
        if (fraction != fraction1)
        {
            WriteLine($"{fraction} не равны {fraction1}");
            if (fraction > fraction1)
                WriteLine($"{fraction} больше {fraction1}");
            else if (fraction < fraction1)
                WriteLine($"{fraction} меньше {fraction1}");
        }
        else if (fraction == fraction1)
            WriteLine($"{fraction} равны {fraction1}");
    }

    // ====================================================================================================

    class Fraction
    {
        private int numerator;
        private int denominator;

        // Реализация каскадных конструкторов
        public Fraction() : this(0) { }

        public Fraction(int numerator) : this(numerator, 1) { }

        public Fraction(int numerator, int denominator)
        {
            this.numerator = numerator;
            if (denominator != 0) this.denominator = denominator;
            else this.denominator = 1;
            Reduction();
        }

        // Деконструктор. Выполняет декомпозицию объекта на отдельные части
        public void Deconstruct(out int num, out int denom)
        {
            num = numerator;
            denom = denominator;
        }

        // Свойства полей класса
        public int Numerator
        {
            get { return numerator; }
            set { numerator = value; }
        }
        public int Denominator
        {
            get { return denominator; }
            set
            {
                if (value != 0) denominator = value;
                else denominator = 1;
            }
        }

        // Перегрузка оператора "+"
        public static Fraction operator +(Fraction a, Fraction b)
        {
            Fraction f = new Fraction();
            f.numerator = a.Numerator * b.Denominator + b.Numerator * a.Denominator;
            f.denominator = a.Denominator * b.Denominator;
            f.Reduction();
            return f;
        }

        // Перегрузка оператора "-"
        public static Fraction operator -(Fraction a, Fraction b)
        {
            Fraction f = new Fraction();
            f.numerator = a.Numerator * b.Denominator - b.Numerator * a.Denominator;
            f.denominator = a.Denominator * b.Denominator;
            f.Reduction();
            return f;
        }

        // Перегрузка оператора "*"
        public static Fraction operator *(Fraction a, Fraction b)
        {
            Fraction f = new Fraction();
            f.numerator = a.Numerator * b.Numerator;
            f.denominator = a.Denominator * b.Denominator;
            f.Reduction();
            return f;
        }

        // Перегрузка оператора "/"
        public static Fraction operator /(Fraction a, Fraction b)
        {
            if (b.Numerator != 0)
            {
                Fraction f = new Fraction();
                f.numerator = a.Numerator * b.Denominator;
                f.denominator = a.Denominator * b.Numerator;
                f.Reduction();
                return f;
            }
            else
            {
                WriteLine("\nНа нуль делить нельзя!!!\n");
                return a;
            }
        }

        // Переопределение метода Equals(), чтобы избавиться от рефлексии в методе
        // (Без сокращения дроби работает неправильно).
        public override bool Equals(object? obj)
        {
            return this.ToString() == obj?.ToString();
        }

        // Переопределение метода GetHashCode(). Переопределяется в паре с Equals()
        public override int GetHashCode()
        {
            return this.ToString().GetHashCode();
        }

        //Перегрузка операторов отношений объектов класса Fraction
        public static bool operator <(Fraction obj1, Fraction obj2)
        {
            return obj1.Numerator * obj2.Denominator < obj2.Numerator * obj1.Denominator;
        }

        public static bool operator >(Fraction obj1, Fraction obj2)
        {
            return obj1.Numerator * obj2.Denominator > obj2.Numerator * obj1.Denominator;
        }

        public static bool operator ==(Fraction obj1, Fraction obj2)
        {
            return obj1.Equals(obj2);
        }

        public static bool operator !=(Fraction obj1, Fraction obj2)
        {
            return !(obj1 == obj2);
        }

        // Перегрузка вывода объекта класса
        public override string ToString()
        {
            return $"{numerator}/{denominator}";
        }

        // Метод ввода значений пользователем
        public void Input()
        {
            Write("Введите значение числителя:\t");
            numerator = Convert.ToInt32(ReadLine());
            do
            {
                Write("Введите значение знаменателя:\t");
                denominator = Convert.ToInt32(ReadLine());
                if (denominator == 0) WriteLine("На нуль делить нельзя!!!");
            } while (denominator == 0);
            Reduction();
        }

        // Метод сокращения дроби
        private void Reduction()
        {
            for (int i = numerator; i > 1; i--)
            {
                if (numerator % i == 0 && denominator % i == 0)
                {
                    numerator /= i;
                    denominator /= i;
                    break;
                }
            }
        }
    }
}

