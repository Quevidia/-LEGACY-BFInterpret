// Made by Quevidia.
// 2021

using System;

namespace BFInterpret // Create the namespace, which will be defined within other scripts.
{
    public class BF // Create the main class itself.
    {
        class MissingBracket : Exception { } // Create an exception that will be thrown when the Brainfuck code contains missing brackets.

        public static string InterpretCode(string BrainfuckCode, string UserInput = null) // Create the main method which is what the user would be wanting to aim for in their code. This method will return a string.
        {
            if (UserInput == null || UserInput == "") UserInput = " "; // Define a whitespace string for UserInput if this argument has not been specified when calling BF.InterpretCode.

            // Make all base variables.
            byte[] Cells = new byte[30000]; // Initiate an array of cells, each holding byte values.
            int Pointer = 0; // Create an int value which will be used as a cell pointer.
            int StringPointer = 0; // Create an int value which will be used as the pointer for each character in the brainfuck code.
            int InputPointer = 0; // Create an int value which will be used as the pointer for the user input (NOT TO BE CONFUSED WITH THE BRAINFUCK CODE).
            string Output = ""; // Create a string value which will be the final output. This is what is going to be returned back to their respective applications.

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
                if (BrainfuckCode[StringPointer] == '+') Cells[Pointer]++; // If the current character in the brainfuck code is +, increment the current cell that the pointer is locating.
                else if (BrainfuckCode[StringPointer] == '-') Cells[Pointer]--; // If the current character in the brainfuck code is -, decrement the current cell that the pointer is locating.
                else if (BrainfuckCode[StringPointer] == '>') Pointer++; // If the current character in the brainfuck code is >, incrememt the pointer's value.
                else if (BrainfuckCode[StringPointer] == '<') Pointer--; // If the current character in the brainfuck code is <, decrement the pointer's value.
                else if (BrainfuckCode[StringPointer] == '.') Output += Convert.ToChar(Cells[Pointer]); // If the current character in the brainfuck code is ., append the ASCII value of the current cell that the pointer is locating to the output string.
                else if (BrainfuckCode[StringPointer] == ',' && InputPointer < UserInput.Length)  // If the current character in the brainfuck code is , and the length of the user input is larger than the value of the input pointer, the following code will run.
                {
                    Cells[Pointer] = Convert.ToByte(UserInput[InputPointer]); // Set the value of the cell that the pointer is locating to the byte value of the current character in the user input.
                    InputPointer++; // Increment the input pointer's value.
                }
                else if (BrainfuckCode[StringPointer] == '[') // If the current character in the brainfuck code is an opening bracket, run the following code.
                {
                    int ValueOfBracket = VerifyPairOfBrackets("Right"); // Place this outside of the if statement, since this will also be used to check if the pair of brackets is valid.
                    if (Cells[Pointer] == 0) StringPointer = ValueOfBracket; // If the value of the current cell is 0, set the string pointer to the closing bracket.
                }
                else if (BrainfuckCode[StringPointer] == ']')
                {
                    int ValueOfBracket = VerifyPairOfBrackets("Left"); // Place this outside of the if statement, since this will also be used to check if the pair of brackets is valid.
                    if (Cells[Pointer] != 0) StringPointer = ValueOfBracket; // If the value of the current cell is not 0, set the string pointer to the opening bracket.
                }
                StringPointer++; // Increment the StringPointer value.
            }

            // Initiate the main loop. This will use a try/catch sequence to halt the loop in case any errors have been encountered during interpretations.
            try { 
                while (StringPointer < BrainfuckCode.Length) { 
                    Interpreter(); // Initiate the interpreter function.
                } 
            }
            catch (IndexOutOfRangeException) { Output = "Memory overflow. Please check your code and see if your cells have been used incorrectly."; } // If the IndexOutOfRangeException exception has been catched, set the output to tell the user that a memory overflow has occurred.
            catch (MissingBracket) { Output = "A pair of brackets contains a missing bracket. Please check your code and see if additional brackets are requipred for your loops."; } // If the MissingBracket exception has been catched, set the output to tell the user that a pair of brackets is still open.
            return Output; // Return the final output.
        }
    }
}
