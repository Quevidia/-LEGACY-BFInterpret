# BFInterpret

BFInterpret, a basic Brainfuck interpreter written in C Sharp! Hope you enjoy!

What it does. Run Brainfuck code.

How it works. If you compile the code that I have provided, it should return BFInterpret.DLL. This can then be referenced in any C Sharp project of yours, and then imported by referencing its namespace which is BFInterpret. When interpreting Brainfuck code, you'll want to look for the BF class, and then the InterpretCode method, which has two arguments. These two arguments are BrainfuckCode, a required string, and UserInput, an optional string). BrainfuckCode makes up the actual Brainfuck code being interpreted, while UserInput is optional and is essentially used by the , statement in your Brainfuck code.

.NET type. .NET 5.0

Enjoy!
