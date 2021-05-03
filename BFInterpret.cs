// Made by Quevidia.
// 2021

using System;
using System.Text;

namespace BFInterpret // Create the namespace, which will be defined within other scripts.
{
    public static class BF // Create the main class itself.
    {
        class MissingBracket : Exception { } // Create an exception that will be thrown when the Brainfuck code contains missing brackets.

        static bool IsPrime(this int Number) // Create a small method that will check whether a number is a prime number or not. This is an extension method thus it will be used as a method for objects of integer type.
        {
            Number = Math.Abs(Number); // Get the absolute value of Number, then set its value to Number.
            if (Number == 0 || Number == 1) return false; // Return false if Number is 0 or 1, since these are not prime numbers.
            if (Number == 2) return true; // Two is a prime number, so if Number is 2, return true.
            for (int i = 3; i <= Math.Ceiling(Math.Sqrt(Number)); i++) // Make a for loop that will range between 3 and the square root of Number rounded to the nearest larger integer.
            {
                if (Number % i == 0) return false; // Check if number modulo i equals to 0. If so, return false, since this means that this number is not a prime number and it can be divided by a number asides from 1 and itself.
            }
            return true; // This will only be reached if the for loop did not return false whatsoever. If the code did get to this point, then simply return true.
        }

        static int[] GetTwoNumbersToMakeUp(this int Number) // Create a small method that will generate two numbers that can make up the number provided as the argument for this method.
        {
            int FirstNumber = 1; int SecondNumber = Number; int Count = 1; // Create two variables that will essentially be used for returning back to methods that have called this method, then create a Count variable that will have the same value as FirstNumber if it were incremented every time in the following while loop.
            while (Count <= Number / Count) // Create a while loop that will continue going until Count is larger than Number divided by Count.
            {
                if ((Number / Count) * Count == Number)
                {
                    FirstNumber = Count; SecondNumber = Number / FirstNumber; // Set FirstNumber's value to be the same as Count, then set SecondNumber's value to to be Number / FirstNumber.
                }
                Count++; // Increment Count.
            }
            return new int[] { FirstNumber, SecondNumber } ; // Return the values of FirstNumber and SecondNumber in an array of int type!
        }

        public static StringBuilder InterpretCode(string BrainfuckCode, string UserInput = null) // Create the method responsible for interpreting all of the Brainfuck code. This will return a string consisting of the output.
        {
            if (UserInput == null) UserInput = ""; // Define an empty string for UserInput if this argument has not been specified when calling BF.InterpretCode.

            // Make all base variables.
            byte[] Cells = new byte[30000]; // Initiate an array of cells, each holding byte values.
            int Pointer = 0; // Create an int value which will be used as a cell pointer.
            int StringPointer = 0; // Create an int value which will be used as the pointer for each character in the brainfuck code.
            int InputPointer = 0; // Create an int value which will be used as the pointer for the user input (NOT TO BE CONFUSED WITH THE BRAINFUCK CODE).
            StringBuilder Output = new(); // Create a string value which will be the final output. This is what is going to be returned back to their respective applications.

            // Create a few additional methods that will be necessary for this code. This includes the one-and-only Brainfuck interpreter!
            int VerifyPairOfBrackets(string Direction = null) // This small method will verify a pair of brackets, which will return an int value.
            {
                if (Direction == null || !(Direction == "Left" || Direction == "Right")) Direction = "Right"; // If the Direction value is null or is not a true direction then it will be set to right.
                int TempPointer = StringPointer; int OpenBrackets = 1; // Create a temporary pointer value. This is only used for verifying loops. Also create a value that will define the amount of open brackets.
                while ((Direction == "Right" && TempPointer < BrainfuckCode.Length - 1) || (Direction == "Left" && TempPointer > 0)) // Check if the direction is right and the length of the brainfuck code is larger than the value of the temporary pointer, or if the direction is left and the value of the temporary pointer is larger than 0.
                {
                    if (Direction == "Right") TempPointer++; else TempPointer--; // If the direction is right, increment the temporary pointer value, otherwise decrement it.
                    if ((Direction == "Right" && BrainfuckCode[TempPointer] == '[') || (Direction == "Left" && BrainfuckCode[TempPointer] == ']')) OpenBrackets++; // If the direction is right and the current character is an opening bracket or the direction is left and the current character is a closing bracket, increment the open brackets value.
                    if ((Direction == "Right" && BrainfuckCode[TempPointer] == ']') || (Direction == "Left" && BrainfuckCode[TempPointer] == '[')) OpenBrackets--; // If the direction is right and the current character is a closing bracket or the direction is left and the current character is an opening bracket, decrement the open brackets value.
                    if (OpenBrackets == 0) break; // If the value of OpenBrackets is 0, end the loop.
                }
                if (OpenBrackets != 0) throw new MissingBracket(); // If the value of OpenBrackets is not 0 - meaning that there is still an open pair of brackets - throw the MissingBracket exception.
                return TempPointer; // Return the temporary pointer value.
            }
            void Interpreter() // This small method will interpret the brainfuck code.
            {
                switch (BrainfuckCode[StringPointer])
                {
                    case '+': // If the current character in the brainfuck code is +, increment the current cell that the pointer is locating.
                        Cells[Pointer]++; break;
                    case '-': // If the current character in the brainfuck code is -, decrement the current cell that the pointer is locating.
                        Cells[Pointer]--; break;
                    case '>': // If the current character in the brainfuck code is >, incrememt the pointer's value.
                        Pointer++; break;
                    case '<': // If the current character in the brainfuck code is <, decrement the pointer's value.
                        Pointer--; break;
                    case '.': // If the current character in the brainfuck code is ., append the ASCII value of the current cell that the pointer is locating to the output string.
                        Output.Append(Convert.ToChar(Cells[Pointer])); break;
                    case ',': // If the current character in the brainfuck code is , and the length of the user input is larger than the value of the input pointer, the following code will run.
                        if (InputPointer < UserInput.Length)
                        {
                            Cells[Pointer] = Convert.ToByte(UserInput[InputPointer]); // Set the value of the cell that the pointer is locating to the byte value of the current character in the user input.
                            InputPointer++; // Increment the input pointer's value.
                        }
                        break;
                    case '[': // If the current character in the brainfuck code is an opening bracket, run the following code.
                        int Left = VerifyPairOfBrackets("Right"); // Place this outside of the if statement, since this will also be used to check if the pair of brackets is valid.
                        if (Cells[Pointer] == 0) StringPointer = Left; // If the value of the current cell is 0, set the string pointer to the closing bracket.
                        break;
                    case ']': // If the current character in the brainfuck code is a closing bracket, run the following code.
                        int Right = VerifyPairOfBrackets("Left"); // Place this outside of the if statement, since this will also be used to check if the pair of brackets is valid.
                        if (Cells[Pointer] != 0) StringPointer = Right; // If the value of the current cell is not 0, set the string pointer to the opening bracket.
                        break;
                }
                StringPointer++; // Increment the StringPointer value.
            }

            // Initiate the main loop. This will use a try/catch sequence to halt the loop in case any errors have been encountered during interpretations.
            try { 
                while (StringPointer < BrainfuckCode.Length) 
                { 
                    Interpreter(); // Initiate the interpreter function.
                } 
            }
            catch (IndexOutOfRangeException) 
            { 
                Output.Clear(); Output.Append("Memory overflow. Please check your code and see if your cells have been used incorrectly."); 
            } // If the IndexOutOfRangeException exception has been catched, set the output to tell the user that a memory overflow has occurred.
            catch (MissingBracket) 
            { 
                Output.Clear(); Output.Append("A pair of brackets contains a missing bracket. Please check your code and see if additional brackets are requipred for your loops."); 
            } // If the MissingBracket exception has been catched, set the output to tell the user that a pair of brackets is still open.
            return Output; // Return the final output.
        }

