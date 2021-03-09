# BFInterpret

BFInterpret, a basic Brainfuck interpreter written in C#! Hope you enjoy!

### What it does: Either run Brainfuck code, or translate 8-bit ASCII text into Brainfuck code.

### How it works: If you compile the code that I have provided, it should return BFInterpret.DLL. This can then be referenced in any C# project of yours, and then imported by referencing its namespace which is BFInterpret. 

When interpreting Brainfuck code, you'll want to look for the BF class, and then the <code>InterpretCode</code> method, which takes in two arguments. These two arguments are BrainfuckCode, a required string, and UserInput, an optional string). BrainfuckCode makes up the actual Brainfuck code being interpreted, while UserInput is optional and is essentially used by the , statement in your Brainfuck code. This method will return a string containing the output from each "." statement.

When converting 8-bit ASCII text into Brainfuck code, you'll want to also look for the BF class, but this time you'll want to use the <code>ASCIItoBF</code> method, which takes in one necessary argument - ASCII, which is the exact text that will be used. If any non-ASCII character is inputted, this method will return a string saying "The string that you have inputted contains non-ASCII characters." If all goes well, it will instead return back the Brainfuck code for the ASCII text - also as a string!

####.NET type: .NET 5.0

Enjoy!
