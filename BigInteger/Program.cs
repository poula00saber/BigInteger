using System;
using System.Diagnostics;

namespace bigInteger
{
    class Program
    {
        static void Main(string[] args)
        {
            // Performance testing setup
            Stopwatch stopwatch = new Stopwatch();

            // ========== ADDITION TEST CASES ==========
            Console.WriteLine("=== ADDITION TESTS ===");
            TestAddition("0", "0", "0");
            TestAddition("5", "7", "12");
            TestAddition("123", "456", "579");
            TestAddition("999", "1", "1000");
            TestAddition("57329817389217483219843341245621641542354235423543254325",
                        "55435432543729845789275437297438547543254325432543254344",
                        "112765249932947329009118778543060189085608560856086508669");

            // ========== SUBTRACTION TEST CASES ==========
            Console.WriteLine("\n=== SUBTRACTION TESTS ===");
            TestSubtraction("5", "3", "2");
            TestSubtraction("100", "1", "99");
            TestSubtraction("1234", "567", "667");
            TestSubtraction("1000", "999", "1");
            TestSubtraction("57329817389217483219843341245621641542354235423543254325",
                           "55435432543729845789275437297438547543254325432543254344",
                           "1894384845487637430567903948183093999100009990999999981");

            // ========== MULTIPLICATION TEST CASES ==========
            Console.WriteLine("\n=== MULTIPLICATION TESTS ===");
            TestMultiplication("0", "12345", "0");
            TestMultiplication("1234", "5678", "7006652");  // Your original test case
            TestMultiplication("123", "456", "56088");
            TestMultiplication("999", "999", "998001");
            TestMultiplication("123456789", "987654321", "121932631112635269");
            TestMultiplication("57329817389217483219843341245621641542354235423543254325",
                             "55435432543729845789275437297438547543254325432543254344",
                             "3178767850521823213438162548979797456012759470499004341405669092380931914349760613990090250240481300");

            // ========== DIVISION TEST CASES ==========
            Console.WriteLine("\n=== DIVISION TESTS ===");
            TestDivision("10", "2", "5");
            TestDivision("100", "25", "4");
            TestDivision("123456", "789", "156");
            TestDivision("999999", "111111", "9");
            TestDivision("57329817389217483219843341245621641542354235423543254325",
                       "123456789",
                       "464388341827717642859347298572475814880");

            // ========== PERFORMANCE TESTING ==========
            Console.WriteLine("\n=== PERFORMANCE TESTS ===");
            PerformanceTest("12345678901234567890", "98765432109876543210");
            PerformanceTest("57329817389217483219843341245621641542354235423543254325",
                          "55435432543729845789275437297438547543254325432543254344");




            // ========== POWER TEST CASES ==========
            Console.WriteLine("\n=== POWER TESTS ===");
            TestPower("2", "3", "8");
            TestPower("5", "0", "1");       // anything^0 = 1
            TestPower("7", "1", "7");
            TestPower("10", "2", "100");
            TestPower("12", "3", "1728");
            TestPower("123", "2", "15129");
            TestPower("999", "3", "997002999");
            TestPower("123456789", "2", "15241578750190521");
            TestPower("3", "10", "59049");
            TestPower("57329817389217483219843341245621641542354235423543254325", "1",
                      "57329817389217483219843341245621641542354235423543254325"); // power of 1 (identity)



        }

        static void TestAddition(string num1, string num2, string expected)
        {
            TestOperation(num1, num2, expected, (a, b) => BigInt.sum(a, b), "+");
        }

        static void TestSubtraction(string num1, string num2, string expected)
        {
            TestOperation(num1, num2, expected, (a, b) => BigInt.subtract(a, b), "-");
        }

        static void TestMultiplication(string num1, string num2, string expected)
        {
            TestOperation(num1, num2, expected, (a, b) => BigInt.Multiplication(a, b), "×");
        }

        static void TestDivision(string num1, string num2, string expected)
        {
            TestOperation(num1, num2, expected, (a, b) => BigInt.divide(a, b).Quotient, "÷");
        }
        // Add this function after TestDivision
        static void TestPower(string baseNum, string exponentNum, string expected)
        {
            TestOperation(baseNum, exponentNum, expected, (a, b) => BigInt.Power(a, b), "^");
        }



        static void TestOperation(string num1, string num2, string expected,
                                Func<BigInt, BigInt, BigInt> operation, string opSymbol)
        {
            try
            {
                BigInt a = new BigInt(num1);
                BigInt b = new BigInt(num2);
                BigInt result = operation(a, b);
                string actual = result.ToString();

                if (actual == expected)
                {
                    Console.ForegroundColor = ConsoleColor.Green;
                    Console.WriteLine($"PASS: {num1} {opSymbol} {num2} = {actual}");
                }
                else
                {
                    Console.ForegroundColor = ConsoleColor.Red;
                    Console.WriteLine($"FAIL: {num1} {opSymbol} {num2}");
                    Console.WriteLine($"  Expected: {expected}");
                    Console.WriteLine($"  Actual:   {actual}");
                }
            }
            catch (Exception ex)
            {
                Console.ForegroundColor = ConsoleColor.Red;
                Console.WriteLine($"ERROR: {num1} {opSymbol} {num2}");
                Console.WriteLine($"  Exception: {ex.Message}");
            }
            finally
            {
                Console.ResetColor();
            }
        }

        static void PerformanceTest(string num1, string num2)
        {
            Console.WriteLine($"\nPerformance Test: {num1.Substring(0, Math.Min(10, num1.Length))}... × {num2.Substring(0, Math.Min(10, num2.Length))}...");

            BigInt a = new BigInt(num1);
            BigInt b = new BigInt(num2);

            Stopwatch sw = new Stopwatch();

            // Warm up
            BigInt.sum(a, b);

            // Addition test
            sw.Start();
            BigInt sum = BigInt.sum(a, b);
            sw.Stop();
            Console.WriteLine($"Addition: {sw.ElapsedMilliseconds} ms");
            sw.Reset();

            // Multiplication test
            sw.Start();
            BigInt product = BigInt.Multiplication(a, b);
            sw.Stop();
            Console.WriteLine($"Multiplication: {sw.ElapsedMilliseconds} ms");
            Console.WriteLine($"Result length: {product.arr.Count} digits");


            // inside PerformanceTest
            // Power test
            sw.Start();
            BigInt powResult = BigInt.Power(a, new BigInt("5"));  // moderate exponent, not too big
            sw.Stop();
            Console.WriteLine($"Power (exp=5): {sw.ElapsedMilliseconds} ms");
            Console.WriteLine($"Power Result length: {powResult.arr.Count} digits");

        }
    }
}