        public static StringBuilder ASCIItoBF(string ASCII) // Create a method that will convert 8-bit ASCII text into Brainfuck code. This will return a string consisting of the final Brainfuck code.
        {
            // Make all of the base values.
            StringBuilder Output = new(); // Create a variable of string type responsible for holding the final Brainfuck code.
            int DecimalValue = 0; // Create a variable of integer type that will hold the decimal value for the cell being used.
            bool FirstTryGone = false; // Create a variable of boolean type that will depict whether the first letter has been done already or not.

            foreach (char Char in ASCII) // Loop through every character in the ASCII string.
            {
                if (Char > 255) return new StringBuilder("The string that you have inputted contains non-ASCII characters."); // Check if the integer value of Char is larger than 255. If so, return a string saying that the string contains a character that is not an ASCII character.
                int Difference = Char - DecimalValue; // Set Difference to be DecimalValue subtracted from Char.

                if (Math.Abs(Difference) <= 10) // Check if the absolute value of Difference is equal to or smaller than 10.
                {
                    if (Difference == Math.Abs(Difference)) Output.Append(new string('+', Difference)); // If Difference is the same as the absolute value of Difference, concatenate Output with + times Difference.
                    else Output.Append(new string('-', Math.Abs(Difference))); // Concatenate Output with - times the absolute value of Difference.
                }
                else
                {
                    int NonPrimeDecimalValue = Char; // Get the decimal value of Char and assign its value to a variable of integer type called NonPrimeDecimalValue.
                    if (FirstTryGone) Output.Append("[-]<"); // If FirstTryGone is true, concatenate output with [-]<. What this does is clear out the cell that is currently being pointed at, then move the pointer to the previous cell.

                    while (NonPrimeDecimalValue.IsPrime()) // Make a while loop that will loop through the code in its scope if NonPrimeDecimalValue.IsPrime() returns true.
                    {
                        NonPrimeDecimalValue++; // Increment NonPrimeDecimalValue.
                    }

                    int[] NumbersToMultiply = NonPrimeDecimalValue.GetTwoNumbersToMakeUp(); // Create an array of integer type that will hold the values of NonPrimeDecimalValue.GetTwoNumbersToMakeUp();
                    int CountDown = 0; // Create a variable of integer type that will hold the value responsible for the amount of increments used for the character being converted to Brainfuck code after the Brainfuck loop has gone by.

                    while (Char != NonPrimeDecimalValue) // Create a loop that will continue until Char is the same as NonPrimeDecimalValue.
                    {
                        NonPrimeDecimalValue--; CountDown++; // Decrement NonPrimeDecimalValue and then increment CountDown.
                    }

                    Output.Append($"{new string('+', NumbersToMultiply[0])}[>{new string('+', NumbersToMultiply[1])}<-]>{new string('-', CountDown)}"); // Concatenate Output with (+ times NumbersToMultiply[0])([>)(+ times NumbersToMultiply[1])(<-]>)(- times CountDown). This will increment the value of the first cell NumbersToMultiply[0] times, then create a loop that will jump to the next cell, increment its value NumbersToMultiply[1] times, go to the previous cell, decrement its value once, then end the loop. After the loop, it will jump to the next cell and then decrement its value CountDown times.
                }

                Output.Append("."); // Concatenate Output with .. This will be responsible for outputting the ASCII data of the value of the current cell.
                DecimalValue = Char; FirstTryGone = true; // Set DecimalValue to hold the value of Char, then set FirstTryGone to true.
            }

            return Output; // Return the final output.
        }
    }
}
